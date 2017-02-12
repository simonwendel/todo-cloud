﻿/*
 * Todo Storage for wifeys Todo app.
 * Copyright (C) 2016  Simon Wendel
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

namespace TodoStorage.Api.Tests.Controllers
{
    using System;
    using System.Security.Principal;
    using System.Threading;
    using Moq;
    using NUnit.Framework;
    using TodoStorage.Api.Authorization;
    using TodoStorage.Api.Controllers;
    using TodoStorage.Security;

    [TestFixture]
    internal class ApiControllerBaseTests : IDisposable
    {
        private TestApiController sut;

        [SetUp]
        public void Setup()
        {
            sut = new TestApiController();
        }

        [Test]
        public void ApplicationId_IfNullPrincipal_ThrowsException()
        {
            SetPrincipal(null);

            Assert.That(() => sut.ApplicationId, Throws.InvalidOperationException);
        }

        [Test]
        public void ApplicationId_IfNotSignedMessagePrincipal_ThrowsException()
        {
            SetPrincipal(Mock.Of<IPrincipal>());

            Assert.That(() => sut.ApplicationId, Throws.InvalidOperationException);
        }

        [Test]
        public void ApplicationId_ReturnsAppIdFromPrincipal()
        {
            var appId = Guid.NewGuid();

            var principal = CreateSignedMessagePrincipal(appId);
            SetPrincipal(principal);

            var sut = new TestApiController();

            Assert.That(sut.ApplicationId, Is.EqualTo(appId));
        }

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (sut != null)
                {
                    sut.Dispose();
                    sut = null;
                }
            }
        }

        #endregion

        private void SetPrincipal(IPrincipal principal)
        {
            Thread.CurrentPrincipal = principal;
        }

        private IPrincipal CreateSignedMessagePrincipal(Guid appId)
        {
            var message = new Mock<IMessage>();
            message
                .SetupGet(m => m.AppId)
                .Returns(appId);

            return new SignedMessagePrincipal(message.Object);
        }

        private class TestApiController : ApiControllerBase
        {
            // Overrides app id property to raise access to internal
            // making it accessible to the external test fixture
            internal new Guid ApplicationId => base.ApplicationId;
        }
    }
}