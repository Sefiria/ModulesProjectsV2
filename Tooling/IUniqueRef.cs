using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tooling
{
    public interface IUniqueRef
    {
        Guid UniqueId { get; set; }
    }
}
