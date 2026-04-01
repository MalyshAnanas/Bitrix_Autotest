using System.Drawing;

namespace Demo.BaseFramework.LogTools
{
    class LogMessageInfo : LogMessage
    {
        public LogMessageInfo(string text) : base("INFO", text, Color.Black)
        {
        }
    }
}
