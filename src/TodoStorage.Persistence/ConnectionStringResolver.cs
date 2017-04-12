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

namespace TodoStorage.Persistence
{
    using System;
    using System.Configuration;
    using SimonWendel.GuardStatements;

    internal class ConnectionStringResolver : IConnectionStringResolver
    {
        private readonly string connectionString;

        public ConnectionStringResolver()
            : this("TodoStorage")
        {
        }

        public ConnectionStringResolver(string connectionStringName)
        {
            Guard.EnsureNotNull(connectionStringName, nameof(connectionStringName));

            var config = ConfigurationManager.ConnectionStrings[connectionStringName];
            if (config == null)
            {
                throw new ArgumentException("Connection string not found.", nameof(connectionStringName));
            }

            connectionString = config.ConnectionString;
        }

        public string ConnectionString => connectionString;
    }
}
