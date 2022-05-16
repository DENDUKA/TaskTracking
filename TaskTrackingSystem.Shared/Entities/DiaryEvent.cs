using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskTrackingSystem.Shared.Entities
{
    public class DiaryEventJSON
    {
        public int ID;
        public string Title;
        public int SomeImportantKeyID;
        public string StartDateString;
        public string EndDateString;
        public string StatusString;
        public string StatusColor;
        public string ClassName;
        public bool AllDay;
        public bool StartEditable;
    }

    [Table("CalendarDiaryEvent")]
    public class CalendarDiaryEvent
    {
        public CalendarDiaryEvent()
        {

        }

        public CalendarDiaryEvent(int id, string name, DateTime dateStart, DateTime dateEnd)
        {
            this.Id = id;
            this.Name = name;
            this.DateStart = dateStart;
            this.DateEnd = dateEnd;
        }

        [Key]
        [Column("ID")]
        public int Id { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        [Column(TypeName = "date")]
        public DateTime DateStart { get; set; }

        [Column(TypeName = "date")]
        public DateTime DateEnd { get; set; }
    }

    public static class DiaryEventExtension
    {
        public static bool IsValid(this CalendarDiaryEvent diaryEvent)
        {
            return !string.IsNullOrEmpty(diaryEvent.Name) &&
                diaryEvent.DateStart != DateTime.MinValue &&
                diaryEvent.DateEnd != DateTime.MinValue &&
                diaryEvent.DateStart < diaryEvent.DateEnd;
        }
    }
}
