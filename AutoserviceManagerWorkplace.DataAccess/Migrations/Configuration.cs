namespace AutoserviceManagerWorkplace.DataAccess.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using AutoserviceManagerWorkplace.DataAccess;

    internal sealed class Configuration : DbMigrationsConfiguration<DataBaseContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "AutoserviceManagerWorkplace.DataBaseContext";
        }

        protected override void Seed(DataBaseContext context)
        {
            
        }
    }
}
