
using GAPP_BS.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Google;

namespace GAPP_BS.Services
{
    public class BlazorSchoolAuthenticationStateProvider //: AuthenticationStateProvider
    {

        //public override Task<AuthenticationState> GetAuthenticationStateAsync()
        //{
        //    // Create a dummy identity for demonstration purposes
        //    var identity = new ClaimsIdentity(new[]
        //    {
        //    new Claim(ClaimTypes.Name, "username")
        //}, "OAuthProvider");

        //    var user = new ClaimsPrincipal(identity);
        //    return Task.FromResult(new AuthenticationState(user));
        //}
        [JSInvokable]
        public void GoogleLogin(GoogleResponse googleResponse)
        {
           // var principal = new System.Security.Claims.ClaimsPrincipal();
           // var user = User.FromGoogleJwt(googleResponse.Credential);
           //// CurrentUser = user;

           // if (user is not null)
           // {
                
           // }

            //NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(principal)));
        }


    }


public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CustomAuthenticationStateProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var user = _httpContextAccessor.HttpContext.User;
            return Task.FromResult(new AuthenticationState(user));
        }
        public async Task Login(string username)
        {
            var claims = new[] { new Claim(ClaimTypes.Name, username) };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await _httpContextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            //NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public async Task Logout()
        {
            //var authProperties = new AuthenticationProperties { RedirectUri = "/" };
            await _httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
           // NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
        public async Task Challenge()
        {
            var authProperties = new AuthenticationProperties { RedirectUri = "https://localhost:7077/authentication/login-callback" };
            await _httpContextAccessor.HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme, authProperties);
        }
    }

}
