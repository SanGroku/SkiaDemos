using System;
using System.Diagnostics;

using Xamarin.Forms;

using SkiaSharp;
using SkiaSharp.Views.Forms;

namespace SkiaSharpFormsDemos.Transforms
{
    public class AnimatedHendecagramArray : ContentPage
    {
        const double cycleTime = 5000;      // in milliseconds

        SKCanvasView canvasView;
        Stopwatch stopwatch = new Stopwatch();
        bool pageIsActive;
        float angle;

        public AnimatedHendecagramArray()
        {
            Title = "Hendecagram Array Animation";

            canvasView = new SKCanvasView();
            canvasView.PaintSurface += OnCanvasViewPaintSurface;
            Content = canvasView;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            pageIsActive = true;
            stopwatch.Start();

            Device.StartTimer(TimeSpan.FromMilliseconds(33), () =>
            {
                double t = stopwatch.Elapsed.TotalMilliseconds % cycleTime / cycleTime;
                angle = (float)(360 * t);
                canvasView.InvalidateSurface();

                if (!pageIsActive)
                {
                    stopwatch.Stop();
                }

                return pageIsActive;
            });
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            pageIsActive = false;
        }

        void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();
            canvas.Translate(info.Width / 2, info.Height / 2);
            float radius = (float)Math.Min(info.Width, info.Height) / 2 - 100;

            using (SKPaint paint = new SKPaint())
            {
                for (int x = 100; x < info.Width + 100; x += 200)
                {
                    for (int y = 100; y < info.Height + 100; y += 200)
                    {
                        paint.Style = SKPaintStyle.Fill;
                        paint.Color = SKColor.FromHsl(angle, 100, 50);

                        float a = radius * (float)Math.Sin(Math.PI * angle / 180);
                        float b = -radius * (float)Math.Cos(Math.PI * angle / 180);
                        canvas.Translate(a, b);
                        canvas.DrawPath(HendecagramArrayPage.HendecagramPath, paint);
                    }
                }
            }
        }
    }
}