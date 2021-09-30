using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using Vortice;
using Vortice.DCommon;
using Vortice.Direct2D1;
using Vortice.Direct3D;
using Vortice.Direct3D11;
using Vortice.DXGI;
using static Vortice.Direct2D1.D2D1;
using static Vortice.Direct3D11.D3D11;

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

        private ID3D11Device device;
        private SurfaceImageSource dxgiSurface;
        private ID2D1RenderTarget d2DRenderTarget;
        private ID3D11Texture2D renderTarget;
        private ID2D1Factory d2DFactory;
        private IDXGIFactory dXGIFactory;

        public event RenderEventHandler? Draw;
        public event RenderEventHandler? CreateResources;

        /// <summary>
        /// 触发重绘
        /// </summary>
        public void Invalidate()
        {
            PrepareAndCallRender();
            dxgiSurface.InvalidateD3DImage();
        }

        public void DrawImage(ID2D1RenderTarget renderTarget)
        {
            device.ImmediateContext.Flush();
        }
        // - event handler ---------------------------------------------------------------


        // - private methods -------------------------------------------------------------

        private void StartD3D()
        {
            var res = D3D11CreateDevice(null, DriverType.Hardware, DeviceCreationFlags.BgraSupport, new[]
            {
                Vortice.Direct3D.FeatureLevel.Level_11_1,
                Vortice.Direct3D.FeatureLevel.Level_11_0,
                Vortice.Direct3D.FeatureLevel.Level_10_1,
                Vortice.Direct3D.FeatureLevel.Level_10_0
            }, out device, out var _);
            if (res.Failure)
            {
                return;
            }
            // device = tempDevice.QueryInterface<ID3D11Device1>();
            d2DFactory = D2D1CreateFactory<ID2D1Factory1>();
            dXGIFactory = DXGI.CreateDXGIFactory1<IDXGIFactory1>();
            dxgiSurface = new SurfaceImageSource();
            imageBox.Source = dxgiSurface;
        }

        private void EndD3D()
        {
            imageBox.Source = null;
            dxgiSurface.SetRenderTarget(null);
            d2DRenderTarget?.Dispose();
            d2DFactory?.Dispose();
            renderTarget?.Dispose();
            dxgiSurface?.Dispose();
            device?.Dispose();
        }

        public ID2D1RenderTarget? CreateRenderTarget()
        {
            if (dxgiSurface == null)
            {
                return null;
            }

            d2DRenderTarget?.Dispose();
            renderTarget?.Dispose();

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

            renderTarget = device.CreateTexture2D(renderDesc);

            var surface = renderTarget.QueryInterface<IDXGISurface>();

            var rtp = new RenderTargetProperties(new PixelFormat(Format.Unknown, Vortice.DCommon.AlphaMode.Premultiplied));
            d2DRenderTarget = d2DFactory.CreateDxgiSurfaceRenderTarget(surface, rtp);
            dxgiSurface.SetRenderTarget(renderTarget);
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

        public ID2D1Bitmap LoadBitmap(string file)
        {
            using (var bitmap = (System.Drawing.Bitmap)System.Drawing.Image.FromFile(file))
            {
                var sourceArea = new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height);
                var bitmapProperties = new BitmapProperties(new PixelFormat(Format.R8G8B8A8_UNorm, Vortice.DCommon.AlphaMode.Premultiplied));
                var size = new System.Drawing.Size(bitmap.Width, bitmap.Height);

                // Transform pixels from BGRA to RGBA
                int stride = bitmap.Width * sizeof(int);
                using (var tempStream = new DataStream(bitmap.Height * stride, true, true))
                {
                    // Lock System.Drawing.Bitmap
                    var bitmapData = bitmap.LockBits(sourceArea, System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppPArgb);

                    // Convert all pixels 
                    for (int y = 0; y < bitmap.Height; y++)
                    {
                        int offset = bitmapData.Stride * y;
                        for (int x = 0; x < bitmap.Width; x++)
                        {
                            // Not optimized 
                            byte B = Marshal.ReadByte(bitmapData.Scan0, offset++);
                            byte G = Marshal.ReadByte(bitmapData.Scan0, offset++);
                            byte R = Marshal.ReadByte(bitmapData.Scan0, offset++);
                            byte A = Marshal.ReadByte(bitmapData.Scan0, offset++);
                            int rgba = R | (G << 8) | (B << 16) | (A << 24);
                            tempStream.Write(rgba);
                        }

                    }
                    bitmap.UnlockBits(bitmapData);
                    tempStream.Position = 0;

                    return d2DRenderTarget.CreateBitmap(size, tempStream.BasePointer, stride, bitmapProperties); ;
                }
            }
        }

        public CanvasLayer CreateLayer()
        {
            return new CanvasLayer(this, Math.Max((int)ActualWidth, 100), Math.Max((int)ActualHeight, 100));
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
