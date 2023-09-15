using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Castle.Components.DictionaryAdapter;
using DemkaTest.Context;
using DemkaTest.Models;
using DemkaTest.Views;
using DynamicData;
using Metsys.Bson;
using ReactiveUI;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace DemkaTest;

public partial class ProductsWindow : Window
{
  public ProductsWindow() 
  {
    InitializeComponent();
    UserNameTextBlock.Text = "�����";
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
          UserNameTextBlock.Text = "�����";
          break;
        }
      case 1:
        {
          
          break;
        }

      default:
        {
          AddButton.IsEnabled = false;          
          break;
        }
    }
    if (UserNameTextBlock.Text != "�����")
    {
      UserNameTextBlock.Text = userName;
    }
    SearchTextBox.AddHandler(KeyUpEvent, SearchBoxOnTextInput, RoutingStrategies.Tunnel);
    ManufacturerComboBox.SelectionChanged += ManufacturerComboboxSelectionChanged;
    listBox.SelectionChanged += EditProductClick;
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
    searchString = searchString.ToLower();
    if (!string.IsNullOrEmpty(searchString))
    {
      products = products.Where(t => t.Productname.ToLower().Contains(searchString) || t.Productdescription.ToLower().Contains(searchString)).ToList();
    }
    listBox.ItemsSource = products.Select(x => new
    {
      x.Productarticlenumber,
      x.Productname,
      x.Productdescription,
      Manufacturer = x.ProductmanufacturerNavigation.Companyname,
      x.Productcost,
      x.Productquantityinstock,
      Productphoto = TryLoadImage(x.Productphoto)
    });
    ListedTextBlock.Text = products.Count().ToString() + "/" + Healper.Database.Products.Count().ToString();
  }

  Bitmap TryLoadImage(string productphoto)
  {
    Bitmap link;
    try
    {
      link = new(@"./Resources/" + productphoto);
    }
    catch
    {
      link = new(@"./Resources/picture.png");
    }
    return link;
  }
  private void LoadManufacturers()
  {
    List<Company> manufacturers = Healper.Database.Companies.ToList();
    manufacturers.Insert(0, new Company
    {
      Companyid = 0,
      Companyname = "��� �������������"
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

  private async void EditProductClick(object? sender, SelectionChangedEventArgs e)
  {
    var listboxObject = listBox.SelectedItem;
    if (listboxObject != null)
    {
      System.Reflection.PropertyInfo pi = listboxObject.GetType().GetProperty("Productarticlenumber");
      string article = (string)(pi.GetValue(listboxObject, null));
      AddProductWindow addProductWindow = new(article);
      await addProductWindow.ShowDialog(this);

      addProductWindow.Closed += (o, arg) =>
      {
        LoadProducts();
      };
    }
    listBox.UnselectAll();
  }
}