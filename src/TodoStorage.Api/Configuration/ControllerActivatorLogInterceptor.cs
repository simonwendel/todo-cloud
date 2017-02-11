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

namespace TodoStorage.Api.Configuration
{
    using System;
    using Common.Logging;
    using Ninject.Extensions.Interception;
    using SimonWendel.GuardStatements;

    public class ControllerActivatorLogInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            Guard.EnsureNotNull(invocation, nameof(invocation));

            try
            {
                invocation.Proceed();
            }
            catch (Exception ex)
            {
                LogManager.GetLogger<ControllerActivatorLogInterceptor>()
                    .Fatal("Exception caught in Controller Activator", ex);

                throw;
            }
        }
    }
}
