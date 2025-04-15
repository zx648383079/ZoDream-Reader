
using System.Drawing;

namespace ZoDream.Shared.Animations
{
    internal class PageFlipEffect
    {
        // 页面大小
        private Size pageSize;

        // 翻页控制点
        private PointF dragPoint;
        private PointF cornerPoint;

        // 构造函数
        public PageFlipEffect(Size size)
        {
            pageSize = size;
            cornerPoint = new PointF(pageSize.Width, 0);
        }

        // 设置拖拽点
        public void SetDragPoint(PointF point)
        {
            dragPoint = point;
        }

        // 绘制翻页效果
        //public void Draw(Graphics g, Image pageImage)
        //{
        //    if (pageImage == null) return;

        //    // 计算基本控制点
        //    PointF[] pts = CalculateControlPoints();

        //    // 创建翻页区域
        //    GraphicsPath path = CreatePageFlipPath(pts);

        //    // 绘制背面阴影
        //    DrawBackShadow(g, pts);

        //    // 绘制翻起的页面
        //    DrawFlippingPage(g, pageImage, path, pts);

        //    // 绘制正面阴影
        //    DrawFrontShadow(g, pts);
        //}

        // 计算控制点
        private PointF[] CalculateControlPoints()
        {
            PointF[] pts = new PointF[4];

            // 起点是右上角
            pts[0] = cornerPoint;

            // 终点是拖拽点
            pts[3] = dragPoint;

            // 中间控制点1
            pts[1] = new PointF(
                cornerPoint.X - (cornerPoint.X - dragPoint.X) / 2,
                cornerPoint.Y);

            // 中间控制点2
            pts[2] = new PointF(
                cornerPoint.X,
                cornerPoint.Y + (dragPoint.Y - cornerPoint.Y) / 2);

            return pts;
        }

        // 创建翻页路径
        //private GraphicsPath CreatePageFlipPath(PointF[] pts)
        //{
        //    GraphicsPath path = new GraphicsPath();

        //    // 从右上角开始
        //    path.AddLine(pts[0], new PointF(0, 0));
        //    path.AddLine(new PointF(0, 0), new PointF(0, pageSize.Height));
        //    path.AddLine(new PointF(0, pageSize.Height), new PointF(pts[3].X, pageSize.Height));

        //    // 添加贝塞尔曲线形成翻页边缘
        //    path.AddBezier(pts[3], pts[2], pts[1], pts[0]);

        //    return path;
        //}

        //// 绘制背面阴影
        //private void DrawBackShadow(Graphics g, PointF[] pts)
        //{
        //    // 创建阴影路径
        //    GraphicsPath shadowPath = new GraphicsPath();
        //    shadowPath.AddLine(new PointF(0, 0), new PointF(0, pageSize.Height));
        //    shadowPath.AddLine(new PointF(0, pageSize.Height), pts[3]);
        //    shadowPath.AddBezier(pts[3], pts[2], pts[1], pts[0]);
        //    shadowPath.AddLine(pts[0], new PointF(0, 0));

        //    // 绘制渐变阴影
        //    PathGradientBrush shadowBrush = new PathGradientBrush(shadowPath);
        //    shadowBrush.CenterColor = Color.FromArgb(180, Color.Black);
        //    shadowBrush.SurroundColors = new Color[] { Color.Transparent };

        //    g.FillPath(shadowBrush, shadowPath);
        //}

        //// 绘制翻起的页面
        //private void DrawFlippingPage(Graphics g, Image pageImage, GraphicsPath path, PointF[] pts)
        //{
        //    // 设置裁剪区域
        //    g.SetClip(path);

        //    // 创建变形矩阵
        //    Matrix m = new Matrix();

        //    // 根据翻页程度调整变形
        //    float flipRatio = (pts[0].X - pts[3].X) / pageSize.Width;

        //    // 绘制页面内容
        //    if (flipRatio > 0.5f)
        //    {
        //        // 翻页过半，显示背面
        //        g.DrawImage(pageImage, new Rectangle(0, 0, pageSize.Width, pageSize.Height));
        //    }
        //    else
        //    {
        //        // 翻页未过半，显示正面
        //        g.DrawImage(pageImage, new Rectangle(0, 0, pageSize.Width, pageSize.Height));
        //    }

        //    g.ResetClip();
        //}

        //// 绘制正面阴影
        //private void DrawFrontShadow(Graphics g, PointF[] pts)
        //{
        //    // 创建阴影路径
        //    GraphicsPath frontShadowPath = new GraphicsPath();
        //    frontShadowPath.AddBezier(pts[3], pts[2], pts[1], pts[0]);

        //    // 绘制阴影
        //    using (Pen shadowPen = new Pen(Color.FromArgb(100, Color.Black), 15))
        //    {
        //        g.DrawPath(shadowPen, frontShadowPath);
        //    }
        //}
    }
}
