﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using LMS.Core.Entities;
using LMS.Core.ViewModels;

namespace LMS.Data
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Course, MainCourseIndexViewModel>();
            //    .ForMember(dest => dest.Modules, from => from.MapFrom(
            //        g => g.Modules.Select(c => new ModuleViewModel { Name = c.Name, Description = c.Description })));

            //CreateMap<IEnumerable<GymClass>, IndexViewModel>()
            // .ForMember(dest => dest.ShowHistory, opt => opt.Ignore())
            //    .ForMember(dest => dest.GymClasses, from => from.MapFrom(g => g.ToList()));
            CreateMap<Module, ModuleViewModel>().ReverseMap();
        }
    }
}