﻿/*
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

using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle("Todo App API Security")]
[assembly: AssemblyProduct("TodoStorage.Security")]
[assembly: AssemblyCopyright("Copyright © Simon Wendel 2016")]
[assembly: ComVisible(false)]
[assembly: Guid("27fa62da-d74b-4bef-916a-24c15688eaf0")]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]

[assembly: InternalsVisibleTo("TodoStorage.Security.Tests")]

// for enabling throwing internal exceptions from security mocks
[assembly: InternalsVisibleTo("TodoStorage.Api.Tests")]

// for enabling use of Moq
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
