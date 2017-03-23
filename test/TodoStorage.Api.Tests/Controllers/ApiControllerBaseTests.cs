/*
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
    using Moq;
    using NUnit.Framework;
    using TodoStorage.Api.Controllers;
    using TodoStorage.Core;

    [TestFixture]
    internal class ApiControllerBaseTests : ControllerTestFixtureBase, IDisposable
    {
        private TestApiController sut;

        [SetUp]
        public void Setup()
        {
            sut = new TestApiController();
        }

        [Test]
        public void Key_IfNullPrincipal_ThrowsException()
        {
            SetPrincipal(null);

            Assert.That(() => sut.Key, Throws.InvalidOperationException);
        }

        [Test]
        public void Key_IfNotSignedMessagePrincipal_ThrowsException()
        {
            SetPrincipal(Mock.Of<IPrincipal>());

            Assert.That(() => sut.Key, Throws.InvalidOperationException);
        }

        [Test]
        public void Key_ReturnsAppIdFromPrincipal()
        {
            var appId = Guid.NewGuid();
            var key = new CollectionKey(appId);
            
            SetPrincipal(appId);

            var sut = new TestApiController();

            Assert.That(sut.Key, Is.EqualTo(key));
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

        private class TestApiController : ApiControllerBase
        {
            // Overrides app id property to raise access to internal
            // making it accessible to the external test fixture
            internal new CollectionKey Key => base.Key;
        }
    }
}
