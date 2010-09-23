using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.XPath;
using MediaMallTechnologies.Plugin;
using Moq;
using NUnit.Framework;
using Rogue.PlayOn.Plugins.Channel9;
using TechTalk.SpecFlow;
using Satisfyr;

// ReSharper disable InconsistentNaming
namespace Channel9Plugin.Tests.Integration.Steps
{
    [Binding]
    public class SettingsSteps : TestBase
    {
        private string _rss;
        private Channel9Settings _settings;

        [BeforeScenario]
        public void Init()
        {
            base.Setup();
        }


        [Given(@"a settings object")]
        public void GivenASettingsObject()
        {
            _settings = new Channel9Settings();

        }


        [Then(@"the settings should have a description of '(.*)'")]
        public void ThenTheSettingsShouldHaveADescriptionOfChannel9MSDN(string desc)
        {
            _settings.Satisfies(s => s.Description == desc);
        }

        [Then(@"the settings should have an image")]
        public void ThenTheSettingsShouldHaveAnImage()
        {
            _settings.Satisfies(s => s.Image != null);
        }

        [Then(@"the settings should have an ID")]
        public void ThenTheSettingsShouldHaveAnID()
        {
            _settings.Satisfies(s => s.ID != null);
        }

        [Then(@"the settings should have the name '(.*)'")]
        public void ThenTheSettingsShouldHaveTheNameChannel9MSDN(string name)
        {
            _settings.Satisfies(s => s.Name == name);
        }

        [Then(@"the settings should have the link '(.*)'")]
        public void ThenTheSettingsShouldHaveTheLinkHttpGithub_ComArildFChannel_9_Playon_Plugin(string url)
        {
            _settings.Link.Satisfies(lnk => lnk == url);
        }

        [Then(@"checking for updates should not be supported")]
        public void ThenCheckingForUpdatesShouldNotBeSupported()
        {
            _settings.CheckForUpdate().Satisfies(u => u == null);
        }

        [Then(@"configuring options should not be supported")]
        public void ThenConfiguringOptionsShouldNotBeSupported()
        {
            _settings.HasOptions.Satisfies(ho => ho == false);
            Assert.Throws<NotSupportedException>(() => _settings.ConfigureOptions(null, null));
        }

        [Then(@"login should not be required")]
        public void ThenLoginShouldNotBeRequired()
        {
            _settings.RequiresLogin.Satisfies(rl => rl == false);
            Assert.Throws<NotSupportedException>(() => _settings.TestLogin(null, null));
        }

    }
}

// ReSharper restore InconsistentNaming