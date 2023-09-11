using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using DemkaTest.Context;
using DemkaTest.Models;
using DynamicData;
using Metsys.Bson;
using System.Collections.Generic;
using System.Linq;

namespace DemkaTest;

public partial class ProductsWindow : Window
{
 
  public ProductsWindow() 
  {
    UserNameTextBlock.Text = "�������";
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

          break;
        }
    }
    if (UserNameTextBlock.Text != "�����")
    {
      UserNameTextBlock.Text = userName;
    }
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
      x.Productname,
      x.Productdescription,
      x.Productmanufacturer,
      x.Productcost,
      x.Productquantityinstock
    });
  }

  private void LoadManufacturers()
  {
    List<Company> manufacturers = Healper.Database.Companies.ToList();
    manufacturers.Insert(0, new Company
    {
      Companyid = 0,
      Companyname = "��� sus"
    });
    ManufacturerComboBox.ItemsSource = manufacturers;
    ManufacturerComboBox.SelectedIndex = 0;
  }

  public void SearchBoxOnTextInput(object? sender, KeyEventArgs e)
  {
    LoadProducts();
  }

  public void ManufacturerComboboxSelectionChanged(object sender, SelectionChangedEventArgs e)
  {
    LoadProducts();
  }

  public static class Healper
  {
    public static readonly NeondbContext Database = new();
  }

  
}