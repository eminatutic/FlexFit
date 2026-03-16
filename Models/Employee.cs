namespace FlexFit.Models
{
    public class Employee 
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        public string License { get; set; }

        public EmployeeType EmployeeType { get; set; }
    }

    public enum EmployeeType
    {
        Instructor,
        Guard
    }
}