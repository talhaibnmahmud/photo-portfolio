#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PhotoTest.Areas.Identity.Data;
using PhotoTest.Data;
using PhotoTest.Models;
using PhotoTest.ViewModels;

namespace PhotoTest.Controllers
{
    public class PostsController : Controller
    {
        private readonly PhotoTestContext _context;
        private readonly UserManager<PhotoTestUser> _userManager;
        private readonly IWebHostEnvironment _hostEnvironment;

        public PostsController(PhotoTestContext context, UserManager<PhotoTestUser> userManager, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _userManager = userManager;
            _hostEnvironment = hostEnvironment;
        }

        // GET: Posts
        public async Task<IActionResult> Index()
        {
            return View(await _context.Post.Include(u => u.User).ToListAsync());
        }

        // GET: Posts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return BadRequest();

            PhotoTestUser currentUser = await _userManager.GetUserAsync(User);

            var post = await _context.Post
                .Include(u => u.User)
                .Include(p => p.Comments)
                .Include(p => p.Favorites)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null) return NotFound();

            var likedPost = await _context.Favorites
                .Include(f => f.Post)
                .Include(f => f.User)
                .AnyAsync(f => f.User == currentUser && f.Post == post);

            var postView = new PostViewModel(post)
            {
                User = currentUser,
                Comments = post.Comments.ToList(),
                Favorites = post.Favorites.ToList(),
                LikedPost = likedPost
            };
            return View(postView);
        }

        // POST: Posts/Details/5
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Details(int? id, [Bind("Text")] PostViewModel model)
        {
            if (id == null) return BadRequest();

            PhotoTestUser photoTestUser = await _userManager.GetUserAsync(User);

            var post = await _context.Post
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null) return NotFound();

            var postView = new PostViewModel(post)
            {
                User = photoTestUser
            };

            Comment newComment = new();
            newComment.CommentMessage = model.Text;
            newComment.Post = post;
            newComment.User = photoTestUser;

            try
            {
                _context.Add(newComment);
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction("Details", new { id });
        }

        // GET: Posts/Create
        [Authorize]
        public IActionResult Create()
        {
            ViewBag.SuccessMessage = "Post created!";
            return View();
        }

        // POST: Posts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Status,ImageFile")] Post post)
        {
            PhotoTestUser photoTestUser = await _userManager.GetUserAsync(User);
            if (photoTestUser == null) return NotFound();

            post.User = photoTestUser;

            string absolutePath = _hostEnvironment.WebRootPath;
            if (post.ImageFile != null)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(post.ImageFile.FileName);
                string fileExtension = Path.GetExtension(post.ImageFile.FileName);
                string filePath = $"{DateTime.UtcNow.Ticks}_{fileName}{fileExtension}";

                post.ImagePath = filePath;
                
                absolutePath = Path.Combine(wwwRootPath + "/uploads/images/" + filePath);
            }

            using (var fileStream = new FileStream(absolutePath, FileMode.Create))
            {
                post.ImageFile.CopyTo(fileStream);
            }

            if (ModelState.IsValid)
            {
                _context.Add(post);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(post);
        }

        // GET: Posts/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return BadRequest();

            var post = await _context.Post.Include(p => p.User).FirstOrDefaultAsync(m => m.Id == id);
            if (post == null) return NotFound();

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null || post.User != currentUser) return Unauthorized();

            return View(post);
        }

        // POST: Posts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Status")] Post post)
        {
            var oldPost = await _context.Post.Include(u => u.User).FirstOrDefaultAsync(o => o.Id == id);
            if (oldPost == null) return NotFound();

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null || oldPost.User != currentUser) return Unauthorized();

            if (post.Status.Trim() == null) return View(post);

            oldPost.Status = post.Status.Trim();
            oldPost.UpdatedDate = DateTime.Now;

            try
            {
                _context.Update(oldPost);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostExists(post.Id)) return NotFound();
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Posts/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();

            var post = await _context.Post
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null) return NotFound();

            PhotoTestUser currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null || post.User != currentUser) return Unauthorized();

            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await _context.Post
                .Include(p => p.User)
                .Include(p => p.Comments)
                .Include(p => p.Favorites)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (post == null) return NotFound();

            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null || post.User != currentUser) return Unauthorized();

            _context.Post.Remove(post);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostExists(int id)
        {
            return _context.Post.Any(e => e.Id == id);
        }
    }
}
