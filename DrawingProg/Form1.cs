using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DrawingProg
{
    public partial class Form1 : Form
    {
        bool donePainting = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            //move this somewhere else. its terribly stupid to have this here. make its own class or something.
            if (!donePainting)
            {
                PerlinDrawer drawer = new PerlinDrawer(this);
            }
        }
    }
}
