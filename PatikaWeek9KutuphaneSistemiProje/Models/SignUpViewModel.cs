using System.ComponentModel.DataAnnotations;

namespace PatikaWeek9KutuphaneSistemiProje.Models
{
    public class SignUpViewModel
    {
        public string Email { get; set; }
        public string Password { get; set; }

        [Compare(nameof(Password))]
        public string PasswordConfirm { get; set; }

        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
    }
}
