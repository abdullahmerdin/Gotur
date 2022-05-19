using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gotur.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }
        [Required, Display(Name = "İsim")]
        public string Name { get; set; }
        [Display(Name = "Açıklama")]
        public string Description { get; set; }
        [Display(Name = "Fiyat")]
        public double Price { get; set; }
        [Display(Name = "Fotoğraf")]
        public string Image { get; set; }
        public int CategoryId { get; set; }


        // İlişkilendirme //
       
        [ForeignKey("CategoryId"), Display(Name = "İsim")]
        
        public Category Category { get; set; }


    }
}
