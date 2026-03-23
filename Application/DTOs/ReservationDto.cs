namespace FlexFit.Application.DTOs
{
    public class ReservationDto
    {
        public int MemberId { get; set; }
        public int ResourceId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
