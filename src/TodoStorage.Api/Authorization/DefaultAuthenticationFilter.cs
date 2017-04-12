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

namespace TodoStorage.Api.Authorization
{
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http.Filters;
    using SimonWendel.GuardStatements;

    internal class DefaultAuthenticationFilter : IAuthenticationFilter
    {
        private readonly IAuthenticator authenticator;

        private readonly IChallenger challenger;

        public DefaultAuthenticationFilter(IAuthenticator authenticator, IChallenger challenger)
        {
            Guard.EnsureNotNull(authenticator, nameof(authenticator));
            Guard.EnsureNotNull(challenger, nameof(challenger));

            this.authenticator = authenticator;
            this.challenger = challenger;
        }

        public bool AllowMultiple => false;

        public Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            authenticator.Authenticate(context);
            return TaskExtension.Completed;
        }

        public Task ChallengeAsync(HttpAuthenticationChallengeContext context, CancellationToken cancellationToken)
        {
            challenger.Challenge(context);
            return TaskExtension.Completed;
        }
    }
}
