using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StockExchangeUI.Models;
using System.Net.Http;
using StockExchange.Models;
using StockExchangeUI.Interfaces;
using Microsoft.Extensions.Options;

namespace StockExchangeUI.Controllers
{
    public class HomeController : Controller
    {
        private IHttpUtilities _httpUtilities;

        public HomeController(IHttpUtilities httpUtilities)
        {
            _httpUtilities = httpUtilities;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ErrorOccured()
        {
            return View();
        }

        public async Task<IActionResult> GetOrderBook()
        {
            var order = await _httpUtilities.GetAllOrders();
            return View("~/Views/Home/OrderBook.cshtml", order.OrderByDescending(t => t.Timestamp));
        }

        public async Task<IActionResult> GetAllTransactions()
        {
            var transactions = await _httpUtilities.GetAllTransactions();
            return View("~/Views/Home/Transactions.cshtml", transactions.OrderByDescending(t => t.Timestamp));
        }

        public async Task<IActionResult> BuySellStock()
        {
            var currentPrice = await _httpUtilities.GetCurrentPrice();
            var orderWithCurrentPrice = new OrderWithCurrentPrice() { CurrentPrice = currentPrice, Order = new Order() };
            return View("~/Views/Home/BuySell.cshtml", orderWithCurrentPrice);
        }

        public IActionResult OrderStatus()
        {
            return View("~/Views/Home/OrderStatusRequest.cshtml");
        }

        
        [HttpPost]
        public async Task<IActionResult> OrderStatus(int customerId)
        {
            var orderStatus = await _httpUtilities.GetOrderStatus(customerId);
            return View("~/Views/Home/OrderStatus.cshtml", orderStatus);
        }

        [HttpPost]
        public async Task<IActionResult> SendOrder([Bind("Order")] OrderWithCurrentPrice orderWithPrice)
        {
            var order = orderWithPrice.Order;
            if (!ModelState.IsValid)
                return StatusCode(500, order);

            order.OrderId = new Random().Next(999_999_999);
            order.Timestamp = DateTime.UtcNow;
            var sentSuccessfully = await _httpUtilities
                .PostNewOrder(order);
            if (sentSuccessfully)
                return RedirectToAction("Index");
            else
                return RedirectToAction("ErrorOccured");
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
