using System;
using System.Numerics;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;
using Microsoft.Graphics.Canvas.Geometry;

namespace PathAnimationCrash
{
    public sealed partial class MainPage : Page
    {
        private readonly Compositor _compositor;

        public MainPage()
        {
            this.InitializeComponent();

            _compositor = Window.Current.Compositor;

            Loaded += async (s, e) =>
            {
                await Task.Delay(2000);

                CreateAnimatedChart1();
            };
        }

        private void CreateAnimatedChart1()
        {
            var shapeVisual = _compositor.CreateShapeVisual();
            shapeVisual.RelativeSizeAdjustment = Vector2.One;
            var viewBox = _compositor.CreateViewBox();
            viewBox.Size = new Vector2(500, 200);
            viewBox.Stretch = CompositionStretch.Fill;
            shapeVisual.ViewBox = viewBox;
            ElementCompositionPreview.SetElementChildVisual(ChartArea1, shapeVisual);

            var chart = _compositor.CreateSpriteShape();
            chart.CenterPoint = viewBox.Size / 2;
            chart.FillBrush = _compositor.CreateColorBrush(Colors.LightBlue);
            var chartGeometry = _compositor.CreatePathGeometry();

            var builder1 = new CanvasPathBuilder(null);
            builder1.BeginFigure(new Vector2(0, 200));
            builder1.AddLine(new Vector2(0, 100));
            builder1.AddCubicBezier(new Vector2(30, 100), new Vector2(60), new Vector2(100, 60));
            builder1.AddCubicBezier(new Vector2(140, 60), new Vector2(200, 120), new Vector2(250, 120));
            builder1.AddCubicBezier(new Vector2(350, 120), new Vector2(340, 40), new Vector2(380, 40));
            builder1.AddCubicBezier(new Vector2(420, 40), new Vector2(400, 120), new Vector2(500, 120));
            builder1.AddLine(new Vector2(500, 200));
            builder1.EndFigure(CanvasFigureLoop.Closed);
            var curvedPath = new CompositionPath(CanvasGeometry.CreatePath(builder1));

            var builder2 = new CanvasPathBuilder(null);
            builder2.BeginFigure(new Vector2(0, 200));
            builder2.AddLine(new Vector2(0, 100));
            builder2.AddCubicBezier(new Vector2(30, 100), new Vector2(60, 100), new Vector2(100, 100));
            builder2.AddCubicBezier(new Vector2(140, 100), new Vector2(200, 100), new Vector2(250, 100));
            builder2.AddCubicBezier(new Vector2(350, 100), new Vector2(340, 100), new Vector2(380, 100));
            builder2.AddCubicBezier(new Vector2(420, 100), new Vector2(400, 100), new Vector2(500, 100));
            builder2.AddLine(new Vector2(500, 200));
            builder2.EndFigure(CanvasFigureLoop.Closed);
            var flatPath = new CompositionPath(CanvasGeometry.CreatePath(builder2));

            chartGeometry.Path = flatPath;
            chart.Geometry = chartGeometry;
            shapeVisual.Shapes.Add(chart);

            var pathAnimation = _compositor.CreatePathKeyFrameAnimation();
            pathAnimation.InsertKeyFrame(1, curvedPath);
            pathAnimation.Duration = TimeSpan.FromMilliseconds(800);
            pathAnimation.DelayTime = TimeSpan.FromMilliseconds(2000);
            chartGeometry.StartAnimation("Path", pathAnimation);
        }
    }
}
