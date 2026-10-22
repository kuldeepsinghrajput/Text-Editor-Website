using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using test_project.Data;

namespace test_project.Controllers
{
    [Authorize]
    public class SavedTextsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public SavedTextsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var texts =await _context.SavedTexts.Where(t => t.UserId == userId).OrderByDescending(t => t.CreatedAt).ToListAsync();


            return View(texts);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Content")]Models.SavedText savedText)
        {
            var userId = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(userId))
            {
                return Challenge();
            }

            // UserId is assigned from the authenticated user, not from form input.
            ModelState.Remove(nameof(Models.SavedText.UserId));

            if (string.IsNullOrWhiteSpace(savedText.Title))
            {
                ModelState.AddModelError(nameof(savedText.Title), "Title is required.");
            }

            if (string.IsNullOrWhiteSpace(savedText.Content))
            {
                ModelState.AddModelError(nameof(savedText.Content), "Content is required.");
            }

            if (!ModelState.IsValid)
            {
                return View(savedText);
            }

            savedText.Title = savedText.Title.Trim();
            savedText.Content = savedText.Content.Trim();
            savedText.UserId = userId;
            savedText.CreatedAt = DateTime.Now;
            _context.Add(savedText);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Details(int id)
        {
            var userId = _userManager.GetUserId(User);
            var savedText = await _context.SavedTexts.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
            if (savedText == null)
            {
                return NotFound();
            }
            return View(savedText);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

 
        public async Task<IActionResult> UpdateField(int id, string field, string value)
        {
            var userId = _userManager.GetUserId(User);

            var existingText = await _context.SavedTexts
                .FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);

            if (existingText == null)
            {
                return NotFound();
            }

            if (field == "Title")
            {
                value = value.Trim();

                if (string.IsNullOrWhiteSpace(value))
                {
                    return BadRequest("Title cannot be empty.");
                }

                if (value.Length > 100)
                {
                    return BadRequest("Title cannot exceed 100 characters.");
                }

                existingText.Title = value;
            }
            else if (field == "Content")
            {
                existingText.Content = value;
            }
            else
            {
                return BadRequest("Invalid field.");
            }

            await _context.SaveChangesAsync();

            return Ok();
        }

    }
}
