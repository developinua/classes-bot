using AutoMapper;

namespace Domain.Mapper;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<Models.UserProfile, Models.UserProfile>()
            .ForMember(dest => dest.Culture, opt => opt.Ignore())
            .AfterMap((src, dest) =>
            {
                dest.Culture = new Models.Culture
                {
                    Id = dest.Culture.Id,
                    Name = dest.Culture.Name,
                    Code = dest.Culture.Code,
                    CreatedAt = src.Culture.CreatedAt,
                    UpdatedAt = DateTime.UtcNow
                };
            });
    }
}