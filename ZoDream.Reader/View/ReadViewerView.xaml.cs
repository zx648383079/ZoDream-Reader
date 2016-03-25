using System;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;

namespace ZoDream.Reader.View
{
    /// <summary>
    /// Description for ReadViewerView.
    /// </summary>
    public partial class ReadViewerView : Window
    {
        /// <summary>
        /// Initializes a new instance of the ReadViewerView class.
        /// </summary>
        public ReadViewerView()
        {
            InitializeComponent();
        }

        private bool front = true;

        private void Grid_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            DoubleAnimation da = new DoubleAnimation();

            da.Duration = new Duration(TimeSpan.FromSeconds(1));

            if (front)

                da.To = 0d;

            else

                da.To = 180d;

            AxisAngleRotation3D aar = FindName("aar") as AxisAngleRotation3D;

            aar.BeginAnimation(AxisAngleRotation3D.AngleProperty, da);
            front = !front;
        }
    }
}