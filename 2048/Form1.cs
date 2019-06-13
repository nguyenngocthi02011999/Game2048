﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
namespace _2048
{
    public partial class Form1 : Form
    {
        private int KhoangCach = 5;

        private Random rand = new Random();
        StreamReader doc = new StreamReader("diem2048.txt");

        private int Diem = 0;
        private Label[,] OSo = new Label[4, 4];
        int[,] SoO = new int[4, 4];
        public Form1()
        {
            InitializeComponent();
        }
        private void frm_laod()
        {
            lb_diem.Text = "0";
            lb_kl.Text = doc.ReadToEnd();
            doc.Close();
            if (lb_kl.Text == "")
                lb_kl.Text = "0";

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    OSo[i, j] = new Label();
                    OSo[i, j].Location = new Point(KhoangCach + i * (100 + KhoangCach), KhoangCach + j * (100 + KhoangCach));
                    OSo[i, j].Size = new Size(100, 100);
                    OSo[i, j].TabIndex = i * 4 + j;
                    OSo[i, j].Name = String.Format("lb%d%d", i, j);
                    OSo[i, j].BackColor = Color.FromName("ActiveBorder");
                    OSo[i, j].Font = new Font("Consolas", 18F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
                    OSo[i, j].TextAlign = ContentAlignment.MiddleCenter;
                    groupBox1.Controls.Add(OSo[i, j]);
                }
            }
            LoadO();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            frm_laod();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (SoO[i, j] == 0)
                        OSo[i, j].Text = "";
                    else
                        OSo[i, j].Text = SoO[i, j].ToString();
                    setMauChoO(i, j);
                }
            }
            lb_diem.Text = Diem.ToString();
        }

        bool RanDomViTriHien()
        {
            bool isDo = false;
            List<int> test = new List<int>();
            for (int i = 0; i < 16; i++)
            {
                if (SoO[i / 4, i % 4] == 0)
                {
                    test.Add(i);
                    isDo = true;
                }
            }
            if (test.Count > 0)
            {
                int set = test[rand.Next(0, test.Count - 1)];
                while (SoO[set / 4, set % 4] != 0 && test.Count > 1)
                {
                    test.Remove(set);
                    set = test[rand.Next(0, test.Count - 1)];
                }
                SoO[set / 4, set % 4] = rand.Next(1, 100) > 90 ? 4 : 2;
                Diem += SoO[set / 4, set % 4];
            }
            return isDo;
        }

        bool PhimLen()
        {
            bool isDo = false;
            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 3; y++)
                {
                    for (int y1 = y + 1; y1 < 4; y1++)
                    {
                        if (SoO[x, y1] > 0)
                        {
                            if (SoO[x, y] == 0)
                            {
                                SoO[x, y] = SoO[x, y1];
                                SoO[x, y1] = 0;
                                y--;
                                isDo = true;
                            }
                            else if (SoO[x, y] == SoO[x, y1])
                            {
                                SoO[x, y] *= 2;
                                SoO[x, y1] = 0;
                                isDo = true;
                            }
                            break;
                        }
                    }
                }
            }
            if (isDo)
                RanDomViTriHien();
            return isDo;
        }

        bool PhimXuong()
        {
            bool isDo = false;
            for (int x = 0; x < 4; x++)
            {
                for (int y = 3; y >= 1; y--)
                {
                    for (int y1 = y - 1; y1 >= 0; y1--)
                    {
                        if (SoO[x, y1] > 0)
                        {
                            if (SoO[x, y] == 0)
                            {
                                SoO[x, y] = SoO[x, y1];
                                SoO[x, y1] = 0;
                                y++;
                                isDo = true;
                            }
                            else if (SoO[x, y] == SoO[x, y1])
                            {
                                SoO[x, y] *= 2;
                                SoO[x, y1] = 0;
                                isDo = true;
                            }
                            break;
                        }
                    }
                }
            }
            if (isDo)
                RanDomViTriHien();
            return isDo;
        }

        bool PhimPhai()
        {
            bool isDo = false;
            for (int y = 0; y < 4; y++)
            {
                for (int x = 3; x >= 1; x--)
                {
                    for (int x1 = x - 1; x1 >= 0; x1--)
                    {
                        if (SoO[x1, y] > 0)
                        {
                            if (SoO[x, y] == 0)
                            {
                                SoO[x, y] = SoO[x1, y];
                                SoO[x1, y] = 0;
                                x++;
                                isDo = true;
                            }
                            else if (SoO[x, y] == SoO[x1, y])
                            {
                                SoO[x, y] *= 2;
                                SoO[x1, y] = 0;
                                isDo = true;
                            }
                            break;
                        }
                    }
                }
            }
            if (isDo)
                RanDomViTriHien();
            return isDo;
        }

        bool PhimTrai()
        {
            bool isDo = false;
            for (int y = 0; y < 4; y++)
            {
                for (int x = 0; x < 3; x++)
                {
                    for (int x1 = x + 1; x1 < 4; x1++)
                    {
                        if (SoO[x1, y] > 0)
                        {
                            if (SoO[x, y] == 0)
                            {
                                SoO[x, y] = SoO[x1, y];
                                SoO[x1, y] = 0;
                                x--;
                                isDo = true;
                            }
                            else if (SoO[x, y] == SoO[x1, y])
                            {
                                SoO[x, y] *= 2;
                                SoO[x1, y] = 0;
                                isDo = true;
                            }
                            break;
                        }
                    }
                }
            }
            if (isDo)
                RanDomViTriHien();
            return isDo;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Up)
                PhimLen();
            if (e.KeyData == Keys.Down)
                PhimXuong();
            if (e.KeyData == Keys.Right)
                PhimPhai();
            if (e.KeyData == Keys.Left)
                PhimTrai();
            if (e.KeyData == Keys.F1)
            {
                if (Convert.ToInt32(lb_diem.Text) > Convert.ToInt32(lb_kl.Text))
                {
                    doc.Close();
                    StreamWriter ghi = new StreamWriter("diem2048.txt");
                    ghi.WriteLine(lb_diem.Text);
                    ghi.Flush();
                    ghi.Close();
                    this.Hide();
                    Form1 fr = new Form1();
                    fr.Show();
                }
                else
                {
                    this.Hide();
                    Form1 fr = new Form1();
                    fr.Show();
                }
            }
            this.Refresh();
            if (CheckGameOver())
            {
                if (Convert.ToInt32(lb_diem.Text) > Convert.ToInt32(lb_kl.Text))
                {
                    doc.Close();
                    StreamWriter ghi = new StreamWriter("diem2048.txt");
                    ghi.WriteLine(lb_diem.Text);
                    ghi.Flush();
                    ghi.Close();
                }
                if (MessageBox.Show("Điểm: " + Diem.ToString() + "\n" + " chơi lại?",
                    "Game Over!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    Application.Exit();
                else
                {
                    this.Hide();
                    Form1 fr = new Form1();
                    fr.Show();
                }
            }
        }
        bool CheckGameOver()
        {

            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 4; y++)
                {
                    if (SoO[x, y] == 0 ||
                        (y < 3 && SoO[x, y] == SoO[x, y + 1]) ||
                        (x < 3 && SoO[x, y] == SoO[x + 1, y]))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        void LoadO()
        {
            Diem = 0;
            for (int x = 0; x < 4; x++)
                for (int y = 0; y < 4; y++)
                    SoO[x, y] = 0;
            RanDomViTriHien();
            RanDomViTriHien();
            this.Refresh();
        }

        void setMauChoO(int x, int y)
        {
            switch (SoO[x, y])
            {
                case 0: OSo[x, y].BackColor = Color.FromArgb(220, 220, 220); break;
                case 2: OSo[x, y].BackColor = Color.FromArgb(255, 192, 192); break;
                case 4: OSo[x, y].BackColor = Color.FromArgb(255, 128, 128); break;
                case 8: OSo[x, y].BackColor = Color.FromArgb(255, 224, 192); break;
                case 16: OSo[x, y].BackColor = Color.FromArgb(255, 192, 128); break;
                case 32: OSo[x, y].BackColor = Color.FromArgb(255, 255, 192); break;
                case 64: OSo[x, y].BackColor = Color.FromArgb(255, 255, 128); break;
                case 128: OSo[x, y].BackColor = Color.FromArgb(192, 255, 192); break;
                case 256: OSo[x, y].BackColor = Color.FromArgb(128, 255, 128); break;
                case 512: OSo[x, y].BackColor = Color.FromArgb(192, 255, 255); break;
                case 1024: OSo[x, y].BackColor = Color.FromArgb(128, 255, 255); break;
                case 2048: OSo[x, y].BackColor = Color.FromArgb(192, 192, 255); break;
                case 4096: OSo[x, y].BackColor = Color.FromArgb(128, 128, 255); break;
                case 8192: OSo[x, y].BackColor = Color.FromArgb(255, 192, 255); break;
            }
        }



    }
}
