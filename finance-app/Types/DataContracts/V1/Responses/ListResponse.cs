using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace finance_app.Types.DataContracts.V1.Responses
{
    public class ListResponse<T>
    {
        public ListResponse(IList<T> items) {
            Items = items;
            Length = items.Count;
        }

        public int? Length { get; set; }
        public int ExcludedItems { get; set; } = 0;
        public IEnumerable<T> Items { get; set; } 
    }
}