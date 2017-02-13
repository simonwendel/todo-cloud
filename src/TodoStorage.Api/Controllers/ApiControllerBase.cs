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
    using System.Web.Http;
    using TodoStorage.Api.Authorization;
    using TodoStorage.Domain;

    public abstract class ApiControllerBase : ApiController
    {
        private CollectionKey key;

        protected CollectionKey Key
        {
            get
            {
                if (key == null)
                {
                    var messagePrincipal = User as SignedMessagePrincipal;
                    if (messagePrincipal == null)
                    {
                        throw new InvalidOperationException();
                    }
                    
                    key = new CollectionKey(messagePrincipal.AppId);
                }

                return key;
            }
        }
    }
}
