using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitApp
{
    class User
    {
        public int Id { get; set; }
        [Required]
        public int Age { get; set; }
        [Required]
        public float Height { get; set; }
        [Required]
        public float Weight { get; set; }
        [Required]
        public bool Sex { get; set; }
    }
}
