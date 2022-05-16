//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TaskTrackingSystem.DAL.EFDAL.EFBD
{
    using System;
    using System.Collections.Generic;
    
    public partial class Project
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Project()
        {
            this.CalendarPlan = new HashSet<CalendarPlan>();
            this.ProjectPathToFile = new HashSet<ProjectPathToFile>();
            this.Task = new HashSet<Task>();
        }
    
        public int Id { get; set; }
        public string Name { get; set; }
        public string Responsible { get; set; }
        public System.DateTime DateStart { get; set; }
        public System.DateTime DateEnd { get; set; }
        public int ProjectType { get; set; }
        public int Status { get; set; }
        public string Customer { get; set; }
        public string ContractNumber { get; set; }
        public Nullable<decimal> Cost { get; set; }
        public int CompanyId { get; set; }
    
        public virtual Account Account { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CalendarPlan> CalendarPlan { get; set; }
        public virtual Company Company { get; set; }
        public virtual ProjectType ProjectType1 { get; set; }
        public virtual Status Status1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProjectPathToFile> ProjectPathToFile { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Task> Task { get; set; }
    }
}
