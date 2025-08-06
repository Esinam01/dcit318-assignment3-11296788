using System;
using System.Collections.Generic;

// 1. Transaction record
public record Transaction(int Id, DateTime Date, decimal Amount, string Category);

// 2. Interface
public interface ITransactionProcessor
{
    void Process(Transaction transaction);
}

// 3. Implement processors
public class BankTransferProcessor : ITransactionProcessor
{
    public void Process(Transaction transaction)
    {
        Console.WriteLine($"[BankTransfer] GHS {transaction.Amount} - {transaction.Category}");
    }
}

public class MobileMoneyProcessor : ITransactionProcessor
{
    public void Process(Transaction transaction)
    {
        Console.WriteLine($"[MobileMoney] GHS {transaction.Amount} - {transaction.Category}");
    }
}

public class CryptoWalletProcessor : ITransactionProcessor
{
    public void Process(Transaction transaction)
    {
        Console.WriteLine($"[CryptoWallet] GHS {transaction.Amount} - {transaction.Category}");
    }
}

// 4. Account base class
public class Account
{
    public string AccountNumber { get; }
    public decimal Balance { get; protected set; }

    public Account(string accountNumber, decimal initialBalance)
    {
        AccountNumber = accountNumber;
        Balance = initialBalance;
    }

    public virtual void ApplyTransaction(Transaction transaction)
    {
        Balance -= transaction.Amount;
        Console.WriteLine($"Applied transaction: -GHS {transaction.Amount} → New Balance: GHS {Balance}");
    }
}

// 5. Sealed SavingsAccount class
public sealed class SavingsAccount : Account
{
    public SavingsAccount(string accountNumber, decimal initialBalance)
        : base(accountNumber, initialBalance)
    {
    }

    public override void ApplyTransaction(Transaction transaction)
    {
        if (transaction.Amount > Balance)
        {
            Console.WriteLine("Insufficient funds for transaction.");
        }
        else
        {
            base.ApplyTransaction(transaction);
        }
    }
}

// 6. FinanceApp simulation
public class FinanceApp
{
    private List<Transaction> _transactions = new List<Transaction>();

    public void Run()
    {
        Console.WriteLine("=== FINANCE MANAGEMENT SYSTEM ===\n");

        // Create savings account
        SavingsAccount account = new SavingsAccount("ACC-123", 1000m);

        // Create sample transactions
        Transaction t1 = new Transaction(1, DateTime.Now, 200m, "Groceries");
        Transaction t2 = new Transaction(2, DateTime.Now, 400m, "Utilities");
        Transaction t3 = new Transaction(3, DateTime.Now, 500m, "Entertainment");

        // Processors
        ITransactionProcessor p1 = new MobileMoneyProcessor();
        ITransactionProcessor p2 = new BankTransferProcessor();
        ITransactionProcessor p3 = new CryptoWalletProcessor();

        // Process each
        p1.Process(t1);
        account.ApplyTransaction(t1);

        p2.Process(t2);
        account.ApplyTransaction(t2);

        p3.Process(t3);
        account.ApplyTransaction(t3);

        // Add to list
        _transactions.AddRange(new[] { t1, t2, t3 });

        Console.WriteLine($"\nFinal Account Balance: GHS {account.Balance}");
    }
}

// 7. Program Entry Point
class Program
{
    static void Main()
    {
        FinanceApp app = new FinanceApp();
        app.Run();
        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }
}
