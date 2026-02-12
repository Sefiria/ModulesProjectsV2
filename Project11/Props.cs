using Project11.Scenes;
using System.Windows.Forms;

namespace Project11
{
    /****************************************/
    /*                                      */
    /*               P R O P S              */
    /*                                      */
    /****************************************/
    public partial class FormMain : Form
    {
        CLI CLI;

        void Init()
        {
            CLI = new CLI();
        }
    }
}
