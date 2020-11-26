using PixelVampire.Imaging.Models;
using System;
using System.Collections.ObjectModel;

namespace PixelVampire.Imaging.ViewModels.Abstractions
{
    /// <summary>
    /// Viewmodel for browsing and displaying a list of loaded images.
    /// </summary>
    public interface IImageExplorerViewModel
    {
        /// <summary>
        /// Gets or sets the selected image inside the explorer.
        /// </summary>
        IImageExplorerItemViewModel SelectedItem { get; set; }

        /// <summary>
        /// Gets all loaded images.
        /// </summary>
        ReadOnlyObservableCollection<IImageExplorerItemViewModel> Children { get; }

        /// <summary>
        /// Gets an observable which notifies new image selections.
        /// </summary>
        IObservable<ImageHandle> Selections { get; }

        /// <summary>
        /// Gets an observable which notifies about image deletion requests.
        /// </summary>
        IObservable<ImageHandle> DeletionRequests { get; }

        /// <summary>
        /// Select the next image.
        /// </summary>
        void SelectNext();

        /// <summary>
        /// Select the previous image.
        /// </summary>
        void SelectPrevious();
    }
}
