using IdentityServer.AuthServer.Repository;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace IdentityServer.AuthServer.Services
{
    public class CustomProfileService(ICustomUserRepository _customUserRepository) : IProfileService
    {
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var subjectId = context.Subject.GetSubjectId();
            var user = await _customUserRepository.FindById(int.Parse(subjectId));

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("name",user.UserName),
                new Claim("city", user.City)
            };

            if(user.Id == 1)
            {
                claims.Add(new Claim("role", "admin"));
            }
            else
            {
                claims.Add(new Claim("role", "customer"));
            }

            context.AddRequestedClaims(claims);
            // jwt içinde görmek istiyorsanız.
            //context.IssuedClaims.AddRange(claims);
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var userId = context.Subject.GetSubjectId();

            var user = await _customUserRepository.FindById(int.Parse(userId));
            context.IsActive = user != null ? true : false;
        }
    }
}
