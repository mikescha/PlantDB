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
                            .ToList();
            }

            var sorted = from p in newList
                         orderby p.ScientificName
                         group p by p.Type into plantGroups
                         select new Grouping<string, Plant>(plantGroups.Key, plantGroups);

            PlantList = new ObservableCollection<Grouping<string, Plant>>(sorted);

            PlantCount = newList.Count();
        }


        #region Includes operations
        // Returns true if the any of the flowering months contained in Wanted matches any of the flowering months in Test
        // e.g. if Wanted = "Jan or Feb" and Test = "Feb or Mar" then true
        //      if Wanted = "Jan or Feb" and Test = "Mar or Apr" then false
        //      if Wanted = "Jan or Feb" and Test = "AllMonths" then true 
        private bool IncludesMonths(FloweringMonths wanted, string candidate)
        {
            FloweringMonths[] target = {FloweringMonths.Jan, FloweringMonths.Feb, FloweringMonths.Mar, FloweringMonths.Apr,
                                        FloweringMonths.May, FloweringMonths.Jun, FloweringMonths.Jul, FloweringMonths.Aug,
                                        FloweringMonths.Sep, FloweringMonths.Oct, FloweringMonths.Nov, FloweringMonths.Dec};

            bool result = false;

            foreach (FloweringMonths f in target)
            {
                result = result || (candidate.Contains(floweringMonthDict[f]) && wanted.HasFlag(f));
            }

            return result || wanted.HasFlag(FloweringMonths.AllMonths);
        }
        #endregion Includes operations

        //Used to convert the string representing a month into a typed value. 
        //TODO: Need to think through the right layer for the conversion from string to type to be at. 
        private FloweringMonths GetMonthFromString(string month)
        {
            FloweringMonths floweringMonth = FloweringMonths.NotApplicable;
            foreach (FloweringMonths f in floweringMonthDict.Keys)
            {
                if (month == floweringMonthDict[f])
                {
                    floweringMonth = f;
                    break;
                }
            }
            return floweringMonth;

        }

    }
}
