using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace bt
{
    public partial class rank_tabel : Form
    {
        private String[] atk_r = new String[5];        //點擊排名
        private String[] level_r = new String[5];      //層級
        private String[] name = new String[5];   //名字
        private String[] r = new String[5];    //名次
        private String n="名字", a="點擊次數", rn="排名", l="層級";
        public rank_tabel()
        {
            InitializeComponent();
        }

        private void rank_tabel_Load(object sender, EventArgs e)
        {
            StreamReader sr = new StreamReader(@"rank.txt", System.Text.Encoding.Default);
            int i=0;
            while (!sr.EndOfStream)
            {               // 每次讀取一行，直到檔尾
                string line = sr.ReadLine();
                string[] sp = line.Split(' ');
                r[i] = sp[0];
                name[i] = sp[1];
                level_r[i] = sp[2];
                atk_r[i] = sp[3];
                i++;
            }

            for (int j = 0; j < 5; j++)
            {
                rn += "\n" + r[j];
                n += "\n" + name[j];
                l += "\n" + level_r[j];
                a += "\n" + atk_r[j];    
            }
            label1.Text = rn;
            label2.Text = n;
            label3.Text = l;
            label4.Text = a;
            sr.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            n="名字";
            a="點擊次數";
            rn = "排名";
            l = "層級";
            for (int j = 0; j < 5; j++)
            {
                r[j] = (j+1).ToString();
                name[j] = "name";
                level_r[j] = "0";
                atk_r[j] = "0";
                rn += "\n" + r[j];
                n += "\n" + name[j];
                l += "\n" + level_r[j];
                a += "\n" + atk_r[j];   
            }
            StreamWriter sw = new StreamWriter(@"rank.txt");
            for (int j = 0; j < r.Length; j++)
            {
                sw.WriteLine(r[j] + " " + name[j] + " " + level_r[j] + " " + atk_r[j]);
            }
            sw.Close();
            label1.Text = rn;
            label2.Text = n;
            label3.Text = l;
            label4.Text = a;
        }
    }
}
