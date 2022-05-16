using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gotur.Models
{
    public class OrderProduct
    {
        [Key]
        public int Id { get; set; }
        public string AppUserId { get; set; }
        public DateTime OrderDate { get; set; }
        public double OrderPrice { get; set; }
        public string OrderStatus { get; set; }
        public string Adress { get; set; }



        // İlişkilendirme // 
        [ForeignKey("AppUserId")]
        public AppUser AppUser { get; set; }


    }
}
