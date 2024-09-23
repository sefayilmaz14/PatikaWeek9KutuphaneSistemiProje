namespace PatikaWeek9KutuphaneSistemiProje.Models
{
    public class BookDetailViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int AuthorId { get; set; }
        public int GenreId { get; set; }
        public DateTime PublishDate { get; set; }
        public string Isbn { get; set; }
        public int CopiesAvailable { get; set; }
        public bool IsMyLibrary { get; set; }
        public string Summary { get; set; }
        
    }
}
