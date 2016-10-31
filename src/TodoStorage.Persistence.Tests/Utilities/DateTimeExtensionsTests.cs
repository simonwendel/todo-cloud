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

namespace TodoStorage.Persistence.Tests.Utilities
{
    using System;
    using System.Data.SqlTypes;
    using NUnit.Framework;

    [TestFixture]
    internal class DateTimeExtensionsTests
    {
        [Test]
        public void SqlNormalize_GivenDateTime_ReturnsSqlDateTime()
        {
            var date = DateTime.Now;
            var expected = new SqlDateTime(date).Value;

            var actual = date.SqlNormalize();

            Assert.That(actual, Is.EqualTo(expected));
        }

        [Test]
        public void SqlNormalize_GivenOutOfRangeDateTime_ThrowsException()
        {
            var date = new DateTime();

            TestDelegate normalizeCall =
                () => date.SqlNormalize();

            Assert.That(normalizeCall, Throws.Exception);
        }

        [Test]
        public void SqlNormalize_GivenNullableDateTimeWithoutValue_ThrowsException()
        {
            DateTime? nullDateTime = null;

            TestDelegate normalizeCall =
                () => nullDateTime.SqlNormalize();

            Assert.That(normalizeCall, Throws.ArgumentNullException);
        }

        [Test]
        public void SqlNormalize_GivenNullableDateTimeWithValue_ReturnsSqlDateTime()
        {
            DateTime? date = DateTime.Now;
            var expected = new SqlDateTime(date.Value).Value;

            var actual = date.SqlNormalize();

            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}