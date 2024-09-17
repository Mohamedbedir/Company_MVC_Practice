using AutoMapper;
using Company.PL.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace Company.PL.MapperProfile
{
    public class RoleProfile:Profile
    {
        public RoleProfile()
        {
            CreateMap<RoleViewModel,IdentityRole>()
                .ForMember(d=>d.Name,o=>o.MapFrom(f=>f.RoleName)).ReverseMap();
        }
    }
}
