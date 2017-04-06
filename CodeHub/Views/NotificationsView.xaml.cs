﻿using CodeHub.Helpers;
using CodeHub.ViewModels;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Ioc;
using Octokit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace CodeHub.Views
{
    public sealed partial class NotificationsView : Windows.UI.Xaml.Controls.Page
    {
        public NotificationsViewmodel ViewModel;

        public NotificationsView()
        {
            this.InitializeComponent();

            ViewModel = new NotificationsViewmodel();
            this.DataContext = ViewModel;
            Loading += NotificationsView_Loading;

            NavigationCacheMode = NavigationCacheMode.Required;
        }

        private void NotificationsView_Loading(FrameworkElement sender, object args)
        {
            //Listening for Sign In message
            Messenger.Default.Register<User>(this, ViewModel.RecieveSignInMessage);
            //listen for sign out message
            Messenger.Default.Register<GlobalHelper.SignOutMessageType>(this, ViewModel.RecieveSignOutMessage); 
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            Messenger.Default.Send(new GlobalHelper.SetHeaderTextMessageType { PageName = "Notifications" });
        }

        private void Notif_PullProgressChanged(object sender, Microsoft.Toolkit.Uwp.UI.Controls.RefreshProgressEventArgs e)
        {
            refreshindicator.Opacity = e.PullProgress;
            refreshindicator.Background = e.PullProgress < 1.0 ? GlobalHelper.GetSolidColorBrush("4078C0FF") : GlobalHelper.GetSolidColorBrush("47C951FF");
        }
        private void Repo_Click(object sender, RoutedEventArgs e)
        {
            SimpleIoc.Default.GetInstance<Services.IAsyncNavigationService>().NavigateAsync(typeof(RepoDetailView), "Repository", (sender as HyperlinkButton).Content);
        }
    }
}
