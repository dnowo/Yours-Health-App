using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitApp
{
    class Diet
    {
        public int Id { get; set; }
        [Required]
        public Meal Meal { get; set; }
        [Required]
        public DateTime Date { get; set; }
    }
}
