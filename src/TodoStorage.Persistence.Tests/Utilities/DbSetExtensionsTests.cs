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

namespace TodoStorage.Persistence.Tests.Utilities
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class DbSetExtensionsTests
    {
        [Test]
        public void Clear_GivenNullDbSet_ThrowsException()
        {
            DbSet<object> nullSet = null;

            Assert.That(() => nullSet.Clear(), Throws.ArgumentNullException);
        }

        [Test]
        public void Clear_GivenDbSet_CallsRemoveRangeWithSelf()
        {
            var mockSet = new Mock<DbSet<object>>();
            IEnumerable<object> parameter = null;

            mockSet
                .Setup(db => db.RemoveRange(It.IsAny<IEnumerable<object>>()))
                .Callback((IEnumerable<object> param) =>
                {
                    parameter = param;
                });

            mockSet.Object.Clear();

            Assert.That(parameter, Is.SameAs(mockSet.Object));
            mockSet.VerifyAll();
        }
    }
}
