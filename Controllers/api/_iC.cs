using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace WebApp.api
{
    [Route("api/item")]
    [ApiController]
    public class ItmController : Controller
    {
        private ISi _SItems;

        public ItmController(ISi sProducts)
        {
            _SItems = sProducts;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var p = await _SItems?.ReadA();
            return View(p);
        }

        [HttpGet]
        public ViewResult Add() { return View(); }

        [HttpPost]
        [ValidateAntiForgeryToken] // Validate the anti-forgery token for security
        // POST: http://localhost:[port]/products/create
        // Handle product creation form submission
        public async Task<IActionResult> Add([Bind("Id,Name,Price,SalePerc,Description")] Itm product, IFormFile? imageFile)
        {
            if (product.SalePerc != 0)
            {
                product.OldPrice = product.Price;
                product.Price = (product.OldPrice * (decimal)product.SalePerc) / 100;
            }
            else
            {
                if (product.OldPrice != null) { product.Price = product.OldPrice; }
                else { product.OldPrice = product.Price; }
            }
            product.IsAvaliable = true;
            ModelState.MarkFieldValid("ImgFormat");
            ModelState.MarkFieldValid("Image");
            if (ModelState.IsValid)
            {
                if (imageFile != null)
                {
                    using (var s = imageFile.OpenReadStream())
                    {
                        using var ms = new MemoryStream();
                        await s.CopyToAsync(ms);
                        product.Image = ms.ToArray();
                        product.ImgFormat = imageFile.ContentType;
                    }
                    await _SItems.UpdateA(product.Id, product);
                    return RedirectToAction(nameof(Index));
                }
                product.ImgFormat = "none";
                await _SItems.UpdateA(product.Id, product);
                return RedirectToAction(nameof(Index));
            }

            return View(product);
        }

        [HttpGet]
        public async Task<IActionResult> Upd(int id)
        {
            var p = await _SItems.GetByIdA(id);
            if (p == null) return NotFound();
            return View(p);
        }

        private decimal CalcNum(decimal[] inp)
        {
            var v1 = inp[1] / 100;
            return (inp[0] * (1-v1))-1;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        // POST: http://localhost:[port]/products/create
        public async Task<IActionResult> Upd([Bind("Id,Name,Price,SalePerc,Description")] Itm product, IFormFile? imageFile)
        {
            if (product.SalePerc != 0) {
                product.OldPrice = product.Price;
                product.Price = CalcNum(new decimal[] { product.OldPrice, Convert.ToDecimal(product.SalePerc) }) ;
                //product.Price = product.OldPrice * (product.SalePerc / (decimal)100.00);
            }
            else {
                if (product.OldPrice == 0) { product.OldPrice = product.Price; }
                else { product.Price = product.OldPrice; }
            }

            //product.IsAvaliable = true;
            product.Price = Convert.ToDecimal(product.Price);
            ModelState.MarkFieldValid("ImgFormat");
            ModelState.MarkFieldValid("Image");
            if (ModelState.IsValid || imageFile == null)
            {
                if (imageFile != null)
                {
                    using (var s = imageFile.OpenReadStream())
                    {
                        using var ms = new MemoryStream();
                        await s.CopyToAsync(ms);
                        product.Image = ms.ToArray();
                        product.ImgFormat = imageFile.ContentType;
                    }
                    await _SItems.UpdateA(product.Id, product);
                    return RedirectToAction(nameof(Index));
                }
                await _SItems.UpdateA(product.Id, product);
                return RedirectToAction(nameof(Index));
            }

            return View(product);
        }

        [HttpGet]
        public async Task<IActionResult> Det(int id)
        {
            var p = await _SItems.GetByIdA(id);
            if (p == null) return NotFound();
            return View(p);
        }

        [HttpGet]
        public async Task<IActionResult> Del(int id)
        {
            var p = await _SItems.GetByIdA(id);
            if (p == null) return NotFound();
            return View(p);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        // POST: http://localhost:[port]/products/update
        // Handle product creation form submission
        public async Task<IActionResult> Del(int id, [Bind("Id,Name,Price,Description")] Itm p)
        {
            p.Price = Convert.ToDecimal(p.Price);
            if (ModelState.IsValid) // Check if the form data is valid
            {
                await _SItems.DeleteA(id); // Create the product asynchronously
                return RedirectToAction(nameof(Index)); // Redirect to the product list
            }
            return NotFound(); // If validation fails, return to the form with the entered data
        }
    }
}
