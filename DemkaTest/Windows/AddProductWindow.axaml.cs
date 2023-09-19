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
    DelivelerTextBox.Text = product.ProductdelivelerNavigation.Companyname;
    ManufacturerTextBox.Text = product.ProductmanufacturerNavigation.Companyname;
    ArticleNumberTextBox.IsReadOnly = true;
    newProduct = false;
  }
  private void LoadStuff()
  {
    image.Source = TryLoadImage(product.Productphoto);
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
      Int32.Parse(DiscountTextBox.Text) < 0 ||
      Int32.Parse(DiscountTextBox.Text) > 100)
      {
        ErrorTextBlock.Text = "Что-то вызывает ошибку";
      }
      else
      {
        product.Productphoto = product.Productarticlenumber + ".jpg";

        Company manufacturerId = Healper.Database.Companies.Where(x => x.Companyname == ManufacturerTextBox.Text).FirstOrDefault();
        Company delivelerId = Healper.Database.Companies.Where(x => x.Companyname == DelivelerTextBox.Text).FirstOrDefault();

        // Оба найдены

        if (manufacturerId != null && delivelerId != null)
        {
          product.Productmanufacturer = manufacturerId.Companyid;
          product.Productdeliveler = delivelerId.Companyid;
        }

        // Оба не найдены

        else if (manufacturerId == null && delivelerId == null)
        {
          Company manufacturer = new(), deliveler  = new();
          if(ManufacturerTextBox.Text == DelivelerTextBox.Text)
          {
            manufacturer.Companyname = ManufacturerTextBox.Text;
            Healper.Database.Add(manufacturer);
          }
          else
          {
            manufacturer.Companyname = ManufacturerTextBox.Text;
            deliveler.Companyname = DelivelerTextBox.Text;
            Healper.Database.Add(manufacturer);
            Healper.Database.Add(deliveler);
            Healper.Database.SaveChanges();
          }
          
          product.Productmanufacturer = Healper.Database.Companies.Where(x => x.Companyname == ManufacturerTextBox.Text).Select(x => x.Companyid).FirstOrDefault();
          product.Productdeliveler = Healper.Database.Companies.Where(x => x.Companyname == DelivelerTextBox.Text).Select(x => x.Companyid).FirstOrDefault();
        }

        // Производитель не найден

        else if (manufacturerId == null && delivelerId != null)
        {
          Company company = new();
          company.Companyname = ManufacturerTextBox.Text;
          Healper.Database.Add(company);
          product.Productmanufacturer = Healper.Database.Companies.Where(x => x.Companyname == ManufacturerTextBox.Text).Select(x => x.Companyid).FirstOrDefault();
        }

        // Доставщик не найден

        else if (manufacturerId != null && delivelerId == null)
        {
          Company company = new();
          company.Companyname = DelivelerTextBox.Text;
          Healper.Database.Add(company);
          product.Productdeliveler = Healper.Database.Companies.Where(x => x.Companyname == DelivelerTextBox.Text).Select(x => x.Companyid).FirstOrDefault();
        }

        if (newProduct)
        {
          Healper.Database.Add(product);
        }
        Healper.Database.SaveChanges();
        Close();
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