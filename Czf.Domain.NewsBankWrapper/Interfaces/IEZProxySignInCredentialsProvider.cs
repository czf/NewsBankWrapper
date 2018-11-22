namespace Czf.Domain.NewsBankWrapper.Interfaces
{
    public interface IEZProxySignInCredentialsProvider
    {
        string GetAccount();
        string GetPassword();
    }
}