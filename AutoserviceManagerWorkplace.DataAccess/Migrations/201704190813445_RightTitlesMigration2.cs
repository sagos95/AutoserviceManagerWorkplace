namespace AutoserviceManagerWorkplace.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RightTitlesMigration2 : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.CarOwners", newName: "CarOwnerModels");
            RenameTable(name: "dbo.Cars", newName: "CarModels");
            RenameTable(name: "dbo.Orders", newName: "OrderModels");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.OrderModels", newName: "Orders");
            RenameTable(name: "dbo.CarModels", newName: "Cars");
            RenameTable(name: "dbo.CarOwnerModels", newName: "CarOwners");
        }
    }
}
