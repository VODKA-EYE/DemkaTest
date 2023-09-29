using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Avalonia.Platform.Storage;
using DemkaTest.Context;
using DemkaTest.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace DemkaTest;

public partial class AddProductWindow : Window
{
  int productCategoryId;
  Product product;
  bool newProduct;
  string url;
  string imageFormat;
  public AddProductWindow()
  {
    InitializeComponent();
    product = new();
    DataContext = product;
    LoadProductCategories();
    newProduct = true;
  }

  public AddProductWindow(string id)
  {
    InitializeComponent();
    product = Healper.Database.Products.Find(id);
    productCategoryId = product.Productcategory;
    DataContext = product;
    image.Source = TryLoadImage(product.Productphoto);
    ProductCategoryComboBox.SelectionChanged += ProductCategoryComboboxSelectionChanged;
    LoadProductCategories();
    DelivelerTextBox.Text = product.ProductdelivelerNavigation.Companyname;
    ManufacturerTextBox.Text = product.ProductmanufacturerNavigation.Companyname;
    ArticleNumberTextBox.IsReadOnly = true;
    newProduct = false;
  }

  // ��� ������������ ����������� ��������
  Bitmap TryLoadImage(string productphoto)
  {
    Bitmap link;
    try
    {
      try
      {
        link = new(@"./Resources/" + productphoto);
      }
      catch
      {
        link = new(@"./Resources/picture.png");
      }
    }
    catch
    {
      link = null;
    }
    return link;
  }

  private async void SelectImage(object sender, RoutedEventArgs e)
  {
    var file = await StorageProvider.OpenFilePickerAsync(new Avalonia.Platform.Storage.FilePickerOpenOptions
    {
      Title = "��������� ��������",
      AllowMultiple = false,
      FileTypeFilter = new FilePickerFileType[]{new("��������"){Patterns = new[]{"*.png", "*.jpg", "*.jpeg", "*.bmp"}}}
    });
    if(file.Count >= 1)
    {
      await using var stream = await file[0].OpenReadAsync();
      using var streamReader = new StreamReader(stream);
      string url = (streamReader.BaseStream as FileStream).Name;
      image.Source = TryLoadSelectedImage(url);
    }
  }
  private void DropImage(object sender, RoutedEventArgs e)
  {
    image.Source = TryLoadSelectedImage(@"./Resources/picture.png");
  }

  // ���������� ��������� ��������
  Bitmap TryLoadSelectedImage(string url)
  {
    Bitmap link;
    try
    {
      try
      {
        link = new(url);
        this.url = url;
        imageFormat = Path.GetExtension(url);
      }
      catch
      {
        link = new(@"./Resources/picture.png");
        this.url = @"./Resources/picture.png";
      }
    }
    catch
    {
      link = null;
      this.url = null;
    }
    return link;
  }

  private void LoadProductCategories()
  {
    List<ProductCategory> category = Healper.Database.ProductCategories.ToList();
    category.Insert(0, new ProductCategory
    {
      Productcategoryid = 0,
      Productcategoryname = "�������� ��� ������"
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
        ErrorTextBlock.Text = "�� ���������� �������� � �����";
      }
      else
      {
        if (product.Productphoto == @"./Resources/picture.png" || url == @"./Resources/picture.png")
        {
          File.Delete(@$"./Resources/{product.Productarticlenumber + imageFormat}");
          product.Productphoto = "";
        }
        else if (product.Productphoto != @"./Resources/picture.png" || url != @"./Resources/picture.png")
        {
          // ��������� ���� � �� � � �����
          if(product.Productphoto == "")
          {
            File.Copy(url, @$"./Resources/{product.Productarticlenumber + imageFormat}", true);
            product.Productphoto = product.Productarticlenumber + imageFormat;
          }
          
        }
        else
        {
          product.Productphoto = "";
        }
        Company manufacturerId = Healper.Database.Companies.Where(x => x.Companyname.ToLower() == ManufacturerTextBox.Text.ToLower()).FirstOrDefault();
        Company delivelerId = Healper.Database.Companies.Where(x => x.Companyname.ToLower() == DelivelerTextBox.Text.ToLower()).FirstOrDefault();

        // ��� �������

        if (manufacturerId != null && delivelerId != null)
        {
          product.Productmanufacturer = manufacturerId.Companyid;
          product.Productdeliveler = delivelerId.Companyid;
        }

        // ��� �� �������

        else if (manufacturerId == null && delivelerId == null)
        {
          Company manufacturer = new(), deliveler  = new();
          if(ManufacturerTextBox.Text == DelivelerTextBox.Text)
          {
            manufacturer.Companyname = ManufacturerTextBox.Text;
            Healper.Database.Add(manufacturer);
            Healper.Database.SaveChanges();
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

        // ������������� �� ������

        else if (manufacturerId == null && delivelerId != null)
        {
          Company company = new();
          company.Companyname = ManufacturerTextBox.Text;
          Healper.Database.Add(company);
          Healper.Database.SaveChanges();
          product.Productmanufacturer = Healper.Database.Companies.Where(x => x.Companyname == ManufacturerTextBox.Text).Select(x => x.Companyid).FirstOrDefault();
        }

        // ��������� �� ������

        else if (manufacturerId != null && delivelerId == null)
        {
          Company company = new();
          company.Companyname = DelivelerTextBox.Text;
          Healper.Database.Add(company);
          Healper.Database.SaveChanges();
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
      ErrorTextBlock.Text = "������ ��� ���������� ������";
    }
  }
  private void CloseThisWindow(object? sender, RoutedEventArgs e)
  {
    this.Close();
  }
}