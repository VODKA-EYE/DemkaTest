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
    UserNameTextBlock.Text = "TESTING";
    SearchTextBox.AddHandler(KeyUpEvent, SearchBoxOnTextInput, RoutingStrategies.Tunnel);
    ManufacturerComboBox.SelectionChanged += ComboboxSelectionChanged;
    SortByPriceComboBox.SelectionChanged += ComboboxSelectionChanged;
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
          AddButton.IsVisible = false;
          AddButton.IsEnabled = false;
          break;
        }
      case 1:
        {
          listBox.SelectionChanged += EditProductClick;
          break;
        }

      default:
        {
          AddButton.IsVisible = false;
          AddButton.IsEnabled = false;          
          break;
        }
    }
    if (UserNameTextBlock.Text != "Гость")
    {
      UserNameTextBlock.Text = userName;
    }
    SearchTextBox.AddHandler(KeyUpEvent, SearchBoxOnTextInput, RoutingStrategies.Tunnel);
    ManufacturerComboBox.SelectionChanged += ComboboxSelectionChanged;
    SortByPriceComboBox.SelectionChanged += ComboboxSelectionChanged;
    LoadProducts();
    LoadManufacturers();
  }

  private void LoadProducts()
  {
    List<Product> products;

    // Выбор производителя
    if (ManufacturerComboBox.SelectedIndex != 0)
    {
      products = Healper.Database.Products.Where(x => x.Productmanufacturer == ManufacturerComboBox.SelectedIndex).ToList();
    }
    else
    {
      products = Healper.Database.Products.ToList();
    }
    
    // Сортировка по цене
    if(SortByPriceComboBox.SelectedIndex == 0) 
    { 
      products = products.OrderBy(x => x.Productcost).ToList();
    }
    else
    {
      products = products.OrderByDescending(x => x.Productcost).ToList();
    }

    // Работа поисковой строки
    string searchString = SearchTextBox.Text ?? "";
    searchString = searchString.ToLower();
    string[] searchStringElements = searchString.Split(' ');
    if (!string.IsNullOrEmpty(searchString))
    {
      foreach(string element in searchStringElements)
      {
        products = products.Where(
          t => t.Productname.ToLower().Contains(element) || 
          t.Productdescription.ToLower().Contains(element) ||
          t.ProductmanufacturerNavigation.Companyname.ToLower().Contains(element) ||
          t.ProductcategoryNavigation.Productcategoryname.ToLower().Contains(element) ||
          t.Productcost.ToString().Contains(element) ||
          t.Productquantityinstock.ToString().Contains(element)
          ).ToList();
      }
    }

    // Отображение данных в листе
    listBox.ItemsSource = products.Select(x => new
    {
      x.Productarticlenumber,
      x.Productname,
      x.Productdescription,
      Manufacturer = x.ProductmanufacturerNavigation.Companyname,
      Productcost = x.Productcost - (x.Productdiscountamount * x.Productcost) / 100,
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
      Companyname = "Все производители"
    });
    ManufacturerComboBox.ItemsSource = manufacturers;
    ManufacturerComboBox.SelectedIndex = 0;
  }

  private void SearchBoxOnTextInput(object? sender, KeyEventArgs e)
  {
    LoadProducts();
  }

  private void ComboboxSelectionChanged(object sender, SelectionChangedEventArgs e)
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

  private void EditProductClick(object? sender, SelectionChangedEventArgs e)
  {
    var listboxObject = listBox.SelectedItem;
    if (listboxObject != null)
    {
      System.Reflection.PropertyInfo pi = listboxObject.GetType().GetProperty("Productarticlenumber");
      string article = (string)(pi.GetValue(listboxObject, null));
      AddProductWindow addProductWindow = new(article);
      addProductWindow.ShowDialog(this);
      addProductWindow.Closed += (o, arg) =>
      {
        LoadProducts();
      };
    }
    listBox.UnselectAll();
  }
}