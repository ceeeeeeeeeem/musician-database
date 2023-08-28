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
    public class BandDtoValidator : AbstractValidator<BandCUDto>
    {
        public BandDtoValidator()
        {
            RuleFor(a => a.Name).NotNull().NotEmpty().MaximumLength(20);
            RuleFor(a => a.Genre).NotNull().MaximumLength(30);
            RuleFor(a => a.Description).MaximumLength(120);
        }
    }
}
