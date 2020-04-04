using MoneySplitter.Infrastructure;
using MoneySplitter.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoneySplitter.Managers
{
	public class TransactionsManager : ITransactionsManager
	{
		private readonly ITransactionsApiService _transactionsApiService;
		private readonly IMembershipService _membershipService;

		public IEnumerable<TransactionModel> UserTransactions { get; private set; }

		public TransactionsManager(
			ITransactionsApiService transactionsApiService,
			IMembershipService membershipService)
		{
			_transactionsApiService = transactionsApiService;
			_membershipService = membershipService;
		}

		public async Task<bool> LoadUserTransactionsAsync()
		{
			var executionResult = await _transactionsApiService.GetAllUserTransactionsAsync();
			UserTransactions = executionResult.Result;

			return executionResult.IsSuccess;
		}

		public async Task<ExecutionResult<IEnumerable<TransactionModel>>> GetFriendTransactionsAsync(int friendId)
		{
			return await _transactionsApiService.GetAllUserTransactionsAsync(friendId);
		}

		public async Task<bool> AddTransactionAsync(AddTransactionModel addTransactionModel)
		{
			var pushingResult =  await _transactionsApiService.AddTransactionAsync(addTransactionModel);

			var balanceUpdateResult =
				await _transactionsApiService.PostBalanceAsync(
					_membershipService.CurrentUser.Ballance - addTransactionModel.Cost);

			_membershipService.CurrentUser.Ballance -= addTransactionModel.Cost;

			return pushingResult && balanceUpdateResult;
		}

		public async Task<bool> MoveUserToInProgressAsync(int transactionId, double cost)
		{
			var pushingResult = await _transactionsApiService.MoveUserToInProgressAsync(transactionId);

			var balanceUpdateResult =
				await _transactionsApiService.PostBalanceAsync(
					_membershipService.CurrentUser.Ballance - cost);

			_membershipService.CurrentUser.Ballance -= cost;

			return pushingResult && balanceUpdateResult;
		}

		public async Task<bool> MoveUserToFinishedAsync(int transactionId, int userId, double incomingCost)
		{
			var pushingResult = await _transactionsApiService.MoveUserToFineshedAsync(transactionId, userId);

			var balanceUpdateResult =
				await _transactionsApiService.PostBalanceAsync(
					_membershipService.CurrentUser.Ballance + incomingCost);

			_membershipService.CurrentUser.Ballance += incomingCost;

			return pushingResult && balanceUpdateResult;
		}

		public async Task<bool> ApproveAllTransactionsForFriendAsync(int friendId)
		{
			var pushingResult = await _transactionsApiService.ApproveTransactionsForFriendAsync(friendId);

			var allTransactions = UserTransactions.Where(w => w.Owner.Id == _membershipService.CurrentUser.Id)
				.Where(w => w.Collaborators.Any(ww => ww.Id == friendId)).Where(www => !www.FinishedIds.Contains(friendId));

			var totalSum = allTransactions.Sum(w => w.SingleCost);

			var balanceUpdateResult =
				await _transactionsApiService.PostBalanceAsync(
					_membershipService.CurrentUser.Ballance + totalSum);

			_membershipService.CurrentUser.Ballance += totalSum;

			return pushingResult && balanceUpdateResult;
		}
	}
}
