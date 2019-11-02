using StockExchange.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace StockExchangeUI.Interfaces
{
    public interface IHttpUtilities
    {
        Task<IEnumerable<Order>> GetAllOrders();
        Task<IEnumerable<Transaction>> GetAllTransactions();
        Task<int> GetCurrentPrice();
        Task<IEnumerable<OrderStatusWithTransactionDetails>> GetOrderStatus(int customerId);
        Task<bool> PostNewOrder(Order order);
    }
}
