﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class OATSDBEntities : DbContext
    {
        public OATSDBEntities()
            : base("name=OATSDBEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public DbSet<Answer> Answers { get; set; }
        public DbSet<FeedBack> FeedBacks { get; set; }
        public DbSet<Invitation> Invitations { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<QuestionType> QuestionTypes { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<SettingConfig> SettingConfigs { get; set; }
        public DbSet<SettingConfigDetail> SettingConfigDetails { get; set; }
        public DbSet<SettingType> SettingTypes { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserInTest> UserInTests { get; set; }
        public DbSet<UserInTestDetail> UserInTestDetails { get; set; }
        public DbSet<UserRoleMapping> UserRoleMappings { get; set; }
    }
}
