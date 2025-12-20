
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Journal_Entry.Models
{
    public class JournalEntry
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public DateTime EntryDate { get; set; }   // One entry per day

        public string Title { get; set; }
        public string Content { get; set; }

        public string PrimaryMood { get; set; }

        public string Tags { get; set; }           // Comma-separated

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
