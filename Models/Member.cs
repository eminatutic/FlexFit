namespace FlexFit.Models
{
    public class Member
    {
            public int Id { get; set; }

            public string FirstName { get; set; }

            public string LastName { get; set; }

            public string JMBG { get; set; }

            public string Address { get; set; }

            public int PenaltyPoints { get; set; }

            public ICollection<Reservation> Reservations { get; set; }

            public ICollection<PenaltyPoint> PenaltyPointHistory { get; set; }
        
    }
}
