using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WuWHO_Web_App.Models
{
    public class WuWHO_db
    {
        public int ID { get; set; }
        public string MAC_ID { get; set; }
        public int RSSI { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH :mm:ss }", ApplyFormatInEditMode = true)]
        public DateTime time_rec { get; set; }
    }

    
}
