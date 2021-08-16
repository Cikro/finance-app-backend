using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace finance_app.Types.DataContracts.V1.Dtos.Enums 
{
    public class EnumDto<T> where T : Enum
    {
        private T _Value { get; set; }
        public T Value
        {
            get
            {
                return _Value;
            }
            set
            {
                _Value = value;
                Name = value.ToString();
            }
        }
        private string _Name {get;set;}
        public string Name
        {
            get
            {
                return _Name;
            }
            private set
            {
                _Name = value;
            }
        }

        public EnumDto(T enumVal)
        {
            Value = enumVal;
        }
    }
}
