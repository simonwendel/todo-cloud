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
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using Ploeh.Hyprlinkr;
    using SimonWendel.GuardStatements;
    using TodoStorage.Core;

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

        public IHttpActionResult Get()
        {
            return Ok<IEnumerable<Todo>>(todoList.Items);
        }

        public IHttpActionResult Get(int id)
        {
            var todo = FindTodoBy(id);
            if (todo == null)
            {
                return NotFound();
            }

            return Ok(todo);
        }

        public IHttpActionResult Post(Todo todo)
        {
            Guard.EnsureNotNull(todo, nameof(todo));

            todoList.Add(todo);

            var redirectUri = Linker.GetUri<TodoController>(c => c.Get(todo.Id.Value));
            return Created(redirectUri, todo);
        }

        [SuppressMessage(
            "Microsoft.Globalization",
            "CA1303:Do not pass literals as localized parameters",
            MessageId = "System.Web.Http.ApiController.BadRequest(System.String)",
            Justification = "This is not something the end user will see. Resource table is overkill.")]
        public IHttpActionResult Put(int id, Todo todo)
        {
            Guard.EnsureNotNull(todo, nameof(todo));

            if (todo.Id.HasValue && todo.Id.Value != id)
            {
                return BadRequest("Id mismatch between URL and body.");
            }

            var persisted = FindTodoBy(id);
            if (persisted == null)
            {
                return Post(todo);
            }

            todoList.Update(todo);
            return Ok(todo);
        }

        public IHttpActionResult Delete(int id)
        {
            var persisted = FindTodoBy(id);
            if (persisted == null)
            {
                return NotFound();
            }

            todoList.Delete(persisted);
            return StatusCode(HttpStatusCode.NoContent);
        }

        private Todo FindTodoBy(int id)
        {
            return todoList.Items
                .Where(t => t.Id.HasValue)
                .SingleOrDefault(t => t.Id.Value == id);
        }
    }
}
