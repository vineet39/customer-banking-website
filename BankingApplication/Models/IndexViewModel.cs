using System.ComponentModel.DataAnnotations;

namespace BankingApplication.Models
{
    public class IndexViewModel
    {
        public Customer Customer { get; set; }

        [Required(ErrorMessage = "The amount is required")]
        [Range(1, double.MaxValue, ErrorMessage = "Please enter a valid amount")]
        [DataType(DataType.Currency)]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "The destination account number is required")]
        [Range(0, 9999, ErrorMessage = "Please enter a valid account number")]
        public int DestinationAccountNumber { get; set; }

        [StringLength(255)]
        public string Comment { get; set; }
    }
}