using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TeknikServis.Models.Entities;
using TeknikServis.Models.ViewModels;

namespace TeknikServisWeb.App_Start
{
    public class AutoMapperConfig
    {
        public static void RegisterMappings()
        {
            Mapper.Initialize(cfg =>
            {
                ArizaMapping(cfg);

            });
        }
        private static void ArizaMapping(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<Ariza, ArizaViewModel>()
                //.ForMember(dest => dest.SubEmplyeeCount, opt => opt.MapFrom(x => x.Employees1.Count))
                //.ForMember(dest => dest.TeknisyenAdi, opt => opt.MapFrom((s, d) => s.Teknisyen?.Name + " " + s.Teknisyen?.Surname))
                //.ForMember(dest => dest.MusteriAdi, opt => opt.MapFrom((s, d) => s.Musteri?.Name + " " + s.Musteri?.Surname))
                // .ForMember(dest => dest.TeknisyenDurumu, opt => opt.MapFrom((s, d) => s.Teknisyen?.TeknisyenDurumu))
                .ReverseMap();
        }

    }
}