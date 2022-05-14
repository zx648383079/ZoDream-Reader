using System;
using System.Collections.Generic;
using System.Text;
using ZoDream.Shared.Models;

namespace ZoDream.Shared.Interfaces
{
    public interface ICanvasLayer: IDisposable
    {

        public double Left { get; }
        public double Top { get; }
        public double ActualWidth { get; }
        public double ActualHeight { get; }

        public IList<PageItem> Content { get; set; }
        public int Page { get; set; }

        public void Add(IEnumerable<CharItem> items);

        public void Add(IEnumerable<PageItem> items);

        public bool IsVisible { get; }

        public void Clear();

        public void Move(double x, double y);

        public void Resize(double x, double y, double width, double height);

        public void Toggle(bool visible);

    }
}
