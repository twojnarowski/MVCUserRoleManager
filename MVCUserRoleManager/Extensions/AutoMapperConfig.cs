using AutoMapper;
using MVCUserRoleManager.Areas.Identity.Data;
using MVCUserRoleManager.Areas.Identity.DataDto;

namespace MVCUserRoleManager.Extensions
{
    public class AutoMapperConfig
    {
        /// <summary>
        /// Initializing an AutoMapper to help in mapping DTOs into regular objects and the other way around.
        /// </summary>
        /// <returns></returns>
        public static IMapper Initialize()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<RoleDto, Role>();
                cfg.CreateMap<Role, RoleDto>();
                cfg.CreateMap<UserDto, User>();
                cfg.CreateMap<User, UserDto>();
            });

            return config.CreateMapper();
        }
    }
}