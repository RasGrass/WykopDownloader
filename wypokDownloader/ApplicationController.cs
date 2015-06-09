using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using wypokDownloader.Model;
using wypokDownloader.View;

namespace wypokDownloader
{
    class ApplicationController
    {
        static ApplicationController _instance;
        public static ApplicationController GetInstance()
        {
            if (_instance == null)
                _instance = new ApplicationController();
            return _instance;
        }

       
        private ApplicationController() { }

    }
}
