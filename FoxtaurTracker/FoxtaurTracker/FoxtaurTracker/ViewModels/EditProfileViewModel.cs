using System.ComponentModel;

namespace FoxtaurTracker.ViewModels;

public class EditProfileViewModel : IQueryAttributable, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;
    
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        
    }

    public void RaisePropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}