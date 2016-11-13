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

namespace TodoStorage.Persistence
{
    using System;
    using Domain;

    internal class PersistedTodoModel
    {
        public PersistedTodoModel()
        {
        }

        public PersistedTodoModel(Todo todo)
        {
            Id = todo.Id;
            Title = todo.Title;
            Description = todo.Description;
            Created = todo.Created;
            Recurring = todo.Recurring;
            NextOccurrence = todo.NextOccurrence;
            ColorValue = todo.Color.Value;
        }

        public Guid AppId { get; set; }

        public int? Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime? Created { get; set; }

        public int? Recurring { get; set; }

        public DateTime? NextOccurrence { get; set; }

        public string ColorValue { get; set; }

        public static Todo Reconstitute(PersistedTodoModel model)
        {
            return new Todo
            {
                Id = model.Id,
                Title = model.Title,
                Description = model.Description,
                Created = model.Created,
                Recurring = model.Recurring,
                NextOccurrence = model.NextOccurrence,
                Color = Color.Pick(model.ColorValue)
            };
        }
    }
}
