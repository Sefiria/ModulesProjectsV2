using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tooling.UI
{
    public interface IUIContainer
    {
        List<UI> Content { get; set; }
    }
}
