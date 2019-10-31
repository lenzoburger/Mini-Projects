using System;
using System.Collections.Generic;
using System.Text;

namespace classes
{
    public class BankAccount
    {
        private static double accountNumberSeed = 1234567898;
        private List<Transaction> transactionHistory = new List<Transaction>();
        public string AccountNumber { get; set; }
        public string Owner { get; set; }
        public decimal Balance
        {
            get
            {
                decimal totalBalance = 0;
                foreach (var trans in transactionHistory)
                {
                    totalBalance += trans.Amount;
                }
                return totalBalance;
            }
        }
        public BankAccount(string owner, decimal initialBalance)
        {
            this.AccountNumber = accountNumberSeed.ToString();
            accountNumberSeed++;

            this.Owner = owner;
            MakeDeposit(initialBalance, DateTime.Now, "Initial Balance");
        }

        public void MakeDeposit(decimal amount, DateTime date, string note)
        {
            if (amount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount), "Amount of deposit must be positive");
            }
            var deposit = new Transaction(amount, date, note);
            transactionHistory.Add(deposit);
        }

        public void MakeWithdrawal(decimal amount, DateTime date, string note)
        {
            if (amount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount), "Amount of withdrawal must be positive");
            }
            if (Balance - amount < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(amount), "Not sufficient funds for this withdrawal");
            }
            var withdrawal = new Transaction(-amount, date, note);
            transactionHistory.Add(withdrawal);
        }

        public string getTransactionHistory()
        {
            var transactionsReport = new StringBuilder();

            transactionsReport.AppendLine($"\n\tAccount Number: {this.AccountNumber}");
            transactionsReport.AppendLine($"Date\t\tAmount\tNote");
            foreach (var item in transactionHistory)
            {
                transactionsReport.AppendLine($"{item.Date.ToShortDateString()}\t${item.Amount}\t{item.Note}");
            }
            return transactionsReport.ToString();
        }
    }
}