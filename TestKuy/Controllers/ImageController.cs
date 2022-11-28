using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TestKuy.Models;

namespace TestKuy.Controllers
{
    public class ImageController : Controller
    {
        private readonly ImageDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        //Search Bar ,Categories
        [HttpGet]
        public async Task<IActionResult> Index(string search, string Category, string GPU, string Brand, string Submit,
            string priceascending, string pricedescending, string nameascending, string namedescending, string Sortby, string df)
        {
            var query = from x in _context.Images select x;

            //Search Bar Start
            ViewData["SearchForDetails"] = search;
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x => x.ProductName.Contains(search) || x.ProductDetail.Contains(search));
            }
            //Search Bar End

            //Sorting Start
            ViewData["PriceAscend"] = string.IsNullOrEmpty(priceascending) ? "PriceAsc" : "";
            ViewData["PriceDescend"] = string.IsNullOrEmpty(pricedescending) ? "PriceDes" : "";
            ViewData["NameAscend"] = string.IsNullOrEmpty(nameascending) ? "NameAsc" : "";
            ViewData["NameDescend"] = string.IsNullOrEmpty(namedescending) ? "NameDes" : "";
            ViewData["Default"] = string.IsNullOrEmpty(df) ? "DF" : "";
            

            switch (Sortby)
            {
                case "PriceAsc":
                    query = query.OrderByDescending(x => x.ProductPrice);
                    break;
                case "PriceDes":
                    query = query.OrderBy(x => x.ProductPrice);
                    break;
                case "NameAsc":
                    query = query.OrderByDescending(x => x.ProductDetail);
                    break;
                case "NameDes":
                    query = query.OrderBy(x => x.ProductDetail);
                    break;
                case "DF":
                    query = query.OrderBy(x => x.ProductId);
                    break;
            }
            //Sorting End

            //Filter Button Start
            if (!string.IsNullOrEmpty(Category))
            {
                query = query.Where(x => x.ProductName.Contains(Category) || x.ProductDetail.Contains(Category));
            }
            //Filter Button End


            //Filter Checkbox Start
            if (!string.IsNullOrEmpty(Submit) && !string.IsNullOrEmpty(GPU) && !string.IsNullOrEmpty(Brand))
            {
                query = query.Where(x => x.ProductName.Contains(GPU) || x.ProductDetail.Contains(GPU));
                if (!string.IsNullOrEmpty(Brand))
                {
                    query = query.Where(x => x.ProductName.Contains(Brand) || x.ProductDetail.Contains(Brand));
                }
            }
            else if (!string.IsNullOrEmpty(Submit) && !string.IsNullOrEmpty(GPU) && string.IsNullOrEmpty(Brand))
            {
                query = query.Where(x => x.ProductName.Contains(GPU) || x.ProductDetail.Contains(GPU));
            }
            else if (!string.IsNullOrEmpty(Submit) && string.IsNullOrEmpty(GPU) && !string.IsNullOrEmpty(Brand))
            {
                query = query.Where(x => x.ProductName.Contains(Brand) || x.ProductDetail.Contains(Brand));
            }
            //Filter Checkbox End


            return View(await query.AsNoTracking().ToListAsync());
        }


        public ImageController(ImageDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            this._hostEnvironment = hostEnvironment;
        }

        //Slider
        //[HttpGet]
        //[Route("image/search/{min}/{max}")]
        //public IActionResult Search(int min,int max)
        //{
        //    var products = _context.Images.Where(p => p.ProductPrice >= min && p.ProductPrice <= max);
        //    return new JsonResult(products);
        //}

        // GET: Image
        public async Task<IActionResult> Index()
        {
            return View(await _context.Images.ToListAsync());
        }

        // GET: Image/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var imageModel = await _context.Images
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (imageModel == null)
            {
                return NotFound();
            }

            return View(imageModel);
        }

        // GET: Image/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Image/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,ProductName,ProductDetail,ProductPrice,ImageFile")] ImageModel imageModel)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(imageModel.ImageFile.FileName);
                string extension = Path.GetExtension(imageModel.ImageFile.FileName);
                imageModel.ImageName = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwRootPath + "/Image", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await imageModel.ImageFile.CopyToAsync(fileStream);
                }
                _context.Add(imageModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(imageModel);
        }

        // GET: Image/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var imageModel = await _context.Images.FindAsync(id);
            if (imageModel == null)
            {
                return NotFound();
            }
            return View(imageModel);
        }

        // POST: Image/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("ProductId,ProductName,ProductDetail,ProductPrice,ImageName")] ImageModel imageModel)
        {
            if (id != imageModel.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(imageModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ImageModelExists(imageModel.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(imageModel);
        }

        // GET: Image/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var imageModel = await _context.Images
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (imageModel == null)
            {
                return NotFound();
            }

            return View(imageModel);
        }

        // POST: Image/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var imageModel = await _context.Images.FindAsync(id);


            //delete image from wwwroot/image
            var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "image", imageModel.ImageName);
            if (System.IO.File.Exists(imagePath))
                System.IO.File.Delete(imagePath);

            _context.Images.Remove(imageModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ImageModelExists(string id)
        {
            return _context.Images.Any(e => e.ProductId == id);
        }

    }
}
