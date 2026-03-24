using System;

namespace FlexFit.Application.DTOs
{
    public class TimeSlotDto
    {
        public int ResourceId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
