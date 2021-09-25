using System;
using AutoMapper;

using finance_app.Types.DataContracts.V1.Dtos.Enums;


namespace finance_app.Types.Mappers.Converters {
    public class EnumToEnumDtoConverter<SourceEnum, DestEnum> :
        ITypeConverter<SourceEnum, EnumDto<DestEnum>>
        where SourceEnum : Enum
        where DestEnum : Enum
    {
        public EnumDto<DestEnum> Convert(SourceEnum source, EnumDto<DestEnum> destination, ResolutionContext context)
        {
            // TODO: How does this behave When SourceEnum === DestinationEnum?
            Enum toDto;
            if (source != null){
                if (typeof(SourceEnum) == typeof(DestEnum)) {
                    toDto = source;
                } else  {
                    toDto = context.Mapper.Map<DestEnum>(source);
                }

                return new EnumDto<DestEnum>((DestEnum) toDto);
            }
            return null;
        }
    }    

    public class EnumDtoToEnumConverter<SourceEnum, DestEnum> :
        ITypeConverter<EnumDto<SourceEnum>, DestEnum>
        where SourceEnum : Enum
    {
        public DestEnum Convert(EnumDto<SourceEnum> source, DestEnum destination, ResolutionContext context)
        {            
            var toEnum = Enum.IsDefined(typeof(SourceEnum), source.Value) ? source.Value : default;  
            return context.Mapper.Map<DestEnum>(toEnum);
        }
    }    
}