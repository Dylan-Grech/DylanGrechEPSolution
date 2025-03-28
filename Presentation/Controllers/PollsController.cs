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

        [HttpGet]
        public IActionResult Create() {
            return View(new Poll());
        }

        [HttpPost]
        public IActionResult Create(Poll poll) {

            _logger.LogInformation("Received poll data: Id = {Id}, Title = {Title}, Option1Text = {Option1Text}, Option2Text = {Option2Text}, Option3Text = {Option3Text}, Option1VotesCount = {Option1VotesCount}, Option2VotesCount = {Option2VotesCount}, Option3VotesCount = {Option3VotesCount}",
                poll.Id, poll.Title, poll.Option1Text, poll.Option2Text, poll.Option3Text, poll.Option1VotesCount, poll.Option2VotesCount, poll.Option3VotesCount);


            if (poll.DateCreated == default)
            {
                poll.DateCreated = DateTime.Now;  // Set the current date and time if not set
                _logger.LogInformation("Date: date = {DateCreated}", poll.DateCreated);
            }

            if (ModelState.IsValid)
            {
                _logger.LogInformation("1");
                _pollRepository.CreatePoll(poll);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                _logger.LogInformation("2");
            }

            return View(poll);
        }
    }
}
