using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BiblioScope.Model;

namespace BiblioScope.View;

public partial class SearchPage : ContentPage
{
    private readonly HardcoverBookService _bookService;

    public SearchPage()
    {
        InitializeComponent();
        _bookService = new HardcoverBookService();
    }

    private async void OnSearchButtonPressed(object sender, EventArgs e)
    {
        var query = BookSearchBar.Text?.Trim();
        if (string.IsNullOrWhiteSpace(query))
        {
            JsonResultLabel.Text = "Please enter a book title.";
            return;
        }

        JsonResultLabel.Text = "Searching...";

        try
        {
            var result = await _bookService.SearchBooksAsync(query, 1, 2);

            var json = JsonSerializer.Serialize(result,
                new JsonSerializerOptions { WriteIndented = true });

            JsonResultLabel.Text = json;
        }
        catch (Exception ex)
        {
            JsonResultLabel.Text = $"Error: {ex.Message}";
        }
    }
}