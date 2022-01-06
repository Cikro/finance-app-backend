using FluentValidation;
using finance_app.Types.DataContracts.V1.Dtos;
using finance_app.Types.DataContracts.V1.Dtos.Enums;
using System;

namespace finance_app.Types.Validators
{
    public class EnumDtoValidator<T> : AbstractValidator<EnumDto<T>> where T : Enum
    {
        public EnumDtoValidator()
        {
            RuleFor(enumDto => enumDto.Value)
                .IsInEnum()
                .WithMessage(enumDto => $"{enumDto.Value.GetType().Name} does not support the value: {{PropertyValue}}.");
        }
    }
}
