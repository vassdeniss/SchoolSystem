using AutoMapper;
using SchoolSystem.Services;

namespace SchoolSystem.Tests.Common.Mocks;

public static class MapperMock
{
    public static IMapper MockMapper()
    {
        MapperConfiguration mapperConfiguration = new(config =>
        {
            config.AddProfile<MapperProfiles>();
        });

        return new Mapper(mapperConfiguration);
    }
}
