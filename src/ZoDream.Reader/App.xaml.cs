﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ZoDream.Reader.ViewModels;

namespace ZoDream.Reader
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static MainViewModel ViewModel = new MainViewModel();
        public static void ChangeTheme()
        {
            ResourceDictionary dict = new ResourceDictionary();
            dict.Source = new Uri("Skins/LightTheme.xaml", UriKind.Relative);
            Application.Current.Resources.MergedDictionaries.Add(dict);
        }
    }
}
