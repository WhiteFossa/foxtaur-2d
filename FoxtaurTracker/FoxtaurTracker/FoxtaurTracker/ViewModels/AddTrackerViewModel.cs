using System.ComponentModel;

namespace FoxtaurTracker.ViewModels;

public class AddTrackerViewModel : IQueryAttributable, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;
    
    public void RaisePropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
    }
}