using IdentityModel;
using IdentityServer.AuthServer.Repository;
using IdentityServer4.Validation;

namespace IdentityServer.AuthServer.Services
{
    public class ResourceOwnerPasswordValidator(ICustomUserRepository _customUserRepository):IResourceOwnerPasswordValidator
    {

        public async  Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var isUser = await _customUserRepository.VallidateUser(context.UserName, context.Password);
            if (isUser)
            {
                var user = await _customUserRepository.FindByEmail(context.UserName);
                context.Result = new GrantValidationResult(user.Id.ToString(), OidcConstants.AuthenticationMethods.Password);
                //pwd 
            }
            //else
            //{
            //    context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Invalid credentials");
            //}
    
        }
    }
}
