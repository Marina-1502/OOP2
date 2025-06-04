using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OOP2
{
    public abstract class Flower
    {
        public int Roots { get; set; } = 1;
        public float LifeDuration { get; set; } = 365;

        public abstract void Bloom();
        public abstract void Reproduction();
        public abstract void DisplayType();

        public void Smell() => MessageBox.Show("Цветок вкусно пахнет");
        public void Photosynthesis() => MessageBox.Show("Происходит фотосинтез");
    }
}
