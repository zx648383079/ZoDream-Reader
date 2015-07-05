using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Win32;
using 在线阅读.Class;
using 在线阅读.Models;

namespace 在线阅读.View
{
    /// <summary>
    /// Interaction logic for BookPage.xaml
    /// </summary>
    public partial class BookPage : Page
    {
        public BookPage()
        {
            InitializeComponent();
        }
        private int _index;
        private UserMethod _method;

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (Model.BookModels==null)
            {
                _method = new UserMethod();
                Model.BookModels = _method.LoadBook();
            }

            RefreshListView();

        }

        private void BooksListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            _index = BooksListView.SelectedIndex;
            if (_index < 0 || _index >= Model.BookModels.Count) return;
            BookModel newModel = Model.BookModels[_index];
            for (int i = _index; i >0; i--)
            {
                Model.BookModels[i] = Model.BookModels[i - 1];
            }
            Model.BookModels[0] = newModel;
            RefreshListView();
            ReadPage readPage = new ReadPage();
            if (NavigationService != null) NavigationService.Navigate(readPage);
        }
        /// <summary>
        /// 刷新列表
        /// </summary>
        private void RefreshListView()
        {
            BooksListView.ItemsSource = null;
            BooksListView.ItemsSource = Model.BookModels;
        }

        private void OkBtn_Click(object sender, RoutedEventArgs e)
        {
            if (NameTb.Text == "" || FileNameTb.Text == "") return;
            Model.BookModels.Add(new BookModel(NameTb.Text, FileNameTb.Text, 0));
            AddGrid.Opacity = 0;
            AddGrid.Height = 0;
            RefreshListView();
        }

        private void OpenBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog {Filter = "文本文件|*.txt|所有文件|*.*", Title = "选择文本文件"};
            if (openFile.ShowDialog()==true)
            {
                FileNameTb.Text = openFile.FileName;
                if (NameTb.Text =="")
                {
                    NameTb.Text = Regex.Match(openFile.FileName, @"\\(?<name>.+?)\.",RegexOptions.RightToLeft).Groups[1].Value;
                }
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menu = (MenuItem)sender;
            switch (menu.Header.ToString())
            {
                case "删除":
                    _index = BooksListView.SelectedIndex;
                    int count = Model.BookModels.Count;
                    if (_index >= 0 && _index < count)
                    {
                        Model.BookModels.RemoveAt(_index);
                        RefreshListView();
                    }
                    break;
                case "添加":
                    AddGrid.Height = 100;
                    AddGrid.Opacity = 1;
                    NameTb.Text = "";
                    FileNameTb.Text = "";
                    break;
                case "设置":
                    SetWindow set = new SetWindow();
                    set.ShowDialog();
                    break;
            }
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            AddGrid.Height = 0;
            AddGrid.Opacity = 0;
            NameTb.Text = "";
            FileNameTb.Text = "";
        }
    }
}
