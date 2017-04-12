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

namespace TodoStorage.Api.Tests.Authorization
{
    using System.Net;
    using System.Security.Claims;
    using Moq;
    using NUnit.Framework;
    using TodoStorage.Api.Authorization;
    using TodoStorage.Api.Tests.Utilities;
    using TodoStorage.Security;

    [TestFixture]
    internal class SignedMessageAuthorizeAttributeTests
    {
        private FakeHttpActionContext context;

        private SignedMessageAuthorizeAttribute sut;

        [SetUp]
        public void Setup()
        {
            context = new FakeHttpActionContext();

            sut = new SignedMessageAuthorizeAttribute();
        }

        [Test]
        public void OnAuthorization_GivenNullPrincipal_SetsNotAuthorized()
        {
            context.RequestContext.Principal = null;

            sut.OnAuthorization(context);

            Assert.That(context.Response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }

        [Test]
        public void OnAuthorization_GivenOtherPrincipal_SetsNotAuthorized()
        {
            context.RequestContext.Principal = new ClaimsPrincipal();

            sut.OnAuthorization(context);

            Assert.That(context.Response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
        }

        [Test]
        public void OnAuthorization_GivenSignedMessagePrincipal_SetsAuthorized()
        {
            context.RequestContext.Principal = new SignedMessagePrincipal(Mock.Of<IMessage>());

            sut.OnAuthorization(context);

            Assert.That(context.Response.StatusCode, Is.Not.EqualTo(HttpStatusCode.Unauthorized));
        }
    }
}
