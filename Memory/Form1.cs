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
    public partial class Form1 : Form
    {
        public int x = -150;
        public int y = 0;
        public int processcounter = 0 ;
        public Form1()
        {
            InitializeComponent();
            panel1.Paint += new PaintEventHandler(panel1_Paint);

        }

        private void paintme(object sender, PaintEventArgs e)
        {
            panel1.Width = 201;
            panel1.AutoScroll = true;
            x = 0;
            y = 0;
           // e.Graphics.Clear(Color.White);
            foreach (var process in methodology.memory)
            {
                string type = process.type;
                Color color;
                string drawString = process.getname();
                int start_address = process.getstratingadd();
                string add_draw = "0x" + start_address;
                double sb = process.getsize();
                panel1.Height += (int)sb + 1;
                System.Drawing.Font drawFont = new System.Drawing.Font("Arial", 12);
                if(type == "h")
                {
                    color = System.Drawing.Color.Red;
                }
                else if (type == "p")
                {
                    color = System.Drawing.Color.Green;
                }
                else
                {
                    color = System.Drawing.Color.Black;
                }
                System.Drawing.SolidBrush drawBrush = new System.Drawing.SolidBrush(color);
                System.Drawing.SolidBrush stringbrush = new System.Drawing.SolidBrush(System.Drawing.Color.White);
                System.Drawing.Graphics graphics = panel1.CreateGraphics();
                System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle(x, y, 100, (int)sb);
                graphics.FillRectangle(drawBrush, rectangle);
                graphics.DrawRectangle(System.Drawing.Pens.Black, rectangle);
                graphics.DrawString(add_draw, drawFont, drawBrush, x + 100, y);
                graphics.DrawString(drawString, drawFont, stringbrush, x, y);
                y = y + (int)sb;
            }

        }
        protected override void OnPaint(PaintEventArgs e)
        {
        
        }
      
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            paintme(this,e);   
            
        }
     
        private void button1_Click(object sender, EventArgs e)
        {
            int size= Convert.ToInt32( textBox1.Text);
            int address = Convert.ToInt32(textBox2.Text);
            methodology.inserthole(new hole(address,size));
            listBox1.Items.Add("Added a hole of size "+ size+" kb at 0x"+ address);
            methodology.memory = methodology.memory.OrderBy(a => a.getstratingadd()).ToList();
            foreach (var item in methodology.memory)
            {
                Console.WriteLine(item.GetType() + " :item at" + item.getstratingadd() + " with size:" + item.getsize());
            }
            Console.WriteLine("%%%%%%%%");
            methodology.holes_counter++;
            panel1.Invalidate();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int size = Convert.ToInt32(textBox4.Text);
            string name = textBox3.Text;
            if (name == "")
            {
                processcounter++;
                name = "process # "+processcounter;
            }
            #region   
            if (comboBox1.Text == "First Fit")
            {
                methodology.FirstFit(new process(name, size));
                comboBox2.Items.Add(name);
            }
            else if (comboBox1.Text == "Best Fit")
            {
                methodology.BestFit(new process(name, size));
                comboBox2.Items.Add(name);
            }
            else
            {
                methodology.WorstFit(new process(name, size));
                comboBox2.Items.Add(name);
            }
            #endregion

            listBox1.Items.Add("Allocated a process of size " +size);
            methodology.memory = methodology.memory.OrderBy(a => a.getstratingadd()).ToList();

            foreach (var item in methodology.memory)
            {
                Console.WriteLine(item.GetType()+" :item at"+item.getstratingadd()+" with size:"+item.getsize());

            }
            Console.WriteLine("%%%%%%%%");
            
           panel1.Invalidate();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            methodology.deallocate(comboBox2.Text);
            comboBox2.Items.Remove(comboBox2.SelectedItem);
            listBox1.Items.Add("removed Process "+comboBox2.Text);
            methodology.memory = methodology.memory.OrderBy(a => a.getstratingadd()).ToList();
            foreach (var item in methodology.memory)
            {
                Console.WriteLine(item.GetType() + " :item at" + item.getstratingadd() + " with size:" + item.getsize());

            }
            Console.WriteLine("%%%%%%%%");
            panel1.Invalidate();
        }
    }
}
