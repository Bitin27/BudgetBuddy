using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

public class DebtRecord
{
    public Guid Id { get; set; }
    public string Description { get; set; }
    public decimal Amount { get; set; }
    public string Type { get; set; }
    public string Category { get; set; }
    public DateTime DueDate { get; set; }
    public bool IsPaid { get; set; }
}

public static class DebtDatabase
{
    private static readonly string FilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "DebtsDB.json");

    public static async Task<List<DebtRecord>> LoadDebtsAsync()
    {
        if (File.Exists(FilePath))
        {
            var json = await File.ReadAllTextAsync(FilePath);
            return JsonSerializer.Deserialize<List<DebtRecord>>(json) ?? new List<DebtRecord>();
        }
        return new List<DebtRecord>();
    }

    public static async Task SaveDebtsAsync(List<DebtRecord> debts)
    {
        var json = JsonSerializer.Serialize(debts, new JsonSerializerOptions { WriteIndented = true });
        await File.WriteAllTextAsync(FilePath, json);
    }
}
