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
using YoutubeExtractor;
using System.IO;
using System.Collections.ObjectModel;
using Microsoft.Phone.BackgroundAudio;
using Microsoft.Phone.Tasks;
using System.Windows.Controls.Primitives;
using System.ComponentModel;
using System.Threading;



namespace WebRequests
{
    public partial class Songsite : PhoneApplicationPage
    {
        Mp3Object sitesong;
        private Popup popup;
        private BackgroundWorker backroungWorker;

        public Songsite()
        {
            InitializeComponent();
            ShowSplash();
            
            
        }

        public void startServerRequest( string link)
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

                            var songtitle = htmlDocument.DocumentNode.SelectSingleNode("//h1[@class='entry-title']//text()");
                            var description = htmlDocument.DocumentNode.SelectSingleNode("//div[@class='format_text entry-content']/p/text()");
                            var songimage = htmlDocument.DocumentNode.SelectSingleNode("//img[@class='attachment-post-thumbnail wp-post-image']/@src");
                            var songlink = htmlDocument.DocumentNode.SelectSingleNode("//p/iframe/@src");
                            
                            if (songtitle != null)
                            {
                                
                               // description.ParentNode.ChildNodes.Count;

                                string link = "";
                                //for (int i = 0; i < description.Count; i++)
                                //{


                                    string imgsrc = songimage.Attributes["src"].Value;

                                    if (songlink != null) link = songlink.Attributes["src"].Value;
                                    else {
                                        sitesongbutton.Content = "No Content";
                                        Trackartistblock.Text = "Sorry, Cant view the video";                                   
                                    };

                                     
                                    //songtitle[i].InnerText = songtitle[i].InnerText.Replace("&#8217;", "'");
                                    sitesong = new Mp3Object(songtitle.InnerHtml.Trim(), fulldescription(description), imgsrc, link);
                                //}

                                    title1.Text = "PMM";
                                    title2.Text = sitesong.SongTitle;
                                    pagecontent.Text = sitesong.SongDescription;
                                    ImageBrush r = new ImageBrush ();
                                    r.ImageSource = sitesong.SongImage;
                                    LayoutRoot.Background = r;

                                    Trackartistblock.Text = sitesong.SongTitle;
                                    
                                //LayoutRoot.Background = sitesong.SongImage;
                            }

                        }




                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
  protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            string link = "";

            if (NavigationContext.QueryString.TryGetValue("msg", out link))

                
            startServerRequest(link);

           
        }

  private string fulldescription(HtmlNode d)
  {
      string fullcontent = "";
      var content = d.ParentNode.FirstChild;

      for (int i = 0; i < d.ParentNode.ChildNodes.Count; i++)
      {
          fullcontent += content.InnerText.Trim() + " ";
          content = content.NextSibling;

      }
      return fullcontent;
  }

  private void sitesongbutton_Click(object sender, RoutedEventArgs e)
  {
      if(sitesong.SongLink.Contains("soundcloud.com"))
      NavigationService.Navigate(new Uri("/NowPlaying.xaml?msg=" + sitesong.gettrackid() + "&msg2=" + sitesong.SongTitle + "&image=" + sitesong.ImageUri, UriKind.RelativeOrAbsolute));

      //else if (sitesong.SongLink.Contains("youtube.com"))
      //NavigationService.Navigate(new Uri("/VideoPlayer.xaml?msg=" + sitesong.getyoutubeaudio() + "&msg2=" + sitesong.SongTitle + "&image=" + sitesong.ImageUri, UriKind.RelativeOrAbsolute));
      
      else if(sitesong.getwebvideo().Length > 0)
      {
         
          
          WebBrowserTask webBrowserTask = new WebBrowserTask();
          webBrowserTask.Uri= new Uri(sitesong.getwebvideo());
          //webBrowserTask.Uri = new Uri("http://www.worldstarhiphop.com/embed/67115?wmode=transparent");
          webBrowserTask.Show();
      }

      
  }

  private void ShowSplash()
  {
      this.popup = new Popup();
      this.popup.Child = new pagesplash();
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
      Thread.Sleep(5000);
  }
  void backroungWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
  {
      this.Dispatcher.BeginInvoke(() =>
      {
          this.popup.IsOpen = false;
      }
      );
  }
    }
  
}

    
  
       
