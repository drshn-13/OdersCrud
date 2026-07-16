namespace OrderAndDetail.Models
{
    //public class MultipleDTO
    //{
    //    public int OrderID { get; set; }

    //    public int OrderDetailID { get; set; }
    //    public string OrderName { get; set; }
    //    public string OrderDes { get; set; }
    //    public List<OrderDTO> orders { get; set; }
    //}
    public class MultipleDTO
    {
        public int OrderID { get; set; }

        public string OrderName { get; set; }

        public string OrderDes { get; set; }

        public List<OrderDTO> orders { get; set; }

        public List<int> DeletedRows { get; set; }
    }
}
