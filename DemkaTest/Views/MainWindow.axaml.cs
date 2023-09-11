using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Threading;
using DemkaTest.Context;
using DemkaTest.Models;
using DemkaTest.ViewModels;
using Microsoft.VisualBasic;
using System;
using System.Linq;

namespace DemkaTest.Views;

public partial class MainWindow : Window
{
  private bool failedCaptcha = false;
  private string captchaText;
  const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
  const string lines = @"/-|\ _~";
  private static Random random = new();
  DispatcherTimer timer = new();
  public MainWindow()
  {
    InitializeComponent();
    CaptchaTextBox.IsVisible = false;
  }

  public void Login(object sender, RoutedEventArgs e)
  {
    string login = LoginTextBlock.Text;
    string password = PasswordTextBlock.Text;
    User authUser = null;
    using (NeondbContext db = new()) 
    {
      authUser = db.Users.Where(b => b.Userlogin == login && b.Userpassword == password).FirstOrDefault();
    }
    if (authUser != null && failedCaptcha == false) 
    {
      string userName = authUser.Usersurname + " " + authUser.Username;
      ProductsWindow products = new(authUser.Userrole, userName);
      products.Show();
      this.Close();
    }
    else if(authUser != null && CaptchaTextBox.Text == captchaText)
    {
      string userName = authUser.Usersurname + " " + authUser.Username;
      ProductsWindow products = new(authUser.Userrole, userName);
      products.Show();
      this.Close();
    }
    else if(failedCaptcha)
    {
      timer.Interval = TimeSpan.FromSeconds(10);
      timer.Tick += TimerTick;
      timer.Start();
      LoginButton.IsEnabled = false;
      LoginTextBlock.IsEnabled = false;
      CaptchaTextBox.IsEnabled = false;
      PasswordTextBlock.IsEnabled = false;
    }
    else
    {
      failedCaptcha = true;
      GenerateCaptcha();
    }
  }

  public void LoginAsGuest(object sender, RoutedEventArgs e)
  {
    ProductsWindow products = new(0,"Гость");
    products.Show();
    this.Close();
  }

  public void GenerateCaptcha()
  {
    CaptchaTextBox.IsVisible = true;
    captchaText = new string(Enumerable.Repeat(chars, 4).Select(s => s[random.Next(s.Length)]).ToArray());
    Captcha1.Text = captchaText[0].ToString();
    Captcha1.Margin = Thickness.Parse(random.Next(0,8).ToString());
    Captcha2.Text = captchaText[1].ToString();
    Captcha2.Margin = Thickness.Parse(random.Next(0, 8).ToString());
    Captcha3.Text = captchaText[2].ToString();
    Captcha3.Margin = Thickness.Parse(random.Next(0, 8).ToString());
    Captcha4.Text = captchaText[3].ToString();
    Captcha4.Margin = Thickness.Parse(random.Next(0, 8).ToString());
    CaptchaNoise.Text = new string(Enumerable.Repeat(lines, 8).Select(s => s[random.Next(s.Length)]).ToArray());
    timer.Stop();
  }

  public void TimerTick(object sender, EventArgs e)
  {
    LoginButton.IsEnabled = true;
    LoginTextBlock.IsEnabled = true;
    CaptchaTextBox.IsEnabled = true;
    PasswordTextBlock.IsEnabled = true;
    GenerateCaptcha();
  }
}
