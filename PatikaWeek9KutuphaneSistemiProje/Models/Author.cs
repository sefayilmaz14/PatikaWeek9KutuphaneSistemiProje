namespace PatikaWeek9KutuphaneSistemiProje.Models
{
    public class Author
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool IsDeleted { get; set; }
        public string About { get; set; }


    }

    
}
