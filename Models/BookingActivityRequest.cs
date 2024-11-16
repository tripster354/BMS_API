namespace BMS_API.Models
{
    public class BookingActivityRequest
    {
        public long ActivityId { get; set; }
        public long UserId {  get; set; }
        public int SeatCount {  get; set; }
        public string ContactIDFs {  get; set; }
        public int BookingStatus { get; set; } = 1;// Default to 1 (confirmed status)
    }
}
