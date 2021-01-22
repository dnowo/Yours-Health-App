using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitApp
{
    class FitAppDB : DbContext
    {
        public FitAppDB() : base()
        {
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<FoodProduct> FoodProducts { get; set; }
        public virtual DbSet<Meal> Meals { get; set; }
        public virtual DbSet<Diet> Diet { get; set; }
        public virtual DbSet<TrainingModel> Trainings { get; set; }
        public virtual DbSet<Sport> Sports { get; set; }
    }
}
