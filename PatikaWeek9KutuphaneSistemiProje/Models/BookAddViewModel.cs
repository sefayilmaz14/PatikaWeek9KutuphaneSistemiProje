using System.ComponentModel.DataAnnotations;

namespace PatikaWeek9KutuphaneSistemiProje.Models
{
    public class BookAddViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Kitap ismi doldurmak zorunludur.")]
        public string Title { get; set; }
        public int AuthorId { get; set; }
        
        
        public DateTime PublishDate { get; set; }
        public string Isbn { get; set; }
        [Required(ErrorMessage = "Kitap kopya sayısı doldurmak zorunludur.")]
        public int CopiesAvailable { get; set; }
        public int GenreId { get; set; }
        public string Summary { get; set; }
       

    }
}
