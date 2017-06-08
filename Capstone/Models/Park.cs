using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Models
{
    public class Park
    {
        public int ParkId { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public DateTime EstablishDate { get; set; }
        public int Area { get; set; }
        public int Visitors { get; set; }
        public string Description { get; set; }

        //public override string ToString()
        //{
        //    return ParkId.ToString().PadRight(5) + Name.PadRight(20) + Location.ToString().PadRight(10) + EstablishDate.ToShortDateString().PadRight(10) + Area.ToString().PadRight(10) + Visitors.ToString().PadRight(10) + Description.ToString().PadRight(10);
        //}
    }
}
