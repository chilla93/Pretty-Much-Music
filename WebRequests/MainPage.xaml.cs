using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using HtmlAgilityPack;
using System.IO;
using System.Collections.ObjectModel;
using Microsoft.Phone.BackgroundAudio;
using Microsoft.Phone.Shell;
using System.Windows.Threading;
using System.Windows.Controls.Primitives;
using System.ComponentModel;
using System.Threading;
using System.Collections;


namespace WebRequests
{

    
    public partial class MainPage : PhoneApplicationPage
    {
         private TextBlock _activeTxb = null;
        private ScrollBar sb = null;
        private ScrollViewer sv = null;
        private bool _isBouncy = false;
        private bool alreadyHookedScrollEvents = false;

        string CurrentMusicPageLink = "";
        string CurrentSearchPageLink = "";
        string CurrentVideoPageLink = "";
        string OldMusicPagelink = "";
        string OldSearchPagelink = "";
        string OldVideoPagelink = "";

        double LayoutRootOrigin = 0;
        double SettingsPaneOrigin = 0;

        private int category = 2;

        private ObservableCollection<Mp3Object> mp3list, videolist, searchlist;
        private ObservableCollection<string> list;
              

        private Popup popup;
        private BackgroundWorker backroungWorker;
        //private ObservableCollection<string> SongTitleList, SongArtistList;
        
        // Constructor
        public MainPage()
        {
            InitializeComponent();
           startServerRequest("http://prettymuchamazing.com/topics/videos/");
            mp3list = new ObservableCollection<Mp3Object>();
            videolist = new ObservableCollection<Mp3Object>();
            searchlist = new ObservableCollection<Mp3Object>();

            ShowSplash();
            DataContext = App.ViewModel;
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);

            list = new ObservableCollection<string> { "First Text Item", "Second Text Item", "Third Text Item" };
            //((ApplicationBarIconButton)(ApplicationBar.Buttons[2])).IsEnabled = false;


            


            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e) 
     { 
         if(!App.ViewModel.IsDataLoaded) 
         { 
             App.ViewModel.LoadData(); 
         } 

         if(alreadyHookedScrollEvents) 
             return; 

         alreadyHookedScrollEvents = true; 
         mp3longlist.AddHandler(ListBox.ManipulationCompletedEvent, (EventHandler<ManipulationCompletedEventArgs>)LB_ManipulationCompleted, true); 
         sb = (ScrollBar)FindElementRecursive(mp3longlist, typeof(ScrollBar)); 
         sv = (ScrollViewer)FindElementRecursive(mp3longlist, typeof(ScrollViewer)); 

         if(sv != null) 
         { 
             // Visual States are always on the first child of the control template 
            FrameworkElement element = VisualTreeHelper.GetChild(sv, 0) as FrameworkElement; 
             if(element != null) 
             { 
                
                 VisualStateGroup vgroup = FindVisualState(element, "VerticalCompression"); 
                 //VisualStateGroup hgroup = FindVisualState(element, "HorizontalCompression"); 
                 if(vgroup != null) 
                 { 
                     vgroup.CurrentStateChanging += new EventHandler<VisualStateChangedEventArgs>(vgroup_CurrentStateChanging); 
                 } 
                 
             } 
         }           

     }

        private void LB_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
        {
            if (_isBouncy)
            {
                _isBouncy = false;
            }
            //isScrollingTxb.Foreground = new SolidColorBrush(Colors.White);
            
        }

        private void vgroup_CurrentStateChanging(object sender, VisualStateChangedEventArgs e)
        {
            

            if (e.NewState.Name == "CompressionBottom")
            {
                switch (category) {
                    case (0):
                        {
                            if (CurrentSearchPageLink != OldSearchPagelink)
                            {
                                OldSearchPagelink = CurrentSearchPageLink;
                                startServerRequest(CurrentSearchPageLink);
                            }
                            break;
                        }

                    case (1):
                        {
                            if (CurrentMusicPageLink != OldMusicPagelink)
                            {
                                OldMusicPagelink = CurrentMusicPageLink;
                                startServerRequest(CurrentMusicPageLink);
                            }
                            break;
                        }

                    case (2):
                        {
                            if (CurrentVideoPageLink != OldVideoPagelink)
                            {
                                OldVideoPagelink = CurrentVideoPageLink;
                                startServerRequest(CurrentVideoPageLink);
                            }
                            break;
                        }
            }
                /*
                 * As an example, this is where you can add code to load new items, have the progress bar animation and so on
                 *                  
                 */
            }
            
        }


  private UIElement FindElementRecursive(FrameworkElement parent, Type targetType) 
       { 
           int childCount = VisualTreeHelper.GetChildrenCount(parent); 
           UIElement returnElement = null; 
           if (childCount > 0) 
           { 
               for (int i = 0; i < childCount; i++) 
               { 
                   Object element = VisualTreeHelper.GetChild(parent, i); 
                   if (element.GetType() == targetType) 
                   { 
                       return element as UIElement; 
                   } 
                   else 
                   { 
                       returnElement = FindElementRecursive(VisualTreeHelper.GetChild(parent, i) as FrameworkElement, targetType); 
                   } 
               } 
           } 
           return returnElement; 
       }


  private VisualStateGroup FindVisualState(FrameworkElement element, string name) 
       { 
           if (element == null) 
               return null; 

           IList groups = VisualStateManager.GetVisualStateGroups(element); 
           foreach (VisualStateGroup group in groups) 
               if (group.Name == name) 
                   return group; 

           return null; 
       }




     public void startServerRequest(string link)
        {

            HttpWebRequest httpReq = (HttpWebRequest)HttpWebRequest.Create(new Uri(link));
            
            httpReq.BeginGetResponse(HTTPWebRequestCallBack, httpReq);
        }   

      private void HTTPWebRequestCallBack(IAsyncResult result)
        {
            string strResponse = "";
            try
            {
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    try
                    {
                        HttpWebRequest httpRequest = (HttpWebRequest)result.AsyncState;
                        WebResponse response = httpRequest.EndGetResponse(result);

                        Stream stream = response.GetResponseStream();
                        StreamReader reader = new StreamReader(stream);
                        strResponse = reader.ReadToEnd();

                        HtmlDocument htmlDocument = new HtmlDocument();
                        htmlDocument.OptionFixNestedTags = true;
                        htmlDocument.LoadHtml(strResponse);

                        if (htmlDocument.DocumentNode != null)
                        {
                            //mp3list = new ObservableCollection<Mp3Object>();
                            //videolist = new ObservableCollection<Mp3Object>();

                            var songtitle = htmlDocument.DocumentNode.SelectNodes("//h2[@class='entry-title']/a/text()");
                            var description = htmlDocument.DocumentNode.SelectNodes("//div[@class='format_text entry-content']/p");
                            var songimage = htmlDocument.DocumentNode.SelectNodes("//img[@class='attachment-175-thumbnail wp-post-image']/@src");
                            var songlink = htmlDocument.DocumentNode.SelectNodes("//h2[@class='entry-title']/a/@href");
                            var pagelink = htmlDocument.DocumentNode.SelectSingleNode("//p[@class='previous']/a/@href");
                            var pagelink2 = htmlDocument.DocumentNode.SelectSingleNode("//p[@class='previous floated']/a/@href");

                            if (description != null)
                            {
                                

                                for (int i = 0; i < songimage.Count; i++)
                                {

                                   if (songlink[i] != null || description[i] != null || songimage[i] != null || songtitle[i] != null) { 
                                    string imgsrc = songimage[i].Attributes["src"].Value;
                                    string link = songlink[i].Attributes["href"].Value;

                                   //if(pagelink != null) CurrentPageLink = pagelink.Attributes["href"].Value;
                                   //if (pagelink2 != null) CurrentPageLink = pagelink2.Attributes["href"].Value;
                                    string pglink = "";

                                    if (pagelink != null) pglink = pagelink.Attributes["href"].Value;
                                    if (pagelink2 != null)  pglink = pagelink2.Attributes["href"].Value;
                                    //songtitle[i].InnerText = songtitle[i].InnerText.Replace("&#8217;", "'");
                                   
                                   // if (description[i].ParentNode == null) description[i] = description[i + 1].ParentNode.Clone();

                                    if (i > 0)
                                    {
                                        while (description[i].ParentNode == description[i - 1].ParentNode)
                                        {
                                            description.RemoveAt(i);                                            
                                        }

                                        while (songtitle[i].ParentNode == songtitle[i - 1].ParentNode)
                                        {
                                            songtitle.RemoveAt(i);
                                        }
                                    }


                                    if (!((ApplicationBarIconButton)(ApplicationBar.Buttons[1])).IsEnabled)
                                    {
                                        mp3list.Add(new Mp3Object(subtitle(songtitle[i]), subtitle(description[i]), imgsrc, link));
                                        this.mp3longlist.ItemsSource = mp3list;
                                        CurrentMusicPageLink = pglink;
                                    }

                                    if (!((ApplicationBarIconButton)(ApplicationBar.Buttons[2])).IsEnabled)
                                    {
                                        videolist.Add(new Mp3Object(subtitle(songtitle[i]), subtitle(description[i]), imgsrc, link));
                                        this.mp3longlist.ItemsSource = videolist;
                                        CurrentVideoPageLink = pglink;

                                    }

                                    if (!((ApplicationBarIconButton)(ApplicationBar.Buttons[0])).IsEnabled)
                                    {
                                        searchlist.Add(new Mp3Object(subtitle(songtitle[i]), subtitle(description[i]), imgsrc, link));
                                        this.mp3longlist.ItemsSource = searchlist;
                                        CurrentSearchPageLink = pglink;

                                    }
                                }
                                }
                                
                            }

                        }
                        

                       

                                      }
                    catch (Exception ex)
                    {
                        MessageBox.Show("OOPS! Cannot connect. Ensure that you are connected to the internet");//+ ex.Message);
                    }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("oops! " + ex.Message);
            }
        }

       private void ShowSplash()
       {
           this.popup = new Popup();
           this.popup.Child = new usersplashscreen();
           this.popup.IsOpen = true;
           StartLoadingData();
       }

       private void StartLoadingData()
       {
           backroungWorker = new BackgroundWorker();
           backroungWorker.DoWork += new DoWorkEventHandler(backroungWorker_DoWork);
           backroungWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backroungWorker_RunWorkerCompleted);
           backroungWorker.RunWorkerAsync();
       }

       void backroungWorker_DoWork(object sender, DoWorkEventArgs e)
       {
           //here we can load data
           Thread.Sleep(3000);
       }
       void backroungWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
       {
           this.Dispatcher.BeginInvoke(() =>
           {
               this.popup.IsOpen = false;
           }
           );
       }

       private void Mp3Object_Tap(object sender, System.Windows.Input.GestureEventArgs e)
       {
           Mp3Object item = (sender as StackPanel).DataContext as Mp3Object;
           //var t = mp3longlist.SelectedItem.ToString();
           NavigationService.Navigate(new Uri("/Songsite.xaml?msg=" + item.SongLink, UriKind.RelativeOrAbsolute));
       }

       private void ShowMusic_Click(object sender, RoutedEventArgs e)
       {
           NavigationService.Navigate(new Uri("/NowPlaying.xaml?", UriKind.RelativeOrAbsolute));
           
           SlideTransition transition = new SlideTransition();
           
           transition.Mode = SlideTransitionMode.SlideRightFadeOut;
           PhoneApplicationPage MainPage = (PhoneApplicationPage)(((PhoneApplicationFrame)Application.Current.RootVisual)).Content;
           
           ITransition trans = transition.GetTransition(MainPage);
           trans.Completed += delegate { trans.Stop(); };
           trans.Begin();
       }

       protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
       {
           base.OnNavigatedTo(e);
           SlideTransition transition = new SlideTransition();
           transition.Mode = SlideTransitionMode.SlideLeftFadeIn;
           PhoneApplicationPage MainPage = (PhoneApplicationPage)(((PhoneApplicationFrame)Application.Current.RootVisual)).Content;

           ITransition trans = transition.GetTransition(MainPage);
           trans.Completed += delegate { trans.Stop(); };
           trans.Begin();
       }
       private string subtitle(HtmlNode d)
       {           
           
           string fullcontent = "";
           var content = d.ParentNode.FirstChild;

          


           
                   for (int i = 0; i < d.ParentNode.ChildNodes.Count; i++)
                   {
                       fullcontent += content.InnerHtml.Trim() + " ";
                       content = content.NextSibling;

                   }
               
           return fullcontent;
       }

       private void Search_Click(object sender, EventArgs e)
       {
           category = 0;
           if(searchbox.Visibility == Visibility.Visible) searchbox.Visibility = Visibility.Collapsed;

           else searchbox.Visibility = Visibility.Visible;

           /*((ApplicationBarIconButton)(ApplicationBar.Buttons[0])).IsEnabled = false;
           ((ApplicationBarIconButton)(ApplicationBar.Buttons[1])).IsEnabled = true;
           ((ApplicationBarIconButton)(ApplicationBar.Buttons[2])).IsEnabled = true;*/

           CategoryHeader.Text = "Search";
           
       }

       /*private void music_click(object sender, EventArgs e)
       {
           category = 1;
           if(mp3list.Count == 0) startServerRequest("http://prettymuchamazing.com/topics/music/");
           else mp3longlist.ItemsSource = mp3list;

           CategoryHeader.Text = "Music";


          // mp3longlist.ScrollIntoView(mp3longlist.Items.First(null)); 
           mp3longlist.ScrollIntoView(mp3longlist.Items[mp3longlist.Items.Count-1]);
           mp3longlist.UpdateLayout();
           mp3longlist.ScrollIntoView(mp3longlist.Items[0]);


           ((ApplicationBarIconButton)(ApplicationBar.Buttons[0])).IsEnabled = true;
           ((ApplicationBarIconButton)(ApplicationBar.Buttons[1])).IsEnabled = false;
           ((ApplicationBarIconButton)(ApplicationBar.Buttons[2])).IsEnabled = true;

           searchbox.Visibility = Visibility.Collapsed;



       }

       private void video_button(object sender, EventArgs e)
       {
           category = 2;
           if (videolist.Count == 0) startServerRequest("http://prettymuchamazing.com/topics/videos/");
           else mp3longlist.ItemsSource = videolist;

           CategoryHeader.Text = "Videos";

          // mp3longlist.ScrollIntoView(mp3longlist.Items.First(null));
           mp3longlist.ScrollIntoView(mp3longlist.Items[mp3longlist.Items.Count-1]);
           mp3longlist.UpdateLayout();
           mp3longlist.ScrollIntoView(mp3longlist.Items[0]);

           ((ApplicationBarIconButton)(ApplicationBar.Buttons[0])).IsEnabled = true;
           ((ApplicationBarIconButton)(ApplicationBar.Buttons[1])).IsEnabled = true;
           ((ApplicationBarIconButton)(ApplicationBar.Buttons[2])).IsEnabled = false;

           searchbox.Visibility = Visibility.Collapsed;
       }*/

       

       private void searchit_Click(object sender, RoutedEventArgs e)
       {
           mp3longlist.ScrollIntoView(mp3longlist.Items[mp3longlist.Items.Count - 1]);
           mp3longlist.UpdateLayout();
           mp3longlist.ScrollIntoView(mp3longlist.Items[0]);

           searchlist = new ObservableCollection<Mp3Object>();
           CurrentSearchPageLink = "http://prettymuchamazing.com/?s=";
           string searchstring = search_input.Text;
           searchstring = searchstring.Replace(" ", "+");

           CurrentSearchPageLink = CurrentSearchPageLink.Insert(CurrentSearchPageLink.Length, searchstring);

           startServerRequest(CurrentSearchPageLink);
           //searchbox.Visibility = Visibility.Collapsed;
           //search_input.Text = "";

           
       }

       private void SettingsPane_ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
       {
           double dY = e.CumulativeManipulation.Translation.Y;
           double dX = e.CumulativeManipulation.Translation.X;
           if (Math.Abs(dY) < Math.Abs(dX))
           {
               if (dX > 0)
               {
                   LayoutRoot.Projection.SetValue(PlaneProjection.LocalOffsetXProperty, e.CumulativeManipulation.Translation.X);
                   SettingsPane.Projection.SetValue(PlaneProjection.LocalOffsetXProperty, e.CumulativeManipulation.Translation.X);
               }
           }
       }

       private void SettingsPane_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
       {
           double dY = e.TotalManipulation.Translation.Y;
           double dX = e.TotalManipulation.Translation.X;
            
                   if (Math.Abs(dY) < Math.Abs(dX))
                   {
                       if (dX > 0)
                       {
                           DynamicStoryboard(-dX, true);

                       }
                   }
               
               // Vertical

}

       private void LayoutRoot_ManipulationStarted(object sender, ManipulationStartedEventArgs e)
       {
           
           LayoutRootOrigin = e.ManipulationOrigin.X;
           //Point x = 0;
           
           

           
       }
       private void LayoutRoot_ManipulationDelta(object sender, ManipulationDeltaEventArgs e)
       {
           double dY = e.CumulativeManipulation.Translation.Y;
           double dX = e.CumulativeManipulation.Translation.X;
           if (Math.Abs(dY) < Math.Abs(dX))
           {
               if (dX < 0)
               {
                   LayoutRoot.Projection.SetValue(PlaneProjection.LocalOffsetXProperty, e.CumulativeManipulation.Translation.X);
                   SettingsPane.Projection.SetValue(PlaneProjection.LocalOffsetXProperty, e.CumulativeManipulation.Translation.X);
               }
           }
       }

       private void LayoutRoot_ManipulationCompleted(object sender, ManipulationCompletedEventArgs e)
       {
           double dY = e.TotalManipulation.Translation.Y;
           double dX = e.TotalManipulation.Translation.X;

           if (Math.Abs(dY) < Math.Abs(dX))
           {
               if (dX < 0)
               {

                   DynamicStoryboard(-420-dX, false);


                  
               }
           }
       }

       private void DynamicStoryboard(double offset, bool open)
       {
           Storyboard Swipe = new Storyboard();
           DoubleAnimation LayoutAnimation = new DoubleAnimation();
           DoubleAnimation SettingsAnimation = new DoubleAnimation();

           // Create a duration of 2 seconds.
           Duration duration = new Duration(TimeSpan.FromSeconds(0.1));

           Swipe.Duration = duration;
           LayoutAnimation.Duration = duration;
           SettingsAnimation.Duration = duration;
           
           Swipe.Children.Add(LayoutAnimation);
           Swipe.Children.Add(SettingsAnimation);

           Storyboard.SetTarget(LayoutAnimation, LayoutRoot);
           Storyboard.SetTarget(SettingsAnimation, SettingsPane);

           Storyboard.SetTargetProperty(LayoutAnimation, new PropertyPath("(UIElement.Projection).(PlaneProjection.GlobalOffsetX)"));
           Storyboard.SetTargetProperty(SettingsAnimation, new PropertyPath("(UIElement.Projection).(PlaneProjection.GlobalOffsetX)"));

          

           if (!open)
           {
               LayoutAnimation.From = 0;
               SettingsAnimation.From = 0;
               LayoutAnimation.To = offset;
               SettingsAnimation.To = offset;
           }

           else 
           {
              // LayoutAnimation.From = offset;
               //SettingsAnimation.From = offset;
               LayoutAnimation.To = offset;
               SettingsAnimation.To = offset;
           }

           // Make the Storyboard a resource.
           //Container.Resources.Add(Swipe);


           //Swipe.Stop();
           Swipe.Begin();
       }

       private void SettingsPane_ManipulationStarted(object sender, ManipulationStartedEventArgs e)
       {
       }

       private void musicshow_tap(object sender, System.Windows.Input.GestureEventArgs e)
       {
           NavigationService.Navigate(new Uri("/NowPlaying.xaml?", UriKind.RelativeOrAbsolute));

           SlideTransition transition = new SlideTransition();

           transition.Mode = SlideTransitionMode.SlideRightFadeOut;
           PhoneApplicationPage MainPage = (PhoneApplicationPage)(((PhoneApplicationFrame)Application.Current.RootVisual)).Content;

           ITransition trans = transition.GetTransition(MainPage);
           trans.Completed += delegate { trans.Stop(); };
           trans.Begin();
       }

      
      


        

       
        // Sample code for building a localized ApplicationBar
        //private void BuildLocalizedApplicationBar()
        //{
        //    // Set the page's ApplicationBar to a new instance of ApplicationBar.
        //    ApplicationBar = new ApplicationBar();

        //    // Create a new button and set the text value to the localized string from AppResources.
        //    ApplicationBarIconButton appBarButton = new ApplicationBarIconButton(new Uri("/Assets/AppBar/appbar.add.rest.png", UriKind.Relative));
        //    appBarButton.Text = AppResources.AppBarButtonText;
        //    ApplicationBar.Buttons.Add(appBarButton);

        //    // Create a new menu item with the localized string from AppResources.
        //    ApplicationBarMenuItem appBarMenuItem = new ApplicationBarMenuItem(AppResources.AppBarMenuItemText);
        //    ApplicationBar.MenuItems.Add(appBarMenuItem);
        //}
    }
}
