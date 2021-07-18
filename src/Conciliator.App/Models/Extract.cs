using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Conciliator.App.Models
{
    public class Extract : Entity
    {
        [Required(ErrorMessage = "{0} is required.")]
        [DisplayName("Transaction Type")]
        public string TransactionType { get; set; }

        [Required(ErrorMessage = "{0} is required.")]
        [DisplayName("Date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm:ss}")]
        public DateTime DatePosted { get; set; }

        [Required(ErrorMessage = "{0} is required.")]
        [DisplayName("Amount")]
        public decimal TransactionAmount { get; set; }

        [Required(ErrorMessage = "{0} is required.")]
        [DisplayName("Description")]
        public string Memo { get; set; }

        public string Note { get; set; } = "";
    }
}