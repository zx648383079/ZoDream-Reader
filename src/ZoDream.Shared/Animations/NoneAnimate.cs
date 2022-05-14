using System;
using System.Collections.Generic;
using System.Text;
using ZoDream.Shared.Interfaces;

namespace ZoDream.Shared.Animations
{
    public class NoneAnimate : ICanvasAnimate
    {
        public bool HasAnimate => false;

        private double LastX;

        public double Move(ICanvasLayer layer, double x, double y)
        {
            var offset = LastX - x;
            return offset * 100 / layer.ActualWidth;
        }

        public void MoveStart(ICanvasLayer layer, double x, double y)
        {
            LastX = x;
        }

        public bool Animate(IList<ICanvasLayer> layers, double offset, bool isTouch)
        {
            if (!isTouch)
            {
                layers[2].Toggle(false);
                layers[0].Toggle(false);
                layers[1].Move(0, 0);
                return true;
            }
            var val = offset * layers[1].ActualWidth / 100;
            if (offset > 0)
            {
                layers[2].Toggle(false);
                layers[0].Toggle(true);
                layers[1].Move(-val, layers[1].Top);
            } else if (offset < 0)
            {
                layers[2].Toggle(true);
                layers[2].Move(-val, layers[2].Top);
            }
            return true;
        }

        public void Dispose()
        {
            
        }

        
    }
}
