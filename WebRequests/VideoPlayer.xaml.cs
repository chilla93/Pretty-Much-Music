using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using MyToolkit;
using MyToolkit.Multimedia;
using Microsoft.Phone.BackgroundAudio;
using Microsoft.Phone.Tasks;
using System.Threading;
using System.Windows.Threading;


namespace WebRequests
{

    
    
    
    public partial class VideoPlayer : PhoneApplicationPage
    {
        string link = "";
        int timecounter = 3;
        DispatcherTimer timer = new DispatcherTimer();
        public VideoPlayer()
        {
            InitializeComponent();
            //timer.Tick +=timer_Tick;
          
        }

        /*private void VideoPlayer_Loaded(object sender, RoutedEventArgs e)
        {
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(3);
            _timer.Tick += new EventHandler(webplay);
        }*/

   

       /* private async void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            var videoUri = await MyToolkit.Multimedia.YouTube.GetVideoUriAsync("PQVlW4xbNuI", MyToolkit.Multimedia.YouTubeQuality.Quality480P, MyToolkit.Multimedia.YouTubeQuality.Quality480P);
            if (videoUri != null)
            {
                player.Source = videoUri.Uri;
                player.Play();
                player.AutoPlay = true;
            }
        }*/  

        
        
        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
           
            string title = "";
            string image = "";

            if (NavigationContext.QueryString.TryGetValue("msg", out link)) ;
            if (NavigationContext.QueryString.TryGetValue("msg2", out title)) ;
            if (NavigationContext.QueryString.TryGetValue("image", out image)) ;



            //title2.Text = title;
            
        }

       

        private async void player_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                var url = await YouTube.GetVideoUriAsync(link, YouTubeQuality.Quality480P);
                
                if (url != null)
                {
                    player.Source = url.Uri;

                    try
                    {
                        if (BackgroundAudioPlayer.Instance.PlayerState == PlayState.Playing) BackgroundAudioPlayer.Instance.Pause();
                    }

                    catch(Exception)
                    {

                    }
                    player.Play();
                    player.AutoPlay = true;


                }


            }

            catch (Exception)
            {
                MessageBox.Show("OOPS! Video Not Available");
                NavigationService.GoBack();
            }
        }

            




            

        }

        

        
    }
