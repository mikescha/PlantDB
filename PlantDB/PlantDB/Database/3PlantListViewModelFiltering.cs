using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Xamarin.Forms;

/* View Model filtering
 * 
 * Contains all the operations for deciding which plants to show
 * 
 */
namespace PlantDB.Data
{
    public partial class PlantListViewModel : INotifyPropertyChanged
    {
        //Refreshes the plant list and all other data associated with it
        //TODO This requeries the plant list every time, even if nothing has changed since last time. Need some kind of optimization to save cycles.
        private void SetPlantList()
        {
            List<Plant> newList;
            if (viewShowing == ViewShowing.Cart)
            {
                newList = PlantData.GetPlantsInCart();
            }
            else
            {
                newList = PlantData.GetAllPlants();
                newList = newList
                            .Where(p => IncludesMonths(TargetPlant.FloweringMonths, p.FloweringMonths))
                            .Where(p => IncludesTypes(TargetPlant.PlantTypes, p.PlantType))
                            .ToList();
            }

            var sorted = from p in newList
                         orderby p.ScientificName
                         group p by p.PlantTypeString into plantGroups
                         select new Grouping<string, Plant>(plantGroups.Key, plantGroups);

            PlantList = new ObservableCollection<Grouping<string, Plant>>(sorted);

            PlantCount = newList.Count();
        }


        #region Includes operations
        // Returns true if the any of the flowering months contained in Wanted matches any of the flowering months in Test
        // e.g. if Wanted = "Jan or Feb" and Test = "Feb or Mar" then true
        //      if Wanted = "Jan or Feb" and Test = "Mar or Apr" then false
        //      if Wanted = "Jan or Feb" and Test = "AllMonths" then true 
        private bool IncludesMonths(FloweringMonths wanted, FloweringMonths candidate)
        {
            FloweringMonths[] target = {FloweringMonths.Jan, FloweringMonths.Feb, FloweringMonths.Mar, FloweringMonths.Apr,
                                        FloweringMonths.May, FloweringMonths.Jun, FloweringMonths.Jul, FloweringMonths.Aug,
                                        FloweringMonths.Sep, FloweringMonths.Oct, FloweringMonths.Nov, FloweringMonths.Dec};

            bool result = false;

            foreach (FloweringMonths i in target)
            {
                result = result || (candidate.HasFlag(i) && wanted.HasFlag(i));
            }

            return result || wanted.HasFlag(FloweringMonths.AllMonths);
        }

        // Returns true if the any of the flowering months contained in Wanted matches any of the flowering months in Test
        // e.g. if Wanted = "Jan or Feb" and Test = "Feb or Mar" then true
        //      if Wanted = "Jan or Feb" and Test = "Mar or Apr" then false
        //      if Wanted = "Jan or Feb" and Test = "AllMonths" then true 
        private bool IncludesTypes(PlantTypes wanted, PlantTypes candidate)
        {
            PlantTypes[] target = {PlantTypes.Annual_herb, PlantTypes.Bush, PlantTypes.Fern, PlantTypes.Grass,
                                   PlantTypes.Perennial_herb, PlantTypes.Tree, PlantTypes.Vine};
            bool result = false;

            foreach (PlantTypes i in target)
            {
                result = result || (candidate.HasFlag(i) && wanted.HasFlag(i));
            }

            return result || wanted.HasFlag(PlantTypes.AllPlantTypes);
        }

        #endregion Includes operations



    }
}
