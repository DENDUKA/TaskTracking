using System;

namespace TaskTrackingSystem.Shared.Attributes
{

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class AccessFieldMetaData : Attribute
    {
        public string DispalayName { get; set; }
        public string BackName { get; set; }
        public double Order { get; set; }
        public string FieldName { get; set; }

        public AccessFieldMetaData(string backName, double order, string displayName = "")
        {
            BackName = backName;
            Order = order;
            DispalayName = displayName;
        }
    }
}