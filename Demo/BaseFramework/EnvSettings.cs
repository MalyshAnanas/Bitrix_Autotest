namespace Demo.BaseFramework
{
    public class EnvSettings
    {
        public static List<string> AppArgs { get; set; } = new List<string>();
        public static bool IsDebug => AppArgs.Contains("debug");
    }
}
