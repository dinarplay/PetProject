using System.ComponentModel.DataAnnotations;

namespace PetProject.Models
{
    public class UserReg
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public int Age { get; set; }
        [EmailAddress(ErrorMessage = "Неправильный адрес")]
        public string Email { get; set; }
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string PasswordConfirm { get; set; }
    }
}
