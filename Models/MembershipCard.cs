namespace FlexFit.Models
{
    public class MembershipCard
    {
        public int Id { get; set; }
        public string CardNumber { get; set; }
       
        public bool IsActive { get; set; }  

        public int? MemberId { get; set; }
        public Member? Member { get; set; }
    }

   
}