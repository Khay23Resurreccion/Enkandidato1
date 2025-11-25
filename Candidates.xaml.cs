using Microsoft.Maui.Controls;
using Microsoft.Maui.ApplicationModel;
using System.Collections.ObjectModel;
using System.Linq;
using System;
using System.Collections.Generic;

namespace EkandidatoApp
{
    public partial class Candidates : ContentPage
    {
        private ObservableCollection<Candidate> allCandidates;

        public Candidates()
        {
            InitializeComponent();
            LoadPositionTabs();
            LoadCandidates();
            ShowCandidatesByPosition("President");
        }

        private void LoadCandidates()
        {
            allCandidates = new ObservableCollection<Candidate>();

            var allFromRepo = CandidateRepository.President
            .Concat(CandidateRepository.VicePresident)
            .Concat(CandidateRepository.Secretary)
            .Concat(CandidateRepository.Treasurer)
            .Concat(CandidateRepository.PRO)
            .Concat(CandidateRepository.SeniorHighRep)
            .Concat(CandidateRepository.FirstYearRep)
            .Concat(CandidateRepository.SecondYearRep)
            .Concat(CandidateRepository.ThirdYearRep)
            .Concat(CandidateRepository.FourthYearRep)
            .ToList();


            if (!allFromRepo.Any())
            {
                allCandidates.Add(new Candidate
                {
                    Name = "No Candidates Found",
                    Party = "Please add candidates from the AddNewCandidates page.",
                    Motto = string.Empty,
                    Position = "President",
                    Photo = "default_photo.png",
                    Platforms = new List<string>(),
                    Achievements = new List<string>(),
                    CampaignVideoUrl = string.Empty,
                    Email = string.Empty,
                    Phone = string.Empty
                });
            }
            else
            {
                foreach (var c in allFromRepo)
                {
                    allCandidates.Add(new Candidate
                    {
                        Name = c.Name,
                        Party = c.Party,
                        Motto = string.IsNullOrEmpty(c.Slogan) ? "\"Committed to Serve\"" : c.Slogan,
                        Position = c.Party.Contains("President", StringComparison.OrdinalIgnoreCase) ? "President" : c.Party,
                        Photo = string.IsNullOrEmpty(c.PhotoPath) ? "default_photo.png" : c.PhotoPath,
                        Platforms = new List<string> { "Awaiting platform information." },
                        Achievements = new List<string> { "Awaiting achievement records." },
                        CampaignVideoUrl = string.Empty,
                        Email = string.Empty,
                        Phone = string.Empty
                    });
                }
            }
        }

        private void LoadPositionTabs()
        {
            var positions = new[]
            {
                "President", "Vice President", "Secretary", "Treasurer", "PRO",
                "Senior High Representative", "First Year Representative", "Second Year Representative",
                "Third Year Representative", "Fourth Year Representative"
            };


            foreach (var pos in positions)
            {
                var button = new Button
                {
                    Text = pos,
                    FontSize = 14,
                    BackgroundColor = pos == "President" ? Color.FromArgb("#5E936C") : Colors.Transparent,
                    TextColor = pos == "President" ? Colors.White : Color.FromArgb("#666666"),
                    CornerRadius = 20,
                    BorderColor = Color.FromArgb("#E0E0E0"),
                    BorderWidth = 1,
                    Padding = new Thickness(15, 8),
                    CommandParameter = pos
                };
                button.Clicked += OnPositionTabClicked;
                PositionTabsLayout.Children.Add(button);
            }
        }

        private void ShowCandidatesByPosition(string position)
        {
            foreach (var child in PositionTabsLayout.Children.OfType<Button>())
            {
                child.BackgroundColor = (string)child.CommandParameter == position ? Color.FromArgb("#5E936C") : Colors.Transparent;
                child.TextColor = (string)child.CommandParameter == position ? Colors.White : Color.FromArgb("#666666");
            }

            var filtered = allCandidates.Where(c => c.Position == position).ToList();
            CandidatesCollectionView.ItemsSource = new ObservableCollection<Candidate>(filtered);
        }

        private void OnPositionTabClicked(object sender, EventArgs e)
        {
            if (sender is Button btn && btn.CommandParameter is string pos)
                ShowCandidatesByPosition(pos);
        }

        private void OnTabClicked(object sender, EventArgs e)
        {
            if (sender is Button btn && btn.CommandParameter is string tab &&
                btn.BindingContext is Candidate candidate)
            {
                candidate.SetActiveTab(tab);
                // Refresh UI
                CandidatesCollectionView.ItemsSource = null;
                CandidatesCollectionView.ItemsSource = new ObservableCollection<Candidate>(
                    allCandidates.Where(c => c.Position == candidate.Position));
            }
        }

        private async void OnWatchVideoClicked(object sender, EventArgs e)
        {
            if (sender is Button button && button.CommandParameter is string url && !string.IsNullOrEmpty(url))
                await Browser.OpenAsync(url);
        }
    }

    public class Candidate
    {
        public string Name { get; set; }
        public string Party { get; set; }
        public string Motto { get; set; }
        public string Position { get; set; }
        public string Photo { get; set; }
        public List<string> Platforms { get; set; }
        public List<string> Achievements { get; set; }
        public string CampaignVideoUrl { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public bool IsPlatformsVisible { get; set; } = true;
        public bool IsAchievementsVisible { get; set; } = false;
        public bool IsMediaVisible { get; set; } = false;

        public Color ActiveTabColor_Platforms => IsPlatformsVisible ? Color.FromArgb("#1E1E1E") : Color.FromArgb("#EFEFEF");
        public Color ActiveTabTextColor_Platforms => IsPlatformsVisible ? Colors.White : Color.FromArgb("#333333");

        public Color ActiveTabColor_Achievements => IsAchievementsVisible ? Color.FromArgb("#1E1E1E") : Color.FromArgb("#EFEFEF");
        public Color ActiveTabTextColor_Achievements => IsAchievementsVisible ? Colors.White : Color.FromArgb("#333333");

        public Color ActiveTabColor_Media => IsMediaVisible ? Color.FromArgb("#1E1E1E") : Color.FromArgb("#EFEFEF");
        public Color ActiveTabTextColor_Media => IsMediaVisible ? Colors.White : Color.FromArgb("#333333");

        public void SetActiveTab(string tab)
        {
            IsPlatformsVisible = tab == "Platforms";
            IsAchievementsVisible = tab == "Achievements";
            IsMediaVisible = tab == "Media";
        }
    }
}
