using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Google.Apis.Auth;
using GraphQLApi.Infrastructure.Security.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace GraphQLApi.Infrastructure.Security
{
    public class GoogleTokenValidator : ISecurityTokenValidator
    {
        private readonly GoogleOptions _options;
        private readonly JwtSecurityTokenHandler _tokenHandler;

        public GoogleTokenValidator(GoogleOptions options)
        {
            _options = options;
            _tokenHandler = new JwtSecurityTokenHandler();
        }

        public bool CanValidateToken => true;

        public int MaximumTokenSizeInBytes { get; set; } = TokenValidationParameters.DefaultMaximumTokenSizeInBytes;

        public bool CanReadToken(string securityToken)
        {
            return _tokenHandler.CanReadToken(securityToken);
        }

        public ClaimsPrincipal ValidateToken(string securityToken, TokenValidationParameters validationParameters, out SecurityToken validatedToken)
        {
            validatedToken = null;
            var clientList = new List<string>(){ _options.ClientId };
            var payload = GoogleJsonWebSignature.ValidateAsync(securityToken, new GoogleJsonWebSignature.ValidationSettings(){Audience =clientList}).Result; // here is where I delegate to Google to validate

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, payload.Name),
                new Claim(ClaimTypes.Name, payload.Email),
                new Claim(JwtRegisteredClaimNames.FamilyName, payload.FamilyName),
                new Claim(JwtRegisteredClaimNames.GivenName, payload.GivenName),
                new Claim(JwtRegisteredClaimNames.Email, payload.Email),
                new Claim(JwtRegisteredClaimNames.Aud, payload.Audience.ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, payload.Subject),
                new Claim(JwtRegisteredClaimNames.Iss, payload.Issuer),
            };
            try
            {
                var principle = new ClaimsPrincipal();
                principle.AddIdentity(new ClaimsIdentity(claims, "Password"));
                return principle;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;

            }
        }
    }
}
