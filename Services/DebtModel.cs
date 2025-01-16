public class DebtService
{
    // Fetch all debts
    public async Task<List<DebtRecord>> GetAllDebtsAsync()
    {
        return await DebtDatabase.LoadDebtsAsync();
    }

    // Fetch only unpaid debts
    public async Task<List<DebtRecord>> GetUnpaidDebtsAsync()
    {
        var debts = await DebtDatabase.LoadDebtsAsync();
        return debts.Where(d => !d.IsPaid).ToList();
    }

    public async Task<List<DebtRecord>> GetpaidDebtsAsync()
    {
        var debts = await DebtDatabase.LoadDebtsAsync();
        return debts.Where(d => d.IsPaid).ToList();
    }
    // Add a new debt
    public async Task AddDebtAsync(DebtRecord debt)
    {
        var debts = await DebtDatabase.LoadDebtsAsync();
        debt.Id = Guid.NewGuid(); // Ensure unique ID for the new debt
        debts.Add(debt);
        await DebtDatabase.SaveDebtsAsync(debts);
    }

    // Edit an existing debt
    public async Task EditDebtAsync(DebtRecord updatedDebt)
    {
        var debts = await DebtDatabase.LoadDebtsAsync();
        var existingDebt = debts.FirstOrDefault(d => d.Id == updatedDebt.Id);
        if (existingDebt != null)
        {
            // Ensure the debt exists and update each field
            existingDebt.Description = updatedDebt.Description;
            existingDebt.Amount = updatedDebt.Amount;
            existingDebt.Type = updatedDebt.Type;
            existingDebt.Category = updatedDebt.Category;
            existingDebt.DueDate = updatedDebt.DueDate;
            existingDebt.IsPaid = updatedDebt.IsPaid; // Ensure the IsPaid status is updated

            // After updating the debt, save the changes back to the database
            await DebtDatabase.SaveDebtsAsync(debts);
        }
    }

    // Delete a debt by ID
    public async Task DeleteDebtAsync(Guid debtId)
    {
        var debts = await DebtDatabase.LoadDebtsAsync();
        debts.RemoveAll(d => d.Id == debtId);
        await DebtDatabase.SaveDebtsAsync(debts);
    }

    // Get total unpaid debt amount
    public async Task<decimal> GetTotalDebtAsync()
    {
        var debts = await DebtDatabase.LoadDebtsAsync();
        return debts.Where(d => !d.IsPaid).Sum(d => d.Amount);
    }

    // Get total paid debt amount
    public async Task<decimal> GetTotalPaidDebtAsync()
    {
        var debts = await DebtDatabase.LoadDebtsAsync();
        return debts.Where(d => d.IsPaid).Sum(d => d.Amount);
    }

    // Filter debts based on dashboard criteria
    public async Task<List<DebtRecord>> GetFilteredDebtsAsync(string description, string type, string category, DateTime? startDate, DateTime? endDate)
    {
        var debts = await DebtDatabase.LoadDebtsAsync();

        return debts.Where(d =>
            (string.IsNullOrEmpty(description) || d.Description.Contains(description, StringComparison.OrdinalIgnoreCase)) &&
            (string.IsNullOrEmpty(type) || d.Type == type) &&
            (string.IsNullOrEmpty(category) || d.Category.Contains(category, StringComparison.OrdinalIgnoreCase)) &&
            (!startDate.HasValue || d.DueDate >= startDate.Value) &&
            (!endDate.HasValue || d.DueDate <= endDate.Value)
        ).ToList();
    }

    // Get debt details by ID (useful for editing)
    public async Task<DebtRecord?> GetDebtByIdAsync(Guid debtId)
    {
        var debts = await DebtDatabase.LoadDebtsAsync();
        return debts.FirstOrDefault(d => d.Id == debtId);
    }
}
