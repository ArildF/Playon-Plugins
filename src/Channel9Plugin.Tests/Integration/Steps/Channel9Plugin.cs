using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MediaMallTechnologies.Plugin;
using Moq;
using Rogue.PlayOn.Plugins.Channel9;
using TechTalk.SpecFlow;
using Satisfyr;

// ReSharper disable InconsistentNaming
namespace Channel9Plugin.Tests.Integration.Steps
{
    [Binding]
    public class Channel9Plugin : TestBase
    {
        private string _rss;
        private Mock<IDownloader> _client;
        private Channel9Provider _provider;
        private Channel9Settings _settings;
        private Payload _payload;
        private AbstractSharedMediaInfo _current;

        [BeforeScenario]
        public void Init()
        {
            base.Setup();

            _client = GetMock<IDownloader>();
        }

        [Given(@"a Channel 9 provider")]
        public void GivenAChannel9Provider()
        {
            _provider = new Channel9Provider();
            _provider.SetPlayOnHost(Create<IPlayOnHost>());
            
            _provider.SetWebClient(_client.Object);
        }

        [Given(@"a settings object")]
        public void GivenASettingsObject()
        {
            _settings = new Channel9Settings();

        }

        [Given(@"an RSS file '(.*)'")]
        public void GivenAnRSSFileChannel9Rss(string file)
        {
            _rss = File.ReadAllText(file);
            _client.Setup(c => c.DownloadString(It.IsAny<string>())).Returns(_rss);
        }

        [Then(@"child (.*) should be named '(.*)'")]
        public void ThenItShouldBeNamedRSS(int childNo, string name)
        {
            _payload.Items[childNo].Title.Satisfies(t => t == name);
        }

        [When(@"I retrieve the children of '(.*)'")]
        public void WhenIRetrieveTheChildrenOfRootRSS(string path)
        {
            var elts = path.Split(new[] {"=>"}, StringSplitOptions.RemoveEmptyEntries);
            var stack = new Queue<string>(elts);
            string id = _provider.ID;

            _current = null;

            while (stack.Count > 0)
            {
                string component = stack.Dequeue();

                if (component != "root")
                {
                    _current = _payload.Items.Where(item => item.Title == component)
                        .First();
                    id = _current.Id;
                }
                _payload = _provider.GetSharedMedia(id, true, 0, 0);
            }
        }

        [Then(@"there should be only 1 child")]
        public void ThenThereShouldBeOnly1Child()
        {
            _payload.Items.Satisfies(items => items.Count == 1);
        }

        [Then(@"child (.*) should have these attributes:")]
        public void ThenChild1ShouldHaveTheseAttributes(int index, Table table)
        {
            var info = _payload.Items.Skip(index - 1).First();

            foreach (var tableRow in table.Rows)
            {
                string name = tableRow["Name"];

                var propInfo = info.GetType().GetProperty(name);

                string stringValue = tableRow["Value"];
                object expectedValue = Convert.ChangeType(stringValue, propInfo.PropertyType);

                var actualValue = propInfo.GetValue(info, null);

                actualValue.Satisfies(i => i.Equals(expectedValue));
            }
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

        [Then(@"there should be (\d+) children")]
        public void ThenThereShouldBe25Children(int num)
        {
            _payload.Items.Count.Satisfies(c => c == num);
        }

        [When(@"I retrieve the children of the root")]
        public void WhenIRetrieveTheChildrenOfTheRoot()
        {
            _payload = _provider.GetSharedMedia(_provider.ID, true, 0, 0);

        }
    }
}

// ReSharper restore InconsistentNaming