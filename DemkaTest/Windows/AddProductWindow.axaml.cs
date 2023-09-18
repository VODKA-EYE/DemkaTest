using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using DemkaTest.Context;
using DemkaTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DemkaTest;

public partial class AddProductWindow : Window
{
  int productCategoryId;
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
    productCategoryId = product.Productcategory;
    DataContext = product;
    LoadStuff();
    ArticleNumberTextBox.IsReadOnly = true;
    newProduct = false;
  }
  private void LoadStuff()
  {
    image.Source = TryLoadImage(product.Productphoto);
    DelivelerTextBox.Text = product.ProductdelivelerNavigation.Companyname;
    ManufacturerTextBox.Text = product.ProductmanufacturerNavigation.Companyname;
    ProductCategoryComboBox.SelectionChanged += ProductCategoryComboboxSelectionChanged;
    LoadProductCategories();
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
  
  private void ProductCategoryComboboxSelectionChanged(object sender, SelectionChangedEventArgs e)
  {
    product.Productcategory = ProductCategoryComboBox.SelectedIndex;
  }
  private void SaveNewProduct(object? sender, RoutedEventArgs e)
  {
    try
    {
      if (DelivelerTextBox.Text == "" ||
      ManufacturerTextBox.Text == "" ||
      ArticleNumberTextBox.Text == "" ||
      ProductCategoryComboBox.SelectedIndex == 0 ||
      Double.Parse(PriceTextBox.Text) <= 0 ||
      Int32.Parse(QuanityTextBox.Text) < 0|| 
      Int32.Parse(DiscountTextBox.Text) < 0)
      {
        //Smth wrong
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
    catch
    {
      
    }
  }
  private void CloseThisWindow(object? sender, RoutedEventArgs e)
  {
    this.Close();
  }
}