using System;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Windows.Controls;
using HtmlAgilityPack;


namespace WebRequests
{
   public class Mp3Object
    {
       public BitmapImage SongImage
       {
           get; 
           set; 
       }
         public string SongTitle
   {
      get;
      set;
   }

         private string tokenInfo
         {
             get;
             set;
         }

         private string token
         {
             get;
             set;
         }

         private string meData
         {
             get;
             set;
         }

         public string SongDescription
   {
      get;
      set;
   }
         public string SongLink
         {
             get;
             set;
         }


         public string Trackid
         {
             get;
             set;
         }
         public string ImageUri
         {
             get;
             set;
         }

   public Mp3Object(string title, string description, string image, string link )
   {
       description = replacestring(description);
       title = replacestring(title);
      this.SongTitle = title;
      this.SongDescription = description;
      this.SongLink = link;
      this.ImageUri = image;
       

      //BitmapImage bi3 = new BitmapImage();
      
      //bi3.UriSource = new Uri(image, UriKind.RelativeOrAbsolute);
      
      this.SongImage = new BitmapImage(new Uri(image, UriKind.RelativeOrAbsolute));
      //BitmapImage image = new BitmapImage(new Uri("/MyProject;component/Images/down.png", UriKind.Relative));
   }

     public string replacestring(string description)
    {
        description = description.Replace("&#8216;", "'");
        description = description.Replace("&#8217;", "'");
        description = description.Replace("&#8211;", "-");
        description = description.Replace("&#8212;", "-");
        description = description.Replace("&#8220;", "\"");
        description = description.Replace("&#8221;", "\"");
        description = description.Replace("&#8230;", "...");
        description = description.Replace("&#8242;", "'");
        description = description.Replace("&#8243;", "\"");
        description = description.Replace("&amp;", "&");

        description = description.Replace("&#038;", "&");
        description = description.Replace("<i>", "");
        description = description.Replace("</i>", "");
        description = description.Replace("<strong>;", "");
        description = description.Replace("</strong>", "");
        description = description.Replace("<em>", "");
        description = description.Replace("</em>", "");
        return description;
    }

    public string gettrackid ()
     {
        Trackid = "";

        
         string link = SongLink;

         if (link.Contains("https://w.soundcloud.com/player/?url=http%3A%2F%2Fapi.soundcloud.com%2Ftracks%2F"))
         {
             link = link.Replace("https://w.soundcloud.com/player/?url=http%3A%2F%2Fapi.soundcloud.com%2Ftracks%2F", "");
             Trackid = link;
         }
         else
         {
             link = link.Replace("https://w.soundcloud.com/player/?url=https%3A//api.soundcloud.com/tracks/", "");
             for (int i = 0; i < link.Length; i++)
             {

                 if (link[i] == '1' || link[i] == '2' || link[i] == '3' || link[i] == '4' || link[i] == '5' || link[i] == '6' || link[i] == '7' || link[i] == '8' || link[i] == '9' || link[i] == '0')
                 {
                     Trackid = Trackid.Insert(i, link[i].ToString());
                 }

                 else break;
             }

         }

        
             return Trackid;
     }

    public string getyoutubeaudio()
    {
        //Trackid = "http://www.youtube.com/watch?v=";
       Trackid = "";

        string link = SongLink;

        if (!link.Contains("http://")) link = link.Replace("//www.youtube.com/embed/", "");
        else link = link.Replace("http://www.youtube.com/embed/", "");
        
        for (int i = 0; i < link.Length; i++)
        {

            if (link[i] != '?')
            {
                Trackid = Trackid.Insert(Trackid.Length, link[i].ToString());
            }

            else break;
        }

        return Trackid;

    }

    public string getwebvideo()
    {
        string link = SongLink;


        if (!link.Contains("http://")) link = link.Replace("//", "http://");
        link = link.Replace("feature=player_embedded", "autoplay=1");

        return link;
    }

    /*public string accesstoken()
    {
        //WebClient to communicate via http
        WebClient client = new WebClient();

        //Client id form SoundCloud
        string ClientId = "6f8c3d888d377485e5efdd8628a9840d";

        //Client secret id from SoundCloud
        string ClientSecret = "3993e0edbe5286cb109e82126923e3d1";

        //Credentials (username & password)
        string username = "chilla.tenga@hotmail.com";
        string password = "jouchim";

        //Authentication data
        string postData = "client_id=" + ClientId
            + "&client_secret=" + ClientSecret
            + "&grant_type=password&username=" + username
            + "&password=" + password;

        //Authentication
        var soundCloudTokenRes = new Uri("https://api.soundcloud.com/oauth2/token");
        
        client = new WebClient();
        client.UploadStringCompleted += new UploadStringCompletedEventHandler(client_UploadStringCompleted);
        client.UploadStringAsync(soundCloudTokenRes, postData);
         
        
       
        

        //SoundCloud API Get Request
        string soundCloudMeRes = "https://api.soundcloud.com/me.xml";
        //string meData = client.DownloadString(soundCloudMeRes + "?oauth_token=" + token);
        client = new WebClient();
        client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(client_DownloadStringCompleted);
        client.DownloadStringAsync(new Uri(soundCloudMeRes + "?oauth_token=" + token));

        return meData;
        
    }
    void client_UploadStringCompleted(object sender, UploadStringCompletedEventArgs e)
    {
        tokenInfo = e.Result;
        //Parse the token
        tokenInfo = tokenInfo.Remove(0, tokenInfo.IndexOf("token\":\"") + 8);
        token = tokenInfo.Remove(tokenInfo.IndexOf("\""));

    }

    void client_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
    {
        meData = e.Result;
    }*/
}
}
