using DynamicData;
using Microsoft.Reactive.Testing;
using PixelVampire.Imaging.Models;
using PixelVampire.Imaging.ViewModels;
using ReactiveUI.Testing;
using SkiaSharp;
using System;
using System.Linq;
using System.Reactive.Linq;
using Xunit;

namespace PixelVampire.Imaging.Tests.ViewModels
{
    public class ImageExplorerViewModelTests
    {
        [Fact]
        public void ImageExplorerViewModel_ThrowsArgumentNull_WhenObservableCacheIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => new ImageExplorerViewModel(null));
        }

        [Fact]
        public void ImageExplorerViewModel_SyncsIntialItemsFromCache_WhenItemsAvailable() =>
            new TestScheduler().With(scheduler =>
            {
                var source = new SourceCache<ImageHandle, string>(x => x.LoadingSettings.FilePath);
                source.AddOrUpdate(GetDummyHandle("foo"));

                var sut = new ImageExplorerViewModel(source.AsObservableCache());
                sut.Activator.Activate();

                scheduler.AdvanceBy(2);

                Assert.Equal(1, sut.Children.Count);
            });

        [Fact]
        public void ImageExplorerViewModel_SelectNextSelectsNextItem_WhenAtLeastTwoItemsAvailable() =>
            new TestScheduler().With(scheduler =>
            {
                var source = new SourceCache<ImageHandle, string>(x => x.LoadingSettings.FilePath);
                source.AddOrUpdate(GetDummyHandle("foo"));
                source.AddOrUpdate(GetDummyHandle("bar"));

                var sut = new ImageExplorerViewModel(source.AsObservableCache());
                sut.Activator.Activate();
                scheduler.AdvanceBy(2);

                Assert.Equal("foo", sut.SelectedItem.ExplorerItem.FilePath);

                sut.SelectNext.Execute().Subscribe();
                scheduler.AdvanceBy(1);

                Assert.Equal("bar", sut.SelectedItem.ExplorerItem.FilePath);
            });

        [Fact]
        public void ImageExplorerViewModel_SelectPrevSelectsPrevItem_WhenAtLeastTwoItemsAvailable() =>
            new TestScheduler().With(scheduler =>
            {
                var source = new SourceCache<ImageHandle, string>(x => x.LoadingSettings.FilePath);
                source.AddOrUpdate(GetDummyHandle("foo"));
                source.AddOrUpdate(GetDummyHandle("bar"));
                source.AddOrUpdate(GetDummyHandle("baz"));

                var sut = new ImageExplorerViewModel(source.AsObservableCache());
                sut.Activator.Activate();
                scheduler.AdvanceBy(2);

                Assert.Equal("foo", sut.SelectedItem.ExplorerItem.FilePath);

                sut.SelectPrevious.Execute().Subscribe();
                scheduler.AdvanceBy(1);

                Assert.Equal("baz", sut.SelectedItem.ExplorerItem.FilePath);
            });

        [Fact]
        public void ImageExplorerViewModel_SelectionsObservableFires_WhenNewItemSelected() =>
            new TestScheduler().With(scheduler =>
            {
                bool success = false;

                var source = new SourceCache<ImageHandle, string>(x => x.LoadingSettings.FilePath);
                source.AddOrUpdate(GetDummyHandle("foo"));
                source.AddOrUpdate(GetDummyHandle("bar"));

                var sut = new ImageExplorerViewModel(source.AsObservableCache());
                sut.Activator.Activate();
                scheduler.AdvanceBy(2);
                sut.Selections.Where(x => x.LoadingSettings.FilePath == "bar").Subscribe(_ => success = true);
                sut.SelectNext.Execute().Subscribe();
                scheduler.AdvanceBy(2);

                Assert.True(success);
            });

        private static ImageHandle GetDummyHandle(string name)
        {
            var bmp = new SKBitmap(10, 10);
            return new ImageHandle(name, bmp, SKEncodedImageFormat.Jpeg)
            {
                Preview = bmp
            };
        }
    }
}
