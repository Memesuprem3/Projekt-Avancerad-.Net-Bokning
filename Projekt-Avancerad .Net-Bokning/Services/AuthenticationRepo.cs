using Microsoft.AspNetCore.Identity;
using Projekt_Avancerad_.Net_Bokning.Services.Interface;
using Projekt_Models;
using System.Threading.Tasks;

namespace Projekt_Avancerad_.Net_Bokning.Services
{
    public class AuthenticationRepo : IAuthentication
    {
        private readonly UserManager<User> _userManager;

        public AuthenticationRepo(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IdentityResult> RegisterAsync(User userModel)
        {
            var userExists = await _userManager.FindByNameAsync(userModel.UserName);
            if (userExists != null)
                return IdentityResult.Failed(new IdentityError { Description = "User already exists!" });

            userModel.PasswordHash = _userManager.PasswordHasher.HashPassword(userModel, userModel.PasswordHash);
            userModel.IsActive = true;

            var result = await _userManager.CreateAsync(userModel, userModel.PasswordHash);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(userModel, userModel.Role);
            }
            return result;
        }

        public async Task<bool> LoginAsync(User loginModel)
        {
            var user = await _userManager.FindByNameAsync(loginModel.UserName);
            if (user == null || !await _userManager.CheckPasswordAsync(user, loginModel.PasswordHash))
                return false;

            return true;
        }
    }
}