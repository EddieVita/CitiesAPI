using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Models
{
    public static class AppDBContextExtensions
    {

        public static void Seed(this AppDBContext context)
        {
            //AppDBContext context = applicationBuilder.ApplicationServices.GetRequiredService<AppDBContext>();

            if (context.Cities.Any())
            {
                return;
            }

            //seed data
            var Cities = new List<City>()
            {
                new City()
                {
                    Name = "New York",
                    Description = "Big Apple",
                    PointsOfInterest = new List<PointOfInterest>()
                    {
                        new PointOfInterest()
                        {
                            Name = "Central Park",
                            Description = "Cool park"
                        },
                        new PointOfInterest()
                        {
                            Name = "Empire State Building",
                            Description = "Super Tall Building"
                        }
                    }
                },
                new City()
                {
                    Name = "Las Vegas",
                    Description  = "Gamblers",
                    PointsOfInterest = new List<PointOfInterest>()
                    {
                        new PointOfInterest()
                        {
                            Name = "Bellagion Fountains",
                            Description = "Cool fountains"
                        },
                        new PointOfInterest()
                        {
                            Name = "Las Vegas Strip",
                            Description = "Bright Lights"
                        }
                    }
                },
                new City()
                {
                    Name = "Houston",
                    Description = "Cow Town",
                    PointsOfInterest = new List<PointOfInterest>()
                    {
                        new PointOfInterest()
                        {
                            Name = "The Astrodome",
                            Description = "Old Dome"
                        },
                        new PointOfInterest()
                        {
                            Name = "BBQ Restaurant",
                            Description = "Fatty food"
                        }
                    }
                }
            };

            context.AddRange(Cities);

            context.SaveChanges();
        }
    }
}
