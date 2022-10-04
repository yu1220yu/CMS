using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CMS.Data;
using CMS.Models;
using Microsoft.AspNetCore.Authorization;

namespace CMS.Controllers
{
    [Authorize]
    public class ProductsController : Controller
    {
        private readonly STEPÑContext _context;

        public ProductsController(STEPÑContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index(string searchCategory, string currentFilter, int? pageNumber)
        {
            //Pagination
            if (searchCategory != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchCategory = currentFilter;
            }
            ViewData["CurrentFilter"] = searchCategory;

            int pageSize = 5;

            //Search
            var result = from model in _context.Product select model;

            if (!string.IsNullOrEmpty(searchCategory))
            {
                result = result.Where(search => search.Category.Name.Contains(searchCategory));
            }

            // ?? null合併運算符
            return View(await PaginationList<Product>.CreateAsync(result.Include(product => product.Category).AsNoTracking(), pageNumber ?? 1, pageSize));
            //return View(await result.Include(product => product.Category).ToListAsync());
        }

        #region Details
        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            DetailViewModel dvm = new DetailViewModel();

            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (product == null)
            {
                return NotFound();
            }
            else
            {
                dvm.Product = product;
            }

            return View(dvm);
        }
        #endregion

        #region Create
        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["Categories"] = new SelectList(_context.Set<Category>(), "Id", "Name");

            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SneakerNum,SneakerImg,Sale,Price,CategoryId")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Categories"] = new SelectList(_context.Set<Category>(), "Id", "Name", product.CategoryId);

            return View(product);
        }
        #endregion

        #region Edit
        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
               .Include(p => p.Category)
               .FirstOrDefaultAsync(m => m.Id == id);
            //var product = await _context.Product.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }
            ViewData["Categories"] = new SelectList(_context.Set<Category>(), "Id", "Name", product.CategoryId);

            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SneakerNum,SneakerImg,Sale,Price,CategoryId")] Product product)
        {
            var prod = new Product();

            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
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
            return View(product);
        }
        #endregion

        #region Delete
        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Product.FindAsync(id);
            _context.Product.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        #endregion

        #region CreateCategory
        public IActionResult CreateCategory()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateCategory(Category category)
        {
            _context.Category.Add(category);
            await _context.SaveChangesAsync();
            return View();
        }
        #endregion

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.Id == id);
        }
    }
}
