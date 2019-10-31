using System;

namespace classes
{
    public class Transaction
    {
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Note { get; set; }
        public Transaction(decimal amount, DateTime date, string note)
        {
            this.Note = note;
            this.Date = date;
            this.Amount = amount;
        }
    }
}