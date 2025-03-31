using Domain.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace DataAccess.Repositories
{
    public class PollFileRepository
    {
        private string _filePath;
        private readonly ILogger<PollFileRepository> _logger;

        public PollFileRepository(string filePath, ILogger<PollFileRepository> logger)
        {
            _filePath = filePath;
        }

        public void CreatePoll(Poll poll)
        {
            try
            {
                List<Poll> polls = ReadPollsFromFile();

                poll.Id = polls.Any() ? polls.Max(p => p.Id) + 1 : 1;

                polls.Add(poll);
                WritePollsToFile(polls);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in CreatePoll: {ex.Message}");
            }
        }

        public List<Poll> GetPolls()
        {
            try
            {
                return ReadPollsFromFile();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return new List<Poll>(); 
            }
        }

        public void UpdatePoll(Poll poll)
        {
            try
            {
                List<Poll> polls = ReadPollsFromFile();
                var existingPoll = polls.FirstOrDefault(p => p.Id == poll.Id);

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

                    WritePollsToFile(polls); 
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

        private List<Poll> ReadPollsFromFile()
        {
            if (!File.Exists(_filePath))
            {
                return new List<Poll>(); 
            }

            try
            {
                var json = File.ReadAllText(_filePath);
                return JsonConvert.DeserializeObject<List<Poll>>(json) ?? new List<Poll>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading from file: {ex.Message}");
                return new List<Poll>();
            }
        }

        private void WritePollsToFile(List<Poll> polls)
        {
            try
            {
                var json = JsonConvert.SerializeObject(polls, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(_filePath, json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing to file: {ex.Message}");
            }
        }
    }
}
