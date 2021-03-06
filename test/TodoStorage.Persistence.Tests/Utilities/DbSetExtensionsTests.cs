﻿/*
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
    using System.Collections.Generic;
    using System.Data.Entity;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    internal class DbSetExtensionsTests
    {
        [Test]
        public void Clear_GivenNullDbSet_ThrowsException()
        {
            DbSet<object> nullSet = null;
            Action clearing = () => nullSet.Clear();
            clearing.Should().ThrowExactly<ArgumentNullException>();
        }

        [Test]
        public void Clear_GivenDbSet_CallsRemoveRangeWithSelf()
        {
            var dataSet = new Mock<DbSet<object>>();

            IEnumerable<object> parameter = null;
            dataSet
                .Setup(db => db.RemoveRange(It.IsAny<IEnumerable<object>>()))
                .Callback<IEnumerable<object>>(param => parameter = param);

            dataSet.Object.Clear();

            parameter.Should().BeSameAs(dataSet.Object);
            dataSet.VerifyAll();
        }
    }
}
