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
    public class ConcertDtoValidator : AbstractValidator<ConcertCUDto>
    {
        public ConcertDtoValidator()
        {
            RuleFor(a => a.Name).NotNull().NotEmpty().MaximumLength(50);
            RuleFor(a => a.Date).NotNull();
            RuleFor(a => a.Description).MaximumLength(120);
        }
    }
}
