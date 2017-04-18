namespace AutoserviceManagerWorkplace.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CodeFirst : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.CarOwners", "PhoneNumber", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.CarOwners", "PhoneNumber", c => c.Int(nullable: false));
        }
    }
}
