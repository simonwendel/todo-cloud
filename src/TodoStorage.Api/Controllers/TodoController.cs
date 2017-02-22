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

namespace TodoStorage.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Web.Http;
    using Ploeh.Hyprlinkr;
    using SimonWendel.GuardStatements;
    using TodoStorage.Domain;

    public class TodoController : ApiControllerBase
    {
        private readonly ITodoList todoList;

        private readonly Func<HttpRequestMessage, IResourceLinker> linkerStrategy;

        public TodoController(ITodoListFactory todoListFactory, Func<HttpRequestMessage, IResourceLinker> linkerStrategy)
        {
            Guard.EnsureNotNull(todoListFactory, nameof(todoListFactory));
            Guard.EnsureNotNull(linkerStrategy, nameof(linkerStrategy));

            todoList = todoListFactory.Create(Key);
            this.linkerStrategy = linkerStrategy;
        }

        private IResourceLinker Linker => linkerStrategy(Request);

        public IEnumerable<Todo> Get()
        {
            return todoList.Items;
        }

        public IHttpActionResult Post(Todo todo)
        {
            Guard.EnsureNotNull(todo, nameof(todo));

            todoList.Add(todo);

            var redirectUri = Linker.GetUri<TodoController>(c => c.Get());
            return Created(redirectUri, todo);
        }
    }
}
