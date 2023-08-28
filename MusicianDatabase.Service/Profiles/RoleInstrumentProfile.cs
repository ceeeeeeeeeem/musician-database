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
    public class RoleInstrumentProfile : Profile
    {
        public RoleInstrumentProfile()
        {
            CreateMap<RoleInstrumentCUDto, RoleInstruments>();
            CreateMap<RoleInstrumentCUDto, RoleInstruments>();
            CreateMap<RoleInstruments, RoleInBandDto>()
                .ForMember(
                    dest => dest.BandName,
                    opt => opt.MapFrom(src => src.Role.Band.Name))
                .ForMember(
                    dest => dest.ArtistFirstName,
                    opt => opt.MapFrom(src => src.Role.Artist.FirstName))
                .ForMember(
                    dest => dest.ArtistLastName,
                    opt => opt.MapFrom(src => src.Role.Artist.LastName))
                .ForMember(
                    dest => dest.Instrument,
                    opt => opt.MapFrom(src => src.Role.RoleInstruments.Select(ri => ri.Instrument.Name)));

        }
    }
}
