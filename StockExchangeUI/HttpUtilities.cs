using StockExchange.Models;
using StockExchangeUI.Interfaces;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace StockExchangeUI
{
    public class HttpUtilities: IHttpUtilities
    {
        private IHttpClientFactory _clientFactory;
        private string _getAllOrdersUrl;
        private string _getAllTransactionsUrl;
        private string _getCurrentPriceUrl;
        private string _getOrderStatusUrl;
        private string _postNewOrderUrl;

        public HttpUtilities(IHttpClientFactory clientFactory, IOptions<Config> config)
        {
            _clientFactory = clientFactory;
            _getAllOrdersUrl = config.Value.GetAllOrders;
            _getAllTransactionsUrl = config.Value.GetAllTransactions;
            _getCurrentPriceUrl = config.Value.GetCurrentPrice;
            _getOrderStatusUrl = config.Value.GetOrderStatus;
            _postNewOrderUrl = config.Value.PostNewOrder;
        }

        public async Task<IEnumerable<Order>> GetAllOrders()
        {
            var response = await GetCallResponse(_getAllOrdersUrl);
            return await ParseReponse<Order>(response);
        }

        public async Task<IEnumerable<Transaction>> GetAllTransactions()
        {
            var response = await GetCallResponse(_getAllTransactionsUrl);
            return await ParseReponse<Transaction>(response);
        }
        public async Task<int> GetCurrentPrice()
        {
            var response = await GetCallResponse(_getCurrentPriceUrl);
            var result = await response.Content?.ReadAsStringAsync();
            return Int16.Parse(result);
        }

        public async Task<IEnumerable<OrderStatusWithTransactionDetails>> GetOrderStatus(int customerId)
        {
            var response = await GetCallResponse(_getOrderStatusUrl+customerId);
            return await ParseReponse<OrderStatusWithTransactionDetails>(response);
        }

        public async Task<bool> PostNewOrder(Order order)
        {
            var httpContent = new StringContent(JsonConvert.SerializeObject(order).ToString());
            httpContent.Headers.ContentType =
                new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var client = _clientFactory.CreateClient();
            var response = await client.PostAsync(_postNewOrderUrl,
                httpContent);
            return response.IsSuccessStatusCode? true: false;
        }

        private async Task<IEnumerable<T>> ParseReponse<T>(HttpResponseMessage response)
        {
            if(!response.IsSuccessStatusCode)
            {
                //Log error
                return new List<T> ();
            }
            var result = await response.Content?.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<T>>(result);
        }

        private async Task<HttpResponseMessage> GetCallResponse(string url)
        {
            var client = _clientFactory.CreateClient();
            return await client.GetAsync(url);
        }

    }
}
