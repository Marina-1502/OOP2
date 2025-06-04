using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OOP2
{
    public class FieldFlower : Flower
    {
        public int Petals { get; set; }
        public float Height { get; set; }

        public override void Bloom() => MessageBox.Show("Полевой цветок скудно цветёт");
        public override void Reproduction() => MessageBox.Show("Полевой цветок размножается самостоятельно");
        public override void DisplayType() => MessageBox.Show("Полевой цветок");

        public void AttractPollinators() => MessageBox.Show("Полевой цветок опыляют шмели, бабочки, пчёлы");
        public void ShowHabitat() => MessageBox.Show("Полевой цветок растёт в поле");
    }
}
