using System;
using Windows.UI.Xaml;
using MoneySplitter.Win10.ViewModels;

namespace MoneySplitter.Win10.Views
{
	public sealed partial class SettingsView
	{
		public SettingsViewModel ViewModel { get; private set; }

		public SettingsView()
		{
			InitializeComponent();

			DataContextChanged += OnDataContextChanged;
		}

		private void OnDataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
		{
			ViewModel = DataContext as SettingsViewModel;
		}

		private async void OnChangeButtonClick(object sender, RoutedEventArgs e)
		{
			await ViewModel.ChangeBalanceAsync(Convert.ToDouble(BalanceTextBox.Text));
		}
	}
}
