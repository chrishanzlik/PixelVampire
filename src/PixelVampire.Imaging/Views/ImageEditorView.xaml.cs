using Microsoft.Win32;
using PixelVampire.Imaging.ViewModels;
using ReactiveUI;
using System;
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
                    .Subscribe(_ => SelectFilesByDialog())
                    .DisposeWith(d);

                this.Events().Drop
                    .Select(x => x.Data.GetData(DataFormats.FileDrop) as string[])
                    .Where(x => x != null)
                    .Select(x => x.Where(x => !string.IsNullOrEmpty(x)))
                    .Subscribe(x => this.ViewModel.LoadFiles(x))
                    .DisposeWith(d);
            });
        }

        private void SelectFilesByDialog()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "Images|*.jpg;*.jpeg;*.png";
            if (openFileDialog.ShowDialog() == true)
            {
                this.ViewModel.LoadFiles(openFileDialog.FileNames);
            }
        }
    }
}
