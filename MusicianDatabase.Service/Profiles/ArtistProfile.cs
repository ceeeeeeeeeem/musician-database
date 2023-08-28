using AutoMapper;
using Microsoft.Data.SqlClient;
using MusicianDatabase.Data.Entities;
using MusicianDatabase.Service.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicianDatabase.Service.Profiles
{
    public class ArtistProfile : Profile
    {
        public ArtistProfile()
        {
            CreateMap<ArtistCUDto, Artist>()
                .ForMember(
                    dest => dest.FirstName,
                    opt => opt.MapFrom(src => $"{src.FirstName}")
                    )
                .ForMember(
                    dest => dest.LastName,
                    opt => opt.MapFrom(src => $"{src.LastName}")
                    )
                .ForMember(
                    dest => dest.Genre,
                    opt => opt.MapFrom(src => $"{src.Genre}")
                    )
                .ForMember(
                    dest => dest.Description,
                    opt => opt.MapFrom(src => $"{src.Description}")
                    );

            // Unfinished
            CreateMap<ArtistCUDto, Artist>()
                .ForMember(
                    dest => dest.FirstName,
                    opt => opt.MapFrom(src => $"{src.FirstName}")
                    )
                .ForMember(
                    dest => dest.LastName,
                    opt => opt.MapFrom(src => $"{src.LastName}")
                    )
                .ForMember(
                    dest => dest.Genre,
                    opt => opt.MapFrom(src => $"{src.Genre}")
                    )
                .ForMember(
                    dest => dest.Description,
                    opt => opt.MapFrom(src => $"{src.Description}")
                    );
            //CreateMap<ArtistRoleDto, Artist>()
            //    .ForMember(
            //        dest => dest.Id,
            //        opt => opt.MapFrom(src => $"{src.ArtistId}")
            //        )
            //    .ForMember(
            //        dest => dest.FirstName,
            //        opt => opt.MapFrom(src => $"{src.FirstName}")
            //        )
            //    .ForMember(
            //        dest => dest.LastName,
            //        opt => opt.MapFrom(src => $"{src.LastName}")
            //        )
            //    .ForMember(
            //        dest => dest.Genre,
            //        opt => opt.MapFrom(src => $"{src.ArtistGenre}")
            //        )
            //    .ForMember(
            //        dest => dest.Roles.Select(r => r.BandId),
            //        opt => opt.MapFrom(src => $"{src.BandId}")
            //        )
            //    .ForMember(
            //        // BandName - through Roles.Band
            //        dest => dest.Roles.Select(r => r.Band.Name),
            //        opt => opt.MapFrom(src => $"{src.BandName}")
            //        )
            //    .ForMember(
            //        // BandGenre - through Roles.Band
            //        dest => dest.Roles.Select(r => r.Band.Genre),
            //        opt => opt.MapFrom(src => $"{src.BandGenre}")
            //        )
            //    .ForMember(
            //        // Instrument - Through Roles.RoleInstruments (both lists)
            //        dest => dest.Roles.SelectMany(r => r.RoleInstruments.Select(ri => ri.Instrument.Name),
            //        opt => opt.MapFrom(src => $"{src.Instrument}")
            //        ));
        }
    }
}
