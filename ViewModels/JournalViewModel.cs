using Journal_Entry.Models;
using Journal_Entry.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Journal_Entry.ViewModels
{
    public class JournalViewModel : INotifyPropertyChanged
    {
        private readonly DatabaseService _db;

        public event PropertyChangedEventHandler PropertyChanged;

        private string _title;
        public string Title
        {
            get => _title;
            set { _title = value; OnPropertyChanged(nameof(Title)); }
        }

        private string _content;
        public string Content
        {
            get => _content;
            set { _content = value; OnPropertyChanged(nameof(Content)); }
        }

        private string _primaryMood;
        public string PrimaryMood
        {
            get => _primaryMood;
            set { _primaryMood = value; OnPropertyChanged(nameof(PrimaryMood)); }
        }

        private string _tags;
        public string Tags
        {
            get => _tags;
            set { _tags = value; OnPropertyChanged(nameof(Tags)); }
        }

        public List<string> MoodList { get; } = new()
        {
            "Happy", "Excited", "Relaxed", "Calm", "Sad", "Stressed"
        };

        public JournalEntry TodayEntry { get; private set; }

        public JournalViewModel(DatabaseService db)
        {
            _db = db;
            LoadTodayEntry();
        }

        public void SaveEntry()
        {
            var entry = new JournalEntry
            {
                EntryDate = DateTime.Today,
                Title = Title,
                Content = Content,
                PrimaryMood = PrimaryMood,
                Tags = Tags,
                CreatedAt = TodayEntry?.CreatedAt ?? DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _db.SaveOrUpdateEntry(entry);

            TodayEntry = entry;
            OnPropertyChanged(nameof(TodayEntry));
        }

        private void LoadTodayEntry()
        {
            TodayEntry = _db.GetTodayEntry();
            if (TodayEntry == null) return;

            Title = TodayEntry.Title;
            Content = TodayEntry.Content;
            PrimaryMood = TodayEntry.PrimaryMood;
            Tags = TodayEntry.Tags;
        }

        private void OnPropertyChanged(string propertyName) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
