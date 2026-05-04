using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ControlEquiposElectronicos.ViewModels;

public class BaseViewModel : INotifyPropertyChanged
{
    private bool isBusy;
    private string title = string.Empty;

    public bool IsBusy
    {
        get => isBusy;
        set
        {
            isBusy = value;
            OnPropertyChanged();
        }
    }

    public string Title
    {
        get => title;
        set
        {
            title = value;
            OnPropertyChanged();
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
