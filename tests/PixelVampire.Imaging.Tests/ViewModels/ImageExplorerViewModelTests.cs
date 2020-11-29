using DynamicData;
using Microsoft.Reactive.Testing;
using Moq;
using PixelVampire.Imaging.Models;
using PixelVampire.Imaging.ViewModels;
using ReactiveUI.Testing;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
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
                var source = new SourceCache<ImageHandle, string>(x => x.OriginalPath);
                source.AddOrUpdate(new ImageHandle("foo", new SKBitmap(100, 100), SKEncodedImageFormat.Png));

                var sut = new ImageExplorerViewModel(source.AsObservableCache());
                sut.Activator.Activate();

                scheduler.AdvanceBy(2);

                Assert.Equal(1, sut.Children.Count);
            });

        [Fact]
        public void ImageExplorerViewModel_SelectNextSelectsNextItem_WhenAtLeastTwoItemsAvailable() =>
            new TestScheduler().With(scheduler =>
            {
                var source = new SourceCache<ImageHandle, string>(x => x.OriginalPath);
                source.AddOrUpdate(new ImageHandle("foo", new SKBitmap(100, 100), SKEncodedImageFormat.Png));
                source.AddOrUpdate(new ImageHandle("bar", new SKBitmap(100, 100), SKEncodedImageFormat.Png));

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
                var source = new SourceCache<ImageHandle, string>(x => x.OriginalPath);
                source.AddOrUpdate(new ImageHandle("foo", new SKBitmap(100, 100), SKEncodedImageFormat.Png));
                source.AddOrUpdate(new ImageHandle("bar", new SKBitmap(100, 100), SKEncodedImageFormat.Png));
                source.AddOrUpdate(new ImageHandle("baz", new SKBitmap(100, 100), SKEncodedImageFormat.Png));

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

                var source = new SourceCache<ImageHandle, string>(x => x.OriginalPath);
                source.AddOrUpdate(new ImageHandle("foo", new SKBitmap(100, 100), SKEncodedImageFormat.Png));
                source.AddOrUpdate(new ImageHandle("bar", new SKBitmap(100, 100), SKEncodedImageFormat.Png));

                var sut = new ImageExplorerViewModel(source.AsObservableCache());
                sut.Activator.Activate();
                scheduler.AdvanceBy(2);
                sut.Selections.Where(x => x.OriginalPath == "bar").Subscribe(_ => success = true);
                sut.SelectNext.Execute().Subscribe();
                scheduler.AdvanceBy(2);

                Assert.True(success);
            });
    }
}
