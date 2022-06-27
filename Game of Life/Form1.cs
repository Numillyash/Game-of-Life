using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Game_of_Life
{
    public partial class Form1 : Form
    {
        System.Windows.Forms.Timer timer1 = new System.Windows.Forms.Timer();

        private readonly Graphics _graphics;
        private Bitmap _bitmap;
        private World world = new World();
        /// <summary>
        /// Инициализация окна
        /// </summary>
        public Form1()
        {
            InitializeComponent();
            _bitmap = new Bitmap(1920, 1080);
            _graphics = Graphics.FromImage(_bitmap);
            pictureBox1.Image = _bitmap;
            _graphics.Clear(Color.White);
            world.Random_Generate();
            paintka();
            InitializeTimer(24);
        }
        private void InitializeTimer(int intervals)
        {
            // Run this procedure in an appropriate event.  
            timer1.Interval = intervals;
            timer1.Enabled = true;
            // Hook up timer's tick event handler.  
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
        }
        private void timer1_Tick(object sender, System.EventArgs e)
        {
            paintka();
            //timer1.Start();
        }

        public const int width = 826;
        public const int length = 1176;

        public void paintka()
        {
            _graphics.Clear(Color.White);
            world.Generate();
            SolidBrush sb = new SolidBrush(Color.Black);
            for (int x = 0; x < length; x+=2)
            {
                for (int y = 0; y < width; y+=2)
                {
                    if(world.map[x/2,y/2] == 1)
                    {
                        _graphics.FillEllipse(sb, new Rectangle(x,y,2,2));
                    }
                }
            }
            Refresh();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }

    public class World
    {
        Random random = new Random();
        public const int width = 413;
        public const int length = 588;

        public byte[,] map = new byte[length, width];
        public byte[,] map_new = new byte[length, width];

        public void Random_Generate()
        {
            for (int x = 0; x < length; x++)
            {
                for (int y = 0; y < width; y++)
                {
                    map[x, y] = (byte)random.Next(0, 2);
                    //Console.Write(map[x, y]);
                }
                //Console.WriteLine();
            }
        }

        public void Generate()
        {
            for (int x = 0; x < length; x++)
            {
                for (int y = 0; y < width; y++)
                {
                    Check(new Point(x, y));
                }
            }
            map = map_new;
        }

        public void Check(Point point)
        {
            if (Sum(point) == 3 && map[point.X, point.Y] == 0)
            {
                map_new[point.X, point.Y] = 1;
            }
            else if ((Sum(point) == 3 || Sum(point) == 2) && map[point.X, point.Y] == 1)
            {
                map_new[point.X, point.Y] = 1;
            }
            else
                map_new[point.X, point.Y] = 0;
        }

        public int Sum(Point point)
        {
            int sum = 0;
            if (point.X == 0)
            {
                if (point.Y == 0)
                {
                    sum = map[0, 1] + map[1, 1] + map[1, 0];
                }
                else if (point.Y == width - 1)
                {
                    sum = map[0, width - 2] + map[1, width - 2] + map[1, width - 1];
                }
                else
                {
                    sum = map[0, point.Y - 1] + map[1, point.Y - 1] + map[1, point.Y] + map[1, point.Y + 1] + map[0, point.Y + 1];
                }
            }
            else if (point.X == length - 1)
            {
                if (point.Y == 0)
                {
                    sum = map[length - 1, 1] + map[length - 2, 1] + map[length - 2, 0];
                }
                else if (point.Y == width - 1)
                {
                    sum = map[length - 1, width - 2] + map[length - 2, width - 2] + map[length - 2, width - 1];
                }
                else
                {
                    sum = map[length - 1, point.Y - 1] + map[length - 2, point.Y - 1] + map[length - 2, point.Y] + map[length - 2, point.Y + 1] + map[length - 1, point.Y + 1];
                }
            }
            else if (point.Y == 0)
            {
                sum = map[point.X - 1, 0] + map[point.X - 1, 1] + map[point.X, 1] + map[point.X + 1, 1] + map[point.X + 1, 0];
            }
            else if (point.Y == width - 1)
            {
                sum = map[point.X - 1, width - 1] + map[point.X - 1, width - 2] + map[point.X, width - 2] + map[point.X + 1, width - 2] + map[point.X + 1, width - 1];
            }
            else
                sum = map[point.X - 1, point.Y - 1] + map[point.X - 1, point.Y] + map[point.X - 1, point.Y + 1]
                    + map[point.X, point.Y - 1] + map[point.X, point.Y + 1]
                    + map[point.X + 1, point.Y - 1] + map[point.X + 1, point.Y] + map[point.X + 1, point.Y + 1];
            return sum;
        }
    }
}
