//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TodoStorage.Persistence.Tests
{
    using System;
    using System.Collections.Generic;
    
    public partial class TodoItem
    {
        public int Id { get; set; }
        public System.Guid StorageKey { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public System.DateTime Created { get; set; }
        public int Recurring { get; set; }
        public Nullable<System.DateTime> NextOccurrence { get; set; }
        public string ColorName { get; set; }
        public string ColorValue { get; set; }
    }
}