﻿using PixelVampire.Imaging.ViewModels;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PixelVampire.Imaging.Views
{
    /// <summary>
    /// Interaktionslogik für ImageExplorerItemView.xaml
    /// </summary>
    public partial class ImageExplorerItemView : ReactiveUserControl<ImageExplorerItemViewModel>
    {
        public ImageExplorerItemView()
        {
            InitializeComponent();

            this.WhenActivated(d =>
            {
                this.BindCommand(ViewModel,
                    x => x.Remove,
                    x => x.RemoveButton).DisposeWith(d);

                this.OneWayBind(ViewModel,
                    x => x.ImageHandle.OriginalName,
                    x => x.FileNameText.Text).DisposeWith(d);

                this.OneWayBind(ViewModel,
                    x => x.ImageHandle.Thumbnail,
                    x => x.ThumbnailImage.Source,
                    x => x.ToBitmapImage());
            });
        }
    }
}
