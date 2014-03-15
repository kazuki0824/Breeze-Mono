using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Breeze_gui_frost
{
    static class Logger
    {
        public static bool Push(string value, int level)
        {
            //TODO
            System.Diagnostics.Debug.Print(value);
            return true;
        }
    }
}
