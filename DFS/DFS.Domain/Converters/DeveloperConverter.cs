using DFS.Domain.Entities;
using DFS.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DFS.Domain.Converters
{
    public class DeveloperConverter
    {
        public static DeveloperViewModel Convert(Developer developer)
        {
            var developerViewModel = new DeveloperViewModel();
            developerViewModel.DevID = developer.DevID;
            developerViewModel.Email = developer.Email;

            return developerViewModel;
        }

        public static List<DeveloperViewModel> ConvertList(IEnumerable<Developer> developers)
        {
            return developers.Select(e =>
            {
                var model = new DeveloperViewModel();
                model.DevID = e.DevID;
                model.Email = e.Email;
                return model;
            }).ToList();
        }

    }
}
