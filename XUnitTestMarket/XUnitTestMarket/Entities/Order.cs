using System;
using System.Collections.Generic;
using System.Text;

namespace XUnitTestMarket.Entities
{
    public class Order
    {
        public long id { get; set; }
        public string storeName { get; set; }
        public int quantity { get; set; }
        public int productCode { get; set; }
        public string observation { get; set; }

        public Order(long id, string storeName, int quantity, int productCode, string observation)
        {
            this.id = id;
            this.storeName = storeName;
            this.quantity = quantity;
            this.productCode = productCode;
            this.observation = observation;
        }
    }
}
