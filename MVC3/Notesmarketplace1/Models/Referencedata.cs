//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Notesmarketplace1.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Referencedata
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Referencedata()
        {
            this.SellerNotes = new HashSet<SellerNote>();
            this.SellerNotes1 = new HashSet<SellerNote>();
            this.UserProfiles = new HashSet<UserProfile>();
            this.SellerNotes11 = new HashSet<SellerNote>();
        }
    
        public int Id { get; set; }
        public string value { get; set; }
        public string DataValue { get; set; }
        public string RefCategory { get; set; }
        public Nullable<System.DateTime> Createddate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> Modifieddate { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public bool isActive { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SellerNote> SellerNotes { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SellerNote> SellerNotes1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserProfile> UserProfiles { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SellerNote> SellerNotes11 { get; set; }
    }
}