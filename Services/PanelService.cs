using HotelProject.Data;
using HotelProject.Models;
using Microsoft.EntityFrameworkCore;
namespace HotelProject.Services
{
    public class PanelService // сервис для админки
    {
        private readonly IDbContextFactory<AppDbContext> _contextFactory; // фабрика для создания подключений к бд

        public PanelService(IDbContextFactory<AppDbContext> contextFactory) // конструктор
        {
            _contextFactory = contextFactory; // сохраняем фабрику
        }
        // метод чтобы получить всех юзеров
        public async Task<List<User>> GetAllUsersAsync()
        {
            using var context = await _contextFactory.CreateDbContextAsync(); // открываем бд
            return await context.Users.ToListAsync(); // берем всех юзеров и делаем список
        }
        // метод чтобы добавить юзера (возвращает успех и текст)
        public async Task<(bool success, string message)> AddUserAsync(string login, string password, string role)
        {
            using var context = await _contextFactory.CreateDbContextAsync(); // открываем бд
            // проверяем есть ли такой логин уже
            if (await context.Users.AnyAsync(u => u.Login == login))
            {
                return (false, "Пользователь с таким логином уже существует"); // если есть - ошибка
            }
            // делаем хеш пароля (чтобы не хранить в открытом виде)
            using var sha256 = System.Security.Cryptography.SHA256.Create(); // создаем шифровальщик
            var bytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)); // шифруем пароль
            var hash = Convert.ToHexString(bytes).ToLower(); // переводим в строку маленькими буквами
            // создаем нового юзера
            var user = new User
            {
                Login = login, // логин
                PasswordHash = hash, // хеш пароля
                Role = role, // роль (админ или юзер)
                Failed = 0, // ошибок 0
                IsBlocked = false // не заблокирован
            };

            context.Users.Add(user); // добавляем в бд
            await context.SaveChangesAsync(); // сохраняем изменения
            return (true, "Пользователь успешно добавлен"); // говорим что все ок
        }
        // метод чтобы разблокировать юзера
        public async Task UnblockUserAsync(int userId)
        {
            using var context = await _contextFactory.CreateDbContextAsync(); // открываем бд
            var user = await context.Users.FindAsync(userId); // ищем юзера по id
            if (user != null) // если нашли
            {
                user.IsBlocked = false; // снимаем блокировку
                user.Failed = 0; // обнуляем счетчик ошибок
                await context.SaveChangesAsync(); // сохраняем в бд
            }
        }
    }
}