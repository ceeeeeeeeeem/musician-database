using AutoMapper;
using MusicianDatabase.Data.Entities;
using MusicianDatabase.Service.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicianDatabase.Service.Profiles
{
    public class BandProfile : Profile
    {
        public BandProfile()
        {
            CreateMap<BandCUDto, Band>();
            CreateMap<BandCUDto, Band>();
            CreateMap<Band, BandAvailableDto>();
        }
    }
}
