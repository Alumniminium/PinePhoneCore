using System;

namespace PinePhoneCore.Helpers
{
    public static class CliArgs
    {
        private static T Assign<T>(string[] array, string argument, T default_value = default)
        {
            if (array == null)
                return default_value;
            if (typeof(T) == typeof(bool))
            {
                int index = Array.IndexOf(array, argument);
                return (T)Convert.ChangeType(index != -1, typeof(T));
            }
            else
            {
                int index = Array.IndexOf(array, argument);
                if (index != -1)
                {
                    string value = array[index + 1].Trim();
                    default_value = (T)Convert.ChangeType(value, typeof(T));
                }
                return default_value;
            }
        }
    }
}