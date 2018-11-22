using System;

namespace Czf.Domain.NewsBankWrapper.Interfaces
{
    public interface IEZProxySignInUriProvider
    {
        /// <summary>
        /// Returns the signin URI to use for POST username and password.
        /// </summary>
        Uri GetSignInUri();
    }
}