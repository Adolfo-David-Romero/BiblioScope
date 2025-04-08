using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using SkiaSharp;

namespace BiblioScope.View;

public partial class ScanBookPage : ContentPage
{
    private string azureKey = "AvhC1CsesAFo11BbMstSUQZrDnPww9J4wYEAXKoa8wkWeS7cSG7CJQQJ99BDACYeBjFXJ3w3AAAFACOGya8v";
    private const string AzureEndpoint = "https://biblioscope-ocr.cognitiveservices.azure.com/";
    public ObservableCollection<string> OCRLines { get; set; } = new();
    public ScanBookPage()
    {
        InitializeComponent();
        BindingContext = this;
    }

    private async void OnPickPhotoClicked(object sender, EventArgs e)
    {
        try
        {
            var photo = await MediaPicker.PickPhotoAsync();
            if (photo == null) return;

            var imagePath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);

            // Copy the picked photo to a memory stream
            using var originalStream = await photo.OpenReadAsync();
            using var memoryStream = new MemoryStream();
            await originalStream.CopyToAsync(memoryStream);
            memoryStream.Position = 0;

            // Write the memory stream to disk
            using var fileStream = File.Create(imagePath);
            memoryStream.Position = 0; // ensure pointer is at start
            await memoryStream.CopyToAsync(fileStream);
            fileStream.Close();

            // Display image
            BookImage.Source = ImageSource.FromFile(imagePath);
            OCRLines.Clear();

            // Run OCR using a fresh stream
            memoryStream.Position = 0;
            var client = new ComputerVisionClient(new ApiKeyServiceClientCredentials(azureKey)) { Endpoint = AzureEndpoint };
            var result = await client.RecognizePrintedTextInStreamAsync(true, memoryStream, OcrLanguages.En);

            // Extract text lines
            foreach (var region in result.Regions)
            foreach (var line in region.Lines)
                OCRLines.Add(string.Join(" ", line.Words.Select(w => w.Text)));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
        }
    }


    private async void OnLineSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is string selectedText)
        {
            await Shell.Current.GoToAsync(nameof(PossibleMatchesPage), new Dictionary<string, object>
            {
                { "Query", selectedText }
            });
        }
    }


    private async void OnTakePhotoClicked(object? sender, EventArgs e)
    {
        try
        {
            var photo = await MediaPicker.CapturePhotoAsync();
            if (photo == null) return;

            var originalStream = await photo.OpenReadAsync();

            //SkiaSharp is a library needed to convert HEIC (default iPhone format) --> JPEG
            // Load image via SkiaSharp
            using var skStream = new SKManagedStream(originalStream);
            using var bitmap = SKBitmap.Decode(skStream);

            if (bitmap == null)
            {
                await DisplayAlert("Error", "Could not decode the image.", "OK");
                return;
            }

            // Convert to JPEG
            using var image = SKImage.FromBitmap(bitmap);
            using var jpegStream = new MemoryStream();
            image.Encode(SKEncodedImageFormat.Jpeg, 90).SaveTo(jpegStream);

            // Save to disk for preview
            var imagePath = Path.Combine(FileSystem.CacheDirectory,
                $"{Path.GetFileNameWithoutExtension(photo.FileName)}.jpg");
            await File.WriteAllBytesAsync(imagePath, jpegStream.ToArray());
            BookImage.Source = ImageSource.FromFile(imagePath);
            OCRLines.Clear();

            // Reset position before sending to Azure
            jpegStream.Position = 0;

            // Azure OCR
            var client = new ComputerVisionClient(new ApiKeyServiceClientCredentials(azureKey))
                { Endpoint = AzureEndpoint };
            var result = await client.RecognizePrintedTextInStreamAsync(true, jpegStream, OcrLanguages.En);

            foreach (var region in result.Regions)
            foreach (var line in region.Lines)
                OCRLines.Add(string.Join(" ", line.Words.Select(w => w.Text)));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to process photo: {ex.Message}", "OK");
        }
    }
}