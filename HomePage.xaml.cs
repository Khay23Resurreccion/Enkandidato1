using Microsoft.Maui.Controls;
using System;
using Microsoft.Maui.Dispatching; // Required for DispatcherPriority
using System.Threading.Tasks;

namespace EkandidatoApp
{
    public partial class HomePage : ContentPage
    {
        #region Private Fields

        private DateTime _electionEndDate = DateTime.Now.AddDays(3).AddHours(2).AddMinutes(38).AddSeconds(12);
        private IDispatcherTimer _countdownTimer;

        #endregion

        #region Initialization and Lifecycle

        public HomePage()
        {
            InitializeComponent();
            StartCountdownTimer();
            LoadInitialPlaceholderData();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            _countdownTimer?.Stop();
        }

        #endregion

        #region Placeholder Data Loading

        private void LoadInitialPlaceholderData()
        {

            UserNameLabel.Text = "Marieden Daniel!";
            StudentIDLabel.Text = "Student ID: 2021123456";
            GradeLevelLabel.Text = "Grade 12-A";
            EligibilityLabel.Text = "Eligible to vote!";

            Candidate1NameLabel.Text = "Jung, Haein";
            Candidate2NameLabel.Text = "Kim, Mingyu";
            Candidate1VotesLabel.Text = "800 Votes";
            Candidate2VotesLabel.Text = "600 Votes";
            LastUpdateLabel.Text = "Vote count as of August 18, 2025 5:00PM";

            DebateStatusLabel.Text = "Video Stream Placeholder (Embed Link Here)";
        }

        #endregion

        #region Election Countdown Logic

        private void StartCountdownTimer()
        {
            _countdownTimer?.Stop();

            _countdownTimer = Dispatcher.CreateTimer();
            _countdownTimer.Interval = TimeSpan.FromSeconds(1);
            _countdownTimer.Tick += (s, e) => UpdateCountdownDisplay();
            _countdownTimer.Start();
        }

        private async void OnVoteNowClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new VotingPage());
        }
        private void UpdateCountdownDisplay()
        {
            var timeRemaining = _electionEndDate - DateTime.Now;

            if (timeRemaining.TotalSeconds <= 0)
            {
                _countdownTimer.Stop();

                DaysLeftLabel.Text = "00";
                HoursLeftLabel.Text = "00";
                MinutesLeftLabel.Text = "00";
                SecondsLeftLabel.Text = "00";
                DaysLeftMessageLabel.Text = "Voting has officially closed. Thank you for participating!";
                return;
            }

            DaysLeftLabel.Text = timeRemaining.Days.ToString("D2");
            HoursLeftLabel.Text = timeRemaining.Hours.ToString("D2");
            MinutesLeftLabel.Text = timeRemaining.Minutes.ToString("D2");
            SecondsLeftLabel.Text = timeRemaining.Seconds.ToString("D2");
            DaysLeftMessageLabel.Text = $"You have {timeRemaining.Days} days left to vote. Let your voice be heard!";
        }

        #endregion

        #region Event Handlers (Buttons and Navigation)

        private async void OnNavIconTapped(object sender, TappedEventArgs e)
        {
            if (e.Parameter is string pageName)
            {
                Page destinationPage = null;

                switch (pageName)
                {
                    case "Home":
                        break;

                    case "Vote":
                        destinationPage = new VotingPage();
                        break;

                    //case "Candidates":
                    //    destinationPage = new Candidates();
                    //    break;

                    case "PollHistory":
                        destinationPage = new PollHistory();
                        break;

                    case "Profile":

                        destinationPage = new AdminProfile();
                        break;

                    default:
                        await DisplayAlert("Navigation Error", $"The page for '{pageName}' is not defined.", "OK");
                        return;
                }

                if (destinationPage != null)
                {
                    await Navigation.PushAsync(destinationPage);
                }
            }
        }

        #endregion
    }
}