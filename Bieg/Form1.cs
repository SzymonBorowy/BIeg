using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;


namespace Bieg
{
    public partial class Form1 : Form
    {
        bool jump, down;
        int G = 18, force, wynik=0;
        double punkty = 0;
        List<PictureBox> listapicturebox = new List<PictureBox>();
        enum wielkosc
        {
            maly = 35,
            sredni = 40,
            duzy = 45
        }

        enum wysokosc
        {
            zero = 0,
            jeden = 12,
            dwa = 20,
            trzy = 45,
            cztery = 80
        }

        enum odleglosc
        {
            gr = 375,
            zero = 500,
            jeden = 750,
            dwa = 1000,
            trzy = 1250,
            cztery = 1500
        }
        public Form1()
        {

            InitializeComponent();
            timer1.Stop();
            
        }
        #region LOSOWANIE
        private int losujWielkosc()
        {
            Random rand = new Random();
            switch(rand.Next(3))
            {
                case 0:
                    return (int)wielkosc.maly;
                case 1:
                    return (int)wielkosc.sredni;
                case 2:
                    return (int)wielkosc.duzy;

            }
            return (int)wielkosc.maly;

        }

        private int losujWysokosc()
        {
            Random rand = new Random();
            switch (rand.Next(7))
            {
                case 0:
                    return (int)wysokosc.zero;
                case 1:
                    return (int)wysokosc.jeden;
                case 2:
                    return (int)wysokosc.dwa;
                case 3:
                    return (int)wysokosc.trzy;
                case 4:
                    return (int)wysokosc.cztery;
                case 5:
                    return (int)wysokosc.zero;
                case 6:
                    return (int)wysokosc.zero;

            }
            return (int)wysokosc.zero;

        }

        private int losujOdleglosc()
        {
            Random rand = new Random();
            switch (rand.Next(6))
            {
                case 0:
                    return (int)odleglosc.zero;
                case 1:
                    return (int)odleglosc.jeden;
                case 2:
                    return (int)odleglosc.dwa;
                case 3:
                    if(predkosc > 14)
                        return (int)odleglosc.dwa;
                    else
                        return (int)odleglosc.trzy;
                case 4:
                    if (predkosc > 14)
                        return (int)odleglosc.jeden;
                    else
                        return (int)odleglosc.cztery;
                case 5:
                    if (predkosc > 11)
                        return (int)odleglosc.zero;
                    else
                        return (int)odleglosc.gr;

            }
            return (int)odleglosc.zero;

        }

        private Color losujKolor()
        {
            Color kolor;
            Random r = new Random();
            int red = r.Next(0, 256);
            int green = r.Next(0, 256);
            int blue = r.Next(0, 256);
            kolor = Color.FromArgb(red, green, blue);

            return kolor;
        }
        #endregion

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawLine(new Pen(Color.DeepSkyBlue, 5), 0, panel1.Height, panel1.Width, panel1.Height);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();

            if(jump != true)
            {
                if(e.KeyCode == Keys.Space || e.KeyCode == Keys.Up)
                {
                    jump = true;
                    force = G;
                }
            }

            if (down != true)
            {
                if (e.KeyCode == Keys.Down)
                { 
                    down = true;
                }
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Down)
            down = false;
        }
        int tick = 0;
        private int predkosc = 5;
        private int co_ile = 200, licznik = 3;

        private void label2_Click(object sender, EventArgs e)
        {
            label2.Enabled = false;
            pobierzZConfiga();
            timer1.Start();
            timer1.Tick += timer1_Tick;
            for (int i = 0; i < 10; i++)
            {
                PictureBox pic = new PictureBox();
                pic.Name = i.ToString();
                
                pic.Height = losujWielkosc();
                pic.Width = losujWielkosc();
                pic.Left = 600 + (1000 * i);
                pic.Top = panel1.Height - pic.Height - losujWysokosc();
                if (pic.Top != panel1.Height - pic.Height)
                    pic.Image = Image.FromFile(@"C:\Users\Szymon\Desktop\Unity obrazy\postac.png");
                else
                    pic.BackColor = losujKolor();
                    pic.BackColor = losujKolor();
                    pic.BackColor = losujKolor();
                pic.Visible = true;
                listapicturebox.Add(pic);
                panel1.Controls.Add(pic);
                System.Threading.Thread.Sleep(10);

            }
        }

        

        private void timer1_Tick(object sender, EventArgs e)
        {
            tick++;
            //timer1.Stop();
            if (tick % 10 == 0)
            {
                punkty += predkosc/5;
                //label1.Text = Convert.ToInt32(punkty).ToString() + "\n" + predkosc.ToString() + "\n" + co_ile.ToString() + "\n" + (panel1.Height - postac.Top).ToString();
                label1.Text = "Punkty: "+ Convert.ToInt32(punkty).ToString() + "\nPredkosc: " + predkosc.ToString() + "\nHigh Score: " + wynik.ToString();
                label1.Refresh();
                if (punkty % co_ile == 0)
                {
                    predkosc += 3;
                    co_ile = co_ile + 100*licznik;
                    licznik++;
                }
            }
            if (jump == true)
            {
                postac.Top -= force;
                force--;
            }
            if (down == true)
            {
                postac.Height = 15;
            }
            else
            {
                postac.Height = 25;
            }


            if (postac.Top + postac.Height >= panel1.Height)
            {
                postac.Top = panel1.Height - postac.Height;
                jump = false;
            }
            else
            {
                postac.Top += 4;
            }

            for (int i = 0; i < 10; i++)
            {
                listapicturebox[i].Left -= predkosc;
                if (listapicturebox[i].Left < 700)
                {
                    if(kolizja(listapicturebox[i]) == true)
                    {
                        timer1.Tick -= timer1_Tick;

                        timer1.Stop();
                        zapiszDoConfiga();
                        MessageBox.Show("Koniec gry !");
                        label2.Enabled = true;
                        foreach (var c in listapicturebox)
                        {
                            panel1.Controls.Remove(c);
                        }
                        listapicturebox.Clear();
                        punkty = 0;
                        tick = 0;
                        predkosc = 5;
                        co_ile = 200;
                        licznik = 3;
                        jump = false;
                        down = false;


                        break;
                    }
                           
                }
                if (listapicturebox[i].Left < -20)
                {
                    listapicturebox[i].Left = listapicturebox[listapicturebox.Count() -1].Left + losujOdleglosc();
                    listapicturebox[i].Height = losujWielkosc();
                    System.Threading.Thread.Sleep(1);
                    listapicturebox[i].Width = losujWielkosc();
                    listapicturebox[i].Top = panel1.Height - listapicturebox[i].Height - losujWysokosc();
                    listapicturebox[i].BackColor = losujKolor();
                    listapicturebox[i].Visible = true;
                    var pom = listapicturebox[i];
                    listapicturebox.Remove(listapicturebox[i]);
                    listapicturebox.Add(pom);
                    panel1.Controls.Add(pom);
                    
                }
                
            }
        }

        private void pobierzZConfiga()
        {
            wynik = Properties.Settings.Default.Wynik;
        }
        private void zapiszDoConfiga()
        {
            if(wynik >= punkty)
                Properties.Settings.Default.Wynik = wynik;
            else if(wynik < punkty)
                Properties.Settings.Default.Wynik = (int)punkty;
            
            Properties.Settings.Default.Save();
        }

        private bool kolizja(PictureBox pic )
        {
            if (postac.Left + postac.Width >= pic.Left  && postac.Left + postac.Width <= pic.Left + pic.Width && postac.Top + postac.Height >= pic.Top && postac.Top <= pic.Top + pic.Height)
            {
                return true;
            }
            return false;
        }
    }
}
