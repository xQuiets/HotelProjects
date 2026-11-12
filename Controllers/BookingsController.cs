using HotelProject.Data;
using HotelProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelProject.Controllers
{
    [Route("api/[controller]")] // ссылка будет /api/bookings
    [ApiController] // значит это апи контроллер
    public class BookingsController : ControllerBase
    {
        private readonly IDbContextFactory<AppDbContext> _contextFactory; // фабрика для бд

        public BookingsController(IDbContextFactory<AppDbContext> contextFactory)
        {
            _contextFactory = contextFactory; // сохраняем фабрику
        }

        // говорим сваггеру какие коды ответа бывают
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]

        // GET: api/bookings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookingDTO>>> GetBookings([FromQuery] string? id)
        {
            // если параметр id был передан
            if (!string.IsNullOrEmpty(id))
            {
                // 1. убираем пробелы по краям. если после этого пусто — ошибка (это ловит %20)
                string trimmedId = id.Trim();
                if (string.IsNullOrEmpty(trimmedId))
                {
                    return BadRequest(new { error = "Некорректный параметр id" }); // отдаем 400
                }

                // 2. проверяем, что это число (если буквы — вернется false)
                if (!int.TryParse(trimmedId, out _))
                {
                    return BadRequest(new { error = "Некорректный параметр id" }); // отдаем 400
                }
            }

            try
            {
                using var context = await _contextFactory.CreateDbContextAsync(); // открываем бд
                // берем бронирования и сразу подтягиваем организацию (название)
                var query = context.Bookings.Include(b => b.Organization).AsQueryable();

                // если id передан — ищем только его
                if (!string.IsNullOrEmpty(id))
                {
                    int bookingId = int.Parse(id.Trim()); // переводим в число
                    query = query.Where(b => b.IdBooking == bookingId); // фильтруем
                }

                var bookings = await query.ToListAsync(); // получаем список из бд

                // делаем ответ
                var result = bookings.Select(b => new BookingDTO
                {
                    Id = b.BookingNumber, // номер брони
                    Caption = $"{b.Organization.Name} - {b.Contact}", // склеиваем название и контакт
                    Contact = b.Contact, // просто контакт
                    FormattedDate = b.DateRequest.ToString("dd.MM.yyyy") // дата в формате день.месяц.год
                }).ToList();

                return Ok(result); // отдаем 200 и список
            }
            catch (Exception)
            {
                // если бд упала — отдаем 500
                return StatusCode(500, new { error = "Внутренняя ошибка сервера" });
            }
        }
    }
}