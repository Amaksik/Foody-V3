using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Options
{   public class RecognitionServiceOptions
    {
        public const string Tokens = "Tokens";

        [Required]
        public string BarcodeApiKey { get; set; }
        [Required]
        public string BarcodeApiToken { get; set; }
        [Required]
        public string LogmealApiBearer { get; set; }
    }
}
