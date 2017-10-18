using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoApi.Models
{
    public class DocDbConfig
    {
        public DocDbConfig()
        {
            Endpoint = "FILLME IN";
            AuthKey = "FILLMEIN";
            Database = "todotest";
            Collection = "ItemsCollection";
        }

        public string Endpoint { get; set; }
        public string AuthKey { get; set; }
        public string Database { get; set; }
        public string Collection { get; set; }
    }
}