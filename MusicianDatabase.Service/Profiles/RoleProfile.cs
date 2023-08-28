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
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<RoleCUDto, Role>();
            CreateMap<RoleCUDto, Role>();
            CreateMap<RoleDto, Role>()
                .ForMember(
                    dest => dest.Id,
                    opt => opt.MapFrom(src => src.RoleId)
                    )
                .ForPath(
                    dest => dest.Artist.FirstName,
                    opt => opt.MapFrom(src => src.ArtistFirstName)
                    )
                .ForPath(
                    dest => dest.Artist.LastName,
                    opt => opt.MapFrom(src => src.ArtistLastName)
                    )
                .ForPath(
                    dest => dest.Band.Name,
                    opt => opt.MapFrom(src => src.BandName)
                    );
            CreateMap<Role, RoleDto>()
                .ForMember(
                    dest => dest.RoleId,
                    opt => opt.MapFrom(src => src.Id)
                    )
                .ForMember(
                    dest => dest.ArtistFirstName,
                    opt => opt.MapFrom(src => src.Artist.FirstName)
                    )
                .ForMember(
                    dest => dest.ArtistLastName,
                    opt => opt.MapFrom(src => src.Artist.LastName)
                    )
                .ForMember(
                    dest => dest.BandName,
                    opt => opt.MapFrom(src => src.Band.Name)
                    )
                .ForMember(
                    dest => dest.Instrument,
                    opt => opt.MapFrom(src => string.Join(", ", src.RoleInstruments.Select(ri => ri.Instrument.Name)))
                    );


            //CreateMap<RoleDto, RoleInstruments>()
            //    .ForPath(
            //        dest => dest.Role.Band.Name,
            //        opt => opt.MapFrom(src => src.BandName)
            //        )
            //    .ForPath(
            //        dest => dest.Role.Artist.FirstName,
            //        opt => opt.MapFrom(src => src.ArtistFirstName)
            //        )
            //    .ForPath(
            //        dest => dest.Role.Band.Name,
            //        opt => opt.MapFrom(src => src.BandName)
            //        )
            //    .ForPath(
            //        dest => dest.Role.Description,
            //        opt => opt.MapFrom(src => src.Description)
            //        )
            //    .ForPath(
            //        dest => dest.Instrument.Name,
            //        opt => opt.MapFrom(src => src.Instrument)
            //        ).ReverseMap();
            //CreateMap<RoleDto, RoleInstruments>().AfterMap((src, dest, context) =>
            //{
            //    dest.Role = new Role
            //    {
            //        Band = new Band { Name = src.BandName },
            //        Artist = new Artist { FirstName = src.ArtistFirstName },
            //        Description = src.Description
            //    };

            //    dest.Instrument = new Instrument { Name = src.Instrument };
            //});
        }
    }
}
