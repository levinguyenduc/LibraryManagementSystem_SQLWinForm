//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace LibraryManagementSystem.EntityFramework
{
    using System;
    using System.Collections.Generic;
    
    public partial class Books
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Books()
        {
            this.IssueBooks = new HashSet<IssueBooks>();
            this.ReturnBooks = new HashSet<ReturnBooks>();
            this.Fines = new HashSet<Fines>();
        }
    
        public int BookID { get; set; }
        public int BookCategoryID { get; set; }
        public int StaffID { get; set; }
        public int DepartmentID { get; set; }
        public string BookName { get; set; }
        public string Author { get; set; }
        public string Title { get; set; }
        public string Edition { get; set; }
        public int NoOfCopies { get; set; }
        public System.DateTime DateOfRegister { get; set; }
        public int Price { get; set; }
    
        public virtual BookCategories BookCategories { get; set; }
        public virtual Departments Departments { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<IssueBooks> IssueBooks { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ReturnBooks> ReturnBooks { get; set; }
        public virtual Staffs Staffs { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Fines> Fines { get; set; }
    }
}
