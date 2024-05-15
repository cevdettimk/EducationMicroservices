using Education.Shared.Dtos;
using Education.Web.Models;
using IdentityModel.Client;

namespace Education.Web.Services.Interfaces
{
    public interface IIdentityService
    {
        Task<Response<bool>> SignIn(SigninInput signinInput);

        Task<TokenResponse> GetAccessTokenByRefreshToken();

        Task RevokeRefreshToken();
    }
}
