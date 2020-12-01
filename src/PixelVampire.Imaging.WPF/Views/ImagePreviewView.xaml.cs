using PixelVampire.Imaging.ViewModels;
using ReactiveUI;
using SkiaSharp;
using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows;

namespace PixelVampire.Imaging.WPF.Views
{
    /// <summary>
    /// Interaktionslogik für ImageExplorerItemView.xaml
    /// </summary>
    public partial class ImagePreviewView
    {
        public ImagePreviewView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                this.WhenAnyValue(x => x.ViewModel.Image)
                    .ObserveOn(RxApp.MainThreadScheduler)
                    .Subscribe(img => {
                        Canvas.Visibility = img != null ? Visibility.Visible : Visibility.Collapsed;
                        Canvas.InvalidateVisual();
                    })
                    .DisposeWith(d);

                Canvas.PaintSurface += (o, e) =>
                {
                    e.Surface.Canvas.Clear();
                    if (ViewModel.Image != null)
                        DrawPreview(e.Surface.Canvas);
                };
            });
        }

        private void DrawPreview(SKCanvas canv)
        {
            if (double.IsNaN(Canvas.ActualWidth) || double.IsNaN(Canvas.ActualHeight)) return;

            var prev = ViewModel.Image;

            if (prev.Width > Canvas.ActualWidth || prev.Height > Canvas.ActualHeight)
            {
                using var draw = prev.ResizeFixedRatio((int)Canvas.ActualWidth, (int)Canvas.ActualHeight, SKFilterQuality.Medium);
                var top = (int)Math.Ceiling(Canvas.ActualHeight - draw.Height) / 2;
                var left = (int)Math.Ceiling(Canvas.ActualWidth - draw.Width) / 2;
                canv.DrawBitmap(draw, new SKPoint(left, top));
            }
            else
            {
                var top = (int)Math.Ceiling(Canvas.ActualHeight - prev.Height) / 2;
                var left = (int)Math.Ceiling(Canvas.ActualWidth - prev.Width) / 2;
                canv.DrawBitmap(prev, new SKPoint(left, top));
            }
        }
    }
}
