using Cepela.Eshop.Web.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cepela.Eshop.Web.Models.Database
{
    public static class DatabaseFake
    {
        public static List<CarouselItem> CarouselItems { get; set; }
        public static List<ProductItem> ProductItems { get; set; }

        static DatabaseFake()
        {
            DatabaseInit dbInit = new DatabaseInit();

            CarouselItems = dbInit.GenerateCarouselItems();
            ProductItems = dbInit.GenerateProductItems();
        }
    }
}
