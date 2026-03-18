using FlexFit.Models;

public class DailyCard : MembershipCard
{
    public DateTime? PurchaseDate { get; set; }

    public ICollection<FitnessObject> FitnessObjects { get; set; } = new List<FitnessObject>();
}