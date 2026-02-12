using Project11.Scenes;
using System.Windows.Forms;

namespace Project11
{
    /****************************************/
    /*                                      */
    /*              U P D A T E             */
    /*                                      */
    /****************************************/
    public partial class FormMain : Form
    {
        new void Update()
        {
            CLI.Update();
        }
    }
}
