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
            Endpoint = "https://todotest.documents.azure.com:443/";
            AuthKey = "rwXrksEn8eJCtbDS2s6SevLuRw02MLF0pnlO75VcpHlp3Ox3cXWfs0LCFEU9TGRA6ivj7VHgln3zLDXcwUtYrw==";
            Database = "todotest";
            Collection = "ItemsCollection";
        }

        public string Endpoint { get; set; }
        public string AuthKey { get; set; }
        public string Database { get; set; }
        public string Collection { get; set; }
    }
}