using Microsoft.Win32;
using PixelVampire.Imaging.ViewModels;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
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
                Observable.FromEventPattern<RoutedEventHandler, RoutedEventArgs>(
                        x => this.SelectFilesButton.Click += x,
                        x => this.SelectFilesButton.Click -= x)
                    .ObserveOnDispatcher()
                    .Select(_ => SelectFilesByDialog())
                    .Where(x => x != null)
                    .InvokeCommand(ViewModel.LoadImages)
                    .DisposeWith(d);

                this.Events().Drop
                    .Select(x => x.Data.GetData(DataFormats.FileDrop) as string[])
                    .Where(x => x != null)
                    .Select(x => x.Where(x => !string.IsNullOrEmpty(x)))
                    .InvokeCommand(ViewModel.LoadImages)
                    .DisposeWith(d);

                this.OneWayBind(ViewModel,
                    x => x.Images,
                    x => x.ImageExplorer.ItemsSource).DisposeWith(d);

                ViewModel.LoadImages.Where(x => x.Images.Any()).ObserveOnDispatcher().Subscribe(x =>
                {
                    Canvas.InvalidateVisual();
                });

                Canvas.PaintSurface += (o, e) =>
                {
                    e.Surface.Canvas.Clear();
                    //var thumb = ViewModel.Images?.LastOrDefault()?.OriginalImage;
                    //if (thumb != null)
                    //    e.Surface.Canvas.DrawBitmap(thumb, new SkiaSharp.SKPoint(0,0));
                };
            });
        }

        private static IEnumerable<string> SelectFilesByDialog()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Multiselect = true,
                Filter = "Images|*.jpg;*.jpeg;*.png"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                return openFileDialog.FileNames;
            }

            return null;
        }
    }
}
