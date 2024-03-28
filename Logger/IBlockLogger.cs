namespace Blocks_api.Logger
{
    public interface IBlockLogger
    {
        void Info(string message);
        void Warn(string message);
        void Error(string message);
        void Fatal(string message);
    }
}
