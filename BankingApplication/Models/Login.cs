using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankingApplication.Models {
    public class Login {
        [Key, DatabaseGenerated (DatabaseGeneratedOption.None), Required, StringLength (50)]
        public string UserID { get; set; }

        [Required, StringLength (4)]
        public int CustomerID { get; set; }
        public Customer Customer { get; set; }

        [Required, StringLength (64)]
        public string Password { get; set; }

        [Required, StringLength (15)]
        public DateTime ModifyDate { get; set; }
    }
}