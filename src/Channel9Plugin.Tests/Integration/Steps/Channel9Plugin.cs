using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;

// ReSharper disable InconsistentNaming
namespace Channel9Plugin.Tests.Integration.Steps
{
    [Binding]
    public class Channel9Plugin
    {
        [Given(@"a Channel 9 provider")]
        public void GivenAChannel9Provider()
        {
            ScenarioContext.Current.Pending();
        }

        [Given(@"an RSS file 'Channel9\.rss'")]
        public void GivenAnRSSFileChannel9Rss()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"it should be named 'RSS'")]
        public void ThenItShouldBeNamedRSS()
        {
            ScenarioContext.Current.Pending();
        }

        [Then(@"there should be only 1 child")]
        public void ThenThereShouldBeOnly1Child()
        {
            ScenarioContext.Current.Pending();
        }

        [When(@"I retrieve the children of the root")]
        public void WhenIRetrieveTheChildrenOfTheRoot()
        {
            ScenarioContext.Current.Pending();
        }
    }
}

// ReSharper restore InconsistentNaming