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
    using NUnit.Framework;
    using TodoStorage.Api.Authorization;
    using TodoStorage.Api.Tests.Utilities;

    [TestFixture]
    internal class HmacChallengerTests
    {
        private FakeHttpAuthenticationChallengeContext context;

        private HmacChallenger sut;

        [SetUp]
        public void Setup()
        {
            context = new FakeHttpAuthenticationChallengeContext();

            sut = new HmacChallenger();
        }

        [Test]
        public void Challenge_GivenNullContext_ThrowsException()
        {
            TestDelegate challengeCall =
                () => sut.Challenge(null);

            Assert.That(challengeCall, Throws.ArgumentNullException);
        }

        [Test]
        public void Challenge_GivenContext_SetsTheContextResult()
        {
            sut.Challenge(context);

            Assert.That(context.Result is AuthenticationChallengeResult);
        }

        [Test]
        public void Challenge_GivenContext_AddsAppropriateScheme()
        {
            sut.Challenge(context);

            var result = context.Result as AuthenticationChallengeResult;

            Assert.That(result.Scheme.Equals(ValidAuthenticationSchemes.Hmac));
        }
    }
}
