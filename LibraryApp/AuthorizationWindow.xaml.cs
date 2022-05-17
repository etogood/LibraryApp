using System.Linq;
using System.Windows;
using LibraryApp.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp;

public partial class AuthorizationWindow : Window
{
    
    private AppDbContext _context;
    private DataStore _store;
    
    public AuthorizationWindow()
    {
        InitializeComponent();
        _context = new AppDbContext();
        _store = new DataStore();
    }

    private void LogInButtonClick(object sender, RoutedEventArgs e)
    {
        try
        {
            var user = _context.Users.Include(x => x.Role).FirstOrDefault(x => x.Login == loginTBX.Text);
            if (user != null && Equals(user.Password, passwordPBX.Password))
            {
                _store.CurrentUser = user;
                new MainWindow(_store).Show();
                Close();
            }
            else
                MessageBox.Show("Введён неверный логин или пароль", "Повторите попытку!", MessageBoxButton.OK,
                    MessageBoxImage.Error);
        }
        catch (SqlException)
        {
            MessageBox.Show("Ошибка при взаимодействии с БД", "Обратитесь к системному администратору!", MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
    }
}