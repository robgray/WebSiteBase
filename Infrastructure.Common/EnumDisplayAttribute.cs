using System;

namespace Infrastructure.Common
{
    [AttributeUsage(AttributeTargets.Field)]
    public sealed class EnumDisplay : Attribute
    {
        public EnumDisplay() : this(null) { }

        public EnumDisplay(string description) : this(description, int.MaxValue) { }

        public EnumDisplay(string description, int sortOrder)
        {
            Description = description;  
            SortOrder = sortOrder;
        }

        public string Description { get; set; }
        
        public int SortOrder { get; set; }        
    }
}
