using System;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Shapes;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace EkandidatoApp;

public partial class ElectionCalendar : ContentPage
{
    public class Event
    {
        public required string Name { get; set; }
        public required Color DotColor { get; set; }
        public required DateTime Date { get; set; }
        public string DisplayDate => Date.ToString("M/d/yyyy");
    }

    private ObservableCollection<Event> allEvents = new ObservableCollection<Event>();
    public ObservableCollection<Event> UpcomingEvents { get; set; } = new ObservableCollection<Event>();

    private DateTime _currentMonth;
    private DateTime _selectedDate;

    public ElectionCalendar()
    {
        InitializeComponent();

        InitializeHardcodedEvents();

        _currentMonth = new DateTime(2025, 4, 1);
        _selectedDate = new DateTime(2025, 4, 20);

        InitializeYearPicker();
        UpdateCalendarGrid();
        UpdateUpcomingEventsList();

        EventsList.ItemsSource = UpcomingEvents;
    }

    private void InitializeHardcodedEvents()
    {
        allEvents.Add(new Event { Name = "Campaign Period Starts", DotColor = Color.FromArgb("#2E6A3E"), Date = new DateTime(2024, 3, 15) });
        allEvents.Add(new Event { Name = "Candidate Debate", DotColor = Colors.Red, Date = new DateTime(2024, 3, 25) });
        allEvents.Add(new Event { Name = "Candidacy Filing Deadline", DotColor = Color.FromArgb("#8A2BE2"), Date = new DateTime(2025, 4, 15) });
        allEvents.Add(new Event { Name = "Voter Registration Drive", DotColor = Colors.Orange, Date = new DateTime(2025, 4, 20) });
        allEvents.Add(new Event { Name = "Post-Election Review", DotColor = Colors.Gray, Date = new DateTime(2025, 12, 1) });
    }

    // NEW: Populates the Picker with a range of years
    private void InitializeYearPicker()
    {
        int currentYear = DateTime.Today.Year;
        // Range: 10 years past, 10 years future (21 years total)
        for (int year = currentYear - 10; year <= currentYear + 10; year++)
        {
            YearPicker.Items.Add(year.ToString());
        }

        // Set the picker to show the current calendar year
        YearPicker.SelectedItem = _currentMonth.Year.ToString();
    }

    private void UpdateCalendarGrid()
    {
        DateGrid.Children.Clear();

        // Update Month Label and ensure Picker is in sync
        MonthLabel.Text = _currentMonth.ToString("MMMM");
        if (YearPicker.SelectedItem?.ToString() != _currentMonth.Year.ToString())
        {
            YearPicker.SelectedItem = _currentMonth.Year.ToString();
        }


        int daysInMonth = DateTime.DaysInMonth(_currentMonth.Year, _currentMonth.Month);
        DateTime firstDayOfMonth = new DateTime(_currentMonth.Year, _currentMonth.Month, 1);
        int offset = (int)firstDayOfMonth.DayOfWeek;

        int currentDay = 1;
        for (int row = 0; row < 6; row++)
        {
            for (int col = 0; col < 7; col++)
            {
                DateTime date;
                bool isCurrentMonth = false;

                if (row * 7 + col < offset)
                {
                    DateTime previousMonth = _currentMonth.AddMonths(-1);
                    int daysInPreviousMonth = DateTime.DaysInMonth(previousMonth.Year, previousMonth.Month);
                    date = new DateTime(previousMonth.Year, previousMonth.Month, daysInPreviousMonth - offset + (row * 7 + col) + 1);
                }
                else if (currentDay <= daysInMonth)
                {
                    date = new DateTime(_currentMonth.Year, _currentMonth.Month, currentDay);
                    isCurrentMonth = true;
                    currentDay++;
                }
                else
                {
                    DateTime nextMonth = _currentMonth.AddMonths(1);
                    date = new DateTime(nextMonth.Year, nextMonth.Month, currentDay - daysInMonth);
                    currentDay++;
                }

                if (!isCurrentMonth && currentDay > daysInMonth + 1 && row > 4) continue;

                Border dateContainer = new Border
                {
                    WidthRequest = 30,
                    HeightRequest = 30,
                    StrokeShape = new RoundRectangle { CornerRadius = 15 },
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    BackgroundColor = Colors.Transparent,
                    StrokeThickness = 0
                };

                Label dateLabel = new Label
                {
                    Text = date.Day.ToString(),
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.Center,
                    FontSize = 14,
                    TextColor = isCurrentMonth ? Colors.Black : Color.FromArgb("#C0C0C0")
                };

                if (date.Date == _selectedDate.Date)
                {
                    dateContainer.BackgroundColor = Color.FromArgb("#ADD8E6");
                    dateLabel.TextColor = Colors.White;
                    dateLabel.FontAttributes = FontAttributes.Bold;
                }

                dateContainer.Content = dateLabel;

                TapGestureRecognizer tapGesture = new TapGestureRecognizer();
                tapGesture.Tapped += (s, args) => OnDateTapped(s, args, date);
                dateContainer.GestureRecognizers.Add(tapGesture);

                DateGrid.Add(dateContainer, col, row);
            }
        }
    }

    private void OnYearPickerSelectedIndexChanged(object sender, EventArgs e)
    {
        if (YearPicker.SelectedItem is string selectedYearString && int.TryParse(selectedYearString, out int newYear))
        {
            if (newYear != _currentMonth.Year)
            {
                _currentMonth = new DateTime(newYear, _currentMonth.Month, 1);

                int day = Math.Min(_selectedDate.Day, DateTime.DaysInMonth(_currentMonth.Year, _currentMonth.Month));
                _selectedDate = new DateTime(_currentMonth.Year, _currentMonth.Month, day);

                UpdateCalendarGrid();
                UpdateUpcomingEventsList();
            }
        }
    }

    private void OnDateTapped(object sender, TappedEventArgs e, DateTime tappedDate)
    {
        _selectedDate = tappedDate;

        if (_selectedDate.Month != _currentMonth.Month)
        {
            _currentMonth = _selectedDate; 
        }

        UpdateCalendarGrid();
        UpdateUpcomingEventsList();
    }

    private void OnPreviousMonthTapped(object sender, TappedEventArgs e)
    {
        _currentMonth = _currentMonth.AddMonths(-1);
        UpdateCalendarGrid();
    }

    private void OnNextMonthTapped(object sender, TappedEventArgs e)
    {
        _currentMonth = _currentMonth.AddMonths(1);
        UpdateCalendarGrid();
    }

    private void UpdateUpcomingEventsList()
    {
        var sortedUpcomingEvents = allEvents
            .Where(e => e.Date.Date >= DateTime.Today.Date)
            .OrderBy(e => e.Date)
            .ToList();

        UpcomingEvents.Clear();
        foreach (var evt in sortedUpcomingEvents)
        {
            UpcomingEvents.Add(evt);
        }
    }
}