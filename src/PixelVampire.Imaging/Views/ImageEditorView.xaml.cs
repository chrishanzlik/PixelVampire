﻿using Microsoft.Win32;
using PixelVampire.Imaging.ViewModels;
using ReactiveUI;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
                    x => x.Images,
                    x => x.ImageExplorer.ItemsSource).DisposeWith(d);

                //TEST: Convert ImageHandle <-> ImageExplorerItemViewModel
                //this.Bind(ViewModel,
                //    x => x.SelectedImage,
                //    x => x.ImageExplorer.SelectedItem).DisposeWith(d);

                // Load files from dialog
                Observable.FromEventPattern<RoutedEventHandler, RoutedEventArgs>(
                        x => this.SelectFilesButton.Click += x,
                        x => this.SelectFilesButton.Click -= x)
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
                    .Subscribe(_ => Canvas.InvalidateVisual())
                    .DisposeWith(d);

                Canvas.PaintSurface += (o, e) =>
                {
                    e.Surface.Canvas.Clear();
                    if (ViewModel.SelectedImage != null)
                    {
                        e.Surface.Canvas.DrawBitmap(ViewModel.SelectedImage.Preview, new SKPoint(0, 0));
                    }
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
