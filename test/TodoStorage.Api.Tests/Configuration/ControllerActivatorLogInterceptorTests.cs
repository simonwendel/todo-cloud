/*
 * Todo Storage for wifeys Todo app.
 * Copyright (C) 2016-2017  Simon Wendel
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

namespace TodoStorage.Api.Tests.Configuration
{
    using System;
    using Common.Logging;
    using Moq;
    using NUnit.Framework;
    using TodoStorage.Api.Configuration;
    using TodoStorage.Api.Tests.Utilities;

    [TestFixture]
    internal class ControllerActivatorLogInterceptorTests
    {
        private ControllerActivatorLogInterceptor sut;

        private Mock<ILog> logger;

        private Mock<Ninject.Extensions.Interception.IInvocation> invocation;

        [SetUp]
        public void Setup()
        {
            invocation = new Mock<Ninject.Extensions.Interception.IInvocation>();

            var loggerFactory = 
                new MockLoggerFactory<ControllerActivatorLogInterceptor>();

            logger = loggerFactory.CreateLogger();
            logger.Setup(
                l => l.Fatal(It.IsAny<string>(), It.IsAny<Exception>()));

            sut = new ControllerActivatorLogInterceptor();
        }

        [Test]
        public void Intercept_GivenNullInvocation_ThrowsException()
        {
            TestDelegate interceptCall =
                () => sut.Intercept(null);

            Assert.That(interceptCall, Throws.ArgumentNullException);
        }

        [Test]
        public void Intercept_WhenInvocationThrowsException_LogsExceptionAndRethrows()
        {
            var exception = new Exception();
            invocation
                .Setup(i => i.Proceed())
                .Throws(exception);

            TestDelegate interceptCall =
                () => sut.Intercept(invocation.Object);

            Assert.That(interceptCall, Throws.Exception);
            logger.Verify(
                l => l.Fatal(
                    It.Is<string>(s => s.Equals("Exception caught in Controller Activator")),
                    It.Is<Exception>(e => e == exception)),
                Times.Once);
        }

        [Test]
        public void Intercept_WhenInvocationSucceeds_DoesNotLogException()
        {
            var exception = new Exception();
            invocation
                .Setup(i => i.Proceed());

            sut.Intercept(invocation.Object);

            logger.Verify(
                l => l.Fatal(
                    It.IsAny<string>(),
                    It.IsAny<Exception>()),
                Times.Never);
        }
    }
}
