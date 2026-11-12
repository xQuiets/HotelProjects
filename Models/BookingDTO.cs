namespace HotelProject.Models
{
    public class BookingDTO
    {
        public int Id { get; set; }
        public string Caption { get; set; } = null!;
        public string Contact { get; set; } = null!;
        public string FormattedDate { get; set; } = null!;
    }
}