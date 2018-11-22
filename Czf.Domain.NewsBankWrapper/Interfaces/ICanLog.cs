namespace Czf.Domain.NewsBankWrapper.Interfaces
{
    public interface ICanLog
    {
        void Error(string message);
        void Info(string message);
    }
}