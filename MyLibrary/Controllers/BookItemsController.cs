using MyLibrary.Data;
using MyLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MyLibrary.Controllers
{
    public class BookItemsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public BookItemsController(ApplicationDbContext context)
        {
            _context = context;
        }
           
        public async Task<IActionResult> Index()
        {
            var _BookItem = await _context.BookItem.ToListAsync();
            if (_BookItem.Count < 1)
                await CreateTestData();

            return _context.BookItem != null ?
                        View(await _context.BookItem.ToListAsync()) :
                        Problem("Набор сущностей 'ApplicationDbContext.BookItem'  = null.");
        }

        public async Task<IActionResult> Details(long? id)
        {
            if (id == null || _context.BookItem == null)
            {
                return NotFound();
            }

            var bookItem = await _context.BookItem
                .FirstOrDefaultAsync(m => m.Id == id);
            if (bookItem == null)
            {
                return NotFound();
            }

            return View(bookItem);
        }

        public IActionResult Create()
        {
            return View();
        }

       [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description")] BookItem bookItem)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bookItem);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(bookItem);
        }

        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null || _context.BookItem == null)
            {
                return NotFound();
            }

            var bookItem = await _context.BookItem.FindAsync(id);
            if (bookItem == null)
            {
                return NotFound();
            }
            return View(bookItem);
        }
                
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long id, [Bind("Id,Name,Description")] BookItem bookItem)
        {
            if (id != bookItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(bookItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookItemExists(bookItem.Id))
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
            return View(bookItem);
        }

        [HttpDelete]
        public async Task<JsonResult> Delete(Int64 id)
        {
            try
            {
                var _BookItems = await _context.BookItem.FindAsync(id);

                if (_BookItems != null)
                {
                    _context.BookItem.Remove(_BookItems);
                }
                await _context.SaveChangesAsync();
                return new JsonResult(_BookItems);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private bool BookItemExists(long id)
        {
            return (_context.BookItem?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private async Task CreateTestData()
        {
            foreach (var item in GetCategoryList())
            {
                _context.BookItem.Add(item);
                await _context.SaveChangesAsync();
            }
        }

        private IEnumerable<BookItem> GetCategoryList()
        {
            return new List<BookItem>
            {
                new BookItem { Name = "Мир смерти", Description="Автор: Гарри Гаррисон" },
                new BookItem { Name = "1984", Description="Автор: Джордж Оруэлл" },
                new BookItem { Name = "Обитаемый остров", Description="Автор: Аркадий и Борис Стругацкие" },
                new BookItem { Name = "Дюна", Description="Автор: Фрэнк Герберт" },
            };
        }

    }
}
