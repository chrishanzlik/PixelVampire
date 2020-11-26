using PixelVampire.Imaging.Models;
using System;
using System.Collections.ObjectModel;

namespace PixelVampire.Imaging.ViewModels.Abstractions
{
    public interface IImageExplorerViewModel
    {
        ImageExplorerItemViewModel SelectedItem { get; set; }

        ReadOnlyObservableCollection<ImageExplorerItemViewModel> Children { get; }

        IObservable<ImageHandle> Selections { get; }

        IObservable<ImageHandle> Deletions { get; }

        void SelectNext();

        void SelectPrevious();
    }
}
