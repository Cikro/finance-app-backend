using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace finance_app.Types.DataContracts.V1.Dtos
{
    public class BaseDto
    {
        public uint? Id { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateLastEdited { get; set; }
    }
}
