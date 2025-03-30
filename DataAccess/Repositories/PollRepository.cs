using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using Microsoft.EntityFrameworkCore;


namespace DataAccess.Repositories
{
    public class PollRepository
    {
        private PollDbContext _context;

        public PollRepository(PollDbContext context)
        {
            _context = context;
        }

        public void CreatePoll(Poll poll)
        {
            try
            {
                _context.Polls.Add(poll);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        public IQueryable<Poll> GetPolls()
        {
            return _context.Polls;
        }

        public void UpdatePoll(Poll poll)
        {
            try
            {
                var existingPoll = _context.Polls.FirstOrDefault(p => p.Id == poll.Id);

                if (existingPoll != null)
                {
                    existingPoll.Title = poll.Title;
                    existingPoll.Option1Text = poll.Option1Text;
                    existingPoll.Option2Text = poll.Option2Text;
                    existingPoll.Option3Text = poll.Option3Text;
                    existingPoll.Option1VotesCount = poll.Option1VotesCount;
                    existingPoll.Option2VotesCount = poll.Option2VotesCount;
                    existingPoll.Option3VotesCount = poll.Option3VotesCount;
                    existingPoll.DateCreated = poll.DateCreated;

                    _context.SaveChanges();
                }
                else
                {
                    Console.WriteLine("Poll not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

    }
}
