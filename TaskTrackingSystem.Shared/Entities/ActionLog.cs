using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskTrackingSystem.Shared.Entities
{
    public class ActionLog
    {
        [Key]
        [Column(Order = 0)]
        public int TypeId { get; set; }

      

        [Key]
        [Column(Order = 1)]
        public DateTime Time { get; set; }

        [Key]
        [Column(Order = 2)]
        [StringLength(50)]
        public string FieldName { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(100)]
        public string UserLogin { get; set; }

        [ForeignKey("UserLogin")]
        public Account Account { get; set; }

        [Key]
        [Column(Order = 4)]
        public string ChangedValue { get; set; }

        [Key]
        [Column(Order = 5)]
        public string NewValue { get; set; }

        [Key]
        [Column(Order = 6)]
        [StringLength(10)]
        public string Type { get; set; }
    }
}