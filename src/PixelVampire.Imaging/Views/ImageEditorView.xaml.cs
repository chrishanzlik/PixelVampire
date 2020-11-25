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
using System.Windows.Controls.Primitives;

namespace PixelVampire.Imaging.Views
{
    /// <summary>
    /// Interaktionslogik für ImageEditorView.xaml
    /// </summary>
    public partial class ImageEditorView
    {
        public ImageEditorView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                this.BindCommand(ViewModel,
                    x => x.SelectNext,
                    x => x.NextButton).DisposeWith(d);

                this.BindCommand(ViewModel,
                    x => x.SelectPrevious,
                    x => x.PrevButton).DisposeWith(d);

                this.OneWayBind(ViewModel,
                    x => x.Images.Count,
                    x => x.ExplorerColumn.Width,
                    x => x > 0 ? new GridLength(300) : new GridLength(0)).DisposeWith(d);

                this.OneWayBind(ViewModel,
                    x => x.SelectedImage,
                    x => x.SettingsColumn.Width,
                    x => x != null ? new GridLength(200) : new GridLength(0)).DisposeWith(d);

                this.OneWayBind(ViewModel,
                    x => x.IsLoading,
                    x => x.LoadingOverlay.Visibility,
                    x => x ? Visibility.Visible : Visibility.Collapsed).DisposeWith(d);

                this.OneWayBind(ViewModel,
                    x => x.ImageExplorer,
                    x => x.Explorer.ViewModel).DisposeWith(d);

                this.OneWayBind(ViewModel,
                    x => x.ImagePreview,
                    x => x.Preview.ViewModel).DisposeWith(d);

                // Load files from dialog
                Observable.Merge(
                        SelectFilesExplorerButton.Events().Click,
                        SelectFilesButton.Events().Click)
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

                this.WhenAnyValue(x => x.ViewModel.SelectedImage)
                    .ObserveOn(RxApp.MainThreadScheduler)
                    .Subscribe(img => {
                        SelectFilesBlock.Visibility = img != null ? Visibility.Collapsed : Visibility.Visible;
                        Preview.Visibility = img != null ? Visibility.Visible : Visibility.Collapsed;
                    })
                    .DisposeWith(d);

                this.WhenAnyValue(x => x.ViewModel.Images.Count)
                    .ObserveOnDispatcher()
                    .Subscribe(cnt =>
                    {
                        PrevButton.Visibility = cnt > 1 ? Visibility.Visible : Visibility.Collapsed;
                        NextButton.Visibility = cnt > 1 ? Visibility.Visible : Visibility.Collapsed;
                    })
                    .DisposeWith(d);
            });
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
    }
}
