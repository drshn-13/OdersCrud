using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderAndDetail.Data;
using OrderAndDetail.Models;
using OrderAndDetail.Models.Entities;

namespace OrderAndDetail.Controllers
{
    public class OrderController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult ListPage()
        {
            var data = from Orders in _context.Orders
                       join OrderDetail in _context.orderDetails
                       on Orders.OrderID equals OrderDetail.OrderID
                       select new OrderDTO
                       {
                           OrderID = Orders.OrderID,
                           OrderName = Orders.OrderName,
                           OrderDes = Orders.OrderDes,
                           OrderDetailID = OrderDetail.OrderDetailID,

                           Amount = OrderDetail.Amount,
                           Quantity = OrderDetail.Quantity,
                           Total = OrderDetail.Total
                       };
            //var data = from Orders in _context.Orders                       
            //           select new OrderDTO
            //           {
            //               OrderID = Orders.OrderID,

            //               OrderName = Orders.OrderName,
            //               OrderDes = Orders.OrderDes,
            //               orderDetails = _context.orderDetails.Where(x => x.OrderID == Orders.OrderID).Select(g => new OrderDetail
            //               {
            //                   OrderDetailID = g.OrderDetailID,
            //                   OrderID = g.OrderID,
            //                   Amount = g.Amount,
            //                   Quantity = g.Quantity

            //               }).ToList()
            //           };
            var list = data
                    .GroupBy(x => x.OrderID)
                    .Select(g => new OrderDTO
                    {
                        OrderID = g.Key,
                        OrderName = g.First().OrderName,
                        OrderDes = g.First().OrderDes,

                        Amount = g.Sum(x => x.Amount),
                        Quantity = g.Sum(x => x.Quantity),
                        Total = g.Sum(x => x.Total),
                        NumberOfOrder = g.Select(x => x.OrderID).Count()
                    })
                    .ToList();

            return View(list);
        }
        [HttpPost]
        public IActionResult Save(MultipleDTO model)
        {
            //if (model.OrderID == 0)
            {
                Orders orders = new Orders
                {
                    OrderName = model.OrderName,
                    OrderDes = model.OrderDes
                };
                _context.Orders.Add(orders);
                _context.SaveChanges();

                foreach (var item in model.orders)

                {
                    OrderDetail detail = new OrderDetail()
                    {
                        OrderID = orders.OrderID,
                        Amount = item.Amount,
                        Quantity = item.Quantity,
                        Total = item.Total
                    };
                    _context.orderDetails.Add(detail);
                    _context.SaveChanges();
                }
                return Json(new
                {
                    Success = true,
                    message = "Saved Successfully"
                });
            }
            //else
            //{
            //    foreach (var item in model.orders)

            //    {
            //        OrderDetail detail = new OrderDetail()
            //        {
            //            OrderID = model.OrderID,
            //            Amount = item.Amount,
            //            Quantity = item.Quantity,
            //            Total = item.Total
            //        };
            //        _context.orderDetails.Add(detail);
            //        _context.SaveChanges();
            //    }
            //    return Json(new
            //    {
            //        Success = true,
            //        message = "Saved Successfully"
            //    });
            //}

        }
        public IActionResult GetOrderDetail(int id)
        {
            var data = _context.orderDetails.Where(x => x.OrderDetailID == id).FirstOrDefault();

            return Json(data);
        }
        public IActionResult GetOrder(int OrderID)
        {
            //mainDTO dto = new mainDTO();

            var data = from Orders in _context.Orders
                       join OrderDetail in _context.orderDetails
                       on Orders.OrderID equals OrderDetail.OrderID
                       where Orders.OrderID == OrderID
                       select new OrderDTO
                       {
                           OrderID = Orders.OrderID,
                           OrderName = Orders.OrderName,
                           OrderDes = Orders.OrderDes,
                           OrderDetailID = OrderDetail.OrderDetailID,
                           Amount = OrderDetail.Amount,
                           Quantity = OrderDetail.Quantity,
                           Total = OrderDetail.Total
                       };

            //dto.DTo1 = data.ToList();
            ////var data = _context.Orders.Where(y => y.OrderID == id).FirstOrDefault();
            //var data2 = _context.orderDetails
            //    .Where(x => x.OrderDetailID == 2)
            //    .Select(x => new OrderDetailsDTO
            //    {
            //        OrderDetailID = x.OrderDetailID,
            //        OrderID = x.OrderID,
            //        Amount = x.Amount,
            //        Quantity = x.Quantity,
            //        Total = x.Total
            //    }) 

            //dto.DTo2 = data2;

            //return Json(dto);
            return Json(data);
        }
        

        [HttpPost]
        public IActionResult Update(MultipleDTO model)
        {
            var order = _context.Orders
                .Where(x => x.OrderID == model.OrderID).FirstOrDefault();

            if (order == null)
            {
                return Json(new
                {
                    success = false,
                    message = "Order not found."
                });
            }

            order.OrderName = model.OrderName;
            order.OrderDes = model.OrderDes;

            // Delete rows
            if (model.DeletedRows != null)
            {
                foreach (var id in model.DeletedRows)
                {
                    var detail = _context.orderDetails
                        .Where(x => x.OrderDetailID == id).FirstOrDefault();

                    if (detail != null)
                    {
                        _context.orderDetails.Remove(detail);
                    }
                }
            }

            // Insert or Update rows
            foreach (var item in model.orders)
            {
                if (item.OrderDetailID == 0)
                {
                    _context.orderDetails.Add(new OrderDetail
                    {
                        OrderID = model.OrderID,
                        Amount = item.Amount,
                        Quantity = item.Quantity,
                        Total = item.Total
                    });
                }
                else
                {
                    var detail = _context.orderDetails
                        .FirstOrDefault(x => x.OrderDetailID == item.OrderDetailID);

                    if (detail != null)
                    {
                        detail.Amount = item.Amount;
                        detail.Quantity = item.Quantity;
                        detail.Total = item.Total;
                    }
                }
            }

            _context.SaveChanges();

            return Json(new
            {
                success = true,
                message = "Updated Successfully"
            });
        }
        //public IActionResult Update(MultipleDTO model)
        //{
        //    if (model.orders.Count != 0)
        //    {
        //        foreach (var item in model.orders)
        //        {
        //            if (item.OrderDetailID != 0)
        //            {
        //                var data = _context.orderDetails.Where(x => x.OrderDetailID == item.OrderDetailID).FirstOrDefault();

        //                _context.orderDetails.Remove(data);
        //                _context.SaveChanges();
        //            }

        //        }
        //    }



        //    var data2 = _context.Orders.Where(x => x.OrderID == model.OrderID).FirstOrDefault();
        //    if (data2 != null)
        //    {
        //        data2.OrderName = model.OrderName;
        //        data2.OrderDes = model.OrderDes;
        //        _context.Orders.Update(data2);
        //        _context.SaveChanges();
        //    }

        //    foreach (var item in model.orders)

        //    {


        //        OrderDetail detail = new OrderDetail()
        //        {
        //            OrderID = model.OrderID,
        //            Amount = item.Amount,
        //            Quantity = item.Quantity,
        //            Total = item.Total

        //        };
        //        _context.orderDetails.Add(detail);
        //        _context.SaveChanges();

        //        //var data = _context.orderDetails.Where(x => x.OrderDetailID == item.OrderDetailID).FirstOrDefault();

        //        //if (data != null)
        //        //{


        //        //    //data.OrderID = model.OrderID;
        //        //    data.Amount = item.Amount;
        //        //    data.Quantity = item.Quantity;
        //        //    data.Total = item.Total;


        //        //    _context.orderDetails.Update(data);

        //        //    _context.SaveChanges();



        //        //}
        //    }
        //    return Json(new
        //    {
        //        success = true,
        //        message = "Updated successfully"

        //    });



        
        //[HttpPost]
        //public IActionResult delete(int id)
        //{
        //    var data = _context.orderDetails.Where(x => x.OrderDetailID == id).FirstOrDefault();

        //    if (data != null)
        //    {
        //        _context.orderDetails.Remove(data);
        //        _context.SaveChanges();

        //        return Json(new
        //        {
        //            success = true,
        //            message = "successfully deleted"
        //        });
        //    }
        //    else
        //    {
        //        return Json(new
        //        {
        //            success = false,
        //            message = "deleted Unsuccefull"
        //        });
        //    }
        //}
        [HttpPost]
        public IActionResult EntireConsumer(int id)
        {
            //var data  = _context.orderDetails.Where(X => X.OrderID == id ).SingleOrDefault();
            //if (data != null)
            //{

            //    _context.orderDetails.Remove(data);
            //    //var data2 = _context.Orders.Where(x => x.OrderID == id).FirstOrDefault();
            //    //_context.Orders.Remove(data2);
            //    _context.SaveChanges();
            var data = _context.orderDetails.Where(x => x.OrderID == id).ToList();
            var data2 = _context.Orders.Where(x => x.OrderID == id).FirstOrDefault();
            if (data != null)
            {
                _context.orderDetails.RemoveRange(data);
                _context.Orders.Remove(data2);
                _context.SaveChanges();

                return Json(new
                {
                    success = true,
                    message = "user deleted successfully "
                });

            }


            else
            {
                return Json(new
                {
                    success = false,
                    message = "deleted Unsuccesfull"
                });
            }
        }
    }
}
