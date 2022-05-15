using System.Windows;
using LibraryApp.Data;

namespace LibraryApp;

public partial class MainWindow : Window
{
    private AppDbContext _context;
    private readonly DataStore _store;
    public MainWindow()
    {
        InitializeComponent();
    }

    public MainWindow(DataStore store)
    {
        InitializeComponent();
        _store = store;
        _context = new AppDbContext();
        CurrentUserNameTB.Text = _store.CurrentUser.FullName;
    }
    
    private void CloseButtonClick(object sender, RoutedEventArgs e)
    {
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
}