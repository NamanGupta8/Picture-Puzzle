using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;

namespace Picture_Puzzle
{
    public partial class Form1 : Form
    {
        System.Timers.Timer y;
        int hour, min, sec;
        int noOfMoves = 0;
        Point EP;
        ArrayList images = new ArrayList();
        String src = @"C:\\puzzle.jpg";
        public Form1()
        {
            EP.X = 180;
            EP.Y = 180;
            InitializeComponent();
        }

        private void Form_Load(object sender, EventArgs e)
        {
            y = new System.Timers.Timer();
            y.Interval = 1000;
            y.Elapsed += OnTimeEvent;


        }

        private void OnTimeEvent(object sender, System.Timers.ElapsedEventArgs e)
        {
            Invoke(new Action(() =>
            {
                sec += 1;
                if (sec == 60)
                {
                    sec = 0;
                    min += 1;
                }
                
                textBox1.Text = string.Format("{0}:{1}:{2}", hour.ToString().PadLeft(2, '0'), min.ToString().PadLeft(2, '0'), sec.ToString().PadLeft(2, '0'));
            }));
        }

        private void Click_Start(object sender, EventArgs e)
        {
            y.Start();
            noOfMoves = 0;
            foreach (Button button in panel1.Controls)
                button.Enabled = true;

            Image original = Image.FromFile(src);

            cropImg(original, 270, 270);

            AddImg(images);
        }

        private void AddImg(ArrayList images)
        {
            int i = 0;
            int[] arr = { 0, 1, 2, 3, 4, 5, 6, 7 };

            arr = shuffleIt(arr);

            foreach (Button b in panel1.Controls)
            {
                if (i < arr.Length)
                {
                    b.Image = (Image)images[arr[i]];
                    i++;
                }
            }
        }

        private int[] shuffleIt(int[] arr)
        {
            
            
            Random rand = new Random();
            arr = arr.OrderBy(x => rand.Next()).ToArray();
            return arr;
        }

        private void cropImg(Image orginal, int p1, int p2)
        {
            Bitmap bimp = new Bitmap(p1, p2);

            Graphics graphic = Graphics.FromImage(bimp);

            graphic.DrawImage(orginal, 0, 0, p1, p2);

            graphic.Dispose();

            int movl = 0, movr = 0;

            for (int x = 0; x < 8; x++)
            {
                Bitmap piece = new Bitmap(90, 90);

                for (int i = 0; i < 90; i++)
                    for (int j = 0; j < 90; j++)
                        piece.SetPixel(i, j,
                            bimp.GetPixel(i + movl, j + movr));

                images.Add(piece);

                movl += 90;

                if (movl == 270)
                {
                    movl = 0;
                    movr += 90;
                }
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            noOfMoves++;
            label2.Text = Convert.ToString(noOfMoves);
            MoveButton((Button)sender);
        }

        private void MoveButton(Button button)
        {
            if (((button.Location.X == EP.X - 90 || button.Location.X == EP.X + 90)
                && button.Location.Y == EP.Y)
                || (button.Location.Y == EP.Y - 90 || button.Location.Y == EP.Y + 90)
                && button.Location.X == EP.X)
            {
                Point swap = button.Location;
                button.Location = EP;
                EP = swap;
            }

            if (EP.X == 180 && EP.Y == 180)
                CheckIt();
        }

        private void CheckIt()
        {
            int count = 0, index;
            foreach (Button buttton in panel1.Controls)
            {
                index = (buttton.Location.Y / 90) * 3 + buttton.Location.X / 90;
                if (images[index] == buttton.Image)
                    count++;
            }
            if (count == 8)
            {
                MessageBox.Show("Puzzle Completed!\n" + "Total time taken : " + textBox1.Text);
                y.Stop();
                Application.DoEvents();
            }
            
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
