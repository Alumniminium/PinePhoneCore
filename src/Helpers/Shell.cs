using System;
using CliWrap;
using CliWrap.Buffered;

namespace PinePhoneCore.Helpers
{
    public class Shell
    {
        public static string GetValue(string executable, string arguments)
        {
            var cmd = Cli.Wrap(executable).WithArguments(arguments).WithValidation(CommandResultValidation.None).ExecuteBufferedAsync().Task.Result;
            Console.WriteLine($"[{cmd.RunTime.TotalMilliseconds}ms] Command: {executable} {arguments}");
            return cmd.StandardOutput.Trim();
        }
        public static void Execute(string executable, string arguments, bool quiet = true)
        {
            try
            {
                var cmd = Cli.Wrap(executable).WithArguments(arguments).WithValidation(CommandResultValidation.None).ExecuteBufferedAsync().Task.Result;
                if (quiet)
                    return;
                Console.WriteLine("StdOut: " + cmd.StandardOutput);
                Console.WriteLine("StdErr: " + cmd.StandardError);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
        public static string FindByPattern(string cmd, string args, string pattern)
        {
            var val = GetValue(cmd, args);
            foreach (var line in val.Split(Environment.NewLine))
            {
                if (line.Contains(pattern))
                {
                    val = line;
                    break;
                }
            }
            var patternIndex = val.IndexOf(pattern);
            var found = val.Substring(patternIndex + pattern.Length, val.Length - (patternIndex + pattern.Length)).Split(' ')[0];
            return found;
        }
    }
}
