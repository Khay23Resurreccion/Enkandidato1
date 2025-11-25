namespace EkandidatoApp
{
    public partial class PollHistory : ContentPage
    {
        public PollHistory()
        {
            InitializeComponent();
            PopulateFilters();
        }

        /// <summary>
        /// Populates the Picker controls (dropdowns) with sample data.
        /// </summary>
        private void PopulateFilters()
        {
            // Data for Status Picker
            StatusPicker.ItemsSource = new List<string>
            {
                "All status",
                "Completed",
                "Pending",
                "Canceled"
            };
            StatusPicker.SelectedIndex = 0; // Selects "All status" by default

            // Data for Year Picker (Populates with the current year and previous years)
            var currentYear = DateTime.Now.Year;
            var years = new List<string>();
            for (int i = 0; i < 5; i++)
            {
                years.Add((currentYear - i).ToString());
            }
            YearPicker.ItemsSource = years;
            YearPicker.SelectedIndex = 0; // Selects the current year by default
        }

        /// <summary>
        /// Handles the filter selection change event.
        /// This method serves as the interactive hook for updating the data.
        /// </summary>
        private void OnFilterChanged(object sender, EventArgs e)
        {
            var selectedStatus = StatusPicker.SelectedItem as string;
            var selectedYear = YearPicker.SelectedItem as string;

            // This debug statement confirms the interactivity is working:
            System.Diagnostics.Debug.WriteLine($"Dashboard filtered. Status: {selectedStatus}, Year: {selectedYear}.");

            // *** Functional Logic Placeholder: ***
            // In a complete application, you would now call a function here
            // to re-query your data source based on `selectedStatus` and `selectedYear`,
            // and then update the header numbers and graph placeholders with the new results.
        }
    }
}