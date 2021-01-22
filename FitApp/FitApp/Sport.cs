using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitApp
{
    class Sport
    {
        public int Id { get; set; }
        [Required] public String SportName { get; set; }

        public ICollection<TrainingModel> Trainings { get; set; }

        public Sport()
        {
            this.Trainings = new HashSet<TrainingModel>();
        }
    }
}
