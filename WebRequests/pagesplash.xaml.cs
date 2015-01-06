using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media;

namespace WebRequests
{
    public partial class pagesplash : UserControl
    {
        public pagesplash()
        {
            InitializeComponent();
            this.progressBar1.IsIndeterminate = true;
        }
    }
}
