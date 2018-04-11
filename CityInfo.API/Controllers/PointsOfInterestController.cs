using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CityInfo.API.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Logging;
using CityInfo.API.Services;
using CityInfo.API.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CityInfo.API.Controllers
{
    [Route("api/cities")]
    public class PointsOfInterestController : Controller
    {
        ILogger<PointsOfInterestController> _logger;
        private readonly IMailService _mailService;
        private readonly ICityRepsitory _cityRepository;

        public PointsOfInterestController(ILogger<PointsOfInterestController> logger, IMailService mailService, ICityRepsitory cityRepsitory)
        {
            _logger = logger;
            _mailService = mailService;
            _cityRepository = cityRepsitory;
            //Can use this to request
            //HttpContext.RequestServices.GetService()
        }

        [HttpGet("{cityId}/pointsofinterest")]
        public IActionResult GetPointsOfInterest(int cityId)
        {
            try
            {
                //var city = MockCitiesRepository.Current.Cities.FirstOrDefault(c => c.Id == cityId);

                if (!_cityRepository.CityExists(cityId))
                {
                    _logger.LogInformation($"City with id {cityId} wasn't found when accessing points of interest.");
                    return NotFound();
                }

                var pointOfInterest = _cityRepository.GetPointsOfInterestForCity(cityId);
                var results = AutoMapper.Mapper.Map<IEnumerable<PointOfInterestViewModel>>(pointOfInterest);

                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception while getting points of interest for city with id {cityId}.", ex);
                return StatusCode(500, "A problem happened while handling your request.");
            }
        }

        [HttpGet("{cityId}/pointsofinterest/{id}", Name = "GetPointOfInterest")]
        public IActionResult GetPointOfInterest(int cityId, int id)
        {
            //var city = MockCitiesRepository.Current.Cities.FirstOrDefault(c => c.Id == cityId);

            if (!_cityRepository.CityExists(cityId))
            {
                _logger.LogInformation($"City with id {cityId} wasn't found when accessing points of interest.");
                return NotFound();
            }

            var pointofinterest = _cityRepository.GetPointOfInterestForCity(cityId,id);

            if (pointofinterest == null)
            {
                return NotFound();
            }

            var pointOfInterestViewModel = AutoMapper.Mapper.Map<PointOfInterestViewModel>(pointofinterest);
            return Ok(pointOfInterestViewModel);
        }

        [HttpPost("{cityId}/pointsofinterest")]
        public IActionResult CreatePointOfInterest(int cityId, [FromBody] PointOfInterestViewModel pointOfInterest)
        {
            if (pointOfInterest == null)
            {
                return BadRequest();
            }

            //Example of a custom validation error.  The name and desctiption cannot be the same
            if (pointOfInterest.Description == pointOfInterest.Name)
            {
                ModelState.AddModelError("Description", "The description and name cannot be the same");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (! _cityRepository.CityExists(cityId))
            {
                //City was not found in the db.
                return NotFound();
            }

            var newPointOfInterest = AutoMapper.Mapper.Map<PointOfInterest>(pointOfInterest);

            //var newPointOfInterest = new PointOfInterest()
            //{
            //    Name = pointOfInterest.Name,
            //    Description = pointOfInterest.Description
            //};

            _cityRepository.AddPointOfInterestForCity(cityId, newPointOfInterest);

            if (!_cityRepository.Save())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            var newPointOfInterestViewModel = AutoMapper.Mapper.Map<PointOfInterestViewModel>(newPointOfInterest);

            //Will return the url of the newly created resource.
            return CreatedAtRoute("GetPointOfInterest", new { cityId = cityId, id = newPointOfInterestViewModel.Id}, newPointOfInterestViewModel);
        }

        [HttpPut("{cityId}/pointsofinterest/{id}")]
        public IActionResult UpdatePointOfInterest(int cityId, int id, [FromBody] PointOfInterest pointOfInterest)
        {

            if (pointOfInterest == null)
            {
                return BadRequest();
            }

            //Example of a custom validation error.  The name and desctiption cannot be the same
            if (pointOfInterest.Description == pointOfInterest.Name)
            {
                ModelState.AddModelError("Description", "The description and name cannot be the same");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_cityRepository.CityExists(cityId))
            {
                return NotFound();
            }

            var pointOfInterestFromStore = _cityRepository.GetPointOfInterestForCity(cityId, id);

            if (pointOfInterestFromStore == null)
            {
                return NotFound();
            }

            //According to the restful implementation, a put request must update ALL FIELDS.  If not, an error should be returned.
            pointOfInterestFromStore.Name = pointOfInterest.Name;
            pointOfInterestFromStore.Description = pointOfInterest.Description;

            if (!_cityRepository.Save())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            return NoContent();
        }

        [HttpPatch("{cityId}/pointsofinterest/{id}")]
        public IActionResult PartiallyUpdatePointOfInterest(int cityId, int id, [FromBody] JsonPatchDocument<PointOfInterestViewModel> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            if (!_cityRepository.CityExists(cityId))
            {
                return NotFound();
            }

            var pointOfInterestFromStore = _cityRepository.GetPointOfInterestForCity(cityId, id);

            if (pointOfInterestFromStore == null)
            {
                return NotFound();
            }

            var pointOfInterestToPatch = AutoMapper.Mapper.Map<PointOfInterestViewModel>(pointOfInterestFromStore);

            patchDoc.ApplyTo(pointOfInterestToPatch, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            AutoMapper.Mapper.Map(pointOfInterestToPatch, pointOfInterestFromStore);

            if (!_cityRepository.Save())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }
            
            return NoContent();
        }

        [HttpDelete("{cityId}/pointsofinterest/{id}")]
        public IActionResult DeletePointOfInterest(int cityId, int id)
        {

            if (!_cityRepository.CityExists(cityId))
            {
                return NotFound();
            }

            var pointOfInterestFromStore = _cityRepository.GetPointOfInterestForCity(cityId, id);

            if (pointOfInterestFromStore == null)
            {
                return NotFound();
            }

            _cityRepository.DeletePointOfInterest(pointOfInterestFromStore);

            if (!_cityRepository.Save())
            {
                return StatusCode(500, "A problem happened while handling your request.");
            }

            //Notifiy administrators that a resource was deleted
            _mailService.Send("A point of interest was deleted.", $"Point of interest {pointOfInterestFromStore.Name} with id {pointOfInterestFromStore.Id} was deleted");
            return NoContent();
        }
    }
}
