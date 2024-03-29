﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ZoDream.Shared.Interfaces
{
    public interface ICanvasAnimate: IDisposable
    {
        public bool HasAnimate { get; }

        public void MoveStart(ICanvasLayer layer, double x, double y);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="layer"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns>返回一个进度/100,负数代表反方向</returns>
        public double Move(ICanvasLayer layer, double x, double y);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="toNext"></param>
        /// <param name="offset">/100负数代表反方向</param>
        /// <returns>返回是否结束</returns>
        public bool Animate(IList<ICanvasLayer> layers, double offset, bool isTouch);
    }
}
