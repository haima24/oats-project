//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OATS_Capstone.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class SettingType
    {
        public SettingType()
        {
            this.TestSettings = new HashSet<TestSetting>();
        }
    
        public int SettingTypeID { get; set; }
        public string SettingTypeKey { get; set; }
        public string SettingTypeDescription { get; set; }
    
        public virtual ICollection<TestSetting> TestSettings { get; set; }
    }
}
