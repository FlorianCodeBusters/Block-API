using Serilog;

namespace Blocks_api.Logger
{
    public class SeriLogger : IBlockLogger
    {
        public void Error(string message)
        {
            Log.Error(message);
        }

        public void Fatal(string message)
        {
            Log.Fatal(message);
        }

        public void Info(string message)
        {
            Log.Information(message);
        }

        public void Warn(string message)
        {
            Log.Warning(message);
        }
    }
}
