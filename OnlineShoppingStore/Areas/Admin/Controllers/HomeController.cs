using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShoppingStore.Data;
using OnlineShoppingStore.Models;

namespace OnlineShoppingStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public  HomeController(ApplicationDbContext context)
        {
            _context = context;
        }


        public IActionResult Index()
        {
            ViewData["TotalProducts"] = _context.Products.Count(); 
            ViewData["TotalOrders"] = _context.Orders.Count();

            ViewData["MCollection"] = _context.Products.Where(p => p.CategoryId == 1).Count();
            ViewData["WCollection"] = _context.Products.Where(p => p.CategoryId == 2).Count();
            ViewData["KCollection"] = _context.Products.Where(p => p.CategoryId == 3).Count();

            ViewData["Pending"] = _context.Orders.Where(o => o.Status == OrderStatus.Pending).Count();
            ViewData["Completed"] = _context.Orders.Where(o => o.Status == OrderStatus.Completed).Count();
            ViewData["Cancel"] = _context.Orders.Where(o => o.Status == OrderStatus.Cancel).Count();
            ViewData["Shipping"] = _context.Orders.Where(o => o.Status == OrderStatus.Shipping).Count();
            var orders = _context.Orders.ToList();
            if (orders.Count() != 0)
            {
                ViewData["UpdateTime"] = _context.Orders.FirstOrDefault().OrderedAt;
                orders = _context.Orders.OrderByDescending(o => o.OrderedAt).Take(10).ToList();
                return View(orders);
            }
            ViewData["UpdateTime"] = DateTime.Now;
            return View(orders);
        }
    }
}