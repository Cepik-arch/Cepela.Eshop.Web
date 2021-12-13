using Cepela.Eshop.Web.Models.Database;
using Cepela.Eshop.Web.Models.Entity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Cepela.Eshop.Web.Controllers
{
    public class Product : Controller
    {
        EshopDbContext eshopDbContext;

        public Product(EshopDbContext EshopDbContext)
        {
            eshopDbContext = EshopDbContext;
        }

        public IActionResult Detail(int ID)
        {
            ProductItem produkt = eshopDbContext.ProductItems.FirstOrDefault(pi => pi.ID == ID);
            return View(produkt);
        }
    }
}
