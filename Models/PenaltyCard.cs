namespace FlexFit.Models
{
    public class PenaltyCard
    {
        public int Id { get; set; }

        public int MemberId { get; set; }

        public Member Member { get; set; }

        public int FitnessObjectId { get; set; }

        public FitnessObject FitnessObject { get; set; }

        public DateTime Date { get; set; }

        public decimal Price { get; set; }

        public string Reason { get; set; }
    }
}