﻿using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Route("api/home")]
    public class HomeController : Controller
    {
        private ApplicationContext context;
        IHostingEnvironment env;
        public HomeController(ApplicationContext _context, IHostingEnvironment _env)
        {
            context = _context;
            env = _env;
        }

        [HttpGet]
        public IActionResult GetProducts()
        {
            var products = context.Products.Include(x => x.Category).Select(x => new ProductsWithCategory
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                CategoryId = x.CategoryId,
                CategoryName = x.Category.Name
            });
            return Ok(products);
        }

        [HttpGet("{id}")]
        public Product Get(string id)
        {
            var product = context.Products.FirstOrDefault(p => p.Id == id);
            if (product != null)
                return product;

            throw new Exception(new HttpResponseMessage(HttpStatusCode.NotFound).ToString());
        }

        [HttpPut]
        public HttpResponseMessage Put([FromBody] Product product)
        {
            if (product != null)
            {

                context.Products.Update(product);
                context.SaveChanges();
                return new HttpResponseMessage(HttpStatusCode.NoContent);
            }
            return new HttpResponseMessage(HttpStatusCode.BadRequest);
        }

        [HttpDelete("{id}")]
        public HttpResponseMessage Delete(string id)
        {
            var prod = context.Products.FirstOrDefault(x => x.Id == id);
            if (prod != null)
            {
                context.Products.Remove(prod);
                context.SaveChanges();
            }
            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }
    }
}