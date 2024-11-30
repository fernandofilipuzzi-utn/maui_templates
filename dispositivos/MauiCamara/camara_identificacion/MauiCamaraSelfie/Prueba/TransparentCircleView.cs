using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prueba
{
    public class TransparentCircleView : SKCanvasView
    {

        public float SquareSize { get; set; } = 300;  
        public float OvalWidth { get; set; } = 180;   
        public float OvalHeight { get; set; } = 100;  

        protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
        {
            base.OnPaintSurface(e);

            var canvas = e.Surface.Canvas;
            var info = e.Info;

            float centerX = info.Width / 2;
            float centerY = info.Height / 2;

            canvas.Clear(SKColors.Transparent);

            using (var paint = new SKPaint())
            {
                paint.Style = SKPaintStyle.Fill; 
                paint.Color = SKColors.White; 

                canvas.DrawRect(centerX - SquareSize / 2, centerY - SquareSize / 2, SquareSize, SquareSize, paint);
            }

            using (var paint = new SKPaint())
            {
                paint.Style = SKPaintStyle.Fill;  
                paint.Color = SKColors.Transparent; 

                canvas.DrawOval(centerX, centerY, OvalWidth / 2, OvalHeight / 2, paint);
            }
        }
    }
}
