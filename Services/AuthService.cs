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
        private readonly IDbContextFactory<AppDbContext> _contextFactory; //фабрика для работы с бд
        private readonly ProtectedLocalStorage _localStorage; //хранилище в браузере (для сессии)
        //сюда система передает фабрику бд и хранилище
        public AuthService(IDbContextFactory<AppDbContext> contextFactory, ProtectedLocalStorage localStorage)
        {
            _contextFactory = contextFactory;
            _localStorage = localStorage;
        }
        //метод для превращения пароля в хеш (шифрование)
        public string GetHash(string password)
        {
            using var sha256 = SHA256.Create(); //создаем алгоритм шифрования
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password)); //шифруем пароль
            return Convert.ToHexString(bytes).ToLower(); //переводим в строку маленькими буквами
        }
        //метод для входа в систему
        public async Task<(User? user, string? error)> AuthenticateAsync(string login, string password)
        {
            using var context = await _contextFactory.CreateDbContextAsync(); //открываем бд
            //ищем пользователя по логину
            var user = await context.Users.FirstOrDefaultAsync(u => u.Login == login);
            //если логин не найден
            if (user == null)
                return (null, "Вы ввели неверный логин или пароль.");
            // если пользователь заблокирован
            if (user.IsBlocked == true)
                return (null, "Вы заблокированы.");
            var hash = GetHash(password); // хешируем введенный пароль
            // если пароль не совпадает с тем, что в базе
            if (user.PasswordHash != hash)
            {
                user.Failed++; // увеличиваем счетчик ошибок

                // если ошибок 3 и больше - блокируем
                if (user.Failed >= 3)
                {
                    user.IsBlocked = true;
                    await context.SaveChangesAsync(); // сохраняем в бд
                    return (null, "Вы заблокированы.");
                }

                await context.SaveChangesAsync(); //сохраняем счетчик ошибок
                return (null, "Вы ввели неверный логин или пароль.");
            }
            user.Failed = 0; // Успешный вход: сбрасываем счетчик ошибок
            await context.SaveChangesAsync(); //сохраняем изменения в бд
            // сохраняем пользователя в память браузера
            await _localStorage.SetAsync("CurrentUser", user);
            return (user, "Вы успешно авторизовались");
        }
        //метод чтобы получить текущего пользователя (кто сейчас вошел)
        public async Task<User?> GetCurrentUserAsync()
        {
            // достаем из памяти браузера
            var result = await _localStorage.GetAsync<User>("CurrentUser");
            return result.Success ? result.Value : null; //если нашли - возвращаем, если нет - null
        }
        //метод для выхода из системы
        public async Task LogoutAsync()
        {
            await _localStorage.DeleteAsync("CurrentUser"); //удаляем пользователя из памяти
        }
    }
}