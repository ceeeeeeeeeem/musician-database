using FluentValidation;
using MusicianDatabase.Data.Entities;
using MusicianDatabase.Service.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicianDatabase.Service.Validators
{
    public class InstrumentDtoValidator : AbstractValidator<InstrumentCUDto>
    {
        public InstrumentDtoValidator()
        {
            RuleFor(a => a.Name).NotNull().NotEmpty().MaximumLength(20);
        }
    }
}
