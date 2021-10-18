using Microsoft.Graphics.Canvas;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Windows.Foundation;
using Windows.UI.Xaml;
using ZoDream.Reader.Controls;
using ZoDream.Shared.Interfaces;
using ZoDream.Shared.Utils;

namespace ZoDream.Reader.Drawing.Animations
{
    public class CoverAnimate : ILayerAnimate
    {
        public CoverAnimate(PageCanvas canvas)
        {
            Canvas = canvas;
        }
        private PageCanvas Canvas;
        private float beginX = 0;
        private float lastX = 0;
        private bool isMove = false;
        private bool beginNextDirect = true;
        private bool lastNextDirect = true;
        private DispatcherTimer timer;
        private Action animateFunc;

        public bool HasAnimate => true;
        public bool IsFinished { get; set; } = true;

        private void initTimer()
        {
            if (timer != null)
            {
                return;
            }
            timer = new DispatcherTimer()
            {
                Interval = TimeSpan.FromMilliseconds(15),
            };
            timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, object e)
        {
            animateFunc?.Invoke();
        }

        public void TouchMoveBegin(Point p)
        {
            beginX = (float)p.X;
            lastX = beginX;
            isMove = false;
        }

        public void TouchMove(Point p)
        {
            if (!isMove)
            {
                beginNextDirect = p.X < beginX;
                isMove = true;
                initLayer();
            }
            var x = (float)p.X;
            lastNextDirect = x < lastX;
            lastX = x;
            if (beginNextDirect)
            {
                Canvas.layerItems[1].X = x - beginX;
                Canvas.Invalidate();
            } else if (Canvas.layerItems[0] != null)
            {
                Canvas.layerItems[0].X = x - beginX - Canvas.layerItems[0].Width;
                Canvas.Invalidate();
            }
            
        }

        private void initLayer()
        {
            if (beginNextDirect)
            {
                Canvas.layerItems[2] = Canvas.CreateLayer(Canvas.CurrentPage + App.ViewModel.Setting.ColumnCount);
                return;
            }
            Canvas.layerItems[0] = Canvas.CreateLayer(Canvas.CurrentPage - App.ViewModel.Setting.ColumnCount);
        }

        public void TouchMoveEnd(Point p)
        {
            if (!isMove)
            {
                // TODO 点击
                if (p.X < Canvas.ActualWidth / 3)
                {
                    Canvas.SwapPrevious();
                }
                else if (p.X > Canvas.ActualWidth * .7)
                {
                    Canvas.SwapNext();
                }
                return;
            }
            if (beginNextDirect)
            {
                if (p.X >= beginX || lastNextDirect != beginNextDirect || Canvas.layerItems[2] == null)
                {
                    isMove = false;
                    Animate(Canvas.layerItems[1], 0f, () =>
                    {
                        Canvas.Invalidate();
                    });
                    return;
                }
                Animate(Canvas.layerItems[1], -Canvas.layerItems[1].Width, () =>
                {
                    isMove = false;
                    Canvas.layerItems[0] = Canvas.layerItems[1];
                    Canvas.layerItems[1] = Canvas.layerItems[2];
                    Canvas.layerItems[2] = null;
                    Canvas.CurrentPage = Canvas.layerItems[1].Page;
                    Canvas.Invalidate();
                });
            } else
            {
                if (p.X <= beginX || lastNextDirect != beginNextDirect || Canvas.layerItems[0] == null)
                {
                    isMove = false;
                    return;
                }
                Animate(Canvas.layerItems[0], 0f, () =>
                {
                    isMove = false;
                    Canvas.layerItems[2] = Canvas.layerItems[1];
                    Canvas.layerItems[1] = Canvas.layerItems[0];
                    Canvas.layerItems[0] = null;
                    Canvas.CurrentPage = Canvas.layerItems[1].Page;
                    Canvas.Invalidate();
                });
            }
        }

        public void Animate(CanvasLayer layer, float distX, Action finished = null)
        {
            Animate(layer, distX < 0, layer.X, distX, finished);
        }


        public void Animate(CanvasLayer layer, bool isNextDirect, float srcX, float distX, Action finished = null)
        {
            if (isNextDirect)
            {
                if (srcX <= distX)
                {
                    layer.X = distX;
                    IsFinished = true;
                    finished?.Invoke();
                    return;
                }
            }
            else
            {
                if (srcX >= distX)
                {
                    layer.X = distX;
                    IsFinished = true;
                    finished?.Invoke();
                    return;
                }
            }
            var tween = new Tween<float>(srcX, distX, 1000, Tween<float>.EaseIn);
            Animate(() =>
            {
                layer.X = tween.Get();
                if (isNextDirect)
                {
                    if (layer.X <= distX)
                    {
                        timer?.Stop();
                        IsFinished = true;
                        finished?.Invoke();
                        return;
                    }
                }
                else
                {
                    if (layer.X >= distX)
                    {
                        timer?.Stop();
                        IsFinished = true;
                        finished?.Invoke();
                        return;
                    }
                }
                Canvas.Invalidate();
            });
        }

        public void Draw(CanvasDrawingSession ds)
        {
            if (isMove)
            {
                if (beginNextDirect)
                {
                    Canvas.layerItems[2]?.Draw(ds);
                    Canvas.layerItems[1]?.Draw(ds);
                }
                else
                {
                    Canvas.layerItems[1]?.Draw(ds);
                    Canvas.layerItems[0]?.Draw(ds);
                }
                return;
            }
            if (IsFinished)
            {
                Canvas.layerItems[1]?.Draw(ds);
                return;
            }
            if (beginNextDirect)
            {
                Canvas.layerItems[1]?.Draw(ds);
                Canvas.layerItems[0]?.Draw(ds);
            } else
            {
                Canvas.layerItems[2]?.Draw(ds);
                Canvas.layerItems[1]?.Draw(ds);
            }
        }

        public void Animate(bool toNext, Action finished)
        {
            beginNextDirect = toNext;
            if (toNext)
            {
                var pre = Canvas.layerItems[0];
                if (pre == null)
                {
                    IsFinished = true;
                    finished.Invoke();
                    Canvas?.Invalidate();
                    return;
                }
                var tween = new Tween<float>(0f, -pre.Width, 1000, Tween<float>.EaseIn);
                Animate(() =>
                {
                    pre.X = tween.Get();
                    if (pre.X <= -pre.Width)
                    {
                        timer?.Stop();
                        IsFinished = true;
                        finished.Invoke();
                    }
                    Canvas?.Invalidate();
                });
            } else
            {
                var pre = Canvas.layerItems[1];
                if (pre == null)
                {
                    IsFinished = true;
                    finished.Invoke();
                    Canvas?.Invalidate();
                    return;
                }
                var tween = new Tween<float>(-pre.Width, 0f, 1000, Tween<float>.EaseIn);
                Animate(() =>
                {
                    pre.X = tween.Get();
                    if (pre.X >= 0)
                    {
                        timer?.Stop();
                        IsFinished = true;
                        finished.Invoke();
                    }
                    Canvas?.Invalidate();
                });
            }
            
        }

        private void Animate(Action func)
        {
            animateFunc = func;
            initTimer();
            IsFinished = false;
            timer.Start();
        }

        public void Dispose()
        {
            timer?.Stop();
        }
    }
}
