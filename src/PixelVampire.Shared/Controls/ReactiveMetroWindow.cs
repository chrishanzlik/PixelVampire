using MahApps.Metro.Controls;
using ReactiveUI;
using System.Windows;

namespace PixelVampire.Shared.Controls
{
    public abstract class ReactiveMetroWindow<TViewModel> : MetroWindow, IViewFor<TViewModel>, IActivatableView
        where TViewModel : class
    {
        static ReactiveMetroWindow()
        {
            ViewModelProperty = DependencyProperty.Register(nameof(ViewModel), typeof(TViewModel), typeof(ReactiveMetroWindow<TViewModel>));
        }

        public static readonly DependencyProperty ViewModelProperty;

        public TViewModel ViewModel
        {
            get => (TViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (TViewModel)value;
        }
    }
}
