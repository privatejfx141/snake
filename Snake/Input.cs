using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snake
{
    class Input
    {
        // load list of available keyboard buttons.
        private static Hashtable keyTable = new Hashtable();

        /// <summary>
        /// Performs check to see if a particular button is pressed.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool KeyPressed(Keys key)
        {
            if (keyTable[key] == null)
            {
                return false;
            }
            return (bool)keyTable[key];
        }

        /// <summary>
        /// Detects if a keyboard button is pressed.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="state"></param>
        public static void ChangeState(Keys key, bool state)
        {
            keyTable[key] = state;
        }
    }
}
