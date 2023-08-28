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
    public class VenueProfile : Profile
    {
        public VenueProfile()
        {
            CreateMap<VenueCUDto, Venue>();
            CreateMap<VenueCUDto, Venue>();
            CreateMap<Venue, VenueAvailableDto>();
        }
    }
}
