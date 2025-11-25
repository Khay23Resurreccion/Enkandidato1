using Firebase.Auth; 
using Firebase.Auth.Providers;
using Google.Cloud.Firestore;
using Google.Cloud.Firestore.V1;
using System.IO;
using Microsoft.Maui.Controls;
using System;
using System.Threading.Tasks;

namespace EkandidatoApp;

public partial class LoginPage : ContentPage
{
    private bool isPasswordHidden = true;

    private const string WebApiKey = "AIzaSyAnSpYawT1fWUqPQ9EZ4z3rbrYTlFLj_1s";
    private const string AuthDomain = "ekandidato-35b7a.firebaseapp.com";

    private const string FirestoreProjectId = "ekandidato-35b7a";
    private const string ServiceAccountResourceName = "EkandidatoApp.Resources.Raw.service-account.json";

    public LoginPage()
    {
        InitializeComponent();

        PasswordEntry.IsPassword = isPasswordHidden;
        MessageLabel.IsVisible = false;
    }

    private void OnTogglePasswordClicked(object sender, EventArgs e)
    {
        isPasswordHidden = !isPasswordHidden;
        PasswordEntry.IsPassword = isPasswordHidden;

        TogglePasswordButton.Source = isPasswordHidden ? "eye_open.png" : "eye_slash.png";
    }

    private async void OnLogInClicked(object sender, EventArgs e)
    {
        string studentNumber = StudentNumberEntry.Text?.Trim();
        string password = PasswordEntry.Text;

        if (string.IsNullOrWhiteSpace(studentNumber) || string.IsNullOrWhiteSpace(password))
        {
            await DisplayAlert("Login Failed", "Please enter both student number and password.", "OK");
            return;
        }

        string email = $"{studentNumber}@sti.edu";

        LoginButton.IsEnabled = false;
        MessageLabel.Text = "Logging in...";
        MessageLabel.TextColor = Color.FromArgb("#0000FF");
        MessageLabel.IsVisible = true;

        string nextPageRoute = "///HomePage";

        try
        {
            var config = new FirebaseAuthConfig
            {
                ApiKey = WebApiKey,
                AuthDomain = AuthDomain,
                Providers = new FirebaseAuthProvider[]
                {
                    new EmailProvider()
                }
            };

            var firebaseClient = new FirebaseAuthClient(config: config);
            await firebaseClient.SignInWithEmailAndPasswordAsync(email, password);

            MessageLabel.Text = "Login successful! Checking profile....";
            MessageLabel.TextColor = Color.FromArgb("#2E7D32");

            var assembly = typeof(LoginPage).Assembly;
            using var stream = assembly.GetManifestResourceStream(ServiceAccountResourceName);

            if (stream == null)
            {
                throw new InvalidOperationException($"Embedded resource '{ServiceAccountResourceName}' not found. Check Build Action and file name.");
            }

            var firestoreDb = FirestoreDb.Create(
              FirestoreProjectId,
              new FirestoreClientBuilder { JsonCredentials = await new StreamReader(stream).ReadToEndAsync() }.Build()
            );

            var docRef = firestoreDb.Collection("Students").Document(studentNumber);
            var snapshot = await docRef.GetSnapshotAsync();

            if (snapshot.Exists)
            {
                bool isPasswordSet = snapshot.GetValue<bool?>("isPasswordSet") ?? false;

                if (!isPasswordSet)
                {
                    nextPageRoute = $"///CreatePassword?studentId={studentNumber}";
                }
            }
            else
            {
                nextPageRoute = $"///CreatePassword?studentId={studentNumber}";
            }

            await Shell.Current.GoToAsync(nextPageRoute);
        }

        catch (FirebaseAuthException ex)
        {
            string errorMessage = "Login failed. Please check your credentials.";

            if (ex.Reason == AuthErrorReason.InvalidEmailAddress || ex.Reason == AuthErrorReason.WrongPassword)
            {
                errorMessage = "Invalid Student Number or Password.";
            }
            else if (ex.Reason == AuthErrorReason.TooManyAttemptsTryLater)
            {
                errorMessage = "Too many failed attempts. Try again later.";
            }

            MessageLabel.Text = errorMessage;
            MessageLabel.TextColor = Colors.Red;
            MessageLabel.IsVisible = true;
        }
        finally
        {
            LoginButton.IsEnabled = true;
        }
    }
}
