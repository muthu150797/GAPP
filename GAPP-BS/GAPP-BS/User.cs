using System.IdentityModel.Tokens.Jwt;

namespace GAPP_BS
{
    public class User
    {
    public static UserData? FromGoogleJwt(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            if (tokenHandler.CanReadToken(token))
            {
                var jwtSecurityToken = tokenHandler.ReadJwtToken(token);

                return new UserData()
                {
                    Username = jwtSecurityToken.Claims.First(c => c.Type == "name").Value,
                    Password = ""
                };
            }

            return null;
        }
    }
    public class UserData
    {
        public string? Username { get; set; }
        public string? Password { get; set; }

    }
}
