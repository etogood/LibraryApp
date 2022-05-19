using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using LibraryApp.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp;

public partial class RequestsPage : Page
{
    public ObservableCollection<Request> RequestsTable { get; set; }
    private readonly DataStore _store;
    
    public RequestsPage()
    {
        InitializeComponent();
    }

    public RequestsPage(DataStore store)
    {
        InitializeComponent();
        _store = store;
        UpdateTable();
    }

    private void AcceptButtonClick(object sender, RoutedEventArgs e)
    {
        if (DataGrid.SelectedItem is not Request request) return;
        using var context = new AppDbContext();
        try
        {
            if (context.Books.Find(request.BookId)!.Amount == 0)
            {
                MessageBox.Show("Книга в данный момент не в наличии", "Невозможно выполнить действие",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            context.TakenBooks.Add(new TakenBook
            {
                BookId = request.BookId,
                DateOfDeadline = DateTime.Today.Add(TimeSpan.FromDays(30)),
                DateOfIssue = DateTime.Today,
                UserId = request.UserId,
                Code = CodeMaker()
            });
            context.Remove(context.Requests.Find(request.RequestId));
            context.Books.Find(request.BookId)!.Amount--;
            context.SaveChanges();
            UpdateTable();

        }
        catch (SqlException)
        {
            MessageBox.Show("Ошибка при взаимодействии с БД", "Обратитесь к системному администратору!", MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }

    private static int CodeMaker()
    {
        return new Random().Next(1000, 9999);
    }

    private void DenyButtonClick(object sender, RoutedEventArgs e)
    {
        if (DataGrid.SelectedItem is not Request request) return;
        using var context = new AppDbContext();
        try
        {
            context.Remove(context.Requests.Find(request.RequestId));
            context.SaveChanges();
            UpdateTable();
        }
        catch (SqlException)
        {
            MessageBox.Show("Ошибка при взаимодействии с БД", "Обратитесь к системному администратору!", MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }

    private void UpdateTable()
    {
        using var context = new AppDbContext();
        try
        {
            RequestsTable = new ObservableCollection<Request>(context.Requests
                .Include(x => x.Book)
                .ThenInclude(x => x.Author)
                .Include(x => x.Book)
                .ThenInclude(x => x.Publisher)
                .Include(x => x.User)
            );
            DataGrid.ItemsSource = RequestsTable;
        }
        catch (SqlException)
        {
            MessageBox.Show("Ошибка при взаимодействии с БД", "Обратитесь к системному администратору!", MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }
}