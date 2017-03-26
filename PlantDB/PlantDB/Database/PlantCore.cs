using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantDB.Data
{
    public partial class Plant
    {

    }

    [Flags]
    public enum FloweringMonths
    {
        Unassigned = 1, Unknown = 2, NotApplicable = 4,
        Jan = 8, Feb = 16, Mar = 32, Apr = 64, May = 128, Jun = 256,
        Jul = 512, Aug = 1024, Sep = 2048, Oct = 4096, Nov = 8192, Dec = 16384,
        AllMonths = (Jan | Feb | Mar | Apr | May | Jun | Jul | Aug | Sep | Oct | Nov | Dec)
    }

}
