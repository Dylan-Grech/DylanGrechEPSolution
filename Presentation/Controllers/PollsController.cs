using Microsoft.AspNetCore.Mvc;
using DataAccess.Repositories;
using Domain.Models;
using Microsoft.Extensions.Logging;

namespace Presentation.Controllers
{
    public class PollsController : Controller
    {
        private readonly PollRepository _pollRepository;
        private readonly ILogger<PollsController> _logger;

        public PollsController(PollRepository pollRepository, ILogger<PollsController> logger)
        {
            _pollRepository = pollRepository;
            _logger = logger;
        }

        public IActionResult Index()
        {
            var list = _pollRepository.GetPolls().OrderByDescending(p => p.DateCreated).ToList();
            return View(list);
        }

        [HttpGet]
        public IActionResult Create() {
            return View(new Poll());
        }

        [HttpPost]
        public IActionResult Create(Poll poll) {

            if (poll.DateCreated == default)
            {
                poll.DateCreated = DateTime.Now; 
            }

            if (ModelState.IsValid)
            {
                _pollRepository.CreatePoll(poll);
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

            return View(poll);
        }

        [HttpPost]
        public IActionResult Edit(Poll poll, string VoteOption)
        {
            if (ModelState.IsValid)
            {
                _logger.LogInformation($"Before Update - Id: {poll.Id}, Title: {poll.Title}, Option 1: {poll.Option1Text} ({poll.Option1VotesCount} votes), Option 2: {poll.Option2Text} ({poll.Option2VotesCount} votes), Option 3: {poll.Option3Text} ({poll.Option3VotesCount} votes), Date Created: {poll.DateCreated.ToString("yyyy-MM-dd HH:mm")}");
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
                _logger.LogInformation($"After Update - Id: {poll.Id}, Title: {poll.Title}, Option 1: {poll.Option1Text} ({poll.Option1VotesCount} votes), Option 2: {poll.Option2Text} ({poll.Option2VotesCount} votes), Option 3: {poll.Option3Text} ({poll.Option3VotesCount} votes), Date Created: {poll.DateCreated.ToString("yyyy-MM-dd HH:mm")}");
                _pollRepository.UpdatePoll(poll);
                _logger.LogInformation($"Poll '{poll.Title}' updated successfully.");
                return RedirectToAction("Index");  
            }

            return View(poll);  
        }

    }
}
