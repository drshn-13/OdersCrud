using OrderAndDetail.Models.Entities;

namespace OrderAndDetail.Models
{
    //public class mainDTO
    //{
    //    public List<OrderDTO> DTo1 { get; set; }
    //    public List<OrderDetailsDTO> DTo2 { get; set; }

    //}
    public class OrderDTO
    {
        public int OrderID { get; set; }
        public string OrderName { get; set; }
        public string OrderDes { get; set; }

        //public List<OrderDetail> orderDetails { get; set; }
        public int OrderDetailID { get; set; }

        //public int OrderID { get; set; }

        public int Amount { get; set; }
        public int Quantity { get; set; }

        public int Total { get; set; }
        public int NumberOfOrder { get; set; }


    }

    //public class OrderDetailsDTO
    //{
    //    public string OrderName { get; set; }
    //    public string OrderDes { get; set; }
    //    public int OrderDetailID { get; set; }

    //    public int OrderID { get; set; }

    //    public int Amount { get; set; }
    //    public int Quantity { get; set; }

    //    public int Total { get; set; }
    //    public int NumberOfOrder { get; set; }


    //}
}
