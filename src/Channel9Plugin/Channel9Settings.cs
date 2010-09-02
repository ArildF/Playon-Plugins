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
        private readonly Guid _id = new Guid("{BCFD3A35-D13C-41DA-B4FF-A68B6DB6F968}");
        public bool TestLogin(string username, string password)
        {
            throw new NotImplementedException();
        }

        public string CheckForUpdate()
        {
            return null;
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
            get { return "Channel 9"; }
        }

        public string ID
        {
            get { return _id.ToString(); }
        }

        public string Description
        {
            get { return "Channel 9 (MSDN)"; }
        }

        public bool RequiresLogin
        {
            get { return false; }
        }

        public bool HasOptions
        {
            get { return false; }
        }
    }
}
