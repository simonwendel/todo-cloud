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
    using System.Web.Http.Filters;
    using Common.Logging;
    using Moq;
    using NUnit.Framework;
    using TodoStorage.Api.Configuration;
    using TodoStorage.Api.Tests.Utilities;

    [TestFixture]
    internal class LogExceptionFilterAttributeTests
    {
        private LogExceptionFilterAttribute sut;

        private Mock<ILog> logger;

        [SetUp]
        public void Setup()
        {
            var loggerFactory = 
                new MockLoggerFactory<LogExceptionFilterAttribute>();

            logger = loggerFactory.CreateLogger();
            logger.Setup(
                l => l.Fatal(It.IsAny<string>(), It.IsAny<Exception>()));

            sut = new LogExceptionFilterAttribute();
        }

        [Test]
        public void OnException_GivenNullContext_ThrowsException()
        {
            TestDelegate exceptionCall =
                () => sut.OnException(null);

            Assert.That(exceptionCall, Throws.ArgumentNullException);
        }

        [Test]
        public void OnException_GivenContext_LogsException()
        {
            var exception = new Exception();
            var context = new HttpActionExecutedContext
            {
                Exception = exception
            };

            sut.OnException(context);

            logger.Verify(
                l => l.Fatal(
                    It.Is<string>(s => s.Equals("Exception caught globally")), 
                    It.Is<Exception>(e => e == exception)), 
                Times.Once);
        }
    }
}
