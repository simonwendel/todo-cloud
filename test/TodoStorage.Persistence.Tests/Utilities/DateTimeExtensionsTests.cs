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

namespace TodoStorage.Persistence.Tests.Utilities
{
    using System;
    using System.Data.SqlTypes;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    internal class DateTimeExtensionsTests
    {
        [Test]
        public void SqlNormalize_GivenDateTime_ReturnsSqlDateTime()
        {
            var date = DateTime.Now;
            var expected = new SqlDateTime(date).Value;
            date.SqlNormalize().Should().Be(expected);
        }

        [Test]
        public void SqlNormalize_GivenOutOfRangeDateTime_ThrowsException()
        {
            var date = new DateTime();
            Action normalizing = () => date.SqlNormalize();
            normalizing.Should().Throw<Exception>();
        }

        [Test]
        public void SqlNormalize_GivenNullableDateTimeWithoutValue_ThrowsException()
        {
            DateTime? nullDateTime = null;
            Action normalizing = () => nullDateTime.SqlNormalize();
            normalizing.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void SqlNormalize_GivenNullableDateTimeWithValue_ReturnsSqlDateTime()
        {
            DateTime? date = DateTime.Now;
            var expected = new SqlDateTime(date.Value).Value;
            date.SqlNormalize().Should().Be(expected);
        }
    }
}
