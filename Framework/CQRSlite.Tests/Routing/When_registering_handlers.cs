﻿using System;
using System.Threading;
using CQRSlite.Routing;
using CQRSlite.Tests.Substitutes;
using Xunit;

namespace CQRSlite.Tests.Routing
{
    public class When_registering_handlers
    {
        private readonly TestServiceLocator _locator;

        public When_registering_handlers()
        {
            _locator = new TestServiceLocator();
            var register = new RouteRegistrar(_locator);
            if (TestHandleRegistrar.HandlerList.Count == 0)
                register.Register(GetType());
        }

        [Fact]
        public void Should_register_all_handlers()
        {
            // 4 public declared handlers, one internal declared
            Assert.Equal(5, TestHandleRegistrar.HandlerList.Count);
        }

        [Fact]
        public void Should_be_able_to_run_all_handlers()
        {
            foreach (var item in TestHandleRegistrar.HandlerList)
            {
                var @event = Activator.CreateInstance(item.Type);
                item.Handler(@event, new CancellationToken());
            }
            foreach (var handler in _locator.Handlers)
            {
                Assert.Equal(1, handler.TimesRun);
            }
        }
    }
}
