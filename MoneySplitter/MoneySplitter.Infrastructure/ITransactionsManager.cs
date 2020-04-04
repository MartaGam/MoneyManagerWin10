using MoneySplitter.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoneySplitter.Infrastructure
{
    public interface ITransactionsManager
    {
        Task<bool> LoadUserTransactionsAsync();
        IEnumerable<TransactionModel> UserTransactions { get; }
        Task<bool> AddTransactionAsync(AddTransactionModel addTransactionModel);
        Task<bool> MoveUserToInProgressAsync(int transactionId, double cost);
        Task<bool> MoveUserToFinishedAsync(int transactionId, int userId, double incomingCost);
        Task<ExecutionResult<IEnumerable<TransactionModel>>> GetFriendTransactionsAsync(int friendId);
        Task<bool> ApproveAllTransactionsForFriendAsync(int friendId);
    }
}
