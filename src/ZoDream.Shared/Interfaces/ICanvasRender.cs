using System.Numerics;
using ZoDream.Shared.Events;

namespace ZoDream.Shared.Interfaces
{
    public interface ICanvasRender
    {
        public ICanvasSource? Source {  get; set; }

        public Vector2 Size { get; }

        /// <summary>
        /// 翻页完成事件
        /// </summary>
        public event PageChangedEventHandler? PageChanged;

        /// <summary>
        /// 画布准备就绪事件
        /// </summary>
        public event CanvasReadyEventHandler? OnReady;

        public ICanvasLayer CreateLayer(Vector2 size);
        /// <summary>
        /// 刷新重绘画板内容
        /// </summary>
        public void Invalidate();
        /// <summary>
        /// 画一层
        /// </summary>
        /// <param name="layer"></param>
        public void DrawLayer(ICanvasLayer layer);
    }
}
