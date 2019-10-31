using System;

namespace classes
{
    class Program
    {
        static void Main(string[] args)
        {
            BankAccount myAccount = new BankAccount("Lencho Burka", 12.40M);
            Console.WriteLine($"Account {myAccount.AccountNumber} was created for {myAccount.Owner} with {myAccount.Balance} initial balance.");

            myAccount.MakeDeposit(2000, DateTime.Now, "Salary");

            Console.WriteLine($"Account {myAccount.AccountNumber} for {myAccount.Owner} has a current balance of: {myAccount.Balance}");

            myAccount.MakeWithdrawal(450, DateTime.Now, "RENT");

            Console.WriteLine($"Account {myAccount.AccountNumber} for {myAccount.Owner} has a current balance of: {myAccount.Balance}");


            RunTests(myAccount);

            Console.WriteLine(myAccount.getTransactionHistory());
        }

        private static void RunTests(BankAccount testAccount)
        {
            // TEST initial balance must be positive
            try
            {
                BankAccount errorAccount = new BankAccount("Lencho Burka", -12);
            }
            catch (ArgumentOutOfRangeException error)
            {
                Console.WriteLine("Exception caught creating account with negative balance");
                Console.WriteLine(error.ToString());
            }

            // TEST deposit amount must be positive
            try
            {
                testAccount.MakeDeposit(-2000, DateTime.Now, "Salary");
            }
            catch (ArgumentOutOfRangeException error)
            {
                Console.WriteLine("Exception caught depositing negative amount");
                Console.WriteLine(error.ToString());
            }

            // TEST withdrawal amount must be positive
            try
            {
                testAccount.MakeWithdrawal(-2000, DateTime.Now, "Salary");
            }
            catch (ArgumentOutOfRangeException error)
            {
                Console.WriteLine("Exception caught withdrawing negative amount");
                Console.WriteLine(error.ToString());
            }

            // TEST balance cannot be negative
            try
            {
                testAccount.MakeWithdrawal(testAccount.Balance + 2, DateTime.Now, "Salary");
            }
            catch (ArgumentOutOfRangeException error)
            {
                Console.WriteLine("Exception caught trying to overdraw");
                Console.WriteLine(error.ToString());
            }
        }
    }
}
