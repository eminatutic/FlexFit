namespace FlexFit.Application.DTOs
{
    public class CreatePenaltyCardDto
    {
        public int MemberId { get; set; }
        public int FitnessObjectId { get; set; }
        public decimal Price { get; set; }
        public string Reason { get; set; }
    }
}
