using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace DemkaTest;

public partial class AddProductWindow : Window
{
  int userRole;
  string userName;
  public AddProductWindow()
  {
      InitializeComponent();
  }
  public AddProductWindow(int userRole, string userName)
  {
    InitializeComponent();
    this.userRole = userRole;
    this.userName = userName;
  }

  public void CloseThisWindow(object sender, RoutedEventArgs e)
  {
    ProductsWindow productWindow = new(userRole, userName);
    productWindow.Show();
    this.Close();
  }
}