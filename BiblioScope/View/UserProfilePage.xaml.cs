using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiblioScope.ViewModel;
using Firebase.Auth;
using Firebase.Storage;

namespace BiblioScope.View;

public partial class UserProfilePage : ContentPage
{
    private readonly FirebaseAuthClient _authClient;
    private readonly FirebaseStorage _storage;

    public UserProfilePage(FirebaseAuthClient authClient)
    {
        InitializeComponent();
        _authClient = authClient;

        _storage = new FirebaseStorage("biblioscope.firebasestorage.app");
        
        //loads profil img once page loads
        base.OnAppearing();
        LoadProfileImage();
    }

    public async void LoadProfileImage()
    {
        try
        {
            var user = _authClient.User;
            if (user == null) return;

            var downloadUrl = await _storage
                .Child("profileImages")
                .Child($"{user.Info.Uid}.jpg")
                .GetDownloadUrlAsync();

            ProfileImage.Source = ImageSource.FromUri(new Uri(downloadUrl));
        }
        catch
        {
            ProfileImage.Source = "default_profile.png"; // fallback
        }
    }

    private async void OnUploadProfilePictureClicked(object sender, EventArgs e)
    {
        var photo = await MediaPicker.PickPhotoAsync();
        if (photo == null) return;

        using var stream = await photo.OpenReadAsync();
        var user = _authClient.User;

        var task = await _storage
            .Child("profileImages")
            .Child($"{user.Info.Uid}.jpg")
            .PutAsync(stream);

        var downloadUrl = task;
        ProfileImage.Source = ImageSource.FromUri(new Uri(downloadUrl));
        await DisplayAlert("Success", "Profile picture updated.", "OK");
    }

    private async void OnDeleteProfilePictureClicked(object sender, EventArgs e)
    {
        try
        {
            var user = _authClient.User;

            await _storage
                .Child("profileImages")
                .Child($"{user.Info.Uid}.jpg")
                .DeleteAsync();

            ProfileImage.Source = "default_profile.png";
            await DisplayAlert("Deleted", "Profile picture removed.", "OK");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            await DisplayAlert("Error", $"Failed to delete: {ex.Message}", "OK");
        }
    }
}