//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MsSqkToXml
{
    using System;
    using System.Collections.Generic;
    
    public partial class Word
    {
        public int WordId { get; set; }
        public string Name { get; set; }
        public string Describtion { get; set; }
        public int CategoryId { get; set; }
    
        public virtual Category Category { get; set; }
    }
}
