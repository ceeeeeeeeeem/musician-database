using FluentValidation;
using MusicianDatabase.Data;
using MusicianDatabase.Data.Entities;
using MusicianDatabase.Service.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicianDatabase.Service.Validators
{
    public class ArtistDtoValidator : AbstractValidator<ArtistCUDto>
    {
        public ArtistDtoValidator()
        {
            RuleFor(a => a.FirstName).NotNull().NotEmpty().MaximumLength(15);
            RuleFor(a => a.LastName).NotNull().NotEmpty().MaximumLength(15);
            RuleFor(a => a.Genre).NotNull().MaximumLength(30);
            RuleFor(a => a.Description).MaximumLength(120);
        }
    }
}
