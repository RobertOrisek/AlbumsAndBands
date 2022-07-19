using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZavrsniTest.Models.DTO
{
    public class AlbumDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int PublishingYear { get; set; }
        public string Genre { get; set; }
        public int CopiesSold { get; set; }
        public int BandId { get; set; }
        public string BandName { get; set; }
    }
}
