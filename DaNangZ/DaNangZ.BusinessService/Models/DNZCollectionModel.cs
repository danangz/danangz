using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaNangZ.BusinessService.Models
{
    public class DNZCollectionModel<T> where T : class
    {
        public IList<T> Data { get; set; }

        public int TotalPages { get; set; }

        public int TotalRecords { get; set; }
    }
}
