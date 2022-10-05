using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentifikarBio.Suporte
{
    class ResolucaoCamera
    {
        private static int corte, width, height,vermelho,amarelo,verde;
        private static double x, y, circwidth;
                         
                       
        public int Widtha   // property
        {
            get { return width; }
            set { width = value; }
        }
        public int Corte   // property
        {
            get { return corte; }
            set { corte = value; }
        }
        public int Heighta   // property
        {
            get { return height; }
            set { height = value; }
        }
        public int Vermelho   // property
        {
            get { return vermelho; }
            set { vermelho = value; }
        }
        public int Amarelo   // property
        {
            get { return amarelo; }
            set { amarelo = value; }
        }
        public int Verde   // property
        {
            get { return verde; }
            set { verde = value; }
        }
        public double X   // property
        {
            get { return x; }
            set { x = value; }
        }
        public double Y   // property
        {
            get { return y; }
            set { y = value; }
        }
        public double Circwidth   // property
        {
            get { return circwidth; }
            set { circwidth = value; }
        }

    }
}
