using FlexFit.Domain.Models;

namespace FlexFit.Application.DTOs
{
    public class CreateResourceDto
    {
        public ResourceType Type { get; set; }
        public ResourceStatus Status { get; set; }
        public int Floor { get; set; }
        public bool IsPremium { get; set; }
        public decimal PremiumFee { get; set; }
        public int FitnessObjectId { get; set; }
    }
}
