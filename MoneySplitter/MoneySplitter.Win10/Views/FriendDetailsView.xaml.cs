using Windows.UI.Xaml;
using MoneySplitter.Win10.ViewModels;

namespace MoneySplitter.Win10.Views
{
	public sealed partial class FriendDetailsView
	{
		FriendDetailsViewModel ViewModel { get; set; }
		public FriendDetailsView()
		{
			InitializeComponent();
			DataContextChanged += (s, e) => { ViewModel = DataContext as FriendDetailsViewModel; };
		}

		private async void OnApproveAllButtonClick(object sender, RoutedEventArgs e)
		{
			await ViewModel.ApproveAllAsync();
		}
	}
}
