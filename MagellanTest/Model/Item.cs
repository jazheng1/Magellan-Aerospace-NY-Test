using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MagellanTest.Model
{
    public class Item
    {
        public int id { get; set; }
        public string item_name { get; set; } = "";
        public int? parent_item { get; set; }
        public int cost { get; set; }
        public DateTime req_date { get; set; }
    }
}