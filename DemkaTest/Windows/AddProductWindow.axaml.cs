using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using DemkaTest.Context;
using DemkaTest.Models;
using System.Collections.Generic;
using System.Linq;

namespace DemkaTest;

public partial class AddProductWindow : Window
{
  int manufacturerId;
  Product product;
  public AddProductWindow()
  {
    InitializeComponent();
    ManufacturerComboBox.SelectionChanged += ManufacturerComboboxSelectionChanged;
    product = new();
    DataContext = product;
    LoadManufacturers();

  }

  public AddProductWindow(string id)
  {
    InitializeComponent();
    ManufacturerComboBox.SelectionChanged += ManufacturerComboboxSelectionChanged;
    product = Healper.Database.Products.Find(id);
    DataContext = product;
    manufacturerId = product.Productmanufacturer;
    LoadManufacturers();
  }

  private void LoadManufacturers()
  {
    List<Company> manufacturers = Healper.Database.Companies.ToList();
    manufacturers.Insert(0, new Company
    {
      Companyid = 0,
      Companyname = "Выберите производителя"
    });
    ManufacturerComboBox.ItemsSource = manufacturers;
    ManufacturerComboBox.SelectedIndex = manufacturerId;
  }

  private void ManufacturerComboboxSelectionChanged(object sender, SelectionChangedEventArgs e)
  {
    product.Productmanufacturer = ManufacturerComboBox.SelectedIndex;
  }
  private void SaveNewProduct(object? sender, RoutedEventArgs e)
  {
    Healper.Database.Add(product);
    Healper.Database.SaveChanges();
    this.Close();
  }
  private void CloseThisWindow(object? sender, RoutedEventArgs e)
  {
    this.Close();
  }
}