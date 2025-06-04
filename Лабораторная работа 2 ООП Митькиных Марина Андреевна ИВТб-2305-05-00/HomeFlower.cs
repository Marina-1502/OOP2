using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OOP2
{
    public class HomeFlower : Flower
    {
        public int OwnerCount { get; set; }
        public float Size { get; set; }

        public override void Bloom() => MessageBox.Show("Домашний цветок обильно цветёт");
        public override void Reproduction() => MessageBox.Show("Домашний цветок размножается с помощью людей");
        public override void DisplayType() => MessageBox.Show("Домашний цветок");

        public void TellStory() => MessageBox.Show("Хозяин цветка рассказывает о его истории");
        public void ShowCashpo() => MessageBox.Show("У домашнего цветка красивое кашпо");
    }
}
