using PixelVampire.Imaging.Models;
using ReactiveUI;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;

namespace PixelVampire.Imaging.ViewModels.Abstractions
{
    /// <summary>
    /// Viewmodel which contains all image manipulation capabilities.
    /// </summary>
    public interface IImageEditorViewModel
    {
        /// <summary>
        /// Load a new image from a local file path.
        /// </summary>
        ReactiveCommand<string, ImageHandle> LoadImage { get; }

        /// <summary>
        /// Exports the selected image.
        /// </summary>
        ReactiveCommand<Unit, string> ExportSelected { get; }

        /// <summary>
        /// Exports all loaded images.
        /// </summary>
        ReactiveCommand<Unit, string> ExportAll { get; }

        /// <summary>
        /// Waits that the user is selecting a folder.
        /// </summary>
        Interaction<string, string> SelectFolder { get; }

        /// <summary>
        /// Gets or sets the selected image.
        /// </summary>
        ImageHandle SelectedImage { get; set; }

        /// <summary>
        /// Gets a value indicating whether this object is busy or not.
        /// </summary>
        bool IsLoading { get; }

        /// <summary>
        /// Gets all loaded images.
        /// </summary>
        ReadOnlyObservableCollection<ImageHandle> Images { get; }

        /// <summary>
        /// Gets the image explorer.
        /// </summary>
        IImageExplorerViewModel ImageExplorer { get; }

        /// <summary>
        /// Gets the image previewer.
        /// </summary>
        IImagePreviewViewModel ImagePreview { get; }

        /// <summary>
        /// Gets the current image settings.
        /// </summary>
        IImageSettingsViewModel Settings { get; }
    }
}
