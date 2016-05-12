using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Memory
{
    public partial class Form2 : Form
    {
        public int x = -150;
        public int y = 0;

        List<memoryitem> FinalList;
        
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
           
            panel1.Width = 201;
            //panel1.Height = 0;
            panel1.AutoScroll = true;
            x = 0;//+ 150;
            y = 0;
            e.Graphics.Clear(Color.White);

            foreach (var process in methodology.memory)
            {
                string drawString = process.getname();
                double sb = process.getsize();
                panel1.Height += (int)sb + 1;
                System.Drawing.Font drawFont = new System.Drawing.Font("Arial", 12);
                System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
                System.Drawing.Graphics graphics = panel1.CreateGraphics();
                System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle(x, y, 100, (int)sb);
                graphics.DrawString(drawString, drawFont, drawBrush, x, y);
                y = y + (int)sb;
                
                graphics.DrawRectangle(System.Drawing.Pens.Black, rectangle);
            }

        }
        public Form2(List<memoryitem> Final)
        {
            InitializeComponent();
 
            FinalList = Final;
            panel1.Paint += new PaintEventHandler(panel1_Paint);

        }


        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }



        private void panel1_Resize(object sender, EventArgs e)
        {
            //panel1.Paint += new PaintEventHandler(panel1_Paint);

        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {

        }
    }
}
