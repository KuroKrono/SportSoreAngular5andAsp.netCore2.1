﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Route("api/category")]
    public class CategoryController : Controller
    {
        private ApplicationContext context;
        IHostingEnvironment env;
        public CategoryController(ApplicationContext _context, IHostingEnvironment _env)
        {
            context = _context;
            env = _env;
        }

        
        [HttpGet]
        public IActionResult GetCategories()
        {
            var categories = context.Categories.ToList();
            return Ok(categories);
        }


        [HttpGet("{id}")]
        public IActionResult GetProductByCategory(string id)
        {
            var category = context.Categories.FirstOrDefault(c => c.Id == id);
            var products = context.Products.Where(p => p.CategoryId == id).Select(x => new ProductsWithCategory()
            {
                Id = x.Id,
                CategoryName = x.Category.Name,
                Name = x.Name,
                Description = x.Description,
                Price = x.Price,
                CategoryId = x.CategoryId
            });
            return Ok(products.ToList());
        }
    }
}