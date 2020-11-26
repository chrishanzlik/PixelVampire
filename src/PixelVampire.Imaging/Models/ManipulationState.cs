namespace PixelVampire.Imaging.Models
{
    /// <summary>
    /// The actual manipulation state of a loaded image.
    /// </summary>
    public class ManipulationState
    {
        /// <summary>
        /// Gets or sets the quality of the image. Value between 0 and 100.
        /// </summary>
        public int Quality { get; set; } = 100;
    }
}
