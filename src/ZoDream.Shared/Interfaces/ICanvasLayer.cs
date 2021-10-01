using System;
using System.Collections.Generic;
using System.Text;
using ZoDream.Shared.Models;

namespace ZoDream.Shared.Interfaces
{
    public interface ICanvasLayer<RenderTargetT, PointT, FontT, ColorT, ImageT>
    {

        public float X { get; set; }

        public float Y { get; set; }

        public float Width { get; set; }

        public float Height { get; set; }

        public int Page { get; set; }

        public void Add(IEnumerable<CharItem> items);

        public void Add(IEnumerable<PageItem> items);

        public void BeginSwap(PointT point);

        public void MoveSwap(PointT point);

        public void EndSwap();

        public void Clear();

        public void Draw(RenderTargetT target);

        public void Draw(RenderTargetT target, FontT font, ColorT foreground, ColorT background, ImageT? backgroundImage);
    }
}
