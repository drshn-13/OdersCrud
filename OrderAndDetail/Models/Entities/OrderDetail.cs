using System.ComponentModel.DataAnnotations.Schema;

namespace OrderAndDetail.Models.Entities
{
    [Table("OrderDetail")]
    public class OrderDetail
    {
        public int OrderDetailID { get; set; }

        public int OrderID { get; set; }

        public int Amount { get; set; }
        public int Quantity { get; set; }

        public int Total { get; set; }

        //public Orders Orders { get; set; } 
    }
}
