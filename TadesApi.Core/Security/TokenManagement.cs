using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

namespace TadesApi.Core.Security
{
    public static class TokenManagement
    {
        /// <summary>
        /// Create Token
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="emailAddress"></param>
        /// <param name="companyName"></param>
        /// <returns></returns>
        public static string CreateToken(long userId, string firstName,  string emailAddress, string username, int roleId)
        {
            var sharedKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(GetTokenKey()));


            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, emailAddress),
                new Claim(ClaimTypes.GivenName, firstName),
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.PrimarySid, userId.ToString()),
                new Claim("RoleId", roleId.ToString(), ClaimValueTypes.Integer),
                //new Claim("ClientId", clientId.ToString()),
        };




            var signinCredentials = new SigningCredentials(sharedKey, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                NotBefore = DateTime.Now,
                Expires = DateTime.Now.AddHours(7),
                SigningCredentials = signinCredentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescription);
            string tokenString = tokenHandler.WriteToken(token);

            return tokenString;

        }
        public static string GetTokenKey()
        {
            return "BetYYYYech!EDecCClaDration!ComMMmon!TokenManagement45DfgRdd90Oklerdskl!";
        }
        public static DateTime ConvertFromUnixTimestamp(int timestamp)
        {
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

            dateTime = dateTime.AddSeconds(timestamp).ToLocalTime();
            return dateTime;
        }
        public static ClaimsPrincipal ParseToken(string jwtToken)
        {
            IdentityModelEventSource.ShowPII = true;

            SecurityToken validatedToken;
            TokenValidationParameters validationParameters = new TokenValidationParameters();

            validationParameters.ValidateIssuer = false;
            validationParameters.ValidateAudience = false;
            validationParameters.ValidateLifetime = true;
            validationParameters.ValidateIssuerSigningKey = true;
            validationParameters.ValidIssuer = "";
            validationParameters.ValidAudience = "";

            validationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(GetTokenKey()));

            ClaimsPrincipal principal = new JwtSecurityTokenHandler().ValidateToken(jwtToken, validationParameters, out validatedToken);


            return principal;
        }
        public static string CreateDemoToken(int custId, string emailAddress, string username, string role, int accountId)
        {
            var sharedKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(GetTokenKey()));

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, emailAddress),
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.PrimarySid, custId.ToString()),
                new Claim(ClaimTypes.PrimaryGroupSid, accountId.ToString()),
                new Claim(ClaimTypes.Role, role),

        };

            var signinCredentials = new SigningCredentials(sharedKey, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                NotBefore = DateTime.Now,
                Expires = DateTime.Now.AddDays(30),
                SigningCredentials = signinCredentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescription);
            string tokenString = tokenHandler.WriteToken(token);

            return tokenString;

        }
    }

}
