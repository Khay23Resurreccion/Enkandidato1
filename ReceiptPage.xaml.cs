using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace EkandidatoApp
{
    public partial class ReceiptPage : ContentPage
    {
        public struct VoteCastItem
        {
            public string Position { get; set; }
            public string Partylist { get; set; }
            public string CandidateName { get; set; }
        }

        public class ReceiptData
        {
            public string ReceiptID { get; set; }
            public string DateTime { get; set; }
            public string StudentName { get; set; }
            public string StudentID { get; set; }
            public string StudentGrade { get; set; }
            public string ElectionID { get; set; }

            public List<VoteCastItem> VotesCast { get; set; }
        }

        private ReceiptData CurrentReceiptData;

        public ReceiptPage()
        {
            InitializeComponent();

            var data = CreateDummyReceiptData();

            PopulateReceipt(data);
        }

        public void PopulateReceipt(ReceiptData data)
        {
            CurrentReceiptData = data;

            ReceiptIDLabel.Text = data.ReceiptID;
            DateTimeLabel.Text = data.DateTime;

            StudentNameLabel.Text = data.StudentName;
            StudentIDLabel.Text = data.StudentID;
            StudentGradeLabel.Text = data.StudentGrade;
            ElectionIDLabel.Text = data.ElectionID;

            PopulateVotesCast(data.VotesCast);
        }
        private void PopulateVotesCast(List<VoteCastItem> votes)
        {
            VotesCastStack.Children.Clear(); 

            foreach (var vote in votes)
            {
                var voteRow = new Grid
                {
                    ColumnDefinitions = new ColumnDefinitionCollection
                    {
                        new ColumnDefinition { Width = GridLength.Star },
                        new ColumnDefinition { Width = GridLength.Auto }
                    },
                    Margin = new Thickness(0, 5)
                };

                var positionStack = new StackLayout();
                positionStack.Children.Add(new Label { Text = vote.Position, FontAttributes = FontAttributes.Bold, FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label)), TextColor = Color.FromHex("#333333") });
                positionStack.Children.Add(new Label { Text = vote.Partylist, FontSize = Device.GetNamedSize(NamedSize.Small, typeof(Label)), TextColor = Color.FromHex("#808080") });

                Grid.SetColumn(positionStack, 0);
                voteRow.Children.Add(positionStack);

                var candidateLabel = new Label
                {
                    Text = vote.CandidateName,
                    FontAttributes = FontAttributes.Bold,
                    FontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label)),
                    TextColor = Color.FromHex("#333333"),
                    HorizontalOptions = LayoutOptions.End,
                    VerticalOptions = LayoutOptions.Center
                };

                Grid.SetColumn(candidateLabel, 1);
                voteRow.Children.Add(candidateLabel);

                VotesCastStack.Children.Add(voteRow);
            }
        }

        private ReceiptData CreateDummyReceiptData()
        {
            return new ReceiptData
            {
                ReceiptID = "VOTE-2025-09-14-001234",
                DateTime = DateTime.Now.ToString("M/d/yyyy, h:mm tt"),
                StudentName = "Maria Santos",
                StudentID = "ID: 2021123456",
                StudentGrade = "Grade 12-A",
                ElectionID = "CSO2025-zxlpfhg",
                VotesCast = new List<VoteCastItem>
                {
                    new VoteCastItem { Position = "President", Partylist = "Unidy Partylist", CandidateName = "Kim, Mingyu" },
                    new VoteCastItem { Position = "Vice-President", Partylist = "Unidy Partylist", CandidateName = "Jang, Wonyoung" },
                    new VoteCastItem { Position = "Secretary", Partylist = "Unidy Partylist", CandidateName = "Jeon, Wonwoo" },
                    new VoteCastItem { Position = "Treasurer", Partylist = "Unidy Partylist", CandidateName = "Diccaya, Jineun" },
                    new VoteCastItem { Position = "Auditor", Partylist = "New Voice Partylist", CandidateName = "Satoru Gojo" }
                }
            };
        }

        private async void OnBackButtonClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//Home");
        }

        private async void OnDownloadReceiptClicked(object sender, EventArgs e)
        {
            Debug.WriteLine($"Attempting to download receipt: {CurrentReceiptData?.ReceiptID}");
            await DisplayAlert("Functionality Placeholder", "Generating and downloading receipt...", "OK");
        }

        private async void OnEmailReceiptClicked(object sender, EventArgs e)
        {
            Debug.WriteLine($"Attempting to email receipt: {CurrentReceiptData?.ReceiptID}");
            await DisplayAlert("Functionality Placeholder", "Composing email with receipt attachment...", "OK");
        }
    }
}