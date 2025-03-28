﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Poll
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Option1Text { get; set; }
        public string Option2Text { get; set; }
        public string Option3Text { get; set; }
        public string Option1VotesCount { get; set; }
        public string Option2VotesCount { get; set; }
        public string Option3VotesCount { get; set; }
        public DateTime DateCreated { get; set; }  // Changed "Question" to "Name"
    }
}

