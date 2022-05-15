using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using LibraryApp.Data;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp;

public partial class TakenBooksPage : Page
{
    public ObservableCollection<TakenBook> TakenBooksTable { get; set; }
    private readonly DataStore _store;
    
    public TakenBooksPage()
    {
        InitializeComponent();
    }

    public TakenBooksPage(DataStore store)
    {
        InitializeComponent();
        _store = store;
        UpdateTable(string.Empty);
    }

    private void UpdateTable(string searchText)
    {
        using var context = new AppDbContext();
        TakenBooksTable = new ObservableCollection<TakenBook>(context.TakenBooks
            .Include(x => x.Book)
            .ThenInclude(x => x.Author)
            .Include(x => x.Book)
            .ThenInclude(x => x.Publisher)
            .Include(x => x.User)
            .Where(x => x.Book.Author.AuthorSurName.Contains(searchText) ||
                        x.Book.BookName.Contains(searchText) ||
                        x.Book.Author.AuthorName.Contains(searchText) ||
                        x.Book.Author.AuthorPatronymic.Contains(searchText) ||
                        x.Book.Genre.GenreName.Contains(searchText) ||
                        x.Book.Publisher.PublisherName.Contains(searchText) ||
                        x.Book.RedactorFullName.Contains(searchText) ||
                        x.Book.YearOfIssue.ToString().Contains(searchText) ||
                        x.Book.ISBN.Contains(searchText) ||
                        x.User.Surname.Contains(searchText) ||
                        x.User.Name.Contains(searchText) ||
                        x.User.Patronymic!.Contains(searchText))
        );
        DataGrid.ItemsSource = TakenBooksTable;
    }

    private void SearchBarTBXOnTextChanged(object sender, TextChangedEventArgs e)
    {
        UpdateTable(SearchBarTBX.Text);
    }

    private void TakeABookButtonClick(object sender, RoutedEventArgs e)
    {
        if (DataGrid.SelectedItem is not TakenBook takenBook) return;
        using var context = new AppDbContext();

        context.TakenBooks.Remove(takenBook);
        context.Books.Find(takenBook.BookId)!.Amount++;
        context.SaveChanges();
        UpdateTable(SearchBarTBX.Text);
    }
}