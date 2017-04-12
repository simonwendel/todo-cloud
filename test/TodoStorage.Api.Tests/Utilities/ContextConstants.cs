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

namespace TodoStorage.Api.Tests.Utilities
{
    using System;
    using System.Net.Http;

    internal class ContextConstants
    {
        public const string FakeContent = "someValue=1&otherValue=this";

        public const string FakeScheme = "xhmac";

        public const string FakeSignature = "9a35c321a56e88e0726dff3955a97f5bdf55ab78ab6e0d4bd39b7e5ac72c9dba";

        public const string FakeNonce = "nonce12";

        public const ulong FakeTimestamp = 399481200ul;

        public static readonly Guid FakeAppId = Guid.NewGuid();

        public static readonly string FakeParameter = $"{ FakeAppId }:{ FakeSignature }:{ FakeNonce }:{ FakeTimestamp }";

        public static readonly Uri FakeUri = new Uri("http://localhost.dev");

        public static readonly HttpMethod FakeMethod = new HttpMethod("GET");
    }
}
