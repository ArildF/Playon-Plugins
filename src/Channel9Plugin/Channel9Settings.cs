using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MediaMallTechnologies.Plugin;
using Rogue.PlayOn.Plugins.Channel9.Properties;

namespace Rogue.PlayOn.Plugins.Channel9
{
    public class Channel9Settings : IPlayOnProviderSettings
    {
        public bool TestLogin(string username, string password)
        {
            throw new NotImplementedException();
        }

        public string CheckForUpdate()
        {
            throw new NotImplementedException();
        }

        public Control ConfigureOptions(NameValueCollection options, EventHandler changeHandler)
        {
            throw new NotImplementedException();
        }

        public Image Image
        {
            get { return Resources.logo; }
        }

        public string Link
        {
            get { return "www.testuri.com"; }
        }

        public string Name
        {
            get { throw new NotImplementedException(); }
        }

        public string ID
        {
            get { throw new NotImplementedException(); }
        }

        public string Description
        {
            get { return "Channel 9 (MSDN)"; }
        }

        public bool RequiresLogin
        {
            get { throw new NotImplementedException(); }
        }

        public bool HasOptions
        {
            get { return false; }
        }
    }
}
