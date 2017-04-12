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
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Moq;
    using NUnit.Framework;
    using TodoStorage.Api.Authorization;

    [TestFixture]
    internal class AuthenticationChallengeResultTests : IDisposable
    {
        private Mock<IHttpActionResult> innerResult;

        private string challengeScheme;

        private CancellationToken cancellationToken;

        private HttpResponseMessage innerResponse;

        private AuthenticationChallengeResult sut;

        [SetUp]
        public void Setup()
        {
            innerResponse = new HttpResponseMessage();
            innerResponse.Headers.WwwAuthenticate.Clear();

            innerResult = new Mock<IHttpActionResult>();
            innerResult
                .Setup(r => r.ExecuteAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(innerResponse));

            challengeScheme = "fancyProtocol";
            cancellationToken = new CancellationToken();

            sut = new AuthenticationChallengeResult(innerResult.Object, challengeScheme);
        }

        [Test]
        public void Ctor_GivenNullInnerResult_ThrowsException()
        {
            TestDelegate constructorCall =
                () => new AuthenticationChallengeResult(null, challengeScheme);

            Assert.That(constructorCall, Throws.ArgumentNullException);
        }

        [Test]
        public void Ctor_GivenNullRealm_ThrowsException()
        {
            TestDelegate constructorCall =
                () => new AuthenticationChallengeResult(innerResult.Object, null);

            Assert.That(constructorCall, Throws.ArgumentNullException);
        }

        [Test]
        public void Ctor_GivenScheme_SetsProperty()
        {
            Assert.That(sut.Scheme, Is.EqualTo(challengeScheme));
        }

        [Test]
        public async Task ExecuteAsync_ExecutesInnerResult()
        {
            var response = await sut.ExecuteAsync(cancellationToken);

            Assert.That(response, Is.SameAs(innerResponse));
            innerResult.Verify(
                r => r.ExecuteAsync(It.Is<CancellationToken>(c => c == cancellationToken)),
                Times.Once);
        }

        [Test]
        public async Task ExecuteAsync_WhenUnauthorized_AddsWwwAuthenticateHeader()
        {
            innerResponse.StatusCode = HttpStatusCode.Unauthorized;

            var response = await sut.ExecuteAsync(cancellationToken);

            Assert.That(
                response.Headers.WwwAuthenticate.Any(
                    h => h.Scheme.Equals(challengeScheme)));
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
                if (innerResponse != null)
                {
                    innerResponse.Dispose();
                    innerResponse = null;
                }
            }
        }

        #endregion
    }
}
