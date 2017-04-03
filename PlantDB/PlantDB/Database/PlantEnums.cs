using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantDB.Data
{
    /// Values which indicate a boolean yes or no, or a special state.
    [Flags]
    public enum YesNoMaybe
    {
        Unassigned = 1, Unknown = 2, NA = 4,
        Yes = 8, No = 16, Maybe = 32
    };

    [Flags]
    public enum FloweringMonths
    {
        Unassigned = 1, Unknown = 2, NA = 4,
        Jan = 8, Feb = 16, Mar = 32, Apr = 64, May = 128, Jun = 256,
        Jul = 512, Aug = 1024, Sep = 2048, Oct = 4096, Nov = 8192, Dec = 16384,
        AllMonths = (Jan | Feb | Mar | Apr | May | Jun | Jul | Aug | Sep | Oct | Nov | Dec)
    }

    [Flags]
    public enum PlantTypes
    {
        Unassigned = 1, Unknown = 2, NA = 4,
        Annual_herb = 8, Bush = 16, Fern = 32, Grass = 64, Perennial_herb = 128, Tree = 256, Vine = 512,
        AllPlantTypes = (Annual_herb | Bush | Fern | Grass | Perennial_herb | Tree | Vine)
    }

    [Flags]
    public enum FlowerColor
    {
        Unassigned = 1, Unknown = 2, NA = 4,
        Red = 8, Orange = 16, Yellow = 32, Green = 64, Blue = 128, Purple = 256, Violet = 512, Lavender = 1024,
        White = 2048, Pink = 4096, Brown = 8192, Textural = 16384,
        AnyColor = (Red | Orange | Yellow | Green | Blue | Purple | Violet | White | Brown) //Intentionally excluing Textural 
    };

    [Flags]
    public enum WateringRequirements
    {
        Unassigned = 1, Unknown = 2, NA = 4,
        Regular = 8, Moderate = 16, Occasional = 32, Infrequent = 64, DroughtTolerant = 128,
        AllWateringTypes = (Regular | Moderate | Occasional | Infrequent | DroughtTolerant)
    }

    [Flags]
    public enum SunRequirements
    {
        Unassigned = 1, Unknown = 2, NA = 4,
        Full = 8, Partial = 16, Shade = 32,
        AllSunTypes = (Full | Partial | Shade)
    }

    [Flags]
    public enum Drainages
    {
        Unassigned = 1, Unknown = 2, NA = 4,
        Fast = 8, Medium = 16, Slow = 32, Standing = 64, WellDraining = 128,
        AllDrainageType = (Fast | Medium | Slow | Standing)
    }
}
