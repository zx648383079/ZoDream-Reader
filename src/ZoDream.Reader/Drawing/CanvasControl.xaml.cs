using SharpDX.Direct2D1;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
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

namespace ZoDream.Reader.Drawing
{
    /// <summary>
    /// CanvasControl.xaml 的交互逻辑
    /// </summary>
    public partial class CanvasControl : UserControl
    {
        public CanvasControl()
        {
            InitializeComponent();
        }

        // - field -----------------------------------------------------------------------

        private SharpDX.Direct3D11.Device device;
        private Texture2D renderTarget;
        private Dx11ImageSource d3DSurface;
        private RenderTarget d2DRenderTarget;
        private SharpDX.Direct2D1.Factory d2DFactory;

        protected ResourceCache resCache = new ResourceCache();


        public event RenderEventHandler? Draw;
        public event RenderEventHandler? CreateResources;

        /// <summary>
        /// 触发重绘
        /// </summary>
        public void Invalidate()
        {
            PrepareAndCallRender();
            d3DSurface.InvalidateD3DImage();
        }

        public void DrawImage(RenderTarget renderTarget)
        {
            device.ImmediateContext.Flush();
        }
        // - event handler ---------------------------------------------------------------


        // - private methods -------------------------------------------------------------

        private void StartD3D()
        {
            device = new SharpDX.Direct3D11.Device(SharpDX.Direct3D.DriverType.Hardware, DeviceCreationFlags.BgraSupport);
            d3DSurface = new Dx11ImageSource();
            imageBox.Source = d3DSurface;
        }

        private void EndD3D()
        {
            imageBox.Source = null;

            Disposer.SafeDispose(ref d2DRenderTarget);
            Disposer.SafeDispose(ref d2DFactory);
            Disposer.SafeDispose(ref d3DSurface);
            Disposer.SafeDispose(ref renderTarget);
            Disposer.SafeDispose(ref device);
        }

        public RenderTarget? CreateRenderTarget()
        {
            if (d3DSurface == null)
            {
                return null;
            }

            d3DSurface.SetRenderTarget(null);

            Disposer.SafeDispose(ref d2DRenderTarget);
            Disposer.SafeDispose(ref d2DFactory);
            Disposer.SafeDispose(ref renderTarget);

            var width = Math.Max((int)ActualWidth, 100);
            var height = Math.Max((int)ActualHeight, 100);

            var renderDesc = new Texture2DDescription
            {
                BindFlags = BindFlags.RenderTarget | BindFlags.ShaderResource,
                Format = Format.B8G8R8A8_UNorm,
                Width = width,
                Height = height,
                MipLevels = 1,
                SampleDescription = new SampleDescription(1, 0),
                Usage = ResourceUsage.Default,
                OptionFlags = ResourceOptionFlags.Shared,
                CpuAccessFlags = CpuAccessFlags.None,
                ArraySize = 1
            };

            renderTarget = new Texture2D(device, renderDesc);

            var surface = renderTarget.QueryInterface<Surface>();

            d2DFactory = new SharpDX.Direct2D1.Factory();
            var rtp = new RenderTargetProperties(new SharpDX.Direct2D1.PixelFormat(Format.Unknown, SharpDX.Direct2D1.AlphaMode.Premultiplied));
            d2DRenderTarget = new RenderTarget(d2DFactory, surface, rtp);
            resCache.RenderTarget = d2DRenderTarget;

            d3DSurface.SetRenderTarget(renderTarget);

            device.ImmediateContext.Rasterizer.SetViewport(0, 0, width, height, 0.0f, 1.0f);
            return d2DRenderTarget;
        }

        private void PrepareAndCallRender()
        {
            if (device == null)
            {
                return;
            }
            Draw?.Invoke(this);
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            StartD3D();
            CreateResources?.Invoke(this);
        }

        private void UserControl_Unloaded(object sender, RoutedEventArgs e)
        {
            EndD3D();
        }
    }
}
