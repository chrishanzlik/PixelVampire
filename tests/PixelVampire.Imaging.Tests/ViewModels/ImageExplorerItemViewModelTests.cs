using Microsoft.Reactive.Testing;
using PixelVampire.Imaging.Models;
using PixelVampire.Imaging.ViewModels;
using PixelVampire.Imaging.ViewModels.Abstractions;
using ReactiveUI.Testing;
using System;
using Xunit;

namespace PixelVampire.Imaging.Tests.ViewModels
{
    public class ImageExplorerItemViewModelTests
    {
        [Fact]
        public void ImageExplorerItemViewModel_ThrowsArgumentNull_WhenItemParamIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new ImageExplorerItemViewModel(null));
        }

        [Fact]
        public void ImageExplorerItemViewModel_RequestCloseCommandReturnsViewModel_Always() =>
            new TestScheduler().With(scheduler =>
            {
                IImageExplorerItemViewModel expected = default;

                var explorerItem = new ImageExplorerItem("dummy/path", new SkiaSharp.SKBitmap());
                var sut = new ImageExplorerItemViewModel(explorerItem);
                sut.RequestRemove.Execute().Subscribe(res => expected = res);

                scheduler.AdvanceBy(1);

                Assert.Equal(expected, sut);
            });
    }
}
