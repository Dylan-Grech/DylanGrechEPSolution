using Microsoft.AspNetCore.Mvc;
using DataAccess.Repositories;
using Domain.Models;
using Microsoft.Extensions.Logging;

namespace Presentation.Controllers
{
    public class PollsFileController : Controller
    {
        private readonly PollFileRepository _pollFileRepository;

        public PollsFileController(PollFileRepository pollFileRepository)
        {
            _pollFileRepository = pollFileRepository;
        }

        public IActionResult Index()
        {
            var list = _pollFileRepository.GetPolls().OrderByDescending(p => p.DateCreated).ToList(); ;
            return View(list);
        }

        [HttpGet]
        public IActionResult Vote(int id)
        {
            var poll = _pollFileRepository.GetPolls().FirstOrDefault(p => p.Id == id);

            if (poll == null)
            {
                return NotFound();
            }

            return View(poll);
        }

        [HttpPost]
        public IActionResult Vote(Poll poll, string VoteOption)
        {
            if (ModelState.IsValid)
            {
                switch (VoteOption)
                {
                    case "1":
                        if (poll.Option1VotesCount != null)
                        {
                            poll.Option1VotesCount += 1;
                        }
                        else
                        {
                            poll.Option1VotesCount = 1;
                        }
                        break;
                    case "2":
                        if (poll.Option2VotesCount != null)
                        {
                            poll.Option2VotesCount += 1;
                        }
                        else
                        {
                            poll.Option2VotesCount = 1;
                        }
                        break;
                    case "3":
                        if (poll.Option3VotesCount != null)
                        {
                            poll.Option3VotesCount += 1;
                        }
                        else
                        {
                            poll.Option3VotesCount = 1;
                        }
                        break;
                    default:
                        break;
                }
                _pollFileRepository.UpdatePoll(poll);
                return RedirectToAction("Index");
            }

            return View(poll);
        }
    }
}
