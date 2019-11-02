using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockExchangeUI
{
    public class Config
    {
        public string GetAllOrders { get; set; }
        public string GetCurrentPrice { get; set; }
        public string GetAllTransactions { get; set; }
        public string GetOrderStatus { get; set; }
        public string PostNewOrder { get; set; }

    }
}
