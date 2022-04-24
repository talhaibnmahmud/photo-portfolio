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
    public class CommentsController : Controller
    {
        private readonly PhotoTestContext _context;
        private readonly UserManager<PhotoTestUser> _userManager;

        public CommentsController(PhotoTestContext context, UserManager<PhotoTestUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: CommentController
        public ActionResult Index()
        {
            return View();
        }

        // GET: CommentController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CommentController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CommentController/Create
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

        // GET: CommentController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CommentController/Edit/5
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

        // GET: CommentController/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var comment = await _context.Comments
                .Include(c => c.Post)
                .Include(c => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (comment == null) return NotFound();

            PhotoTestUser currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null || comment.User != currentUser) return Unauthorized();

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            return RedirectToRoute(new { controller = "Posts", action = "Details", comment?.Post?.Id });
        }

        // POST: CommentController/Delete/5
        [Authorize]
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
