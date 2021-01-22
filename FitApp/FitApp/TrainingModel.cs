using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitApp
{
    class TrainingModel
    {
        public int Id { get; set; }
        [Required]
        public TimeSpan TrainingTime { get; set; }
        [Required]
        public DateTime TrainingDate { get; set; }
        [Required] 
        public int KcalBurned { get; set; }
        [Required]
        public Sport TrainingType { get; set; }

    }
}
