namespace FlexFit.MongoModels.Models
{
    public class Incident
    {
        public int MemberId { get; set; }
        public int FitnessObjectId { get; set; }
        public DateTime Time { get; set; }
        public string Reason { get; set; } // npr. Neovlašćen pokušaj ulaza
    }
}
