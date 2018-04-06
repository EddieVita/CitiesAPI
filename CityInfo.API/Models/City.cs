using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Models
{
    public class City
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "A city name is required.")]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required(ErrorMessage = "A city description is required.")]
        [MaxLength(255)]
        public string Description { get; set; }

        public int NumberOfPointsOfInterest
        {
            get { return PointsOfInterest.Count(); }
        }

        public virtual IList<PointOfInterest> PointsOfInterest { get; set; } = new List<PointOfInterest>(); 
    }
}
