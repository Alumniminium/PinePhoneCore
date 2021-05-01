namespace AaronLuna.ConsoleProgressBar
{
    using System;
    using System.Linq;
    using System.Threading;

    public class FileTransferProgressBar : ConsoleProgressBar
    {
        long _lastReportTicks;

        public FileTransferProgressBar(long fileSizeInBytes, TimeSpan timeout)
        {
            _lastReportTicks = DateTime.Now.Ticks;

            FileSizeInBytes = fileSizeInBytes;
            BytesReceived = 0;
            TimeSpanFileStalled = timeout;
            DisplayBytes = true;

            Timer = new Timer(TimerHandler);

            // A progress bar is only for temporary display in a console window.
            // If the console output is redirected to a file, draw nothing.
            // Otherwise, we'll end up with a lot of garbage in the target file.
            if (!Console.IsOutputRedirected)
            {
                ResetTimer();
            }
        }

        public long FileSizeInBytes { get; set; }
        public long BytesReceived { get; set; }
        public TimeSpan TimeSpanFileStalled { get; set; }
        public bool DisplayBytes { get; set; }

        public event EventHandler<ProgressEventArgs> FileTransferStalled;

        public new void Report(double value)
        {
            var ticks = DateTime.Now.Ticks;
            Interlocked.Exchange(ref _lastReportTicks, ticks);

            // Make sure value is in [0..1] range
            value = Math.Max(0, Math.Min(1, value));
            Interlocked.Exchange(ref CurrentProgress, value);
        }

        void TimerHandler(object state)
        {
            lock (Timer)
            {
                if (Disposed) return;
                var elapsedTicks = DateTime.Now.Ticks - _lastReportTicks;
                var elapsed = TimeSpan.FromTicks(elapsedTicks);

                UpdateText(GetProgressBarText(CurrentProgress));
                ResetTimer();

                if (elapsed < TimeSpanFileStalled) return;

                FileTransferStalled?.Invoke(this,
                new ProgressEventArgs
                {
                    LastDataReceived = new DateTime(_lastReportTicks),
                    TimeOutTriggered = DateTime.Now
                });
            }
        }

        string GetProgressBarText(double currentProgress)
        {
            const string singleSpace = " ";

            var numBlocksCompleted = (int)(currentProgress * NumberOfBlocks);

            var completedBlocks =
                Enumerable.Range(0, numBlocksCompleted).Aggregate(
                    string.Empty,
                    (current, _) => current + CompletedBlock);

            var incompleteBlocks =
                Enumerable.Range(0, NumberOfBlocks - numBlocksCompleted).Aggregate(
                    string.Empty,
                    (current, _) => current + IncompleteBlock);

            var progressBar = $"{StartBracket}{completedBlocks}{incompleteBlocks}{EndBracket}";
            var percent = $"{currentProgress:P0}".PadLeft(4, '\u00a0');

            var fileSizeInBytes = $"{FileSizeInBytes}";
            var padLength = fileSizeInBytes.Length;
            var bytesReceived = $"{BytesReceived}".PadLeft(padLength, '\u00a0');
            var bytes = $"{bytesReceived} of {fileSizeInBytes}";

            var animationFrame = AnimationSequence[AnimationIndex++ % AnimationSequence.Length];
            var animation = $"{animationFrame}";

            progressBar = DisplayBar
                ? progressBar + singleSpace
                : string.Empty;

            percent = DisplayPercentComplete
                ? percent + singleSpace
                : string.Empty;

            bytes = DisplayBytes
                ? bytes + singleSpace
                : string.Empty;

            if (!DisplayAnimation || currentProgress is 1)
            {
                animation = string.Empty;
            }

            return progressBar + bytes + percent + animation;
        }
    }
}