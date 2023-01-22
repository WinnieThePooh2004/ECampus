﻿using AutoMapper;
using ECampus.Domain.Mapping.Converters;
using ECampus.Shared.DataTransferObjects;
using ECampus.Shared.Models;

namespace ECampus.Domain.Mapping
{
    public class AuditoryProfile : Profile
    {
        public AuditoryProfile()
        {
            CreateMap<Auditory, AuditoryDto>().ReverseMap();
            this.CreateListWithPaginationDataMap<Auditory, AuditoryDto>();
        }
    }
}