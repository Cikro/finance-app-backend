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
            if (source != null){
                return new EnumDto<DestEnum>(context.Mapper.Map<DestEnum>(source));
            }
            return new EnumDto<DestEnum>(default);
        }
    }    
}