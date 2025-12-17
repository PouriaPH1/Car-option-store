using System;
using System.Collections.Generic;
using System.Linq;
using _0_Framework.Application;
using _01_LampshadeQuery.Contracts.Product;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Nancy.Json;
using ShopManagement.Application.Contracts.Order;

namespace ServiceHost.Pages
{
    public class CartModel : PageModel
    {
        public List<CartItem> CartItems;
        public const string CookieName = "cart-items";
        private readonly IProductQuery _productQuery;

        public CartModel(IProductQuery productQuery)
        {
            CartItems = new List<CartItem>();
            _productQuery = productQuery;
        }

        public void OnGet()
        {
            var serializer = new JavaScriptSerializer();
            var value = Request.Cookies[CookieName];
            
            if (string.IsNullOrEmpty(value))
            {
                CartItems = new List<CartItem>();
                return;
            }
            
            var cartItems = serializer.Deserialize<List<CartItem>>(value);
            if (cartItems == null || !cartItems.Any())
            {
                CartItems = new List<CartItem>();
                return;
            }
            
            foreach (var item in cartItems)
                item.CalculateTotalItemPrice();

            CartItems = _productQuery.CheckInventoryStatus(cartItems);
        }

        public IActionResult OnGetRemoveFromCart(long id)
        {
            var serializer = new JavaScriptSerializer();
            var value = Request.Cookies[CookieName];
            Response.Cookies.Delete(CookieName);
            
            if (string.IsNullOrEmpty(value))
                return RedirectToPage("/Cart");
                
            var cartItems = serializer.Deserialize<List<CartItem>>(value);
            if (cartItems == null)
                return RedirectToPage("/Cart");
                
            var itemToRemove = cartItems.FirstOrDefault(x => x.Id == id);
            if (itemToRemove != null)
                cartItems.Remove(itemToRemove);
                
            var options = new CookieOptions {Expires = DateTime.Now.AddDays(2)};
            Response.Cookies.Append(CookieName, serializer.Serialize(cartItems), options);
            return RedirectToPage("/Cart");
        }

        public IActionResult OnGetGoToCheckOut()
        {
            var serializer = new JavaScriptSerializer();
            var value = Request.Cookies[CookieName];
            
            if (string.IsNullOrEmpty(value))
                return RedirectToPage("/Cart");
                
            var cartItems = serializer.Deserialize<List<CartItem>>(value);
            if (cartItems == null || !cartItems.Any())
                return RedirectToPage("/Cart");
                
            foreach (var item in cartItems)
            {
                item.TotalItemPrice = item.UnitPrice * item.Count;
            }

            CartItems = _productQuery.CheckInventoryStatus(cartItems);

            return RedirectToPage(CartItems.Any(x => !x.IsInStock) ? "/Cart" : "/Checkout");
        }
    }
}