namespace EkandidatoApp;

public partial class ForgotPage : ContentPage
{
    private bool fPVisible = false;
    private bool rtVisible = false;
    public ForgotPage()
    {
        InitializeComponent();
    }

    private void TogglePasswordClicked(object sender, EventArgs e)
    {
        if (sender == FTPasswordButton)
        {
            fPVisible = !fPVisible;
            FPEntry.IsPassword = !fPVisible;
            FTPasswordButton.Source = fPVisible ? "eye_slash.png" : "eye_open.png";
        }
        else if (sender == RFTPasswordButton)
        {
            rtVisible = !rtVisible;
            RFPasswordEntry.IsPassword = !rtVisible;
            RFTPasswordButton.Source = rtVisible ? "eye_slash.png" : "eye_open.png";
        }
    }
}