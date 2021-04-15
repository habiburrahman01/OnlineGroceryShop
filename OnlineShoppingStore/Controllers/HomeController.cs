using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShoppingStore.Data;
using OnlineShoppingStore.Models;
using OnlineShoppingStore.Utility;

namespace OnlineShoppingStore.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public HomeController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            var products = _context.Products.Where(p => p.Quantity > 1).OrderByDescending(p => p.ModifiedDate).ToList().Take(12);
            return View(products);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Products(string sortOrder, string currentFilter, string Category, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["Category"] = Category;
            ViewData["NameSortParm"] = sortOrder == "Name" ? "Name_desc" : "Name";
            ViewData["PriceSortParm"] = sortOrder == "Price" ? "Price_desc" : "Price";
            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;
            var products = _context.Products
                .Include(b => b.Category)
                .Where(s => s.Quantity > 0);
            if (Category != null)
            {
                products = _context.Products
                        .Include(b => b.Category)
                        .Where(s => s.Category.CategoryName == Category);
            }

            if (!String.IsNullOrEmpty(searchString))
            {
                products = products.Where(s => s.ProductName.Contains(searchString)
                || s.Description.Contains(searchString)
                || s.Category.CategoryName.Contains(searchString)
                || s.Price.ToString().Contains(searchString));
            }

            switch (sortOrder)
            {
                case "Name":
                    products = products.OrderBy(s => s.ProductName);
                    ViewData["SortStatus"] = ": Name (A-Z)";
                    ViewData["ChangeSort1"] = "(Z-A)";
                    ViewData["ChangeSort2"] = "(Low-High)";
                    break;
                case "Name_desc":
                    products = products.OrderByDescending(s => s.ProductName);
                    ViewData["SortStatus"] = ": Name (Z-A)";
                    ViewData["ChangeSort1"] = "(A-Z)";
                    ViewData["ChangeSort2"] = "(Low-High)";
                    break;
                case "Price":
                    products = products.OrderBy(s => s.Price);
                    ViewData["SortStatus"] = ": Price (Low-High)";
                    ViewData["ChangeSort2"] = "(High-Low)";
                    ViewData["ChangeSort1"] = "(A-Z)";
                    break;
                case "Price_desc":
                    products = products.OrderByDescending(s => s.Price);
                    ViewData["SortStatus"] = ": Price (High-Low)";
                    ViewData["ChangeSort2"] = "(Low-High)";
                    ViewData["ChangeSort1"] = "(A-Z)";
                    break;
                default:
                    products = products.OrderByDescending(s => s.ModifiedDate);
                    ViewData["ChangeSort1"] = "(A-Z)";
                    ViewData["ChangeSort2"] = "(Low-High)";
                    break;
            }
            int pageSize = 12;
            return View(await PaginatedList<Product>.CreateAsync(products.AsNoTracking(), pageNumber ?? 1, pageSize));
        }

        [AllowAnonymous]
        // GET: Products/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> AddToCart(string id, int quantity, string ReturnUrl)
        {
            var cartItem = new List<CartViewModel>();
            if (id == null)
            {
                return NotFound();
            }
            var product = await _context.Products.FindAsync(id);
            if(product == null)
            {
                return NotFound(); 
            }
            cartItem = HttpContext.Session.Get<List<CartViewModel>>("CartItem");
            if (cartItem == null)
            {
                cartItem = new List<CartViewModel>();
            }

            var cart = new CartViewModel()
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                Image = product.ProductImage,
                Quantity = quantity,
                Description = product.Description,
                Stock = product.Quantity,
                PricePerQuantity = product.Price,
                TotalPrice = quantity * product.Price
            };
                cartItem.Add(cart);
                HttpContext.Session.Set("CartItem", cartItem);
            return LocalRedirect(ReturnUrl);
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult RemoveFromCart(string id, int quantity, string ReturnUrl)
        {
            if (id == null)
            {
                return NotFound();
            }
            var cartItem = HttpContext.Session.Get<List<CartViewModel>>("CartItem");
            if (cartItem == null)
            {
                return NotFound();
            }
            var item = cartItem.Where(i => i.ProductId == id && i.Quantity == quantity).FirstOrDefault();
            cartItem.Remove(item);
            HttpContext.Session.Set("CartItem", cartItem);
            if(ReturnUrl == null)
            {
                return RedirectToAction(nameof(Index));
            }
            return LocalRedirect(ReturnUrl);
        }

        [AllowAnonymous]
        public IActionResult CartItem(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var cartItem = HttpContext.Session.Get<List<CartViewModel>>("CartItem");
            if (cartItem == null)
            {
                return NotFound();
            }
            var item = cartItem.Where(i => i.ProductId == id).FirstOrDefault();
            var model = new CartViewModel()
            {
                ProductId = item.ProductId,
                ProductName = item.ProductName,
                Image = item.Image,
                Description = item.Description,
                Stock = item.Stock,
                PricePerQuantity = item.PricePerQuantity,
                Quantity = item.Quantity,
                TotalPrice = item.TotalPrice
            };
            return View(model);
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> CartItem(string id, int newQuantity, string ReturnUrl)
        {
            if (id == null)
            {
                return NotFound();
            }
             var cartItem = HttpContext.Session.Get<List<CartViewModel>>("CartItem");
            if (cartItem == null)
            {
                return NotFound();
            }
            var item = cartItem.Where(i => i.ProductId == id).FirstOrDefault();
            cartItem.Remove(item);
            HttpContext.Session.Set("CartItem", cartItem);
            await UpdateToCart(id, newQuantity);
            if (ReturnUrl == null)
            {
                return RedirectToAction(nameof(Index));
            }
            return LocalRedirect(ReturnUrl);
        }
        [AllowAnonymous]
        private async Task<string> UpdateToCart(string id, int newQuantity)
        {
            var cartItem = HttpContext.Session.Get<List<CartViewModel>>("CartItem");
            if (cartItem == null)
            {
                cartItem = new List<CartViewModel>();
            }
            var product = await _context.Products.FindAsync(id);
            var cart = new CartViewModel()
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                Image = product.ProductImage,
                Quantity = newQuantity,
                Description = product.Description,
                Stock = product.Quantity,
                PricePerQuantity = product.Price,
                TotalPrice = newQuantity * product.Price
            };
             cartItem.Add(cart);
             HttpContext.Session.Set("CartItem", cartItem);
            return product.ProductId;
        }

        [Authorize]
        public IActionResult CartItems()
        {
            var cartItem = HttpContext.Session.Get<List<CartViewModel>>("CartItem");
            if (cartItem == null)
            {
                return NotFound();
            }
            var model = new List<CartViewModel>();
            model = cartItem;
            return View(model);

        }

        [Authorize]
        public async Task<IActionResult> ConfirmOrder(string status)
        {
            if(status==null)
            {
                return RedirectToAction(nameof(Index));
            }
            var user = await _userManager.GetUserAsync(User);
            var cartItems = HttpContext.Session.Get<List<CartViewModel>>("CartItem");
            if (cartItems == null)
            {
                return RedirectToAction(nameof(Index));
            }
            var model = new ConfirmOrderViewModel()
            {
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                CartItems = cartItems
            };
            return View(model);

        }


        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmOrder(ConfirmOrderViewModel model)
        {
            var cartItems = HttpContext.Session.Get<List<CartViewModel>>("CartItem");
            if (cartItems == null)
            {
                return NotFound();
            }
            model.CartItems = cartItems;
            var order = new Order()
            {
                OrderedBy = _userManager.GetUserId(User),
                OrderedAt = DateTime.Now,
                FullName = model.FullName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                Address = model.Address,
                ShippingCharge = model.ShippingCharge,
                TotalPrice = model.TotalPrice,
                Payment = model.Payment
            };
            order.OrderItems = new List<OrderItem>();
            foreach(var item in model.CartItems)
            {
                var orderItem = new OrderItem()
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    PricePerQuantity = item.PricePerQuantity,
                    TotalPrice = item.TotalPrice
                };
                order.OrderItems.Add(orderItem);
                UpdateQuantity(item.ProductId, item.Quantity);
            }
            if (ModelState.IsValid)
            {

                _context.Add(order);
                await _context.SaveChangesAsync();
                HttpContext.Session.Remove("CartItem");
                return RedirectToAction(nameof(OrderDetails), new {id = order.OrderId });
            }
            return View(model);
        }

        [AllowAnonymous]
        private void UpdateQuantity(string id, int quantity)
        {
                var product = _context.Products.FirstOrDefault(p => p.ProductId == id);
                product.Quantity = product.Quantity - quantity;
                _context.Update(product);
        }

        [Authorize]
        // GET: Orders
        public async Task<IActionResult> Orders()
        {
            var user = _userManager.GetUserId(User);
            return View(await _context.Orders.Where(o => o.OrderedBy == user).ToListAsync());
        }

        [Authorize]
        // GET: Orders/Details/5
        public async Task<IActionResult> OrderDetails(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = _userManager.GetUserId(User);
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                     .ThenInclude(o => o.Product)
                .Include(o => o.Shipping)
                .Include(o => o.Payment)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.OrderId == id && m.OrderedBy == user);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        [AllowAnonymous]
        public IActionResult Contact()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult About()
        {
            return View();
        }
        //[AllowAnonymous]
        //public IActionResult More()
        //{
        //    return View();
        //}

        [AllowAnonymous]
        public IActionResult Privacy()
        {
            return View();
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
