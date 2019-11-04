using System;
using System.Collections.Generic;
using System.Text;
using XUnitTestMarket.Entities;

namespace XUnitTestMarket
{
    public static class MockTest
    {
        public static List<Order>  getRequestOrders()
        {
            Order order1 = new Order(100, "Casas bahia", 10, 1, "frágil");
            Order order2 = new Order(101, "Casas bahia", 2, 2, "");
            List<Order> orders = new List<Order>() { order1, order2 };
            return orders;
        }
    }
}
