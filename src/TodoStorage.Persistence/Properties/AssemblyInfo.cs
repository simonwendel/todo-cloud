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

using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle("Todo App Storage Persistence Model and Infrastructure")]
[assembly: AssemblyProduct("TodoStorage.Persistence")]
[assembly: AssemblyCopyright("Copyright © Simon Wendel 2016")]
[assembly: ComVisible(false)]
[assembly: Guid("d327172c-35dc-4566-9c3f-7f947aaf71e9")]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]

[assembly: InternalsVisibleTo("TodoStorage.Persistence.Tests")]

// for enabling use of Moq
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
