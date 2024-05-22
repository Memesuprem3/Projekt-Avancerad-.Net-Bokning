using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Projekt_Models;
using System.Threading.Tasks;

public class UserLogin
{
    private readonly UserManager<User> _userManager;
    private readonly IConfiguration _configuration;
    private readonly PasswordHasher<User> _passwordHasher;

    public UserLogin(UserManager<User> userManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
        _passwordHasher = new PasswordHasher<User>();
    }

    public async Task<User> FindUserByUsernameAsync(string username)
    {
        return await _userManager.FindByNameAsync(username);
    }

    public bool VerifyPassword(User user, string password)
    {
        var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
        return verificationResult == PasswordVerificationResult.Success;
    }

    public string HashPassword(User user, string password)
    {
        return _passwordHasher.HashPassword(user, password);
    }
}