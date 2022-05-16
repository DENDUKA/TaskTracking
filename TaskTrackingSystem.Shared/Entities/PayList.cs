using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TaskTrackingSystem.Shared.Entities.PayListItem;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace TaskTrackingSystem.Shared.Entities
{
    public class PayList
    {
        [Key]
        [JsonIgnore]
        public int Id { get; set; }
        [JsonIgnore]
        public string Login { get; set; }
        [ForeignKey("Login")]
        [JsonIgnore]
        public Account Account { get; set; }
        [JsonIgnore]
        public DateTime Date { get; set; }
        [JsonIgnore]
        public string Json
        {
            get => JsonConvert.SerializeObject(this);
            set
            {
                var plFromJson = JsonConvert.DeserializeObject<PayList>(value);

                this.Title = plFromJson.Title;
                this.Name = plFromJson.Name;
                this.Position = plFromJson.Position;
                this.Salary = plFromJson.Salary;
                this.Hold = plFromJson.Hold;
                this.Organization = plFromJson.Organization;
                this.Accrued = plFromJson.Accrued;
                this.Paid = plFromJson.Paid;
                this.ToPay = plFromJson.ToPay;
                this.Error = plFromJson.Error;
                this.TotalTaxableIncome = plFromJson.TotalTaxableIncome;
                this.DebtWorkerStartMonth = plFromJson.DebtWorkerStartMonth;
                this.DebtCompanyStartMonth = plFromJson.DebtCompanyStartMonth;
                this.AccruedItems = plFromJson.AccruedItems;
                this.HoldItems = plFromJson.HoldItems;
                this.PaidItems = plFromJson.PaidItems;
            }
        }

        [NotMapped]
        public string Title { get; set; }
        [NotMapped]
        public string Name { get; set; }
        [NotMapped]
        public string Position { get; set; }
        [NotMapped]
        public float Salary { get; set; }
        [NotMapped]
        public float Hold { get; set; }
        [NotMapped]
        public string Organization { get; set; }
        [NotMapped]
        public float Accrued { get; set; }
        [NotMapped]
        public float Paid { get; set; }
        [NotMapped]
        public float ToPay { get; set; }
        [NotMapped]
        public string Error { get; set; }
        [NotMapped]
        public string TotalTaxableIncome { get; set; }
        [NotMapped]
        public float DebtWorkerStartMonth { get; set; }
        [NotMapped]
        public float DebtCompanyStartMonth { get; set; }
        [NotMapped]
        [JsonIgnore]
        public string StatusBD { get; set; }

        [NotMapped]
        public List<AccruedItem> AccruedItems = new List<AccruedItem>();
        [NotMapped]
        public List<HoldItem> HoldItems = new List<HoldItem>();
        [NotMapped]
        public List<PaidItem> PaidItems = new List<PaidItem>();
    }
}
