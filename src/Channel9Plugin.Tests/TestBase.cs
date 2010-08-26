using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;

namespace Channel9Plugin.Tests
{
    [TestFixture]
    public class TestBase
    {
        protected readonly MockFactory Factory =
            new MockFactory(MockBehavior.Loose) 
            { DefaultValue = DefaultValue.Mock, CallBase = true};

        private AutoMockContainer _autoContainer;


        [SetUp]
        public virtual void Setup()
        {
            _autoContainer = new AutoMockContainer(Factory);

            OnSetup();
        }

        protected virtual void OnSetup()
        {
            

        }

    	protected T Create<T>() where T : class
    	{
    	    return _autoContainer.Create<T>();
    	}

    	protected Mock<T> GetMock<T>() where T : class
    	{
    	    return _autoContainer.GetMock<T>();
    	}

        protected void Register<T>(T instance)
        {
            _autoContainer.Register(instance);
        }
    }
}
