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
using Microsoft.VisualBasic;

namespace bt
{
    public partial class Form1 : Form
    {
        private static String[] sub = new String[53];   //存字幕
        private static int label_c=0; //計算label的點擊次數
        private static int pgbar=100,atk=0,level=10;    //依序時間、攻擊、層級
        private static int[] atk_r = new int[5];        //點擊排名
        private static int[] level_r = new int[5];      //層級
        private static String[] name = new String[5];   //名字
        private static int[] r = new int[5];    //名次

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            pictureBox6.Visible = false;
            pictureBox1.Visible = false;
            pictureBox2.Visible = false;
            pictureBox3.Visible = false;
            pictureBox4.Visible = false;
            progressBar1.Visible = false;
            subtitle(); //載入字幕
            label1.Text = sub[0] + "\n ";
        }

        private void subtitle()
        {
            StreamReader sr = new StreamReader(@"subtitle.txt", System.Text.Encoding.Default);
            int i = 0;
            while (!sr.EndOfStream)
            {               // 每次讀取一行，直到檔尾
                string line = sr.ReadLine();
                sub[i] = line;            // 讀取文字到 line 變數
                i++;
            }
            sr.Close();
        }

        //設定字幕以及字幕劇情，詳見"字幕說明文件.docx"
        private void label1_Click(object sender, EventArgs e)
        {
            if (label_c != 11 && label_c != 25 && label_c != 30 && label_c != 34 && label_c != 39 && label_c != 52)
                label_c++;      //這裡是有特殊劇情時Label不能按
            label1.Text = sub[label_c] + "\n ";
            if(label_c==2)
                pictureBox2.Visible = true; //開始解鎖
            if (label_c == 15)
                pictureBox6.Enabled = true;     //怪物可互動
            if (label_c == 27)
            {
                pictureBox1.Visible = true;     //各種按鍵解鎖
                pictureBox3.Visible = true;
                pictureBox4.Visible = true;
            }
            if (label_c == 30)
                pictureBox2.Enabled = true;     //以下參見"字幕說明文件.docx"
            if (label_c == 34)
                pictureBox2.Enabled = true;
            if (label_c == 36)
                pictureBox6.Image = Image.FromFile("npc\\" + 46 + ".jpg");
            if (label_c == 39)
                pictureBox2.Enabled = true;
            if (label_c == 41)
            {
                pictureBox5.Visible = true;
                pictureBox5.Image = Image.FromFile("npc\\48.jpg");
            }
            if (label_c == 43)
                pictureBox5.Image = Image.FromFile("npc\\49.jpg");
            if (label_c == 45)
                pictureBox5.Image = Image.FromFile("npc\\48.jpg");
            if (label_c == 48)
            {
                pictureBox6.Image = Image.FromFile("npc\\46.jpg");
                this.BackgroundImage = Image.FromFile("npc\\50.jpg");
            }
            if (label_c == 49)
                pictureBox5.Image = Image.FromFile("npc\\49.jpg");
            if (label_c == 52)
            {
                developers_list dl = new developers_list();
                dl.Show();
            }

        }
        //開始鍵
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            label2.Text = "Level:" + level;     //層級設定
            pgbar = 100;    //計量條設定
            atk = 0;    //次數歸0
            progressBar1.Value = pgbar;
            //教學用
            if (level == 10)
            {
                label1.Text = sub[12] + "\n ";  //這是當第一次二下開始鍵的時候，字幕直接跳到12
                label_c = 13;
                label1.Enabled = true;  //以下所有東西變得可互動還有設置
                pictureBox6.Visible = true;
                pictureBox6.Image = Image.FromFile("npc\\10.jpg");
                progressBar1.Visible = true;
            }
            else    //正常遊戲情況下
            {
                pictureBox6.Enabled = true;     //怪物
                pictureBox6.Image = Image.FromFile("npc\\" + level + ".jpg");   //設定怪物圖
            }
        }

        //monster
        private void pictureBox6_Click(object sender, EventArgs e)
        {
            timer1.Enabled = true;  //開始計時
            pictureBox2.Enabled = false;    //start
            pictureBox5.Visible = false;     //npc
            atk++;
            atk++;
            label1.Text = atk.ToString();
        }

        //計時
        private void timer1_Tick(object sender, EventArgs e)
        {
            pgbar -= 1;
            if (pgbar >= 0)
                progressBar1.Value = pgbar;
            else
            {
                timer1.Enabled = false;
                pictureBox6.Enabled = false;
                pictureBox2.Enabled = true;
                judge();    //結果交由判斷
            }

        }

        private void judge()
        {
            if (level == 10)    //新手教學時
            {
                if (atk >= level)
                {
                    label1.Enabled = true;  
                    pictureBox5.Visible = true; //NPC出現
                    label_c = 26;   //字幕直接跳到26
                    label1.Text = sub[label_c] + "\n ";
                    pictureBox2.Enabled = false;    //還未教學完所以開始先不互動
                    level ++;
                    rank();
                }
                else
                {
                    label1.Text = "開始ボタンを押したらもう一度チュートリアルします";
                    pictureBox5.Visible = true; //重新教學
                    label1.Enabled = false;
                }
            }
            else if (level < 39)    //在39關前才有跳級功能
            {
                if (atk >= (level + 5))
                {
                    pictureBox5.Visible = true;
                    label1.Text = "おめでとうございます！あなたは強すぎるので，報酬としてレベルスキップあげよう！\n ";
                    level += 5;
                    rank();
                }
                else if (atk > level)
                {
                    pictureBox5.Visible = true;
                    label1.Text = "おめでとうございます！次のレベル進みましょう！\n ";
                    level++;
                    rank();
                }
                else
                {
                    pictureBox5.Visible = true;
                    label1.Text = "あら、失敗しましたね。もう一度やり直しましょう。";
                    rank();
                }
            }
            else if (level < 44)    //同上
            {
                if (atk > level)
                {
                    pictureBox5.Visible = true;
                    label1.Text = "おめでとうございます！次のレベル進みましょう！\n ";
                    level++;
                    rank();
                }
                else
                {
                    pictureBox5.Visible = true;
                    label1.Text = "あら、失敗しましたね。もう一度やり直しましょう。";
                    rank();
                }
            }
            else
            {
                hurdle();   //44和45是特別關卡
            }

        }

        //特別關卡設計
        private void hurdle()
        {
            if (level == 44)    //44關設計
            {
                
                if (atk > level)
                {
                    pictureBox5.Visible = true;
                    label_c = 31;   //設定字幕
                    label1.Text = sub[label_c] + "\n ";
                    pictureBox2.Enabled = false;    //遇到劇情開始先不互動
                    level++;
                    rank();
                }
                else
                {
                    pictureBox5.Visible = true;
                    label1.Text = "あら、失敗しましたね。もう一度やり直しましょう。";
                    rank();
                }
            }
            else if (level == 45)
            {
                if (atk > level)
                {
                    label_c = 35;
                    label1.Text = sub[label_c] + "\n "; //設定字幕
                    pictureBox2.Enabled = false;    //遇到劇情開始先不互動
                    level++;
                    rank();
                }
                else
                {
                    pictureBox5.Visible = true;
                    label1.Text = "あら、失敗しましたね。もう一度やり直しましょう。";
                    rank();
                }
            }
            else if (level == 46)
            {
                if (atk > 50)
                {
                    label_c = 40;   //設定字幕
                    label1.Text = sub[label_c] + "\n ";
                    pictureBox2.Enabled = false;    //遇到劇情開始先不互動
                    pictureBox5.Visible = false;    //NPC圖先不出現
                    pictureBox6.Image = Image.FromFile("npc\\47.jpg");  //怪物圖載入圖47
                    rank();
                }
                else
                {
                    label1.Text = "バイバイ(@^^)/~~~";
                    rank();
                }
            }
        }

        private void rank()
        {
            StreamReader sr = new StreamReader(@"rank.txt", System.Text.Encoding.Default);
            int i = 0;
            while (!sr.EndOfStream)
            {               // 每次讀取一行，直到檔尾
                string line = sr.ReadLine();
                string[] sp = line.Split(' ');
                r[i] = int.Parse(sp[0]);
                name[i] = sp[1];
                level_r[i] = int.Parse(sp[2]);
                atk_r[i] = int.Parse(sp[3]);
                i++;
            }
            sr.Close();

            for (int j = 0; j < atk_r.Length; j++)
            {
                
                if (atk > atk_r[j]) 
                {
                    for (int k = atk_r.Length - 1; k > j; k--)     //如果遇到大於某一名次的，則後面名次後退一格
                    {
                        name[k] = name[k - 1];
                        level_r[k] = level_r[k - 1];
                        atk_r[k] = atk_r[k - 1];
                    }
                    name[j] = Microsoft.VisualBasic.Interaction.InputBox("おめでとうございます！スコアがランキングリストに追加されました。\n名前を入力してください", "ランキング登録", string.Empty, -1, -1);
                    level_r[j] = level;
                    atk_r[j] = atk;
                    break;
                }
            }
            //更新排名
            StreamWriter sw = new StreamWriter(@"rank.txt");
            for (int j = 0; j < r.Length; j++)
            {
                sw.WriteLine(r[j] + " " + name[j] + " " + level_r[j] + " " + atk_r[j]);
            }
            sw.Close();


        }
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            timer1.Stop();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            explain ex = new explain();
            ex.Show(this);
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            rank_tabel rt = new rank_tabel();
            rt.Show(this);
        }

        
    }
}
