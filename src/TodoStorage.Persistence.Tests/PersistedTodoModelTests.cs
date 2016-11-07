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

namespace TodoStorage.Persistence.Tests
{
    using Domain;
    using NUnit.Framework;
    using Ploeh.AutoFixture;
    using SimonWendel.ObjectExtensions;

    [TestFixture]
    internal class PersistedTodoModelTests
    {
        [Test]
        public void Reconstitute_GivenPersistedModel_ReturnsOriginalTodo()
        {
            var fixture = new Fixture();

            var originalTodo = fixture
                .Create<Todo>()
                .SetProperty(t => t.Color, Color.Pick("violet"));

            var sut = new PersistedTodoModel(originalTodo);
            var reconstitutedTodo = PersistedTodoModel.Reconstitute(sut);

            Assert.That(reconstitutedTodo, Is.EqualTo(originalTodo));
        }
    }
}
