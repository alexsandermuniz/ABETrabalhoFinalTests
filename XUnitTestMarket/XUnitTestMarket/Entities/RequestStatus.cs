using System;
using System.Collections.Generic;
using System.Text;

namespace XUnitTestMarket.Entities
{
    public class RequestChangeStatus
    {
        public string status { get; set; }

        public RequestChangeStatus(string status)
        {
            this.status = status;
        }
    }
}
