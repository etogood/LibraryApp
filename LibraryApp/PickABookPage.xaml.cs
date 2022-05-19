using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using LibraryApp.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp;

public partial class PickABookPage : Page
{
    public ObservableCollection<Book> BooksTable { get; set; }
    private ObservableCollection<Author> AuthorsComboBox { get; set; }
    private ObservableCollection<Publisher> PublishersComboBox { get; set; }
    private ObservableCollection<Genre> GenresComboBox { get; set; }
    private readonly DataStore _store;

    public PickABookPage(DataStore store)
    {
        InitializeComponent();
        _store = store;
        if (_store.CurrentUser.Role.RoleName == "Студент")
        {
            Add1.IsEnabled = false;
            Add2.IsEnabled = false;
            Add3.IsEnabled = false;
            AddBookButton.IsEnabled = false;
            surnameBox.IsEnabled = false;
            nameBox.IsEnabled = false;
            patronymicBox.IsEnabled = false;
            publisherBox.IsEnabled = false;
            genreBox.IsEnabled = false;

        }
        using var context = new AppDbContext();
        try
        {
            BooksTable = new ObservableCollection<Book>(context.Books
                .Include(x => x.Author)
                .Include(x => x.Genre)
                .Include(x => x.Publisher)
            );
            AuthorsComboBox = new ObservableCollection<Author>(context.Authors);
            PublishersComboBox = new ObservableCollection<Publisher>(context.Publishers);
            GenresComboBox = new ObservableCollection<Genre>(context.Genres);
            DataGrid.ItemsSource = BooksTable;
            authorCB.ItemsSource = AuthorsComboBox;
            publisherCB.ItemsSource = PublishersComboBox;
            genreCB.ItemsSource = GenresComboBox;
        }
        catch (SqlException)
        {
            MessageBox.Show("Ошибка при взаимодействии с БД", "Обратитесь к системному администратору!", MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }

    private void TakeABookButtonClick(object sender, RoutedEventArgs e)
    {
        if (DataGrid.SelectedItem is not Book book) return;
        using var context = new AppDbContext();
        try
        {
            context.Requests.Add(new Request
            {
                BookId = book.BookId,
                UserId = _store.CurrentUser.UserId
            });
            context.SaveChanges();
        }
        catch (SqlException)
        {
            MessageBox.Show("Ошибка при взаимодействии с БД", "Обратитесь к системному администратору!", MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }
    
    private void SearchBarTBXOnTextChanged(object sender, TextChangedEventArgs e)
    {
        using var context = new AppDbContext();
        try
        {
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
        catch (SqlException)
        {
            MessageBox.Show("Ошибка при взаимодействии с БД", "Обратитесь к системному администратору!", MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }

    private void DataGrid_OnRowEditEnding(object? sender, DataGridRowEditEndingEventArgs e)
    {
        if (e.Row.Item is not Book book) return;
        try
        {
            using var context = new AppDbContext();
            context.Books.Update(book);
            context.SaveChanges();
            MessageBox.Show("Запись обновлена!", "Успех!", MessageBoxButton.OK, MessageBoxImage.Information);

        }
        catch (SqlException)
        {
            MessageBox.Show("Ошибка при взаимодействии с БД", "Обратитесь к системному администратору!", MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
        catch (DbUpdateException)
        {
            MessageBox.Show("Ошибка обновлении БД", "Обратитесь к системному администратору!", MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
        catch (Exception)
        {
            MessageBox.Show("Ошибка!", "Обратитесь к системному администратору!", MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }

    private void AuthorButtonClick(object sender, RoutedEventArgs e)
    {
        using var context = new AppDbContext();
        try
        {
            if (nameBox.Text == string.Empty || surnameBox.Text == string.Empty)
            { 
                MessageBox.Show("Заполните поля", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            context.Authors.Add(new Author
            {
                AuthorSurName = surnameBox.Text,
                AuthorName = nameBox.Text,
                AuthorPatronymic = patronymicBox.Text
            });
            context.SaveChanges();
        }
        catch (Exception)
        {
            MessageBox.Show("Неправильный ввод", "Ошибка!", MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }

    private void GenreButtonClick(object sender, RoutedEventArgs e)
    {
        using var context = new AppDbContext();
        try
        {
            if (genreBox.Text == string.Empty)
            { 
                MessageBox.Show("Заполните поле", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            context.Genres.Add(new Genre
            {
                GenreName = genreBox.Text
            });
            context.SaveChanges();
            MessageBox.Show("Операция завершена!", "Успех!", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception)
        {
            MessageBox.Show("Неправильный ввод", "Ошибка!", MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }

    private void PublisherButtonClick(object sender, RoutedEventArgs e)
    {
        using var context = new AppDbContext();
        try
        {
            if (publisherBox.Text == string.Empty)
            { 
                MessageBox.Show("Заполните поле", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            context.Publishers.Add(new Publisher
            {
                PublisherName = publisherBox.Text
            });
            context.SaveChanges();
            MessageBox.Show("Операция завершена!", "Успех!", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception)
        {
            MessageBox.Show("Неправильный ввод", "Ошибка!", MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        new AddNewBookWindow(this).Show();
    }

    private void PickABookPage_OnKeyDown(object sender, KeyEventArgs e)
    {
        if (_store.CurrentUser.Role.RoleName == "Студент") return;
        if (e.Key != Key.Delete) return;
        if (DataGrid.SelectedItem is not Book book) return;
        if (MessageBox.Show("Вы действительно хотите удалить книгу  \'" + book.BookName + "\' навсегда?",
                "Подтвердите действие", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) !=
            MessageBoxResult.Yes) return;
        using var context = new AppDbContext();
        context.Books.Remove(book);
        context.SaveChanges();
        BooksTable = new ObservableCollection<Book>(context.Books
            .Include(x => x.Author)
            .Include(x => x.Genre)
            .Include(x => x.Publisher)
        );
        DataGrid.ItemsSource = BooksTable;
    }
}