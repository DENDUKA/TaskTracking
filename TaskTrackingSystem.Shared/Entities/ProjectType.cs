using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TaskTrackingSystem.Shared.Entities.Access;

namespace TaskTrackingSystem.Shared.Entities
{
    [Table("ProjectType")]
    public class ProjectType
    {
        public ProjectType(int id, string name)
        {
            Id = id;
            Name = name;
        }
        [Key]
        public int Id { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        public virtual List<Project> Project { get; set; }

        public virtual List<ProjectTypeAccess> ProjectTypeAccess { get; set; }

        private ProjectType()
        { }

        #region StaticVariables
        public static int OtherProjects = 5;
        #endregion
    }
}