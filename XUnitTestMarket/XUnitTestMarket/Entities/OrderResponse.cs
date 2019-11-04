using System;
using System.Collections.Generic;
using System.Text;

namespace XUnitTestMarket.Entities
{
    public class OrderResponse
    {
        public long budgetCode;
        public DateTime date;

        public OrderResponse(long budgetCode, DateTime date)
        {
            this.budgetCode = budgetCode;
            this.date = date;
        }
    }
}
