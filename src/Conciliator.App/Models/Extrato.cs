using System;

namespace Conciliator.App.Models
{
    public class Extrato : Entity
    {
        public string TransactionType { get; set; }
        public DateTime DatePosted { get; set; }
        public decimal TransactionAmount { get; set; }
        public string Memo { get; set; }
    }
}