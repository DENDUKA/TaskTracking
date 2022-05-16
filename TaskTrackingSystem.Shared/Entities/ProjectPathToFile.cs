using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskTrackingSystem.Shared.Entities
{
    [Table("ProjectPathToFile")]
    public class ProjectPathToFile
    {        
        public ProjectPathToFile(int id, int projectId, string description, string path)
        {
            this.Id = id;
            this.ProjectId = projectId;
            this.Description = description;
            this.Path = path;
        }

        private ProjectPathToFile()
        { }

        [Key]
        public int Id { get; set; }

        public int ProjectId { get; set; }

        [ForeignKey("ProjectId")]
        public virtual Project Project { get; set; }

        [StringLength(150)]
        public string Description { get; set; }

        [StringLength(150)]
        public string Path { get; set; }
    }
}