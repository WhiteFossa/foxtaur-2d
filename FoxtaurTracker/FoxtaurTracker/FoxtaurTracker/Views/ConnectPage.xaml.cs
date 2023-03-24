namespace FoxtaurTracker.Views;

public partial class ConnectPage : ContentPage
{
	public ConnectPage()
	{
		InitializeComponent();
	}
	
	/// <summary>
	/// Disabling hardware back button
	/// </summary>
	protected override bool OnBackButtonPressed()
	{
		return true;
	}
}