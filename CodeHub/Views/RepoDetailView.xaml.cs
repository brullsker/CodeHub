using System;
using Windows.Graphics.Display;
using Windows.UI.Xaml;
using GalaSoft.MvvmLight.Messaging;
using CodeHub.Helpers;
using CodeHub.ViewModels;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Controls;
using Windows.Web.Http;
using CodeHub.Services;
using UICompositionAnimations;
using UICompositionAnimations.Enums;
using Windows.System;
using Microsoft.Toolkit.Uwp.UI.Controls;

namespace CodeHub.Views
{
    public sealed partial class RepoDetailView : Windows.UI.Xaml.Controls.Page
    {
        public RepoDetailViewmodel ViewModel;
        public RepoDetailView()
        {
            this.Loaded += (s, e) => TopScroller.InitializeScrollViewer(MainScrollViewer);
            this.Unloaded += (s, e) => TopScroller.Dispose();
            this.InitializeComponent();
            ViewModel = new RepoDetailViewmodel();
            this.DataContext = ViewModel;
        }
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            Messenger.Default.Send(new GlobalHelper.SetHeaderTextMessageType { PageName = "Repository" });

            await ViewModel.Load(e.Parameter);

            FindName("LanguageText");
            FindName("DescriptionText");
            FindName("calendarSymbol");
            FindName("createdDateText");
            FindName("editSymbol");
            FindName("updatedDateText");
            FindName("sizeSymbol");
            FindName("sizeCount");
            FindName("sizeUnitText");


            if (SettingsService.Get<bool>(SettingsKeys.ShowReadme))
            {
                ReadmeLoadingRing.IsActive = true;
                ViewModel.Readme = (await RepositoryUtility.GetReadmeForRepository(ViewModel.Repository.Id)).Content;
                ReadmeLoadingRing.IsActive = false;
            }
            else
            {
                ReadmeLoadingRing.IsActive = false;
            }
        }

        // Scrolls the page content back to the top
        private void TopScroller_OnTopScrollingRequested(object sender, EventArgs e)
        {
            MainScrollViewer.ChangeView(null, 0, null, false);
        }

        private async void ReadmeTextBlock_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri(e.Link));
        }
    }
}
