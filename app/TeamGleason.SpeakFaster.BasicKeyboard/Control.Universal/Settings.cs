using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamGleason.SpeakFaster.BasicKeyboard.Control.Properties
{
    internal class Settings
    {
        internal static readonly Settings Default = new Settings();

        internal string WindowRect { get; set; }

        internal string AltWindowRect { get; set; }

        internal void Save() { }
    }
}
