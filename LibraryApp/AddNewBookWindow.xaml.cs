using System;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Linq;
using System.Windows;
using LibraryApp.Data;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp;

public partial class AddNewBookWindow : Window
{
    public ObservableCollection<Genre> Genres { get; set; }
    public ObservableCollection<Publisher> Publishers { get; set; }
    public ObservableCollection<Author> Authors { get; set; }

    private PickABookPage _page;
    public AddNewBookWindow(PickABookPage page)
    {
        InitializeComponent();
        _page = page;
        using var context = new AppDbContext();
        Genres = new ObservableCollection<Genre>(context.Genres);
        Publishers = new ObservableCollection<Publisher>(context.Publishers);
        Authors = new ObservableCollection<Author>(context.Authors);
        GenreCBX.ItemsSource = Genres;
        PublisherCBX.ItemsSource = Publishers;
        AuthorCBX.ItemsSource = Authors;
    }

    private void AddNewBookButtonClick(object sender, RoutedEventArgs e)
    {
        using var context = new AppDbContext();
        try
        {
            if (!int.TryParse(AmountTBX.Text, out var amount) ||
                !int.TryParse(AmountOfPagesTBX.Text, out var pages) ||
                !int.TryParse(AmountOfPagesTBX.Text, out var year))
                throw new FormatException();
            context.Books.Add(new Book
            {
                Amount = amount,
                AuthorId = ((Author)AuthorCBX.SelectedItem).AuthorId,
                BookName = BookNameTBX.Text,
                GenreId = ((Genre)GenreCBX.SelectedItem).GenreId,
                ISBN = ISBNTBX.Text,
                NumberOfPages = pages,
                PublisherId = ((Publisher)PublisherCBX.SelectedItem).PublisherId,
                RedactorFullName = RedactorTBX.Text,
                YearOfIssue = year
            });
            context.SaveChanges();
            _page.DataGrid.ItemsSource = _page.BooksTable = new ObservableCollection<Book>(context.Books
                .Include(x => x.Author)
                .Include(x => x.Genre)
                .Include(x => x.Publisher)
                .Where(x => x.Author.AuthorSurName.Contains(_page.SearchBarTBX.Text) ||
                            x.BookName.Contains(_page.SearchBarTBX.Text) ||
                            x.Author.AuthorName.Contains(_page.SearchBarTBX.Text) ||
                            x.Author.AuthorPatronymic.Contains(_page.SearchBarTBX.Text) ||
                            x.Genre.GenreName.Contains(_page.SearchBarTBX.Text) ||
                            x.Publisher.PublisherName.Contains(_page.SearchBarTBX.Text) ||
                            x.RedactorFullName.Contains(_page.SearchBarTBX.Text) ||
                            x.YearOfIssue.ToString().Contains(_page.SearchBarTBX.Text) ||
                            x.ISBN.Contains(_page.SearchBarTBX.Text))
            );
            MessageBox.Show("Новая запись добавлена!", "Успех!", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (FormatException)
        {
            MessageBox.Show("Заполните верно все поля!", "Повторите попытку!", MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
        catch (DbUpdateException)
        {
            MessageBox.Show("Ошибка при обращении к базе данных", "Повторите попытку!", MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
        catch (Exception)
        {
            MessageBox.Show("Ошибка!", "Обратитесь к системному администратору!", MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }
}