﻿using CityInfo.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.ViewModels
{
    public class CityViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public IList<PointOfInterestViewModel> PointsOfInterest { get; set; } = new List<PointOfInterestViewModel>();
    }
}
