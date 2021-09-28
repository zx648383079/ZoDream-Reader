using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Interop;
using Vortice.Direct3D11;
using Vortice.Direct3D9;

namespace ZoDream.Reader.Drawing
{
    public class SurfaceImageSource : D3DImage, IDisposable
    {
        private static int ActiveClients = 0;
        private static IDirect3D9Ex? D3DContext;
        private static IDirect3DDevice9Ex? D3DDevice;

        private IDirect3DTexture9 renderTarget;

        // - property --------------------------------------------------------------------

        public int RenderWait { get; set; } = 2; // default: 2ms

        public SurfaceImageSource()
        {
            StartD3D();
            ActiveClients++;
        }

        public void InvalidateD3DImage()
        {
            if (renderTarget != null)
            {
                base.Lock();
                if (RenderWait != 0)
                {
                    Thread.Sleep(RenderWait);
                }
                base.AddDirtyRect(new System.Windows.Int32Rect(0, 0, base.PixelWidth, base.PixelHeight));
                base.Unlock();
            }
        }


        public void SetRenderTarget(ID3D11Texture2D? target)
        {
            if (target == null)
            {
                return;
            }
            var format = TranslateFormat(target);
            var handle = GetSharedHandle(target);

            if (!IsShareable(target))
            {
                throw new ArgumentException("Texture must be created with ResouceOptionFlags.Shared");
            }

            if (format == Format.Unknown)
            {
                throw new ArgumentException("Texture format is not compatible with OpenSharedResouce");
            }

            if (handle == IntPtr.Zero)
            {
                throw new ArgumentException("Invalid handle");
            }

            renderTarget = D3DDevice.CreateTexture(target.Description.Width, target.Description.Height, 1, Usage.RenderTarget, format, Pool.Default, ref handle);
            using (var surface = renderTarget.GetSurfaceLevel(0))
            {
                base.Lock();
                base.SetBackBuffer(D3DResourceType.IDirect3DSurface9, surface.NativePointer);
                base.Unlock();
            }
        }

        public void Dispose()
        {
            SetRenderTarget(null);

            renderTarget?.Dispose();

            ActiveClients--;
            EndD3D();
        }

        // - private methods -------------------------------------------------------------

        private void StartD3D()
        {
            if (ActiveClients != 0)
            {
                return;
            }

            var presentParams = GetPresentParameters();
            var createFlags = CreateFlags.HardwareVertexProcessing | CreateFlags.Multithreaded | CreateFlags.FpuPreserve;
            D3D9.Create9Ex(out D3DContext);
            D3DDevice = D3DContext.CreateDeviceEx(0, DeviceType.Hardware, IntPtr.Zero, createFlags, presentParams);
        }

        private void EndD3D()
        {
            if (ActiveClients != 0)
            {
                return;
            }
            renderTarget?.Dispose();
            D3DDevice?.Dispose();
            D3DContext?.Dispose();
        }

        [DllImport("user32.dll", SetLastError = false)]
        public static extern IntPtr GetDesktopWindow();

        private static void ResetD3D()
        {
            if (ActiveClients == 0)
            {
                return;
            }

            var presentParams = GetPresentParameters();
            D3DDevice.ResetEx(ref presentParams);
        }

        private static PresentParameters GetPresentParameters()
        {
            var presentParams = new PresentParameters();

            presentParams.Windowed = true;
            presentParams.SwapEffect = SwapEffect.Discard;
            presentParams.DeviceWindowHandle = GetDesktopWindow();
            presentParams.PresentationInterval = PresentInterval.Default;

            return presentParams;
        }

        private IntPtr GetSharedHandle(ID3D11Texture2D texture)
        {
            using (var resource = texture.QueryInterface<Vortice.DXGI.IDXGIResource>())
            {
                return resource.SharedHandle;
            }
        }

        private static Format TranslateFormat(ID3D11Texture2D texture)
        {
            switch (texture.Description.Format)
            {
                case Vortice.DXGI.Format.R10G10B10A2_UNorm: return Format.A2B10G10R10;
                case Vortice.DXGI.Format.R16G16B16A16_Float: return Format.A16B16G16R16F;
                case Vortice.DXGI.Format.B8G8R8A8_UNorm: return Format.A8R8G8B8;
                default: return Format.Unknown;
            }
        }

        private static bool IsShareable(ID3D11Texture2D texture)
        {
            return (texture.Description.OptionFlags & ResourceOptionFlags.Shared) != 0;
        }
    }
}
