using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MagellanTest.Model
{
    public class Item
    {
        [Key]
        public int id { get; set; }

        [Required]
        [StringLength(50)]
        public string item_name { get; set; } = "";

        public int? parent_item { get; set; }
        
        [Required]
        public int cost { get; set; }

        [Required]
        public DateTime req_date { get; set; }
    }
}