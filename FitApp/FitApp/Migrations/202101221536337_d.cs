namespace FitApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class d : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Diets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Date = c.DateTime(nullable: false),
                        Meal_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Meals", t => t.Meal_Id, cascadeDelete: true)
                .Index(t => t.Meal_Id);
            
            CreateTable(
                "dbo.Meals",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Type = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.FoodProducts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        Weight = c.Double(nullable: false),
                        Kcal = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Sports",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SportName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.TrainingModels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TrainingTime = c.Time(nullable: false, precision: 7),
                        TrainingDate = c.DateTime(nullable: false),
                        KcalBurned = c.Int(nullable: false),
                        TrainingType_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Sports", t => t.TrainingType_Id, cascadeDelete: true)
                .Index(t => t.TrainingType_Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Age = c.Int(nullable: false),
                        Height = c.Single(nullable: false),
                        Weight = c.Single(nullable: false),
                        Sex = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.FoodProductMeals",
                c => new
                    {
                        FoodProduct_Id = c.Int(nullable: false),
                        Meal_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.FoodProduct_Id, t.Meal_Id })
                .ForeignKey("dbo.FoodProducts", t => t.FoodProduct_Id, cascadeDelete: true)
                .ForeignKey("dbo.Meals", t => t.Meal_Id, cascadeDelete: true)
                .Index(t => t.FoodProduct_Id)
                .Index(t => t.Meal_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TrainingModels", "TrainingType_Id", "dbo.Sports");
            DropForeignKey("dbo.Diets", "Meal_Id", "dbo.Meals");
            DropForeignKey("dbo.FoodProductMeals", "Meal_Id", "dbo.Meals");
            DropForeignKey("dbo.FoodProductMeals", "FoodProduct_Id", "dbo.FoodProducts");
            DropIndex("dbo.FoodProductMeals", new[] { "Meal_Id" });
            DropIndex("dbo.FoodProductMeals", new[] { "FoodProduct_Id" });
            DropIndex("dbo.TrainingModels", new[] { "TrainingType_Id" });
            DropIndex("dbo.Diets", new[] { "Meal_Id" });
            DropTable("dbo.FoodProductMeals");
            DropTable("dbo.Users");
            DropTable("dbo.TrainingModels");
            DropTable("dbo.Sports");
            DropTable("dbo.FoodProducts");
            DropTable("dbo.Meals");
            DropTable("dbo.Diets");
        }
    }
}
