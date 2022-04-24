using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PhotoTest.Areas.Identity.Data;
using PhotoTest.Data;
using PhotoTest.Models;

namespace PhotoTest.Controllers
{
    public class FavoritesController : Controller
    {
        private readonly PhotoTestContext _context;
        private readonly UserManager<PhotoTestUser> _userManager;

        public FavoritesController(PhotoTestContext context, UserManager<PhotoTestUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: FavoritesController
        public ActionResult Index()
        {
            return View();
        }

        // GET: FavoritesController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: FavoritesController/Create
        [Authorize]
        public async Task<IActionResult> Create(int id)
        {
            var post = await _context.Post
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null) return NotFound();

            var currentUser = await _userManager.GetUserAsync(User);

            if (!await _context.Favorites.AnyAsync(f => f.Post == post && f.User == currentUser))
            {
                var favorite = new Favorite
                {
                    Post = post,
                    User = currentUser
                };

                post.FavoriteCount++;

                _context.Favorites.Add(favorite);
                _context.Post.Update(post);
                await _context.SaveChangesAsync();
            }

            return RedirectToRoute(new { controller = "Posts", action = "Details", post.Id });
        }

        // POST: FavoritesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: FavoritesController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: FavoritesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: FavoritesController/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var post = await _context.Post
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null) return NotFound();

            var currentUser = await _userManager.GetUserAsync(User);
            var favorite = await _context.Favorites.FirstOrDefaultAsync(f => f.Post == post && f.User == currentUser);

            if (favorite != null)
            {
                post.FavoriteCount--;

                _context.Favorites.Remove(favorite);
                _context.Post.Update(post);
                await _context.SaveChangesAsync();
            }

            return RedirectToRoute(new { controller = "Posts", action = "Details", post.Id });
        }

        // POST: FavoritesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
