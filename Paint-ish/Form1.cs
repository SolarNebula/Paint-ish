using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Globalization;

namespace Paint_ish
{
    public partial class Form1 : Form
    {
        SerialPort port;
        Graphics g;
        Boolean cursorMoving = false;
        Pen cursorPen;
        int cursorX = -1, cursorY = -1;
        int x = 200, y = 200;
        int[] int_velocity = new int[3];


        public Form1()
        {
            InitializeComponent();

            port = new SerialPort("COM3", 115200);
            port.Open();

            g = canvas.CreateGraphics();
            cursorPen = new Pen(Color.Black, 5);
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            cursorPen.StartCap = System.Drawing.Drawing2D.LineCap.Round;
            cursorPen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
        }

        private void canvas_MouseDown(object sender, MouseEventArgs e)
        {
            cursorMoving = true;
            cursorX = e.X;
            cursorY = e.Y;

        }

        private void canvas_MouseUp(object sender, MouseEventArgs e)
        {
            cursorMoving = false;
            cursorX = -1;
            cursorY = -1;
        }

        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (cursorX != -1 && cursorY != -1 && cursorMoving == true)
            {
                g.DrawLine(cursorPen, new Point(cursorX, cursorY), e.Location);
                cursorX = e.X;
                cursorY = e.Y;
            }

        }


        private void timer1_Tick(object sender, EventArgs e)
        {
            string data = string.Empty;
            data = port.ReadLine();
            data = data.Substring(0, data.Length - 1);

            textBox1.Text = data ;
            String[] data_split = data.Split(' ');

                    
            for (int i = 0;i < data_split.Length; i++)
            {
                

                float temp = float.Parse(data_split[1], CultureInfo.InvariantCulture.NumberFormat);
                int_velocity[i] = (int)temp ;
            }
            g.DrawLine(cursorPen, new Point(x, y), new Point(x + int_velocity[0], y + int_velocity[1]));

            x += int_velocity[0];
            y += int_velocity[1];

        }

        private void blackBox_Click(object sender, EventArgs e)
        {
            PictureBox color = (PictureBox)sender;
            cursorPen.Color = color.BackColor;
        }
    }
}
