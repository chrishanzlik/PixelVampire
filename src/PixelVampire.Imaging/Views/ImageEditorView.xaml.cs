using Microsoft.Win32;
using PixelVampire.Imaging.ViewModels;
using ReactiveUI;
using SkiaSharp;
using SkiaSharp.Views.Desktop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace PixelVampire.Imaging.Views
{
    /// <summary>
    /// Interaktionslogik für ImageEditorView.xaml
    /// </summary>
    public partial class ImageEditorView : ReactiveUserControl<ImageEditorViewModel>
    {
        public ImageEditorView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                this.OneWayBind(ViewModel,
                    x => x.Images.Count,
                    x => x.ExplorerColumn.Width,
                    x => x > 0 ? new GridLength(300) : new GridLength(0)).DisposeWith(d);

                this.OneWayBind(ViewModel,
                    x => x.SelectedImage,
                    x => x.SettingsColumn.Width,
                    x => x != null ? new GridLength(200) : new GridLength(0)).DisposeWith(d);

                this.OneWayBind(ViewModel,
                    x => x.Images,
                    x => x.ImageExplorer.ItemsSource).DisposeWith(d);

                this.OneWayBind(ViewModel,
                    x => x.IsLoading,
                    x => x.LoadingOverlay.Visibility,
                    x => x ? Visibility.Visible : Visibility.Collapsed).DisposeWith(d);

                this.Bind(ViewModel,
                    x => x.SelectedImage,
                    x => x.ImageExplorer.SelectedItem,
                    x => ViewModel.Images.FirstOrDefault(vm => vm.ImageHandle == x),
                    x => (x as ImageExplorerItemViewModel)?.ImageHandle).DisposeWith(d);

                // Hide next and prev buttons, when not needed
                ViewModel.WhenAnyValue(x => x.Images.Count)
                    .ObserveOn(RxApp.MainThreadScheduler)
                    .Subscribe(cnt => {
                        NextButton.Visibility = cnt > 1 ? Visibility.Visible : Visibility.Collapsed;
                        PrevButton.Visibility = cnt > 1 ? Visibility.Visible : Visibility.Collapsed;
                    })
                    .DisposeWith(d);

                // Load files from dialog
                Observable.Merge(
                        ClicksOf(SelectFilesExplorerButton),
                        ClicksOf(SelectFilesButton))
                    .ObserveOnDispatcher()
                    .Select(_ => SelectFilesByDialog())
                    .Where(x => x != null)
                    .SelectMany(x => x)
                    .InvokeCommand(ViewModel.LoadImage)
                    .DisposeWith(d);

                // Load files from drag & drop
                this.Events().Drop
                    .Select(x => x.Data.GetData(DataFormats.FileDrop) as string[])
                    .Where(x => x != null)
                    .Select(x => x.Where(x => !string.IsNullOrEmpty(x)))
                    .SelectMany(x => x)
                    .InvokeCommand(ViewModel.LoadImage)
                    .DisposeWith(d);

                // Redraw preview when selection changes
                ViewModel
                    .WhenAnyValue(x => x.SelectedImage)
                    .ObserveOn(RxApp.MainThreadScheduler)
                    .Subscribe(img => {
                        SelectFilesBlock.Visibility = img != null ? Visibility.Collapsed : Visibility.Visible;
                        Canvas.Visibility = img != null ? Visibility.Visible : Visibility.Collapsed;
                        Canvas.InvalidateVisual();
                    })
                    .DisposeWith(d);

                Canvas.PaintSurface += (o, e) =>
                {
                    e.Surface.Canvas.Clear();
                    if (ViewModel.SelectedImage != null)
                        DrawPreview(e.Surface.Canvas);
                };
            });
        }

        private void DrawPreview(SKCanvas canv)
        {
            if (double.IsNaN(Canvas.ActualWidth) || double.IsNaN(Canvas.ActualHeight)) return;

            var prev = ViewModel.SelectedImage.Preview;

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

        private static IEnumerable<string> SelectFilesByDialog()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Multiselect = true,
                Filter = "Images|*.jpg;*.jpeg;*.png;*.gif;*.bmp"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                return openFileDialog.FileNames;
            }

            return null;
        }

        private IObservable<EventPattern<RoutedEventArgs>> ClicksOf(Button button)
        {
            return Observable.FromEventPattern<RoutedEventHandler, RoutedEventArgs>(
                x => button.Click += x,
                x => button.Click -= x);
        }
    }
}
