using System.ComponentModel.DataAnnotations;

namespace PatikaWeek9KutuphaneSistemiProje.Models
{
    public class AuthorListViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Yazar ismi doldurmak zorunludur.")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Yazar soyismi doldurmak zorunludur.")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Yazar doğum tarihi doldurmak zorunludur.")]
        public DateTime DateOfBirth { get; set; }
        public string About { get; set; }
    }
}
