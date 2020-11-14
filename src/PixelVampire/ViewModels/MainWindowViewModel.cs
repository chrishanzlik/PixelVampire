using PixelVampire.ViewModels.Abstractions;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PixelVampire.ViewModels
{
    public class MainWindowViewModel : ViewModelBase, IScreen
    {
        public MainWindowViewModel()
        {
            Router = new RoutingState();

            Router.Navigate.Execute(new ImageEditorViewModel());
        }

        public RoutingState Router { get; }
    }
}
