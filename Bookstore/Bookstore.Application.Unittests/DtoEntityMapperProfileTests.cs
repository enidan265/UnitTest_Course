using AutoMapper;
using Xunit;

namespace Bookstore.Application.Unittests
{
    public class DtoEntityMapperProfileTests
    {
        [Fact]
        public void Mapping_Configuration_Is_Valid()
        {
            var config =
                new MapperConfiguration(cfg => cfg.AddMaps(typeof(DtoEntityMapperProfile)));

            config.AssertConfigurationIsValid();
        }
    }
}
