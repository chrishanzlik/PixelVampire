using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelVampire.Imaging
{
    public class ImageLoadResult
    {
        internal ImageLoadResult()
        {
            Images = new List<ImageHandle>();
        }

        public IList<ImageHandle> Images { get; set; }
        public int LoadedCount { get; set; }
        public int RejectedCount { get; set; }
    }
}
