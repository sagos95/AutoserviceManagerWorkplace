namespace AutoserviceManagerWorkplace.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class removeCarOwnersfromOrderList3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.OrderModels", "CarOwner_CarOwnerId", "dbo.CarOwnerModels");
            DropIndex("dbo.OrderModels", new[] { "CarOwner_CarOwnerId" });
            
        }
        
        public override void Down()
        {
           
            CreateIndex("dbo.OrderModels", "CarOwner_CarOwnerId");
            AddForeignKey("dbo.OrderModels", "CarOwner_CarOwnerId", "dbo.CarOwnerModels", "CarOwnerId");
        }
    }
}
