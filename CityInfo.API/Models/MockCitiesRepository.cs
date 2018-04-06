using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Models
{
    public class MockCitiesRepository
    {
        public static MockCitiesRepository Current { get; } = new MockCitiesRepository();
        public List<City> Cities { get; set; }

        public MockCitiesRepository()
        {
            Cities = new List<City>()
            {
                new City()
                {
                    Id = 1,
                    Name = "New York",
                    Description = "Big Apple",
                    PointsOfInterest = new List<PointOfInterest>()
                    {
                        new PointOfInterest()
                        {
                            Id = 1,
                            Name = "Central Park",
                            Description = "Cool park"
                        },
                        new PointOfInterest()
                        {
                            Id = 2,
                            Name = "Empire State Building",
                            Description = "Super Tall Building"
                        }
                    }
                },
                new City()
                {
                    Id = 2,
                    Name = "Las Vegas",
                    Description  = "Gamblers",
                    PointsOfInterest = new List<PointOfInterest>()
                    {
                        new PointOfInterest()
                        {
                            Id = 3,
                            Name = "Bellagion Fountains",
                            Description = "Cool fountains"
                        },
                        new PointOfInterest()
                        {
                            Id = 4,
                            Name = "Las Vegas Strip",
                            Description = "Bright Lights"
                        }
                    }
                },
                new City()
                {
                    Id = 3,
                    Name = "Houston",
                    Description = "Cow Town",
                    PointsOfInterest = new List<PointOfInterest>()
                    {
                        new PointOfInterest()
                        {
                            Id = 5,
                            Name = "The Astrodome",
                            Description = "Old Dome"
                        },
                        new PointOfInterest()
                        {
                            Id = 6,
                            Name = "BBQ Restaurant",
                            Description = "Fatty food"
                        }
                    }
                }
            };

        }
    }
}
