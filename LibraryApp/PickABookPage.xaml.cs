using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using LibraryApp.Data;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp;

public partial class PickABookPage : Page
{
    public ObservableCollection<Book> BooksTable { get; set; }
    private readonly DataStore _store;
    
    public PickABookPage()
    {
        InitializeComponent();
    }

    public PickABookPage(DataStore store)
    {
        InitializeComponent();
        _store = store;
        using var context = new AppDbContext();
        BooksTable = new ObservableCollection<Book>(context.Books
            .Include(x => x.Author)
            .Include(x => x.Genre)
            .Include(x => x.Publisher)
        );
        DataGrid.ItemsSource = BooksTable;
    }

    private void TakeABookButtonClick(object sender, RoutedEventArgs e)
    {
        if (DataGrid.SelectedItem is not Book book) return;
        using var context = new AppDbContext();

        context.Requests.Add(new Request
        {
            BookId = book.BookId,
            UserId = _store.CurrentUser.UserId
        });
        context.SaveChanges();
    }
    
    private void SearchBarTBXOnTextChanged(object sender, TextChangedEventArgs e)
    {
        using var context = new AppDbContext();
        BooksTable = new ObservableCollection<Book>(context.Books
            .Include(x => x.Author)
            .Include(x => x.Genre)
            .Include(x => x.Publisher)
            .Where(x => x.Author.AuthorSurName.Contains(SearchBarTBX.Text) ||
                        x.BookName.Contains(SearchBarTBX.Text) ||
                        x.Author.AuthorName.Contains(SearchBarTBX.Text) ||
                        x.Author.AuthorPatronymic.Contains(SearchBarTBX.Text) ||
                        x.Genre.GenreName.Contains(SearchBarTBX.Text) ||
                        x.Publisher.PublisherName.Contains(SearchBarTBX.Text) ||
                        x.RedactorFullName.Contains(SearchBarTBX.Text) ||
                        x.YearOfIssue.ToString().Contains(SearchBarTBX.Text) ||
                        x.ISBN.Contains(SearchBarTBX.Text))
        );
        DataGrid.ItemsSource = BooksTable;
    }
}