using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caliburn.Micro;
using MoneySplitter.Infrastructure;

namespace MoneySplitter.Win10.ViewModels
{
	public class SettingsViewModel : Screen
	{
		private readonly IMembershipService _membershipService;
		private readonly ITransactionsApiService _transactionsApiService;
		private readonly INavigationManager _navigationManager;

		private double _balance;

		public double Balance
		{
			get => _balance;
			set
			{
				_balance = value;
				NotifyOfPropertyChange(nameof(Balance));
			}
		}

		public SettingsViewModel(
			IMembershipService membershipService,
			ITransactionsApiService transactionsApiService,
			INavigationManager navigationManager)
		{
			_navigationManager = navigationManager;
			_membershipService = membershipService;
			_transactionsApiService = transactionsApiService;
		}

		public async Task ChangeBalanceAsync(double balance)
		{
			var result = await _transactionsApiService.PostBalanceAsync(balance);
			_membershipService.CurrentUser.Ballance = balance;
			_navigationManager.GoBack();
		}

		protected override void OnActivate()
		{
			base.OnActivate();

			Balance = _membershipService.CurrentUser.Ballance;
		}
	}
}
