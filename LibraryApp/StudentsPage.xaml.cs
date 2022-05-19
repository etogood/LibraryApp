using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using LibraryApp.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp;

public partial class StudentsPage : Page
{
    private readonly DataStore _store;
    public ObservableCollection<User> UsersTable { get; set; }
    private readonly AppDbContext _context;

    public StudentsPage()
    {
        InitializeComponent();
    }
    
    public StudentsPage(DataStore store)
    {
        InitializeComponent();
        _store = store;
        _context = new AppDbContext();
        try
        {
            UsersTable = new ObservableCollection<User>(_context.Users
                .Include(x => x.Role)
                .Where(x => x.Role.RoleName == "Студент")
            );
            DataGrid.ItemsSource = UsersTable;
        }
        catch (SqlException)
        {
            MessageBox.Show("Ошибка при взаимодействии с БД", "Обратитесь к системному администратору!", MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }

    private void SearchBarTBXOnTextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            UsersTable = new ObservableCollection<User>(_context.Users
                .Include(x => x.Role)
                .Where(x => x.Role.RoleName == "Студент" && (
                    x.Surname.Contains(SearchBarTBX.Text) ||
                    x.Name.Contains(SearchBarTBX.Text) ||
                    x.Patronymic!.Contains(SearchBarTBX.Text) ||
                    x.Login.Contains(SearchBarTBX.Text)))
            );
            DataGrid.ItemsSource = UsersTable;
        }
        catch (SqlException)
        {
            MessageBox.Show("Ошибка при взаимодействии с БД", "Обратитесь к системному администратору!", MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        if (DataGrid.SelectedItem is not User user) return;
        try
        {
            if (_context.Users.FirstOrDefault(x => x.Login == user.Login) == null && _context.Users.Find(user.UserId) == null)
            {
                _context.Users.Add(new User
                {
                    Login = user.Login,
                    Password = user.Password,
                    Surname = user.Surname,
                    Name = user.Name,
                    Patronymic = user.Patronymic,
                    RoleId = 2
                });
            }
            else _context.Users.Update(user);
            _context.SaveChanges();
            UsersTable = new ObservableCollection<User>(_context.Users
                .Include(x => x.Role)
                .Where(x => x.Role.RoleName == "Студент" && (
                            x.Surname.Contains(SearchBarTBX.Text) ||
                            x.Name.Contains(SearchBarTBX.Text) ||
                            x.Patronymic!.Contains(SearchBarTBX.Text) ||
                            x.Login.Contains(SearchBarTBX.Text)))
            );
            DataGrid.ItemsSource = UsersTable;
            MessageBox.Show("Операция завершена!", "Успех!", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (SqlException)
        {
            MessageBox.Show("Ошибка при взаимодействии с БД", "Обратитесь к системному администратору!", MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
        catch (Exception)
        {
            MessageBox.Show("Неправильно введена информация", "Повторите попытку!", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}