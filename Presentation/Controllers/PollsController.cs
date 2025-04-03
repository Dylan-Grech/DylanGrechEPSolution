using Microsoft.AspNetCore.Mvc;
using DataAccess.Repositories;
using Domain.Models;
using Microsoft.Extensions.Logging;

namespace Presentation.Controllers
{
    public class PollsController : Controller
    {
        private readonly PollRepository _pollRepository;
        private readonly PollFileRepository _pollFileRepository;
        private readonly ILogger<PollsController> _logger;

        public PollsController(PollRepository pollRepository, PollFileRepository pollFileRepository, ILogger<PollsController> logger)
        {
            _pollRepository = pollRepository;
            _pollFileRepository = pollFileRepository;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var list = _pollRepository.GetPolls().OrderByDescending(p => p.DateCreated).ToList();
            return View(list);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new Poll());
        }

        [HttpPost]
        public IActionResult Create(Poll poll, string StorageOption)
        {

            if (poll.DateCreated == default)
            {
                poll.DateCreated = DateTime.Now;
            }

            if (ModelState.IsValid)
            {
                if (StorageOption == "File")
                {
                    _pollFileRepository.CreatePoll(poll);
                }
                else
                {
                    _pollRepository.CreatePoll(poll);
                }
                return RedirectToAction("Index", "Home");
            }

            return View(poll);
        }

        [HttpGet]
        public IActionResult Vote(int id)
        {
            var poll = _pollRepository.GetPolls().FirstOrDefault(p => p.Id == id);

            if (poll == null)
            {
                return NotFound();
            }

            var votedPolls = HttpContext.Session.GetString("VotedPolls");
            if (votedPolls != null && votedPolls.Split(',').Contains(id.ToString()))
            {
                TempData["ErrorMessage"] = "You have already voted on this poll.";
                return RedirectToAction("Index");
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

                _pollRepository.UpdatePoll(poll);

                var votedPolls = HttpContext.Session.GetString("VotedPolls");
                if (string.IsNullOrEmpty(votedPolls))
                {
                    votedPolls = poll.Id.ToString();
                }
                else
                {
                    votedPolls = $"{votedPolls},{poll.Id}";
                }

                HttpContext.Session.SetString("VotedPolls", votedPolls);

                return RedirectToAction("Index");
            }

            return View(poll);
        }
    }
}
