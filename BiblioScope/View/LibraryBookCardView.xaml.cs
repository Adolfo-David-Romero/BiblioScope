using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using BiblioScope.Model;

namespace BiblioScope.View;

public partial class LibraryBookCardView : ContentView
{
    public LibraryBookCardView()
    {
        InitializeComponent();
    }

    public static readonly BindableProperty BookProperty =
        BindableProperty.Create(nameof(Book), typeof(Book), typeof(LibraryBookCardView), propertyChanged: OnBookChanged);

    public Book Book
    {
        get => (Book)GetValue(BookProperty);
        set => SetValue(BookProperty, value);
    }

    private static void OnBookChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is LibraryBookCardView view && newValue is Book book)
        {
            // Bind everything to self so all bindings like Book and ViewCommand work
            view.BindingContext = view;
        }
    }

    public static readonly BindableProperty ViewCommandProperty =
        BindableProperty.Create(nameof(ViewCommand), typeof(Command), typeof(LibraryBookCardView));

    public Command ViewCommand
    {
        get => (Command)GetValue(ViewCommandProperty);
        set => SetValue(ViewCommandProperty, value);
    }
    public static readonly BindableProperty CommandProperty =
        BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(LibraryBookCardView));

    public ICommand Command
    {
        get => (ICommand)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public static readonly BindableProperty CommandParameterProperty =
        BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(LibraryBookCardView));

    public object CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }
}