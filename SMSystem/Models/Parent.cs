//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace SMSystem.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Parent
    {
        public int ParentId { get; set; }
        public string FatherName { get; set; }
        public string FatherEmail { get; set; }
        public Nullable<int> FatherMobile { get; set; }
        public string MotherName { get; set; }
        public string MotherEmail { get; set; }
        public Nullable<int> MotherMobile { get; set; }
        public string MotherOccupation { get; set; }
        public string FatherOccupation { get; set; }
    }
}
