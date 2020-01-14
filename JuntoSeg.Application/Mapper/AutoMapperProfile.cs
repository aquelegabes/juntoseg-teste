using AutoMapper;
using JuntoSeg.Application.ViewModel.Requests;
using JuntoSeg.Application.ViewModel.Response;
using JuntoSeg.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace JuntoSeg.Application.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserReq>().ReverseMap();
            CreateMap<User, UserResp>().ReverseMap();
        }
    }
}
