using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ZavrsniTest.Models
{
    public class Album
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public int PublishingYear { get; set; }
        [Required]
        public string Genre { get; set; }
        [Required]
        public int CopiesSold { get; set; }

        public int BandId { get; set; }
        public Band Band { get; set; }
    }
}
