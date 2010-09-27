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
    public class Channel9PluginSteps : TestBase
    {
        private string _rss;
        private Mock<IDownloader> _client;
        private Channel9Provider _provider;
        private Payload _payload;
        private AbstractSharedMediaInfo _current;
        private SharedMediaFileInfo _fileInfo;
        private string _xml;
        private VideoResource _video;
        private SharedMediaFolderInfo _folder;

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


        [Given(@"a file '(.*)' at the URL '(.*)'")]
        public void GivenAnRSSFileChannel9RssAtUrl(string file, string url)
        {
            string path = Path.Combine("Resources", file);

            _rss = File.ReadAllText(path);
            _client.Setup(c => c.DownloadString(url)).Returns(_rss);
        }

        [Given(@"the following files for URLs:")]
        public void GivenTheFollowingFilesForURLs(Table table)
        {
            foreach (var tableRow in table.Rows)
            {
                GivenAnRSSFileChannel9RssAtUrl(tableRow["File"], tableRow["Url"]);
            }
        }

        [Then(@"item (.*) should be named '(.*)'")]
        public void ThenItShouldBeNamedRSS(int childNo, string name)
        {
            _payload.Items[childNo].Title.Satisfies(t => t == name);
        }

        [When(@"I browse '(.*)'")]
        public void WhenIBrowse(string path)
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

        [When(@"I browse item \#(\d+) of '(.*)'")]
        public void WhenIRetrieveChild1OfRootRSS(int num, string path)
        {
            WhenIBrowse(path);

            _fileInfo = (SharedMediaFileInfo) _payload.Items.Skip(num - 1).Take(1).Single();

        }

        [When(@"I browse (\d+) items starting from index (\d+) of '(.*)'")]
        public void WhenIBrowse4ItemsStartingFromIndex3OfRootRSS(int count, int start, string path)
        {
            WhenIBrowse(path);
            _payload = _provider.GetSharedMedia(_current.Id, true, start, count);
        }

        [When(@"I browse the first (\d+) items of '(.*)'")]
        public void WhenIBrowseTheFirst5ItemsOfRootRSS(int count, string path)
        {
            WhenIBrowse(path);
            _payload = _provider.GetSharedMedia(_current.Id, true, 0, count);
        }

        [When(@"I browse '(.*)' without children")]
        public void WhenIBrowseWithoutChildren(string path)
        {
            string folder = path.Substring(0, path.LastIndexOf("=>"));

            WhenIBrowse(folder);

            int index = int.Parse(path.Substring(folder.Length + "=>".Length));

            _payload = _provider.GetSharedMedia(_payload.Items[index -1].Id, false, 0, 0);
        }

        [When(@"item \#(\d+) is a video file")]
        public void WhenIExamineChild1AsAVideoFile(int childNo)
        {
            _video = (VideoResource) _payload.Items[childNo - 1];
        }

        [When(@"item \#(\d+) is a folder")]
        public void WhenIExamineChild1AsAFolder(int childNo)
        {
            _folder = (SharedMediaFolderInfo) _payload.Items[childNo - 1];

        }

        [When(@"I examine the item as XML")]
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

        [Then(@"the video should have a media URL of '(.*)'")]
        public void ThenTheVideoFileShouldHaveAMediaURLOf(string url)
        {
            _video.Path.Satisfies(p => p == url);
        }

        [Then(@"the video should have a duration of (\d+)")]
        public void ThenTheVideoFileShouldHaveALengthOf12(long duration)
        {
            _video.Duration.Satisfies(fs => fs == duration);
        }

        [Then(@"the folder should have a title of '(.*)'")]
        public void ThenTheFolderShouldHaveATitleOfInTheOffice(string folder)
        {
            _folder.Title.Satisfies(t => t == folder);
        }

        [Then(@"the video should have a thumbnail '(.*)'")]
        public void ThenTheVideoFileShouldHaveAThumbnail(string url)
        {
            _video.Satisfies(v => v.ThumbnailUrl == url);
        }

        [Then(@"the video should have a publication date of '(.*)'")]
        public void ThenTheVideoFileShouldHaveAPublicationDate(string dateString)
        {
            DateTime dt = DateTime.ParseExact(dateString, "R", CultureInfo.InvariantCulture);

            _video.Satisfies(v => v.Date == dt);
        }



        [Then(@"it should be a media file")]
        public void ThenThePayloadShouldBeAMediaFile()
        {
            _payload.IsContainer.Satisfies(ic => ic == false);
        }

        [Then(@"item (.*) should have these attributes:")]
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



        [Then(@"there should be (\d+) items")]
        public void ThenThereShouldBe25Children(int num)
        {
            _payload.Items.Count.Satisfies(c => c == num);
        }

        [Then(@"the provider should have an image")]
        public void ThenTheProviderShouldHaveAnImage()
        {
            _provider.Satisfies(p => p.Image != null);
        }

        [Then(@"the provider should have the name '(.*)'")]
        public void ThenTheProviderShouldHaveTheNameChannel9MSDN(string name)
        {
            _provider.Satisfies(p => p.Name == name);
        }

        [Then(@"I should get an error when I browse '(.*)'")]
        public void ThenIShouldGetAnErrorWhenIBrowseRootRSS(string path)
        {
            Assert.Throws<InvalidFeedException>(() => WhenIBrowse(path));
        }

        [Then(@"the item names should have sort prefixes ordered by publication date descending")]
        public void ThenTheChildrenShouldHaveSortPrefixesOrderedByPublicationDateDescending()
        {
            var sortedByDate = _payload.Items.Cast<SharedMediaFileInfo>().OrderByDescending(smfi => smfi.Date).ToArray();
            var sortedByPrefixes =
                _payload.Items.Cast<SharedMediaFileInfo>().OrderBy(smfi => smfi.MetadataProperties["SortIndex"])
                    .ToArray();

            CollectionAssert.AreEqual(sortedByDate, sortedByPrefixes);
        }

        [Then(@"the video description should not contain HTML tags")]
        public void ThenTheDescriptionShouldNotContainHTMLTags()
        {
            var regex = new Regex(@"<.*>");

            _video.Description.Satisfies(d => !regex.IsMatch(d));
        }

        [When(@"I browse the root")]
        public void WhenIBrowseTheRoot()
        {
            _payload = _provider.GetSharedMedia(_provider.ID, true, 0, 0);

        }
    }
}

// ReSharper restore InconsistentNaming