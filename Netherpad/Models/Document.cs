//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Netherpad.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Document
    {
        public int DocumentId { get; set; }
        public int ParentId { get; set; }
        public string Name { get; set; }
        public string Identifier { get; set; }
        public string Content { get; set; }
        public int Version { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public Nullable<System.DateTime> DeletedOn { get; set; }
    }
}