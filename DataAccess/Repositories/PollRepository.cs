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
        private readonly PollDbContext _context;

        public PollRepository(PollDbContext context)
        {
            _context = context;
        }

        // Retrieve all polls from the database
        public async Task<List<Poll>> GetAllPollsAsync()
        {
            return await _context.Polls.ToListAsync();
        }

        // Retrieve a poll by its ID
        public async Task<Poll> GetPollByIdAsync(int id)
        {
            return await _context.Polls.FirstOrDefaultAsync(p => p.Id == id);
        }

        // Add a new poll to the database
        public async Task AddPollAsync(Poll poll)
        {
            _context.Polls.Add(poll);
            await _context.SaveChangesAsync();
        }

        // Update an existing poll
        public async Task UpdatePollAsync(Poll poll)
        {
            _context.Polls.Update(poll);
            await _context.SaveChangesAsync();
        }

        // Delete a poll by ID
        public async Task DeletePollAsync(int id)
        {
            var poll = await _context.Polls.FindAsync(id);
            if (poll != null)
            {
                _context.Polls.Remove(poll);
                await _context.SaveChangesAsync();
            }
        }
    }
}
