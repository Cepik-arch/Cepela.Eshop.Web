using Cepela.Eshop.Web.Models.Database;
using Cepela.Eshop.Web.Models.Entity;
using Cepela.Eshop.Web.Models.ViewModels;
using Cepela.Eshop.Web.Models.Implementation;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Cepela.Eshop.Web.Models.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Cepela.Eshop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = nameof(Roles.Admin) + ", " + nameof(Roles.Manager))]
    public class ProductsController : Controller
    {
        readonly EshopDbContext eshopDbContext;
        IWebHostEnvironment env;

        public ProductsController(EshopDbContext eshopDb, IWebHostEnvironment env)
        {
            eshopDbContext = eshopDb;
            this.env = env;
        }

        public IActionResult Select()
        {
            IndexViewModel indexVM = new IndexViewModel();
            indexVM.ProductItems = eshopDbContext.ProductItems.ToList();

            return View(indexVM);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ProductItem productItem)
        {
            /*if (String.IsNullOrWhiteSpace(productItem.Image) == false &&
                String.IsNullOrWhiteSpace(productItem.Name) == false &&
                String.IsNullOrWhiteSpace(productItem.About) == false)
            {

                if (DatabaseFake.ProductItems != null && DatabaseFake.ProductItems.Count > 0)
                {
                    productItem.ID = DatabaseFake.ProductItems.Last().ID + 1;
                }
                DatabaseFake.ProductItems.Add(productItem);
                return RedirectToAction(nameof(ProductsController.Select));
            }*/

            FileUpload fileUpload = new FileUpload(env.WebRootPath, "img/Products", "image");
            if (fileUpload.CheckFileContent(productItem.Image) && fileUpload.CheckFileLength(productItem.Image))
            {
                productItem.ImageSource = await fileUpload.FileUploadAsync(productItem.Image);
                if (String.IsNullOrWhiteSpace(productItem.ImageSource) == false)
                {
                    eshopDbContext.ProductItems.Add(productItem);

                    await eshopDbContext.SaveChangesAsync();

                    return RedirectToAction(nameof(ProductsController.Select));
                }
            }
            return View(productItem);
        }
    
        public IActionResult Edit(int ID)
        {
            ProductItem productItem  = eshopDbContext.ProductItems.FirstOrDefault(pi => pi.ID == ID);

            if (productItem != null)
            {
                return View(productItem);
        }
            return NotFound();
        }

    [HttpPost]
    public async Task<IActionResult> Edit(ProductItem pItem)
    {
        ProductItem productItem = eshopDbContext.ProductItems.FirstOrDefault(pi => pi.ID == pItem.ID);
        if (productItem != null)
        {
            if (pItem.Image != null)
            {
                FileUpload fileUpload = new FileUpload(env.WebRootPath, "img/Products", "image");
                if (fileUpload.CheckFileContent(pItem.Image) && fileUpload.CheckFileLength(pItem.Image))
                {
                    productItem.ImageSource = await fileUpload.FileUploadAsync(pItem.Image);
                }
            }
                productItem.Name = pItem.Name;
                productItem.About = pItem.About;
                productItem.Price = pItem.Price;
                productItem.Category = pItem.Category;

                await eshopDbContext.SaveChangesAsync();

            return RedirectToAction(nameof(ProductsController.Select));

        }
        return View(productItem);
    }
    public async Task<IActionResult> Delete(int ID)
        {
            ProductItem productItem = eshopDbContext.ProductItems.Where(pi => pi.ID == ID).FirstOrDefault(pi => pi.ID == ID);

            if (productItem != null)
            {
                eshopDbContext.ProductItems.Remove(productItem);
                await eshopDbContext.SaveChangesAsync();
            }

            return RedirectToAction(nameof(ProductsController.Select));
        }
    }
}

