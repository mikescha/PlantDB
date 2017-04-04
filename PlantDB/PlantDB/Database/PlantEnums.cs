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

    //Must be kept in sync with the list of strings in PageLocation.xaml.cs
    public enum Counties
    {
        All = 0,
        Alameda =  1,
        Alpine =  2,
        Amador =  3,
        Butte =  4,
        Calaveras =  5,
        ContraCosta =  6,
        Colusa =  7,
        DelNorte =  8,
        ElDorado =  9,
        Fresno =  10,
        Glenn =  11,
        Humboldt =  12,
        Imperial =  13,
        Inyo =  14,
        Kings =  15,
        Kern =  16,
        Lake =  17,
        Lassen =  18,
        LosAngeles =  19,
        Madera =  20,
        Mendocino =  21,
        Merced =  22,
        Mono =  23,
        Monterey =  24,
        Modoc =  25,
        Mariposa =  26,
        Marin =  27,
        Napa =  28,
        Nevada =  29,
        Orange =  30,
        Placer =  31,
        Plumas =  32,
        Riverside =  33,
        Sacramento =  34,
        SantaBarbara =  35,
        SanBernardino =  36,
        SanBenito =  37,
        SantaClara =  38,
        SantaCruz =  39,
        SanDiego =  40,
        SanFrancisco =  41,
        Shasta =  42,
        Sierra =  43,
        Siskiyou =  44,
        SanJoaquin =  45,
        SanLuisObispo =  46,
        SanMateo =  47,
        Solano =  48,
        Sonoma =  49,
        Stanislaus =  50,
        Sutter =  51,
        Tehama =  52,
        Trinity =  53,
        Tulare =  54,
        Tuolumne =  55,
        Ventura =  56,
        Yolo =  57,
        Yuba =  58
    }
}
