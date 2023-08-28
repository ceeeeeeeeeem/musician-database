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
    public class VenueDtoValidator : AbstractValidator<VenueCUDto>
    {
        public VenueDtoValidator()
        {
            RuleFor(a => a.Name).NotNull().NotEmpty().MaximumLength(50);
            RuleFor(a => a.Genre).MaximumLength(50);
            RuleFor(a => a.Description).MaximumLength(120);

            // max 35 char per line for international postal address
            RuleFor(a => a.Address).MaximumLength(70);

        }
    }
}
