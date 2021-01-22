namespace FitApp.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<FitApp.FitAppDB>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "FitApp.FitAppDB";
        }

        protected override void Seed(FitApp.FitAppDB context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.
        }
    }
}
