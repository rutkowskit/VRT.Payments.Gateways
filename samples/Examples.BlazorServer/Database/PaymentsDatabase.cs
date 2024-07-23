using Examples.BlazorServer.Database.Entities;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text.Json;

namespace Examples.BlazorServer.Database;

public sealed class PaymentsDatabase : IDisposable
{
    private readonly ReplaySubject<List<Payment>> _paymentSubject = new(1);
    private readonly CompositeDisposable _disposables;
    private sealed class DbData
    {
        public List<Payment> Payment { get; set; } = [];
    }

    private static readonly string DbDir = Path.Combine(Directory.GetCurrentDirectory(), "Db");
    private static readonly string DbPath = Path.Combine(DbDir, "database.json");
    private static readonly SemaphoreSlim WriteSemaphore = new(1, 1);

    private readonly DbData _data;

    public PaymentsDatabase()
    {
        _disposables = [];
        ObservablePayment = _paymentSubject.AsObservable();

        Directory.CreateDirectory(DbDir);
        _data = LoadDatabase();
        NotifyDbDataChanged();
    }
    public List<Payment> Payment => _data.Payment;
    public IObservable<List<Payment>> ObservablePayment { get; }

    public async Task SaveChangesAsync()
    {
        if (WriteSemaphore.Wait(5000) is false)
        {
            throw new ApplicationException("Nie udało się zapisać pliku bazy danych");
        }
        try
        {
            var dbJson = JsonSerializer.Serialize(this);
            await File.WriteAllTextAsync(Path.Combine("Db", "database.json"), dbJson);
            NotifyDbDataChanged();
        }
        finally
        {
            WriteSemaphore.Release();
        }
    }

    private static DbData LoadDatabase()
    {
        if (WriteSemaphore.Wait(5000) is false)
        {
            throw new ApplicationException("Nie udało się otworzyć pliku bazy danych");
        }
        try
        {
            if (File.Exists(DbPath) is false)
            {
                return new DbData();
            }

            var json = File.ReadAllText(DbPath);
            var db = JsonSerializer.Deserialize<DbData>(json);
            return db ?? new();
        }
        finally
        {
            WriteSemaphore.Release();
        }
    }

    public void Dispose()
    {
        _disposables.Dispose();
    }

    private void NotifyDbDataChanged()
    {
        _paymentSubject.OnNext(_data.Payment);
    }
}
