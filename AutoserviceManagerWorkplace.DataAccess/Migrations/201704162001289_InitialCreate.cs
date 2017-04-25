namespace AutoserviceManagerWorkplace.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CarOwners",
                c => new
                    {
                        CarOwnerId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Surname = c.String(),
                        Patronymic = c.String(),
                        BirthYear = c.Int(nullable: false),
                        PhoneNumber = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CarOwnerId);
            
            CreateTable(
                "dbo.Cars",
                c => new
                    {
                        CarId = c.Int(nullable: false, identity: true),
                        Brand = c.String(),
                        Model = c.String(),
                        Year = c.Int(nullable: false),
                        AutomaticTransmission = c.Boolean(nullable: false),
                        EnginePower = c.Int(nullable: false),
                        CarOwner_CarOwnerId = c.Int(),
                    })
                .PrimaryKey(t => t.CarId)
                .ForeignKey("dbo.CarOwners", t => t.CarOwner_CarOwnerId)
                .Index(t => t.CarOwner_CarOwnerId);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        OrderId = c.Int(nullable: false, identity: true),
                        OrderContent = c.String(),
                        StartDateOfWork = c.DateTime(nullable: false),
                        EndDateOfWork = c.DateTime(nullable: false),
                        Price = c.Int(nullable: false),
                        Car_CarId = c.Int(),
                        CarOwner_CarOwnerId = c.Int(),
                    })
                .PrimaryKey(t => t.OrderId)
                .ForeignKey("dbo.Cars", t => t.Car_CarId)
                .ForeignKey("dbo.CarOwners", t => t.CarOwner_CarOwnerId)
                .Index(t => t.Car_CarId)
                .Index(t => t.CarOwner_CarOwnerId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orders", "CarOwner_CarOwnerId", "dbo.CarOwners");
            DropForeignKey("dbo.Orders", "Car_CarId", "dbo.Cars");
            DropForeignKey("dbo.Cars", "CarOwner_CarOwnerId", "dbo.CarOwners");
            DropIndex("dbo.Orders", new[] { "CarOwner_CarOwnerId" });
            DropIndex("dbo.Orders", new[] { "Car_CarId" });
            DropIndex("dbo.Cars", new[] { "CarOwner_CarOwnerId" });
            DropTable("dbo.Orders");
            DropTable("dbo.Cars");
            DropTable("dbo.CarOwners");
        }
    }
}
