using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetMap.Models.Map
{
    public interface ITileProvider
    {
        string Name { get; }
        ITileProvider Instance { get; }
    }
}
