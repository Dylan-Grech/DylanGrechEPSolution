using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class Poll
    {
        public int Id { get; set; }
        public string Name { get; set; }  // Changed "Question" to "Name"
    }
}

