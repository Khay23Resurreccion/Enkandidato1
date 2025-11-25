using Firebase.Auth;
using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;
using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace EkandidatoApp;

[QueryProperty(nameof(StudentId), "studentId")]
public partial class CreatePassword : ContentPage
{
    private const string WebApiKey = "AIzaSyAnSpYawT1fWUqPQ9EZ4z3rbrYTlFLj_1s";
    private const string AuthDomain = "ekandidato-35b7a.firebaseapp.com";
    private const string FirestoreProjectId = "ekandidato-35b7a";
    private const string ServiceAccountResourceName = "EkandidatoApp.Resources.Raw.service-account.json";

    private bool fPasswordVisible = false;
    private bool sPasswordVisible = false;

    public string StudentId { get; set; } = null!;

    public CreatePassword()
    {
        InitializeComponent();
        FPasswordEntry.IsPassword = true;
        SPasswordEntry.IsPassword = true;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (StudentId != null)
        {
            StudentNumberDisplay.Text = StudentId;
        }
    }

    private void TogglePasswordClicked(object sender, EventArgs e)
    {
        if (sender == FTogglePasswordButton)
        {
            fPasswordVisible = !fPasswordVisible;
            FPasswordEntry.IsPassword = !fPasswordVisible;
            FTogglePasswordButton.Source = fPasswordVisible ? "eye_slash.png" : "eye_open.png";
        }
        else if (sender == STogglePasswordButton)
        {
            sPasswordVisible = !sPasswordVisible;
            SPasswordEntry.IsPassword = !sPasswordVisible;
            STogglePasswordButton.Source = sPasswordVisible ? "eye_slash.png" : "eye_open.png";
        }
    }

    private async void OnCreatePasswordClicked(object sender, EventArgs e)
    {
        string studentNumber = StudentId;
        string newPassword = FPasswordEntry.Text;
        string confirmPassword = SPasswordEntry.Text;

        if (string.IsNullOrWhiteSpace(studentNumber))
        {
            await DisplayAlert("Error", "Student ID is missing. Please log in again.", "OK");
            return;
        }
        if (string.IsNullOrWhiteSpace(newPassword) || newPassword.Length < 6)
        {
            await DisplayAlert("Error", "Password must be at least 6 characters long.", "OK");
            return;
        }
        if (newPassword != confirmPassword)
        {
            await DisplayAlert("Error", "Passwords do not match.", "OK");
            return;
        }

        CreatePasswordButton.IsEnabled = false;
        MessageLabel.Text = "Updating password...";
        MessageLabel.TextColor = Color.FromArgb("#0000FF");
        MessageLabel.IsVisible = true;

        try
        {
            var config = new FirebaseAuthConfig { ApiKey = WebApiKey, AuthDomain = AuthDomain };
            var firebaseClient = new FirebaseAuthClient(config: config);

            var currentUser = firebaseClient.User;

            if (currentUser == null)
            {
                throw new InvalidOperationException("Authentication session expired. Please return to login.");
            }

            await currentUser.ChangePasswordAsync(newPassword);

            await SavePasswordSetFlagAsync(studentNumber);

            MessageLabel.Text = "Password set successfully! Redirecting...";
            MessageLabel.TextColor = Color.FromArgb("#2E7D32");

            Shell.Current.FlyoutBehavior = FlyoutBehavior.Flyout;
            await Shell.Current.GoToAsync("//HomePage");
        }
        catch (Exception ex)
        {
            string error = ex is FirebaseAuthException faEx ? $"Firebase Error: {faEx.Reason}" : ex.Message;

            MessageLabel.Text = $"Failed to set password: {error}";
            MessageLabel.TextColor = Colors.Red;
            System.Diagnostics.Debug.WriteLine($"[ERROR] Password Set Failure: {ex.Message}");
        }
        finally
        {
            CreatePasswordButton.IsEnabled = true;
        }
    }

    private async Task SavePasswordSetFlagAsync(string studentNumber)
    {
        try
        {
            var assembly = typeof(CreatePassword).Assembly;
            using var stream = assembly.GetManifestResourceStream(ServiceAccountResourceName);

            if (stream == null)
            {
                System.Diagnostics.Debug.WriteLine($"[FATAL ERROR] Failed to load embedded resource: {ServiceAccountResourceName}");
                return;
            }

            string jsonCredentials;
            using (var reader = new StreamReader(stream))
            {
                jsonCredentials = await reader.ReadToEndAsync();
            }

            var firestoreClient = new Google.Cloud.Firestore.V1.FirestoreClientBuilder
            {
                JsonCredentials = jsonCredentials
            }.Build();

            var firestoreDb = Google.Cloud.Firestore.FirestoreDb.Create(
                projectId: FirestoreProjectId,
                client: firestoreClient
            );

            var docRef = firestoreDb.Collection("Students").Document(studentNumber);

            await firestoreDb.RunTransactionAsync(async transaction =>
            {
                DocumentSnapshot snapshot = await transaction.GetSnapshotAsync(docRef);

                var updates = new Dictionary<string, object>
            {
                { "isPasswordSet", true }
            };

                transaction.Set(docRef, updates, SetOptions.MergeAll);

            });

            System.Diagnostics.Debug.WriteLine($"[SUCCESS] Firestore flag updated for {studentNumber}");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"[ERROR] Failed to save Firestore flag: {ex.Message}");
        }
    }
}

