using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using DemkaTest.Context;
using DemkaTest.Models;
using DemkaTest.Views;
using DynamicData;
using Metsys.Bson;
using System.Collections.Generic;
using System.Linq;

namespace DemkaTest;

public partial class ProductsWindow : Window
{
  int userRole;
  string userName;
  public ProductsWindow() 
  {
    InitializeComponent();
    UserNameTextBlock.Text = "Гость";
    SearchTextBox.AddHandler(KeyUpEvent, SearchBoxOnTextInput, RoutingStrategies.Tunnel);
    ManufacturerComboBox.SelectionChanged += ManufacturerComboboxSelectionChanged;
    LoadProducts();
    LoadManufacturers();
  }
  public ProductsWindow(int userRole, string userName)
  {
    InitializeComponent();
    switch (userRole)
    {
      case 0:
        {
          UserNameTextBlock.Text = "Гость";
          break;
        }
      case 1:
        {

          break;
        }

      default:
        {

          break;
        }
    }
    if (UserNameTextBlock.Text != "Гость")
    {
      UserNameTextBlock.Text = userName;
    }
    this.userRole = userRole;
    this.userName = userName;
    SearchTextBox.AddHandler(KeyUpEvent, SearchBoxOnTextInput, RoutingStrategies.Tunnel);
    ManufacturerComboBox.SelectionChanged += ManufacturerComboboxSelectionChanged;
    LoadProducts();
    LoadManufacturers();
  }

  private void LoadProducts()
  {
    List<Product> products;
    if (ManufacturerComboBox.SelectedIndex != 0)
    {
      products = Healper.Database.Products.Where(x => x.Productmanufacturer == ManufacturerComboBox.SelectedIndex).ToList();
    }
    else
    {
      products = Healper.Database.Products.ToList();
    }
    
    string searchString = SearchTextBox.Text ?? "";
    if(!string.IsNullOrEmpty(searchString))
    {
      products = products.Where(t => t.Productname.Contains(searchString) || t.Productdescription.Contains(searchString)).ToList();
    }
    listBox.ItemsSource = products.Select(x => new
    {
      x.Productarticlenumber,
      x.Productname,
      x.Productdescription,
      Manufacturer = x.ProductmanufacturerNavigation.Companyname,
      x.Productcost,
      x.Productquantityinstock,
      Productphoto = "/Resources/"+x.Productarticlenumber
    });

    ListedTextBlock.Text = products.Count().ToString() + "/" + Healper.Database.Products.Count().ToString();
  }

  private void LoadManufacturers()
  {
    List<Company> manufacturers = Healper.Database.Companies.ToList();
    manufacturers.Insert(0, new Company
    {
      Companyid = 0,
      Companyname = "Все производители"
    });
    ManufacturerComboBox.ItemsSource = manufacturers;
    ManufacturerComboBox.SelectedIndex = 0;
  }

  private void SearchBoxOnTextInput(object? sender, KeyEventArgs e)
  {
    LoadProducts();
  }

  private void ManufacturerComboboxSelectionChanged(object sender, SelectionChangedEventArgs e)
  {
    LoadProducts();
  }

  private void LogOut(object sender, RoutedEventArgs e)
  {
    MainWindow loginwindow = new();
    loginwindow.Show();
    this.Close();
  }
  private async void OpenAddProductWindow(object sender, RoutedEventArgs e)
  {
    AddProductWindow addProductWindow = new();
    await addProductWindow.ShowDialog(this);
    LoadProducts();
  }

  private async void EditProductClick(object? sender, RoutedEventArgs e)
  {
    string button = (string)(sender as Button).Tag;
    AddProductWindow addProductWindow = new(button);
    await addProductWindow.ShowDialog(this);
    LoadProducts();
  }
}