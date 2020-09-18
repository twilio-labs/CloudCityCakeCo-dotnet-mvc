
using CloudCityCakeCo.Models.Entities;
using CloudCityCakeCo.Models.Helpers;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace CloudCityCakeCo.Extensions
{
    public static class UserManagerExtension
    {
        public async static Task<UserIdentityResult> CheckIfPartialUserAsync<T>(
            this UserManager<T> userManager,
            T user,
            string password) where T : User
        {
            UserIdentityResult result = new UserIdentityResult();
            IdentityResult identityResult;
            var userEntity = await userManager.FindByEmailAsync(user.Email);
            if (userEntity is null)
            {
                identityResult = await userManager.CreateAsync(user, password);
            }
            else
            {
                userEntity.AuthyId = user.AuthyId;
                userEntity.EmailConfirmed = user.EmailConfirmed;
                userEntity.TwoFactorEnabled = user.TwoFactorEnabled;
                userEntity.PhoneNumberConfirmed = user.PhoneNumberConfirmed;
                userEntity.SecurityStamp = Guid.NewGuid().ToString();
                identityResult = await userManager.UpdateAsync(userEntity);
                await userManager.AddPasswordAsync(userEntity, password);
            }
            result.IdentityResult = identityResult;
            result.User = userEntity;

            return result;
        }
    }
}
