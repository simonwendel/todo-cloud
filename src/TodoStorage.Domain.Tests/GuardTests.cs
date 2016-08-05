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

namespace TodoStorage.Domain.Tests
{
    using NUnit.Framework;

    [TestFixture]
    public class GuardTests
    {
        [Test]
        public void NullParameter_GivenNonNullObject_DoesNothing()
        {
            var nonNullObject = new object();

            Guard.NullParameter(nonNullObject);
        }

        [Test]
        public void NullParameter_GivenNonNullObjectAndParameterName_DoesNothing()
        {
            var nonNullObject = new object();

            Guard.NullParameter(nonNullObject, nameof(nonNullObject));
        }

        [Test]
        public void NullParameter_GivenNullObject_ThrowsExeption()
        {
            object nullObject = null;
            TestDelegate guardStatement = () => Guard.NullParameter(nullObject);

            Assert.That(guardStatement, Throws.ArgumentNullException);
        }

        [Test]
        public void NullParameter_GivenNullObjectAndParameterName_ThrowsExeption()
        {
            object nullObject = null;
            TestDelegate guardStatement = () => Guard.NullParameter(nullObject, nameof(nullObject));

            Assert.That(guardStatement, Throws.ArgumentNullException);
        }
    }
}
