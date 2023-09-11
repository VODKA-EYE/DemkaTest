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
 
  public ProductsWindow() { }
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
    LoadProducts();
    LoadManufacturers();
  }

  private void LoadProducts()
  {
    var db = new NeondbContext().Products.ToList();
    listBox.DataContext = db;
  }

  private void LoadManufacturers()
  {
    var manufacturers = new NeondbContext().Companies.ToList();
    manufacturers.Insert(0, new Company
    {
      Companyid = 0,
      Companyname = "Все производители"
    });
    ManufacturerComboBox.ItemsSource = manufacturers;
    ManufacturerComboBox.SelectedIndex = 0;
  }

  public void SearchBoxActivation(object sender, RoutedEventArgs e)
  {
    
    if(SearchTextBox.Text.Length % 2 != 0)
    {
      SearchTextBox.Foreground = Brushes.DarkOliveGreen;
    }
    else
    {
      SearchTextBox.Foreground = Brushes.Olive;
    }
  }
}