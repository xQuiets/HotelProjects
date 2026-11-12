using System.Security.Cryptography;
using System.Text;
using HotelProject.Data;
using HotelProject.Models;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.EntityFrameworkCore;

namespace HotelProject.Services
{
    public class AuthService
    {
        private readonly IDbContextFactory<AppDbContext> _contextFactory;
        private readonly ProtectedLocalStorage _localStorage;

        public AuthService(IDbContextFactory<AppDbContext> contextFactory, ProtectedLocalStorage localStorage)
        {
            _contextFactory = contextFactory;
            _localStorage = localStorage;
        }

        public string GetHash(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToHexString(bytes).ToLower();
        }

        public async Task<(User? user, string? error)> AuthenticateAsync(string login, string password)
        {
            using var context = await _contextFactory.CreateDbContextAsync();
            var user = await context.Users.FirstOrDefaultAsync(u => u.Login == login);

            if (user == null)
                return (null, "Вы ввели неверный логин или пароль.");

            if (user.IsBlocked == true)
                return (null, "Вы заблокированы.");

            var hash = GetHash(password);

            if (user.PasswordHash != hash)
            {
                user.Failed++;
                if (user.Failed >= 3)
                {
                    user.IsBlocked = true;
                    await context.SaveChangesAsync();
                    return (null, "Вы заблокированы.");
                }
                await context.SaveChangesAsync();
                return (null, "Вы ввели неверный логин или пароль.");
            }

            user.Failed = 0; // Успешный вход: сбрасываем счетчик ошибок
            await context.SaveChangesAsync();
            await _localStorage.SetAsync("CurrentUser", user);

            return (user, "Вы успешно авторизовались");
        }

        public async Task<User?> GetCurrentUserAsync()
        {
            var result = await _localStorage.GetAsync<User>("CurrentUser");
            return result.Success ? result.Value : null;
        }

        public async Task LogoutAsync()
        {
            await _localStorage.DeleteAsync("CurrentUser");
        }
    }
}