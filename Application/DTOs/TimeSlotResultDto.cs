using System;

namespace FlexFit.Application.DTOs
{
    public class TimeSlotResultDto
    {
        public int Id { get; set; }
        public int ResourceId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int AvailableSpots { get; set; }
    }
}
