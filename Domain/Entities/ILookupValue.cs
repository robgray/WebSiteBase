using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public interface ILookupValue
    {
        int Id { get; }
        string Code { get; set; }
        string Description { get; set; }
        int SortOrder { get; set; }
    }
}
