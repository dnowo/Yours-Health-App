using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitApp
{
    class Meal
    {
        public Meal()
        {
            this.Ingredients = new HashSet<FoodProduct>();
        }

        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Type { get; set; }

        public virtual ICollection<FoodProduct> Ingredients { get; set; }
    }
}
