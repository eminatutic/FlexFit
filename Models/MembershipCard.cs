namespace FlexFit.Models
{
    public abstract class MembershipCard
    {
        public int Id { get; set; }

        public CardType CardType { get; set; }

        public int MemberId { get; set; }

        public Member Member { get; set; }
    }

    public enum CardType
    {
        Daily,
        Subscription
    }
}