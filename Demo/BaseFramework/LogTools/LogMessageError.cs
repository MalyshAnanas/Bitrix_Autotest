using System.Drawing;

namespace Demo.BaseFramework.LogTools
{
    class LogMessageError : LogMessage
    {
        public LogMessageError(string text) : base("ERROR", text, Color.Red)
        {
        }
    }
}
