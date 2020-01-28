using System.ComponentModel.DataAnnotations;

namespace BankingApplication.Models {
    public class LoginViewModel {
        [Required]
        public string UserID { get; set; }

        [Required]
        public string Password { get; set; }
    }
}