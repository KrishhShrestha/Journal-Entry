using SQLite;
using Journal_Entry.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Journal_Entry.Services
{
    public class DatabaseService
    {
        private readonly SQLiteConnection _db;

        public DatabaseService(string dbPath)
        {
            _db = new SQLiteConnection(dbPath);
            _db.CreateTable<JournalEntry>();
        }

        public JournalEntry GetTodayEntry()
        {
            var today = DateTime.Today;
            return _db.Table<JournalEntry>()
                      .FirstOrDefault(e => e.EntryDate == today);
        }

        public void SaveOrUpdateEntry(JournalEntry entry)
        {
            var existing = GetTodayEntry();

            if (existing == null)
            {
                _db.Insert(entry);
            }
            else
            {
                entry.Id = existing.Id;
                entry.CreatedAt = existing.CreatedAt;
                _db.Update(entry);
            }
        }

        public List<JournalEntry> GetAllEntries()
        {
            return _db.Table<JournalEntry>()
                      .OrderByDescending(e => e.EntryDate)
                      .ToList();
        }
        public void DeleteEntry(int id)
        {
            _db.Delete<JournalEntry>(id);
        }
    }
}
