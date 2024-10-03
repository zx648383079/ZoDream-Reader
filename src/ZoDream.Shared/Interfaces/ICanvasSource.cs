using System.Threading.Tasks;
using ZoDream.Shared.Interfaces.Tokenizers;

namespace ZoDream.Shared.Interfaces
{
    public interface ICanvasSource
    {
        public INovelPage? Current { get; }

        public ICanvasAnimate Animator { get; }

        public Task ReadyAsync(ICanvasRender render);

        /// <summary>
        /// 更新了字体大小，页面尺寸修改要重新生成
        /// </summary>
        /// <returns></returns>
        public Task InvalidateAsync();
        /// <summary>
        /// 下一页
        /// </summary>
        /// <returns></returns>
        public Task<bool> ReadNextAsync();
        /// <summary>
        /// 上一页
        /// </summary>
        /// <returns></returns>
        public Task<bool> ReadPreviousAsync();
    }
}
