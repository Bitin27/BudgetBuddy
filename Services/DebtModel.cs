using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class DebtService
{
    public async Task<List<DebtRecord>> GetAllDebtsAsync()
    {
        return await DebtDatabase.LoadDebtsAsync();
    }

    public async Task AddDebtAsync(DebtRecord debt)
    {
        var debts = await DebtDatabase.LoadDebtsAsync();
        debts.Add(debt);
        await DebtDatabase.SaveDebtsAsync(debts);
    }

    public async Task EditDebtAsync(DebtRecord updatedDebt)
    {
        var debts = await DebtDatabase.LoadDebtsAsync();
        var existingDebt = debts.FirstOrDefault(d => d.Id == updatedDebt.Id);
        if (existingDebt != null)
        {
            existingDebt.Description = updatedDebt.Description;
            existingDebt.Amount = updatedDebt.Amount;
            existingDebt.Type = updatedDebt.Type;
            existingDebt.Category = updatedDebt.Category;
            existingDebt.DueDate = updatedDebt.DueDate;
            existingDebt.IsPaid = updatedDebt.IsPaid;
        }
        await DebtDatabase.SaveDebtsAsync(debts);
    }

    public async Task DeleteDebtAsync(Guid debtId)
    {
        var debts = await DebtDatabase.LoadDebtsAsync();
        debts.RemoveAll(d => d.Id == debtId);
        await DebtDatabase.SaveDebtsAsync(debts);
    }

    public async Task<decimal> GetTotalDebtAsync()
    {
        var debts = await DebtDatabase.LoadDebtsAsync();
        return debts.Where(d => !d.IsPaid).Sum(d => d.Amount);
    }
}
