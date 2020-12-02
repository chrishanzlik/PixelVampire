using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace PixelVampire.Imaging.Models
{
    /// <summary>
    /// The actual manipulation state of a loaded image.
    /// </summary>
    public class ManipulationState : ReactiveObject
    {
        /// <summary>
        /// Gets or sets the quality of the image. Value between 0 and 100.
        /// </summary>
        [Reactive] public int Quality { get; set; } = 100;
    }
}
