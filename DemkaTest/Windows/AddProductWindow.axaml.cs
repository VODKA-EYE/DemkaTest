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
  int productCategoryId;
  int delivelerId;
  Product product;
  bool newProduct;
  public AddProductWindow()
  {
    InitializeComponent();
    product = new();
    DataContext = product;
    LoadStuff();
    newProduct = true;
  }

  public AddProductWindow(string id)
  {
    InitializeComponent();
    product = Healper.Database.Products.Find(id);
    delivelerId = product.Productdeliveler;
    manufacturerId = product.Productmanufacturer;
    productCategoryId = product.Productcategory;
    DataContext = product;
    LoadStuff();
    newProduct = false;
  }
  private void LoadStuff()
  {
    DelivelerComboBox.SelectionChanged += DelivelerComboboxSelectionChanged;
    ManufacturerComboBox.SelectionChanged += ManufacturerComboboxSelectionChanged;
    ProductCategoryComboBox.SelectionChanged += ProductCategoryComboboxSelectionChanged;
    LoadDelivelers();
    LoadManufacturers();
    LoadProductCategories();
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
  private void LoadDelivelers()
  {
    List<Company> deliveler = Healper.Database.Companies.ToList();
    deliveler.Insert(0, new Company
    {
      Companyid = 0,
      Companyname = "Выберите поставщика"
    });
    DelivelerComboBox.ItemsSource = deliveler;
    DelivelerComboBox.SelectedIndex = delivelerId;
  }

  private void LoadProductCategories()
  {
    List<ProductCategory> category = Healper.Database.ProductCategories.ToList();
    category.Insert(0, new ProductCategory
    {
      Productcategoryid = 0,
      Productcategoryname = "Выберите тип товара"
    });
    ProductCategoryComboBox.ItemsSource = category;
    ProductCategoryComboBox.SelectedIndex = productCategoryId;
  }
  
  private void ManufacturerComboboxSelectionChanged(object sender, SelectionChangedEventArgs e)
  {
    product.Productmanufacturer = ManufacturerComboBox.SelectedIndex;
  }
  private void DelivelerComboboxSelectionChanged(object sender, SelectionChangedEventArgs e)
  {
    product.Productdeliveler = DelivelerComboBox.SelectedIndex;
  }
  private void ProductCategoryComboboxSelectionChanged(object sender, SelectionChangedEventArgs e)
  {
    product.Productcategory = ProductCategoryComboBox.SelectedIndex;
  }
  private void SaveNewProduct(object? sender, RoutedEventArgs e)
  {
    if(DelivelerComboBox.SelectedIndex == 0 || ManufacturerComboBox.SelectedIndex == 0 || ProductCategoryComboBox.SelectedIndex == 0)
    {

    }
    else
    {
      product.Productphoto = product.Productarticlenumber + ".jpg";
      if (newProduct)
      {
        Healper.Database.Add(product);
      }
      Healper.Database.SaveChanges();
      this.Close();
    }
  }
  private void CloseThisWindow(object? sender, RoutedEventArgs e)
  {
    this.Close();
  }
}