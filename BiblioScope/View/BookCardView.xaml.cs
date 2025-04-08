using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using BiblioScope.Model;

namespace BiblioScope.View;

/// <summary>Reusable UI component the represents a book in card form with in the search context/// </summary>
public partial class BookCardView : ContentView
{
    public BookCardView()
    {
        InitializeComponent();
        
        
    }
    // The Book to bind to (sets BindingContext)
    public static readonly BindableProperty BookProperty =
        BindableProperty.Create(nameof(Book), typeof(Book), typeof(BookCardView), propertyChanged: OnBookChanged);

    public Book Book
    {
        get => (Book)GetValue(BookProperty);
        set => SetValue(BookProperty, value);
    }

    private static void OnBookChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is BookCardView view && newValue is Book book)
        {
            view.BindingContext = book;
        }
    }

    // Command to execute on tap
    public static readonly BindableProperty CommandProperty =
        BindableProperty.Create(nameof(Command), typeof(Command), typeof(BookCardView));

    public Command Command
    {
        get => (Command)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    // Optional Command Parameter
    public static readonly BindableProperty CommandParameterProperty =
        BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(BookCardView));

    public object CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }
}