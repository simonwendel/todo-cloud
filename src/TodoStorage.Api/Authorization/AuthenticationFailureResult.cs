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
    using System.Diagnostics.CodeAnalysis;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http;
    using SimonWendel.GuardStatements;

    internal class AuthenticationFailureResult : IHttpActionResult
    {
        private readonly string failureReason;

        private readonly HttpRequestMessage failingRequest;

        public AuthenticationFailureResult(string failureReason, HttpRequestMessage failingRequest)
        {
            Guard.EnsureNotNull(failureReason, nameof(failureReason));
            Guard.EnsureNotNull(failingRequest, nameof(failingRequest));

            this.failureReason = failureReason;
            this.failingRequest = failingRequest;
        }

        [SuppressMessage(
            "Microsoft.Reliability", 
            "CA2000:Dispose objects before losing scope", 
            Justification = "The response is registered for dispose at end of request. I count on Web API to do the right thing here.")]
        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage(HttpStatusCode.Unauthorized)
            {
                RequestMessage = failingRequest,
                ReasonPhrase = failureReason
            };

            // I love this extension method! Without it, this is tricky...
            failingRequest.RegisterForDispose(response);

            return Task.FromResult(response);
        }
    }
}
