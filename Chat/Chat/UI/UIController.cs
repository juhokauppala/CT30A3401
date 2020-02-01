using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chat.UI
{
    public class UIController
    {
        private static UIController singleton = null;

        public static UIController GetInstance()
        {
            if (singleton == null)
                singleton = new UIController();

            return singleton;
        }

        private UIController()
        {

        }

        public void Refresh()
        {

        }
     
    }
}
