﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BudgetBuddy.Services
{
    public class TransactionService
    {
        public TransactionService()
        {
        }

        // Get all transactions from the database
        public async Task<List<FinancialTransaction>> GetTransactionsAsync()
        {
            var database = await TransactionDatabase.LoadDatabaseAsync();
            return database.Transactions;
        }

        // Add a new transaction to the database
        public async Task AddTransactionAsync(FinancialTransaction transaction)
        {
            var database = await TransactionDatabase.LoadDatabaseAsync();
            database.Transactions.Add(transaction);
            await TransactionDatabase.SaveDatabaseAsync(database);
        }

        // Calculate total income (inflow)
        public async Task<decimal> GetTotalIncomeAsync()
        {
            var transactions = await GetTransactionsAsync();
            return transactions.Where(t => t.Type == "Income").Sum(t => t.Amount);
        }

        // Calculate total expense (outflow)
        public async Task<decimal> GetTotalExpenseAsync()
        {
            var transactions = await GetTransactionsAsync();
            return transactions.Where(t => t.Type == "Expense").Sum(t => t.Amount);
        }

        // Calculate total debt
        public async Task<decimal> GetTotalDebtAsync()
        {
            var transactions = await GetTransactionsAsync();
            return transactions.Where(t => t.Type == "Debt").Sum(t => t.Amount);
        }

        // Calculate total balance (income + debt - expense)
        public async Task<decimal> GetTotalBalanceAsync()
        {
            var totalIncome = await GetTotalIncomeAsync();
            var totalDebt = await GetTotalDebtAsync();
            var totalExpense = await GetTotalExpenseAsync();
            return totalIncome + totalDebt - totalExpense;
        }
    }
}
