using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CityInfo.API.Models;
using CityInfo.API.Services;
using CityInfo.API.ViewModels;
using Microsoft.Extensions.Logging;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CityInfo.API.Controllers
{
    [Route("api/cities")]
    public class CitiesController : Controller
    {
        private readonly ICityRepsitory _cityRepository;
        ILogger<CitiesController> _logger;

        public CitiesController(ILogger<CitiesController> logger, ICityRepsitory cityRepsitory)
        {
            _cityRepository = cityRepsitory;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetCities()
        {
            var cities = _cityRepository.GetCities();

            var results = AutoMapper.Mapper.Map<IEnumerable<CitySummary>>(cities);

            return Ok(results);
        }

        [HttpGet("{id}")]
        public IActionResult GetCity(int id, bool includePointsOfInterest = false)
        {
            try
            {
                var city = _cityRepository.GetCity(id, includePointsOfInterest);

                if (city == null)
                {
                    return NotFound();
                }

                if (includePointsOfInterest)
                {
                    //var cityViewModel = new CityViewModel()
                    //{
                    //    Id = city.Id,
                    //    Name = city.Name,
                    //    Description = city.Description
                    //};

                    //foreach (var point in city.PointsOfInterest)
                    //{
                    //    cityViewModel.PointsOfInterest.Add(
                    //        new PointOfInterest()
                    //        {
                    //            Id = point.Id,
                    //            Name = point.Name,
                    //            Description = point.Description
                    //        });
                    //}
                    var cityResult = AutoMapper.Mapper.Map<CityViewModel>(city);

                    return Ok(cityResult);
                }

                //var CitySummary = new CitySummary() { Id = city.Id, Name = city.Name, Description = city.Description};

                var CitySummary = AutoMapper.Mapper.Map<CitySummary>(city);
                return Ok(CitySummary);
            }
            catch (Exception ex)
            {

                _logger.LogCritical($"Exception while getting a city with id {id}.", ex);
                return StatusCode(500, "A problem happened while handling your request.");
            }
        
        }
    }
}
