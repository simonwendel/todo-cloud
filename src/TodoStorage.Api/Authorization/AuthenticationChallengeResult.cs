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

namespace TodoStorage.Api.Authorization
{
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http;
    using SimonWendel.GuardStatements;

    internal class AuthenticationChallengeResult : IHttpActionResult
    {
        private readonly IHttpActionResult innerResult;

        private readonly string scheme;

        public AuthenticationChallengeResult(IHttpActionResult innerResult, string scheme)
        {
            Guard.EnsureNotNull(innerResult, nameof(innerResult));
            Guard.EnsureNotNull(scheme, nameof(scheme));

            this.innerResult = innerResult;
            this.scheme = scheme;
        }

        public string Scheme => scheme;

        public async Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = await innerResult.ExecuteAsync(cancellationToken);
            if (
                response.StatusCode == HttpStatusCode.Unauthorized &&
                response.Headers.WwwAuthenticate.Any(h => h.Scheme.Equals(scheme)) == false)
            {
                response.Headers.WwwAuthenticate.Add(new AuthenticationHeaderValue(Scheme));
            }

            return response;
        }
    }
}
