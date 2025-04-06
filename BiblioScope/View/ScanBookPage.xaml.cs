using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;

namespace BiblioScope.View;

public partial class ScanBookPage : ContentPage
{
    private string azureKey = "AvhC1CsesAFo11BbMstSUQZrDnPww9J4wYEAXKoa8wkWeS7cSG7CJQQJ99BDACYeBjFXJ3w3AAAFACOGya8v";
    private const string AzureEndpoint = "https://biblioscope-ocr.cognitiveservices.azure.com/";
    public ScanBookPage()
    {
        InitializeComponent();
    }
    private async void OnPickPhotoClicked(object sender, EventArgs e)
    {
        try
        {
            var photo = await MediaPicker.PickPhotoAsync();
            if (photo == null) return;

            string imagePath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);

            using (var stream = await photo.OpenReadAsync())
            using (var fileStream = File.OpenWrite(imagePath))
            {
                await stream.CopyToAsync(fileStream);
            }

            BookImage.Source = ImageSource.FromFile(imagePath);
            OcrOutput.Text = "Running Azure OCR...";

            string ocrText = await RunAzureOCR(imagePath);
            OcrOutput.Text = ocrText;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Something went wrong: {ex.Message}", "OK");
        }
    }

    private async Task<string> RunAzureOCR(string imagePath)
    {
        var client = new ComputerVisionClient(new ApiKeyServiceClientCredentials(azureKey))
        {
            Endpoint = AzureEndpoint
        };

        using var imageStream = File.OpenRead(imagePath);
        var result = await client.RecognizePrintedTextInStreamAsync(true, imageStream, OcrLanguages.En);

        var sb = new StringBuilder();

        foreach (var region in result.Regions)
        {
            foreach (var line in region.Lines)
            {
                foreach (var word in line.Words)
                    sb.Append(word.Text + " ");
                sb.AppendLine();
            }
        }

        return sb.ToString();
    }
}