using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderAndDetail.Models.Entities
{
    [Table("Orders")]
    public class Orders
    {

        [Key]public int OrderID { get; set; }

        public string OrderName { get; set; }
        public string OrderDes { get; set; }
    }
}
