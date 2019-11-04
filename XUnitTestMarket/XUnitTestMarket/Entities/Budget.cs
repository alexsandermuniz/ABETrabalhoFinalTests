using System;
using System.Collections.Generic;
using System.Text;

namespace XUnitTestMarket.Entities
{
    public class Budget
    {
        public long id { get; set; }
        public List<Order> orders { get; set; }
        public int budgetCode { get; set; }
        public DateTime date { get; set; }
        public double value { get; set; }
        public string customer { get; set; }
        public string status { get; set; }
    }
}
