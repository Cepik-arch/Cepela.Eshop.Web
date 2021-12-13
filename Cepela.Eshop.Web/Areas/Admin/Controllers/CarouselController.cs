﻿using Cepela.Eshop.Web.Models.Database;
using Cepela.Eshop.Web.Models.Entity;
using Cepela.Eshop.Web.Models.Identity;
using Cepela.Eshop.Web.Models.Implementation;
using Cepela.Eshop.Web.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cepela.Eshop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = nameof(Roles.Admin) + ", " + nameof(Roles.Manager))]
    public class CarouselController : Controller
    {
        readonly EshopDbContext eshopDbContext;
        IWebHostEnvironment env;

        public CarouselController(EshopDbContext eshopDb, IWebHostEnvironment env)
        {
            eshopDbContext = eshopDb;
            this.env = env;
        }

        public IActionResult Select()
        {
            IndexViewModel indexVM = new IndexViewModel();
            indexVM.CarouselItems = eshopDbContext.CarouselItems.ToList();

            return View(indexVM);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CarouselItem carouselItem)
        {

            /*if (DatabaseFake.CarouselItems != null && DatabaseFake.CarouselItems.Count > 0)
            {
                carouselItem.ID = DatabaseFake.CarouselItems.Last().ID + 1;
            }
            */
            if (ModelState.IsValid)
            {
                FileUpload fileUpload = new FileUpload(env.WebRootPath, "img/Carousels", "image");
                if (fileUpload.CheckFileContent(carouselItem.Image) && fileUpload.CheckFileLength(carouselItem.Image))
                {
                    carouselItem.ImageSource = await fileUpload.FileUploadAsync(carouselItem.Image);

                    ModelState.Clear();
                    TryValidateModel(carouselItem);
                    if (ModelState.IsValid)
                    {
                        eshopDbContext.CarouselItems.Add(carouselItem);

                        await eshopDbContext.SaveChangesAsync();

                        return RedirectToAction(nameof(CarouselController.Select));
                    }
                }
            }
            return View(carouselItem);
        }
        public IActionResult Edit(int ID)
        {
            CarouselItem carouselItem = eshopDbContext.CarouselItems.FirstOrDefault(ci => ci.ID == ID);

            if (carouselItem != null)
            {
                return View(carouselItem);
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> Edit(CarouselItem cItem)
        {
            CarouselItem carouselItem = eshopDbContext.CarouselItems.FirstOrDefault(ci => ci.ID == cItem.ID);

            if (carouselItem != null)
            {
                if (cItem.Image != null)
                {
                    FileUpload fileUpload = new FileUpload(env.WebRootPath, "img/Carousels", "image");
                    if (fileUpload.CheckFileContent(cItem.Image) && fileUpload.CheckFileLength(cItem.Image))
                    {
                        cItem.ImageSource = await fileUpload.FileUploadAsync(cItem.Image);
                        carouselItem.ImageSource = cItem.ImageSource;
                    }
                }
                else
                {
                    cItem.ImageSource= "-";
                }


                ModelState.Clear();
                TryValidateModel(cItem);
                if (ModelState.IsValid)
                {


                    carouselItem.ImageAlt = cItem.ImageAlt;

                    await eshopDbContext.SaveChangesAsync();

                    return RedirectToAction(nameof(CarouselController.Select));
                }

            }
            return View(carouselItem);
        }
        
        public async Task<IActionResult> Delete(int ID)
        {
            CarouselItem carouselItem = eshopDbContext.CarouselItems.Where(ci => ci.ID == ID).FirstOrDefault(ci => ci.ID == ID);

            if(carouselItem != null)
            {
                eshopDbContext.CarouselItems.Remove(carouselItem);

                await eshopDbContext.SaveChangesAsync();
            }

            return RedirectToAction(nameof(CarouselController.Select));
        }
    }
}
