using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FitApp
{
    class FoodProduct
    {
        public FoodProduct()
        {
            this.Meals = new HashSet<Meal>();
        }

        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public double Weight { get; set; }
        [Required]
        public int Kcal { get; set; }

        public virtual ICollection<Meal> Meals { get; set; }
    }
}
