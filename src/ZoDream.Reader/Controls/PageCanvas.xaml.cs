using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ZoDream.Reader.Events;
using ZoDream.Shared.Interfaces;
using ZoDream.Shared.Models;

namespace ZoDream.Reader.Controls
{
    /// <summary>
    /// PageCanvas.xaml 的交互逻辑
    /// </summary>
    public partial class PageCanvas : UserControl, ICanvasRender
    {
        public PageCanvas()
        {
            InitializeComponent();
        }


        private int KidIndex = -1;
        private Point lastMousePoint = new Point(0, 0);

        public event ControlEventHandler? OnPrevious;
        public event ControlEventHandler? OnNext;

        private TextBlock GetTextBlock(int i)
        {
            if (DrawerCanvas.Children.Count > i)
            {
                return DrawerCanvas.Children[i] as TextBlock;
            }
            var tb = new TextBlock();
            DrawerCanvas.Children.Add(tb);
            return tb;
        }

        public void Draw(CharItem item)
        {
            var tb = GetTextBlock(++ KidIndex);
            tb.Text = item.Code.ToString();
            tb.FontFamily = FontFamily;
            tb.FontSize = FontSize;
            Canvas.SetLeft(tb, item.X);
            Canvas.SetTop(tb, item.Y);
        }

        public void Draw(PageItem page)
        {
            foreach (var item in page)
            {
                Draw(item);
            }
        }

        public void Draw(IEnumerable<PageItem> pages)
        {
            foreach (var item in pages)
            {
                Draw(item);
            }
        }

        public void Flush()
        {
            KidIndex = -1;
            foreach (var item in DrawerCanvas.Children)
            {
                (item as TextBlock).Text = string.Empty;
            }
        }

        private void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            lastMousePoint = e.GetPosition(this);
        }

        private void UserControl_MouseUp(object sender, MouseButtonEventArgs e)
        {
            var p = e.GetPosition(this);
            var diff = p - lastMousePoint;
            if (diff.Length < 5)
            {
                // TODO 点击
                if (p.X < ActualWidth / 3)
                {
                    OnPrevious?.Invoke(this);
                } else if (p.X > ActualWidth * .7)
                {
                    OnNext?.Invoke(this);
                }
                return;
            }

            if (Math.Abs(diff.X) > Math.Abs(diff.Y))
            {
                if (diff.X > 0)
                {
                    OnPrevious?.Invoke(this);
                } else if (diff.X < 0)
                {
                    OnNext?.Invoke(this);
                }
            }
        }
    }
}
