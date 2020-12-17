using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.Language.CodeGeneration;
using Microsoft.EntityFrameworkCore;
using WebQuiz.Areas.User.Models;
using WebQuiz.Data;
using WebQuiz.Data.Models;

namespace WebQuiz.Areas.User.Controllers
{
    [Area("User")]
    public class QuotesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public QuotesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: User/Quotes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Quotes.ToListAsync());
        }

        // GET: User/Quotes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quote = await _context.Quotes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (quote == null)
            {
                return NotFound();
            }

            return View(quote);
        }

        // GET: User/Quotes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: User/Quotes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Text,Author")] Quote quote)
        {
            if (ModelState.IsValid)
            {
                _context.Add(quote);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(quote);
        }

        // GET: User/Quotes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quote = await _context.Quotes.FindAsync(id);
            if (quote == null)
            {
                return NotFound();
            }

            return View(quote);
        }

        // POST: User/Quotes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Text,Author")] Quote quote)
        {
            if (id != quote.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(quote);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuoteExists(quote.Id))
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

            return View(quote);
        }

        // GET: User/Quotes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var quote = await _context.Quotes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (quote == null)
            {
                return NotFound();
            }

            return View(quote);
        }

        // POST: User/Quotes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var quote = await _context.Quotes.FindAsync(id);
            _context.Quotes.Remove(quote);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QuoteExists(int id)
        {
            return _context.Quotes.Any(e => e.Id == id);
        }

        public async Task<IActionResult> RandomQuote()
        {
            var quotes = await _context.Quotes.ToListAsync();

            var userId = _userManager.GetUserId(User);

            var user = await _context.Users
                .Where(x => x.Id == userId)
                .Include(x => x.AnsweredQuotes)
                .SingleOrDefaultAsync();

            if (user == null)
                return NotFound();

            var viewModel = new UserQuoteViewModel { AppUser = user };

            if (quotes.Count < 3)
                return View(viewModel);

            foreach (var quote in quotes)
            {
                if (user.AnsweredQuotes.Any(x => x.Id == quote.Id))
                    continue;

                viewModel.Quote = quote;
                break;
            }

            var rnd = new Random();

            var rndPossibleAnswers = quotes
                .Select(x => x.Author)
                .OrderBy(x => rnd.Next())
                .Take(3)
                .ToList();

            if (viewModel.Quote == null)
            {
                return View(viewModel);
            }

            if (!rndPossibleAnswers.Contains(viewModel.Quote.Author))
            {
                rndPossibleAnswers.RemoveAt(0);
                rndPossibleAnswers.Insert(rnd.Next(0, rndPossibleAnswers.Count - 1), viewModel.Quote.Author);
            }

            viewModel.PossibleAnswers = rndPossibleAnswers.OrderBy(x => Guid.NewGuid()).ToList();
            viewModel.RndIndex = rnd.Next(0, 2);

            return View(viewModel);
        }

        public async Task<IActionResult> ChangeSetting()
        {
            var userInDb = await _userManager.GetUserAsync(User);

            userInDb.Settings = userInDb.Settings == Settings.Multiple ? Settings.Binary : Settings.Multiple;

            await _context.SaveChangesAsync();

            return RedirectToAction("RandomQuote");
        }

        public async Task<IActionResult> AcceptAnswer(string trueAuthor, string chosenAuthor, string binaryAnswer)
        {
            var userId = _userManager.GetUserId(User);

            var user = await _context.Users
                .Where(x => x.Id == userId)
                .Include(x => x.AnsweredQuotes)
                .SingleOrDefaultAsync();

            var quote = await _context.Quotes.FirstAsync(x => x.Author == trueAuthor);

            if (user == null || quote == null)
            {
                return NotFound();
            }

            var quoteToAdd = new QuoteAnswer()
            {
                Author = trueAuthor,
                Text = quote.Text
            };

            if (binaryAnswer == null)
            {
                quoteToAdd.AnswerAuthor = chosenAuthor;
            }
            else
            {
                if (binaryAnswer == "yes")
                    quoteToAdd.AnswerAuthor = chosenAuthor;
                else if (binaryAnswer == "no" && chosenAuthor != trueAuthor)
                {
                    quoteToAdd.AnswerAuthor = trueAuthor;
                }
                else if (binaryAnswer == "no" && chosenAuthor == trueAuthor)
                {
                    quoteToAdd.AnswerAuthor = chosenAuthor;
                }
            }

            user.AnsweredQuotes.Add(quoteToAdd);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}


