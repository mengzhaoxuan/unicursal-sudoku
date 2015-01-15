using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using SuDoKu.Resources;
using System.Diagnostics;
using System.ComponentModel;

namespace SuDoKu
{
    /// <summary>
    /// Loading Opacity binding source
    /// </summary>
    public class LoadingOpacity :INotifyPropertyChanged
    {
        private string visibility;
        public string Visibility
        {
            get { return visibility; } 
            set
            {
                visibility = value;
                NotifyPropertyChanged("Visibility"); 
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChanged(string p)
        {
            if(PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(p));
            }
        }
    }


    public partial class MainPage : PhoneApplicationPage
    {
        public static LoadingOpacity LoadingOpa = new LoadingOpacity() { Visibility = "Collapsed", };
        // 构造函数
        public MainPage()
        {
            InitializeComponent();

            //binding opacity soucre
            Loading.DataContext = LoadingOpa;

            // 用于本地化 ApplicationBar 的示例代码
            //BuildLocalizedApplicationBar();
        }

        private void NewGame(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("new game.");

            LoadingOpa.Visibility = "Visible";

            this.NavigationService.Navigate(new Uri("/NewGame.xaml", UriKind.Relative));
        }

        private void Specification(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("/Specification.xaml",UriKind.Relative));
        }

        private void Quit(object sender, RoutedEventArgs e)
        {
            App.Quit();
        }


        // 用于生成本地化 ApplicationBar 的示例代码
        //private void BuildLocalizedApplicationBar()
        //{
        //    // 将页面的 ApplicationBar 设置为 ApplicationBar 的新实例。
        //    ApplicationBar = new ApplicationBar();

        //    // 创建新按钮并将文本值设置为 AppResources 中的本地化字符串。
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // 使用 AppResources 中的本地化字符串创建新菜单项。
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}