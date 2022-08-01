using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace finance_app.Types.DataContracts.V1.Dtos
{
    /// <summary>
    /// Represents an object WITHIN a request object.
    /// TODO: This Feels bad, maybe find a nicer way of doing things.
    /// </summary>
    public class BaseRequestObject
    {
        /// <summary>
        /// The Id of the object
        /// </summary>
        public uint? Id { get; set; }
    }
}
