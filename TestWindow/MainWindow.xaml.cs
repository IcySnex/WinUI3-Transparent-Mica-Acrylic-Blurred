using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System;
using Windows.UI;
using System.Runtime.InteropServices;

namespace TestWindow;

public sealed partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        Helpers.Window.hWnd = Helpers.Window.GetHWnd(this);
        Helpers.Window.MakeTransparent();

        Helpers.Window.App.TitleBar.ExtendsContentIntoTitleBar = true;
        Helpers.Window.App.TitleBar.ButtonBackgroundColor = Colors.Transparent;
        Helpers.Window.App.TitleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
        Helpers.Window.App.TitleBar.ButtonHoverBackgroundColor = Color.FromArgb(50, 255, 255, 255);
        Helpers.Window.App.TitleBar.ButtonPressedBackgroundColor = Color.FromArgb(90, 255, 255, 255);
    }


    bool UseDarkMode = true;
    private void UseDarkMode_Toggled(object sender, RoutedEventArgs e)
    {
        if (Mica)
            Helpers.Window.SetMica(Mica, !UseDarkMode);
        if (Acrylic)
            Helpers.Window.SetAcrylic(Acrylic, !UseDarkMode);
        if (Blur)
            Helpers.Window.SetBlur(Blur, !UseDarkMode);
    }

    bool Mica;
    private void Mica_Click(object sender, RoutedEventArgs e)
    {
        Acrylic = false;
        Helpers.Window.SetAcrylic(Acrylic, UseDarkMode);
        Acrylic_Btn.Content = $"Acrylic ({Acrylic})";
        Blur = false;
        Helpers.Window.SetBlur(Blur, UseDarkMode);
        Blur_Btn.Content = $"Blur ({Blur})";
        Solid = false;
        Main.Background = new SolidColorBrush(Colors.Transparent);
        Solid_Btn.Content = $"Solid ({Solid})";

        Mica = !Mica;
        Helpers.Window.SetMica(Mica, UseDarkMode);
        Mica_Btn.Content = $"Mica ({Mica})";
    }

    bool Acrylic;
    private void Acrylic_Click(object sender, RoutedEventArgs e)
    {
        Mica = false;
        Helpers.Window.SetMica(Mica, UseDarkMode);
        Mica_Btn.Content = $"Mica ({Mica})";
        Blur = false;
        Helpers.Window.SetBlur(Blur, UseDarkMode);
        Blur_Btn.Content = $"Blur ({Blur})";
        Solid = false;
        Main.Background = new SolidColorBrush(Colors.Transparent);
        Solid_Btn.Content = $"Solid ({Solid})";

        Acrylic = !Acrylic;
        Helpers.Window.SetAcrylic(Acrylic, UseDarkMode);
        Acrylic_Btn.Content = $"Acrylic ({Acrylic})";
    }

    bool Blur;
    private void Blur_Click(object sender, RoutedEventArgs e)
    {
        Mica = false;
        Helpers.Window.SetMica(Mica, UseDarkMode);
        Mica_Btn.Content = $"Mica ({Mica})";
        Acrylic = false;
        Helpers.Window.SetAcrylic(Acrylic, UseDarkMode);
        Acrylic_Btn.Content = $"Acrylic ({Acrylic})";
        Solid = false;
        Main.Background = new SolidColorBrush(Colors.Transparent);
        Solid_Btn.Content = $"Solid ({Solid})";

        Blur = !Blur;
        Helpers.Window.SetBlur(Blur, UseDarkMode);
        Blur_Btn.Content = $"Blur ({Blur})";
    }

    bool Solid = true;
    private void Solid_Click(object sender, RoutedEventArgs e)
    {
        Mica = false;
        Helpers.Window.SetMica(Mica, UseDarkMode);
        Mica_Btn.Content = $"Mica ({Mica})";
        Acrylic = false;
        Helpers.Window.SetAcrylic(Acrylic, UseDarkMode);
        Acrylic_Btn.Content = $"Acrylic ({Acrylic})";
        Blur = false;
        Helpers.Window.SetBlur(Blur, UseDarkMode);
        Blur_Btn.Content = $"Blur ({Blur})";

        Solid = !Solid;
        Main.Background = new SolidColorBrush(Solid ? Colors.DimGray : Colors.Transparent);
        Solid_Btn.Content = $"Solid ({Solid})";
    }


    private async void Dialog_Click(object sender, RoutedEventArgs e)
    {
        StackPanel sp = new StackPanel();
        sp.Children.Add(new FontIcon() { FontFamily = new FontFamily("Segoe UI Emoji"), Glyph = "\U0001F439", FontSize = 50 });
        sp.Children.Add(new TextBlock() { HorizontalAlignment = HorizontalAlignment.Center, Text = "WinUI3 Transparent/Mica/Acrylic/Blurred Window!" });

        await new ContentDialog() { Title = "Information", Content = sp, CloseButtonText = "Ok", XamlRoot = Content.XamlRoot }.ShowAsync();
    }

    private void Maximize_Click(object sender, RoutedEventArgs e)
    {
        Helpers.Win32.ShowWindow(Helpers.Window.hWnd, 3);
    }
    private void Normal_Click(object sender, RoutedEventArgs e)
    {
        Helpers.Win32.ShowWindow(Helpers.Window.hWnd, 1);
    }
}