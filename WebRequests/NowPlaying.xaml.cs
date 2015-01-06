/* 
    Copyright (c) 2011 Microsoft Corporation.  All rights reserved.
    Use of this sample source code is subject to the terms of the Microsoft license 
    agreement under which you licensed this sample source code and is provided AS-IS.
    If you did not accept the terms of the license agreement, you are not authorized 
    to use this sample source code.  For the terms of the license, please see the 
    license agreement between you and Microsoft.
  
    To see all Code Samples for Windows Phone, visit http://go.microsoft.com/fwlink/?LinkID=219604 
  
*/
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Threading;
using Microsoft.Phone.Controls;
using Microsoft.Phone.BackgroundAudio;
using Microsoft.Phone.Shell;
using System.Windows.Media.Imaging;
using System.IO.IsolatedStorage;
using System.IO;
using System.Windows.Resources;
using YoutubeExtractor;
using System.Linq;
using System.Threading;
using Newtonsoft.Json;
using System.Net;
using System.Windows.Controls;



namespace WebRequests
{
    
    public partial class NowPlaying : PhoneApplicationPage
    {
        // Timer for updating the UI
        DispatcherTimer _timer;
        DispatcherTimer _timer2;

        double tracktime = 0;
        string relevantsearchterm = "";
        string PMAartwork = "";

        //public List<AudioTrack> _playList = new List<AudioTrack>(10);

        // Indexes into the array of ApplicationBar.Buttons
        const int prevButton = 0;
        const int playButton = 1;
        const int pauseButton = 2;
        const int nextButton = 3;

        ObservableCollection<AudioTrack> playlist;
        ObservableCollection<RootObject> Tracklist;

        int x = 0;


        // Constructor
        public NowPlaying()
        {
            
            InitializeComponent();
            Tracklist = new ObservableCollection<RootObject>();
            playlist = new ObservableCollection<AudioTrack>();
            Loaded += new RoutedEventHandler(NowPlaying_Loaded);
        }


        /// <summary> 
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void NowPlaying_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                // Initialize a timer to update the UI every half-second.
                _timer = new DispatcherTimer();
                _timer.Interval = TimeSpan.FromSeconds(0.5);
                _timer.Tick += new EventHandler(UpdateState);

                //timer2
                

                BackgroundAudioPlayer.Instance.PlayStateChanged += new EventHandler(Instance_PlayStateChanged);

                if (BackgroundAudioPlayer.Instance.PlayerState == PlayState.Playing)
                {
                    // If audio was already playing when the app was launched, update the UI.
                    positionIndicator.IsIndeterminate = false;
                    positionIndicator.Maximum = BackgroundAudioPlayer.Instance.Track.Duration.TotalSeconds;
                    UpdateButtons(true, false, true, true);
                    UpdateState(null, null);
                    _timer.Start();
                }
            }

            catch(Exception)
            {

            }
        }


        /// <summary>
        /// PlayStateChanged event handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
       public void Instance_PlayStateChanged(object sender, EventArgs e)
        {
            switch (BackgroundAudioPlayer.Instance.PlayerState)
            {
                case PlayState.Playing:
                    // Update the UI.
                    positionIndicator.IsIndeterminate = false;
                    positionIndicator.Maximum = BackgroundAudioPlayer.Instance.Track.Duration.TotalSeconds;
                    UpdateButtons(true, false, true, true);
                    UpdateState(null, null);

                    // Start the timer for updating the UI.
                    _timer.Start();
                    break;

                case PlayState.Paused:
                    // Update the UI.
                    UpdateButtons(true, true, false, true);
                    UpdateState(null, null);

                    // Stop the timer for updating the UI.
                    _timer.Stop();
                    break;
            }
        }

       private void CheckDuration(object sender, EventArgs e)
       {
           if (tracktime == 0)
           {
               //MessageBox.Show("Sorry! Track Not Streamable on this device");
               //NavigationService.GoBack();
               ApplicationTitle.Text = "Sorry! Track Might Not be Streamable on this device";

           }

           _timer2.Stop();
       }



        /// <summary>
        /// Helper method to update the state of the ApplicationBar.Buttons
        /// </summary>
        /// <param name="prevBtnEnabled"></param>
        /// <param name="playBtnEnabled"></param>
        /// <param name="pauseBtnEnabled"></param>
        /// <param name="nextBtnEnabled"></param>
        void UpdateButtons(bool prevBtnEnabled, bool playBtnEnabled, bool pauseBtnEnabled, bool nextBtnEnabled)
        {
            // Set the IsEnabled state of the ApplicationBar.Buttons array
            ((ApplicationBarIconButton)(ApplicationBar.Buttons[prevButton])).IsEnabled = prevBtnEnabled;
            ((ApplicationBarIconButton)(ApplicationBar.Buttons[playButton])).IsEnabled = playBtnEnabled;
            ((ApplicationBarIconButton)(ApplicationBar.Buttons[pauseButton])).IsEnabled = pauseBtnEnabled;
            ((ApplicationBarIconButton)(ApplicationBar.Buttons[nextButton])).IsEnabled = nextBtnEnabled;
        }


        /// <summary>
        /// Updates the status indicators including the State, Track title, 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateState(object sender, EventArgs e)
        {
            try
            {
                txtState.Text = string.Format("State: {0}", BackgroundAudioPlayer.Instance.PlayerState);

                if (BackgroundAudioPlayer.Instance.Track != null)
                {
                    txtTrack.Text = string.Format("Track: {0}", BackgroundAudioPlayer.Instance.Track.Title);

                    // Set the current position on the ProgressBar.
                    positionIndicator.Value = BackgroundAudioPlayer.Instance.Position.TotalSeconds;
                    tracktime = positionIndicator.Value;

                    // Update the current playback position.
                    TimeSpan position = new TimeSpan();
                    position = BackgroundAudioPlayer.Instance.Position;
                    textPosition.Text = String.Format("{0:d2}:{1:d2}:{2:d2}", position.Hours, position.Minutes, position.Seconds);

                    // Update the time remaining digits.
                    TimeSpan timeRemaining = new TimeSpan();
                    timeRemaining = BackgroundAudioPlayer.Instance.Track.Duration - position;
                    textRemaining.Text = String.Format("-{0:d2}:{1:d2}:{2:d2}", timeRemaining.Hours, timeRemaining.Minutes, timeRemaining.Seconds);

                }

            }

            catch(Exception)
            {

            }
            
        }


        /// <summary>
        /// Click handler for the Skip Previous button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void prevButton_Click(object sender, EventArgs e)
        {
            // Show the indeterminate progress bar.
            positionIndicator.IsIndeterminate = true;

            // Disable the button so the user can't click it multiple times before 
            // the background audio agent is able to handle their request.
            ((ApplicationBarIconButton)(ApplicationBar.Buttons[prevButton])).IsEnabled = false;

            // Tell the backgound audio agent to skip to the previous track.
            BackgroundAudioPlayer.Instance.SkipPrevious();
            
            
        }


        /// <summary>
        /// Click handler for the Play button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void playButton_Click(object sender, EventArgs e)
        {
            try
            {
                
                // Tell the backgound audio agent to play the current track.
                BackgroundAudioPlayer.Instance.Play();
            }

            catch(Exception)
            {

            }
        }


        /// <summary>
        /// Click handler for the Pause button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pauseButton_Click(object sender, EventArgs e)
        {
            // Tell the backgound audio agent to pause the current track.
            BackgroundAudioPlayer.Instance.Pause();
        }

        private bool _isSettingsOpen = false;
        /// <summary>
        /// Click handler for the Skip Next button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
         
        private void nextButton_Click(object sender, EventArgs e)
        {
            // Show the indeterminate progress bar.
            positionIndicator.IsIndeterminate = true;

            // Disable the button so the user can't click it multiple times before 
            // the background audio agent is able to handle their request.
            ((ApplicationBarIconButton)(ApplicationBar.Buttons[nextButton])).IsEnabled = false;

            // Tell the backgound audio agent to skip to the next track.
            BackgroundAudioPlayer.Instance.SkipNext();
                     
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
             string link = "";
             string title = "";
             string oldlink = "";
             

             
            
             if (NavigationContext.QueryString.TryGetValue("msg", out link));
             if (NavigationContext.QueryString.TryGetValue("msg2", out title));
             if (NavigationContext.QueryString.TryGetValue("image", out PMAartwork));

              if ((link != "")&&(x==0))
                 {
                     WebClient soundcloudobject = new WebClient();
                     soundcloudobject.DownloadStringCompleted += new DownloadStringCompletedEventHandler(soundcloudobject_DownloadStringCompleted);
                     soundcloudobject.DownloadStringAsync(new Uri("http://api.soundcloud.com/tracks.json?client_id=eaf7649b0de68f902a4607c0b730e226&ids="+link));

                     x = 1;
                     
                 }
              else
              {
                  startServerRequest();
              }

                 if (BackgroundAudioPlayer.Instance.Track != null)
                 {
                     AlbumArt.Source = new BitmapImage(BackgroundAudioPlayer.Instance.Track.AlbumArt);                     
                 }

                  

            
            //if(BackgroundAudioPlayer.Instance.Track != null) BackgroundAudioPlayer.Instance.Play();
            //Loaded += new RoutedEventHandler(NowPlaying_Loaded);
            

            SlideTransition transition = new SlideTransition();
            transition.Mode = SlideTransitionMode.SlideLeftFadeIn;
            PhoneApplicationPage phonePage = (PhoneApplicationPage)(((PhoneApplicationFrame)Application.Current.RootVisual)).Content;

            ITransition trans = transition.GetTransition(phonePage);
            trans.Completed += delegate { trans.Stop(); };
            trans.Begin();
        }

        void soundcloudobject_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                var rootObject = JsonConvert.DeserializeObject<RootObject[]>(e.Result);

                if (rootObject.Length > 0)
                {
                    linkmusic(rootObject[0].id.ToString(), rootObject[0].title, PMAartwork);

                    relevantsearchterm = rootObject[0].title;

                    if (!rootObject[0].streamable) ApplicationTitle.Text = "Track might not be streamable on the device";
                }

                else
                {
                    ApplicationTitle.Text = "Track has been removed by user";
                }
                startServerRequest();

            }

            catch (System.Reflection.TargetInvocationException)
            {
                MessageBox.Show("Please ensure you are connected to the internet");
            }
        }

        private void linkmusic(string link, string title, string image)
        {
             try
             {

                 AudioTrack sitetrack = new AudioTrack(new Uri("https://api.soundcloud.com/tracks/" + link + "/stream?client_id=eaf7649b0de68f902a4607c0b730e226"), title, null, null, new Uri(image, UriKind.RelativeOrAbsolute), "", EnabledPlayerControls.All);
                ApplicationTitle.Text = "";

                /*_timer2 = new DispatcherTimer();
                _timer2.Interval = TimeSpan.FromSeconds(6);
                _timer2.Tick += new EventHandler(CheckDuration);
                _timer2.Start();*/
                // this.SongImage = new BitmapImage(new Uri(image, UriKind.RelativeOrAbsolute));
                //playlist.Add(sitetrack);


                BackgroundAudioPlayer.Instance.Track = sitetrack;
                AlbumArt.Source = new BitmapImage(BackgroundAudioPlayer.Instance.Track.AlbumArt);

             }

             catch (Exception)
             {

             }


                //BackgroundAudioPlayer.Instance.PlayStateChanged += Instance_PlayStateChanged;

            
        }
        private void LayoutRoot_ManipulationCompleted(object sender, System.Windows.Input.ManipulationCompletedEventArgs e)
        {
            double dY = e.TotalManipulation.Translation.Y;
            double dX = e.TotalManipulation.Translation.X;

            if (Math.Abs(dY) > Math.Abs(dX))
            {
                if(dY<0)
                {
                    VisualStateManager.GoToState(this, "SettingsOpenState", true);

                }
                // Vertical

            }
        }

        private void SettingsPane_ManipulationCompleted(object sender, System.Windows.Input.ManipulationCompletedEventArgs e)
        {
            double dY = e.TotalManipulation.Translation.Y;
            double dX = e.TotalManipulation.Translation.X;

            if (Math.Abs(dY) > Math.Abs(dX))
            {
                if (dY > 0)
                {
                    VisualStateManager.GoToState(this, "SettingsClosedState", true);

                }
                // Vertical

            }

        }

        public void startServerRequest()
        {

            /*var client = new RestClient("http://api.soundcloud.com/tracks.json?q=love&client_id=eaf7649b0de68f902a4607c0b730e226");
            var request = new RestRequest { RequestFormat = DataFormat.Json };
            client.ExecuteAsync<List<string>>(request, response =>
            {
                songlist.ItemsSource = response.Data;
            });*/

            WebClient webClient = new WebClient();
            webClient.DownloadStringCompleted += new DownloadStringCompletedEventHandler(webClient_DownloadStringCompleted);
            webClient.DownloadStringAsync(new Uri("http://api.soundcloud.com/tracks.json?q="+relevantsearchterm+"&client_id=eaf7649b0de68f902a4607c0b730e226&limit=10&filter=streamable"));

        }

        void webClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                var rootObject = JsonConvert.DeserializeObject<RootObject[]>(e.Result);

                for (int i = 0; i < rootObject.Length; i++)
                {
                    Tracklist.Add(rootObject[i]);
                    
                }

                trackimages.ItemsSource = Tracklist;
            }

            catch (System.Reflection.TargetInvocationException)
            {
                MessageBox.Show("Please ensure you are connected to the internet");
            }
        }

        private void Track_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            RootObject item = (sender as StackPanel).DataContext as RootObject;

            if (item.artwork_url != null) linkmusic(item.id.ToString(), item.title, item.artwork_url);
            else
            {
                linkmusic(item.id.ToString(), item.title, "PMM.png");
                
            }
            VisualStateManager.GoToState(this, "SettingsClosedState", true);

        }
    }
}
