public class AuthState
{
    public bool IsAuthenticated { get; private set; }
    public string CurrentCurrency { get; private set; }

    public event Action OnChange;

    public void Login(string currency)
    {
        IsAuthenticated = true;
        CurrentCurrency = currency;
        NotifyStateChanged();
    }

    public void Logout()
    {
        IsAuthenticated = false;
        CurrentCurrency = null;
        NotifyStateChanged();
    }

    private void NotifyStateChanged() => OnChange?.Invoke();
}
