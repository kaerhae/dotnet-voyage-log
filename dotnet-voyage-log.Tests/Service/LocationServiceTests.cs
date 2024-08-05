
using dotnet_voyage_log.Interfaces;
using dotnet_voyage_log.Models;
using dotnet_voyage_log.Service;
using Microsoft.Extensions.Logging;
using Moq;

namespace dotnet_voyage_log.Tests.Service;

public class LocationServiceTests
{
    private ILocationService _service;

    private Mock<ILocationRepository> _repository;
    private Mock<ILogger<ILocationService>> _logger;

    public LocationServiceTests()
    {
        _repository = new Mock<ILocationRepository>();
        _logger = new Mock<ILogger<ILocationService>>();
        _service = new LocationService(_repository.Object, _logger.Object);
    }

    [Fact]
    public void GetAllCountries_ShouldReturnCountries()
    {
        List<Country> countries = new List<Country>(){
            new Country(){ Id = 1, Name = "Finland", Regions = new List<Region>() { new Region(){ Name = "Uusimaa"}}}
        };
        _repository.Setup(x => x.GetAllCountries()).Returns(countries);

        var result = _service.GetAllCountries();

        Assert.Equal(countries[0].Id, result[0].Id);
        Assert.Equal(countries[0].Name, result[0].Name);
        Assert.Equal(countries[0].Regions[0].Name, result[0].Regions[0].Name);

    }

    [Fact]
    public void GetAllRegions_ShouldReturnRegions()
    {
        List<Region> regions = new List<Region>(){
            new Region() { Id=0, Name="Skåne", CountryId=0}
        };
        _repository.Setup(x => x.GetAllRegions()).Returns(regions);

        var result = _service.GetAllRegions();

        Assert.Equal(regions[0].Id, result[0].Id);
        Assert.Equal(regions[0].Name, result[0].Name);
        Assert.Equal(regions[0].CountryId, result[0].CountryId);
    }

    [Fact]
    public void GetSingleCountry_ShouldReturnCountry()
    {
        Country country = new Country(){
            Id = 1, 
            Name = "Finland", 
            Regions = new List<Region>() { new Region(){ Name = "Uusimaa"}}
        };
        _repository.Setup(x => x.GetSingleCountry("Finland")).Returns(country);

        var result = _service.GetSingleCountry("Finland");

        Assert.Equal(country.Id, result.Id);
        Assert.Equal(country.Name, result.Name);
        Assert.Equal(country.Regions[0].Name, result.Regions[0].Name);

    }

    [Fact]
    public void GetSingleRegion_ShouldReturnRegion()
    {
        Region region = new Region(){
            Id=0, Name="Skåne", CountryId=0
        };
        _repository.Setup(x => x.GetSingleRegion("Skåne")).Returns(region);

        var result = _service.GetSingleRegion("Skåne");

        Assert.Equal(region.Id, result.Id);
        Assert.Equal(region.Name, result.Name);
        Assert.Equal(region.CountryId, result.CountryId);

    }

    [Fact]
    public void GetSingleCountry_ShouldThrowErrorWhenNull()
    {
        _repository.Setup(x => x.GetSingleCountry("Finland")).Returns((Country)null);

        var exception = Assert.Throws<Exception>(() => _service.GetSingleCountry("Finland"));
        Assert.Equal("Country not found", exception.Message);

    }

    [Fact]
    public void GetSingleRegion_ShouldThrowErrorWhenNull()
    {
        _repository.Setup(x => x.GetSingleRegion("Skåne")).Returns((Region)null);


        var exception = Assert.Throws<Exception>(() => _service.GetSingleRegion("Skåne"));
        Assert.Equal("Region not found", exception.Message);

    }
}