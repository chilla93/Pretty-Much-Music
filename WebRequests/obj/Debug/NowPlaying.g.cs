﻿#pragma checksum "C:\Users\Owner\Documents\Visual Studio 2013\Projects\WP projects\Web Requests\WebRequests\NowPlaying.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "1BBB5953FE3FB9C10915F82AFF2CD4C4"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34014
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace WebRequests {
    
    
    public partial class NowPlaying : Microsoft.Phone.Controls.PhoneApplicationPage {
        
        internal System.Windows.Controls.Grid Container;
        
        internal System.Windows.VisualStateGroup SettingsStateGroup;
        
        internal System.Windows.VisualState SettingsClosedState;
        
        internal System.Windows.VisualState SettingsOpenState;
        
        internal System.Windows.Controls.Grid SettingsPane;
        
        internal System.Windows.Controls.Grid TrackPanel;
        
        internal Microsoft.Phone.Controls.LongListSelector trackimages;
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.StackPanel TitlePanel;
        
        internal System.Windows.Controls.TextBlock ApplicationTitle;
        
        internal System.Windows.Controls.TextBlock PageTitle;
        
        internal System.Windows.Controls.Grid ContentPanel;
        
        internal System.Windows.Controls.TextBlock txtState;
        
        internal System.Windows.Controls.TextBlock txtTrack;
        
        internal System.Windows.Controls.Image AlbumArt;
        
        internal System.Windows.Controls.ProgressBar positionIndicator;
        
        internal System.Windows.Controls.TextBlock textPosition;
        
        internal System.Windows.Controls.TextBlock textRemaining;
        
        internal Microsoft.Phone.Shell.ApplicationBar AppBar;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/WebRequests;component/NowPlaying.xaml", System.UriKind.Relative));
            this.Container = ((System.Windows.Controls.Grid)(this.FindName("Container")));
            this.SettingsStateGroup = ((System.Windows.VisualStateGroup)(this.FindName("SettingsStateGroup")));
            this.SettingsClosedState = ((System.Windows.VisualState)(this.FindName("SettingsClosedState")));
            this.SettingsOpenState = ((System.Windows.VisualState)(this.FindName("SettingsOpenState")));
            this.SettingsPane = ((System.Windows.Controls.Grid)(this.FindName("SettingsPane")));
            this.TrackPanel = ((System.Windows.Controls.Grid)(this.FindName("TrackPanel")));
            this.trackimages = ((Microsoft.Phone.Controls.LongListSelector)(this.FindName("trackimages")));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.TitlePanel = ((System.Windows.Controls.StackPanel)(this.FindName("TitlePanel")));
            this.ApplicationTitle = ((System.Windows.Controls.TextBlock)(this.FindName("ApplicationTitle")));
            this.PageTitle = ((System.Windows.Controls.TextBlock)(this.FindName("PageTitle")));
            this.ContentPanel = ((System.Windows.Controls.Grid)(this.FindName("ContentPanel")));
            this.txtState = ((System.Windows.Controls.TextBlock)(this.FindName("txtState")));
            this.txtTrack = ((System.Windows.Controls.TextBlock)(this.FindName("txtTrack")));
            this.AlbumArt = ((System.Windows.Controls.Image)(this.FindName("AlbumArt")));
            this.positionIndicator = ((System.Windows.Controls.ProgressBar)(this.FindName("positionIndicator")));
            this.textPosition = ((System.Windows.Controls.TextBlock)(this.FindName("textPosition")));
            this.textRemaining = ((System.Windows.Controls.TextBlock)(this.FindName("textRemaining")));
            this.AppBar = ((Microsoft.Phone.Shell.ApplicationBar)(this.FindName("AppBar")));
        }
    }
}

