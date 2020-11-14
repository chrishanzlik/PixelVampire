using DynamicData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelVampire.ViewModels.Abstractions
{
    public interface IImageEditorViewModel
    {
        ISourceList<string> FilePaths { get; }
    }
}
