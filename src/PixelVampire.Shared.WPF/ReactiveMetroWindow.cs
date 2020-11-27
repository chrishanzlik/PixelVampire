using MahApps.Metro.Controls;
using ReactiveUI;
using System.Windows;

namespace PixelVampire.Controls.WPF
{
    /// <summary>
    /// Basic window view which combines MahApps with RxUi
    /// </summary>
    /// <typeparam name="TViewModel">Type of the viewmodel.</typeparam>
    public abstract class ReactiveMetroWindow<TViewModel> : MetroWindow, IViewFor<TViewModel>, IActivatableView
        where TViewModel : class
    {
        static ReactiveMetroWindow()
        {
            ViewModelProperty = DependencyProperty.Register(nameof(ViewModel), typeof(TViewModel), typeof(ReactiveMetroWindow<TViewModel>));
        }

        public static readonly DependencyProperty ViewModelProperty;

        /// <inheritdoc />
        public TViewModel ViewModel
        {
            get => (TViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        /// <inheritdoc />
        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (TViewModel)value;
        }
    }
}
