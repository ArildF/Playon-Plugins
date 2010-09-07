using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.XPath;
using MediaMallTechnologies.Plugin;
using Moq;
using Rogue.PlayOn.Plugins.Channel9;
using TechTalk.SpecFlow;
using Satisfyr;

// ReSharper disable InconsistentNaming
namespace Channel9Plugin.Tests.Integration.Steps
{
    [Binding]
    public class Channel9PluginSteps : TestBase
    {
        private string _rss;
        private Mock<IDownloader> _client;
        private Channel9Provider _provider;
        private Channel9Settings _settings;
        private Payload _payload;
        private AbstractSharedMediaInfo _current;
        private SharedMediaFileInfo _fileInfo;
        private string _xml;
        private VideoResource _video;

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

        [When(@"I retrieve the payload of '(.*)'")]
        public void WhenIRetrieveThePayloadOfRootRSS(string path)
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

        [When(@"I retrieve media child \#(\d+) of '(.*)'")]
        public void WhenIRetrieveChild1OfRootRSS(int num, string path)
        {
            WhenIRetrieveThePayloadOfRootRSS(path);

            _fileInfo = (SharedMediaFileInfo) _payload.Items.Skip(num - 1).Take(1).Single();

        }

        [When(@"I retrieve the payload of '(.*)' without children")]
        public void WhenIRetrieveThePayloadOfRootRSS1WithoutChildren(string path)
        {
            string folder = path.Substring(0, path.LastIndexOf("=>"));

            WhenIRetrieveThePayloadOfRootRSS(folder);

            int index = int.Parse(path.Substring(folder.Length + "=>".Length));

            _payload = _provider.GetSharedMedia(_payload.Items[index -1].Id, false, 0, 0);
        }

        [When(@"I examine child \#(\d+) as a video file")]
        public void WhenIExamineChild1AsAVideoFile(int childNo)
        {
            _video = (VideoResource) _payload.Items[childNo - 1];
        }

        [When(@"I resolve the item into XML")]
        public void WhenIResolveTheItemIntoXML()
        {
            _xml = _provider.Resolve(_fileInfo);

        }

        [Then(@"there should be only 1 child")]
        public void ThenThereShouldBeOnly1Child()
        {
            _payload.Items.Satisfies(items => items.Count == 1);
        }

        [Then(@"the xml should contain ""(.*)""")]
        public void ThenTheXmlShouldContainMediaUrlTypeWmv(string xpath)
        {
            var doc = new XPathDocument(new StringReader(_xml));

            var node = doc.CreateNavigator().Select(xpath).Cast<XPathNavigator>().SingleOrDefault();

            node.Satisfies(n => n != null);
        }

        [Then(@"the video file should have a media URL of '(.*)'")]
        public void ThenTheVideoFileShouldHaveAMediaURLOf(string url)
        {
            _video.Path.Satisfies(p => p == url);
        }

        [Then(@"the video file should have a duration of (\d+)")]
        public void ThenTheVideoFileShouldHaveALengthOf12(long duration)
        {
            _video.Duration.Satisfies(fs => fs == duration);
        }

        [Then(@"the video file should have a thumbnail '(.*)'")]
        public void ThenTheVideoFileShouldHaveAThumbnail(string url)
        {
            _video.Satisfies(v => v.ThumbnailUrl == url);
        }



        [Then(@"the payload should be a media file")]
        public void ThenThePayloadShouldBeAMediaFile()
        {
            _payload.IsContainer.Satisfies(ic => ic == false);
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