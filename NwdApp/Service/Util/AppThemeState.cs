namespace NwdApp.Service.Util
{
    public class AppThemeState
    {
        public bool IsDarkMode { get; private set; }
        public event Action? OnChange;

        public void SetDarkMode(bool value)
        {
            if (IsDarkMode == value) return;
            IsDarkMode = value;
            OnChange?.Invoke();
        }
    }
}
