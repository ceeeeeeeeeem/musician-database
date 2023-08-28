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
    public class ConcertProfile : Profile
    {
        public ConcertProfile()
        {
            CreateMap<ConcertCUDto, Concert>();
            CreateMap<ConcertCUDto, Concert>();
            CreateMap<ConcertQuickCreateDto, Concert>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date))
                .ForMember(dest => dest.VenueId, opt => opt.MapFrom(src => src.VenueId))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));
                // didn't map band, can i delete formember lines?
        }
    }
}
