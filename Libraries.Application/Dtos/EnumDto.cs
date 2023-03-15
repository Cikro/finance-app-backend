using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.Dtos
{
    public class EnumDto<T> where T : Enum
    {
        private T _value { get; set; }
        public T Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                Name = value.ToString();
            }
        }
        private string _name { get; set; }
        public string Name
        {
            get
            {
                return _name;
            }
            private set
            {
                _name = value;
            }
        }


        public EnumDto() { }
        public EnumDto(T enumVal)
        {
            Value = enumVal;
        }
    }
}
