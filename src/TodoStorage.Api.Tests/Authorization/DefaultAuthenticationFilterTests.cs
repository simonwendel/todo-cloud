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

namespace TodoStorage.Api.Tests.Authorization
{
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http.Filters;
    using Moq;
    using NUnit.Framework;
    using TodoStorage.Api.Authorization;
    using TodoStorage.Api.Tests.Utilities;

    [TestFixture]
    internal class DefaultAuthenticationFilterTests
    {
        private Mock<IAuthenticator> authenticator;

        private Mock<IChallenger> challenger;

        private FakeHttpAuthenticationContext authenticationContext;

        private FakeHttpAuthenticationChallengeContext challengeContext;

        private CancellationToken token;

        private DefaultAuthenticationFilter sut;

        [SetUp]
        public void Setup()
        {
            authenticator = new Mock<IAuthenticator>();
            challenger = new Mock<IChallenger>();

            authenticationContext = new FakeHttpAuthenticationContext();
            challengeContext = new FakeHttpAuthenticationChallengeContext();
            token = new CancellationToken();

            sut = new DefaultAuthenticationFilter(authenticator.Object, challenger.Object);
        }

        [Test]
        public void Ctor_GivenNullAuthenticator_ThrowsException()
        {
            TestDelegate constructorCall =
                () => new DefaultAuthenticationFilter(null, challenger.Object);

            Assert.That(constructorCall, Throws.ArgumentNullException);
        }

        [Test]
        public void Ctor_GivenNullChallenger_ThrowsException()
        {
            TestDelegate constructorCall =
                () => new DefaultAuthenticationFilter(authenticator.Object, null);

            Assert.That(constructorCall, Throws.ArgumentNullException);
        }

        [Test]
        public void AllowMultiple_IsFalse()
        {
            Assert.That(sut.AllowMultiple, Is.False);
        }

        [Test]
        public async Task AuthenticateAsync_GivenContext_CallsAuthenticator()
        {
            await sut.AuthenticateAsync(authenticationContext, token);

            authenticator.Verify(
                a => a.Authenticate(It.Is<HttpAuthenticationContext>(c => c == authenticationContext)), 
                Times.Once);
        }

        [Test]
        public async Task ChallengeAsync_GivenContext_CallsChallenger()
        {
            await sut.ChallengeAsync(challengeContext, token);

            challenger.Verify(
                a => a.Challenge(It.Is<HttpAuthenticationChallengeContext>(c => c == challengeContext)),
                Times.Once);
        }
    }
}
