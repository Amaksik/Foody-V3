using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.DTO
{
    public class RecognitionResult
    {
        public int id { get; set; }
        public string name { get; set; }
        public double prob { get; set; }
    }
}
