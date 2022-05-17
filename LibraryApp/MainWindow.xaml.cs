using System.Windows;
using LibraryApp.Data;

namespace LibraryApp;

public partial class MainWindow : Window
{
    private readonly DataStore _store;
    public MainWindow()
    {
        InitializeComponent();
    }

    public MainWindow(DataStore store)
    {
        InitializeComponent();
        _store = store;
        CurrentUserNameTB.Text = _store.CurrentUser.FullName;
        if (_store.CurrentUser.Role.RoleName != "Студент") return;
        SeeRequestsButton.IsEnabled = false;
        StudentsButton.IsEnabled = false;
        TakenBooksButton.IsEnabled = false;
    }

    private void CloseButtonClick(object sender, RoutedEventArgs e)
    {
        new AuthorizationWindow().Show();
        Close();
    }

    private void PickTheBookButtonClick(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigate(new PickABookPage(_store));
    }

    private void SeeRequestsButtonClick(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigate(new RequestsPage(_store));
    }

    private void TakenBooksButtonClick(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigate(new TakenBooksPage(_store));
    }

    private void StudentsButtonClick(object sender, RoutedEventArgs e)
    {
        MainFrame.Navigate(new StudentsPage(_store));
    }
}