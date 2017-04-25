namespace AutoserviceManagerWorkplace.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class nullableenddateofwork : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.OrderModels", "EndDateOfWork", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.OrderModels", "EndDateOfWork", c => c.DateTime(nullable: false));
        }
    }
}
