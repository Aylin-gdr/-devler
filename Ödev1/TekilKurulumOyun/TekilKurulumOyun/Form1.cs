using System;
using System.Collections.Generic; // Gerekli: Liste için
using System.Drawing;
using System.Windows.Forms;

namespace TekilKurulumOyun
{
    public partial class Form1 : Form
    {
        // Oyun Nesneleri
        private Player oyuncuKalbi;
        private List<EnemyMermi> mermiler = new List<EnemyMermi>();

        // Klavye kontrol bayraklarý
        private bool yukari, asagi, sol, sag;

        public Form1()
        {
            InitializeComponent();

            // --- 1. Ödev Mekanizmasý: Tekil Kurulum Kontrolü ---
            // (Eðer daha önce test ettiysen Registry'den silmeyi unutma!)
            // CheckInstallation(); // Ödevi teslim ederken bu satýrý aç!

            OyunHazirla();
        }

        private void OyunHazirla()
        {
            this.BackColor = Color.Black; // Ekraný siyah yapalým
            this.DoubleBuffered = true;   // Ekranýn titremesini engeller (Önemli!)

            // OOP Nesnelerini Oluþtur
            oyuncuKalbi = new Player(this);
            oyuncuKalbi.Sprite.BringToFront(); // Kalbi en öne getir

            for (int i = 0; i < 5; i++)
            {
                var mermi = new EnemyMermi(this);
                mermi.Sprite.BringToFront(); // Mermileri en öne getir
                mermiler.Add(mermi);
            }

            timer1.Start(); // Timer'ýn çalýþtýðýndan emin olalým
        }
        // --- Klavye Tuþuna Basýlýnca ---
        protected override void OnKeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up) yukari = true;
            if (e.KeyCode == Keys.Down) asagi = true;
            if (e.KeyCode == Keys.Left) sol = true;
            if (e.KeyCode == Keys.Right) sag = true;
        }

        // --- Klavye Tuþu Býrakýlýnca ---
        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Up) yukari = false;
            if (e.KeyCode == Keys.Down) asagi = false;
            if (e.KeyCode == Keys.Left) sol = false;
            if (e.KeyCode == Keys.Right) sag = false;
        }

        // --- Oyun Döngüsü (Her 20ms'de bir çalýþýr) ---
        private void timer1_Tick(object sender, EventArgs e)
        {
            // 1. Oyuncuyu Hareket Ettir
            int dx = 0, dy = 0;
            if (yukari) dy = -1;
            if (asagi) dy = 1;
            if (sol) dx = -1;
            if (sag) dx = 1;

            oyuncuKalbi.HareketEt(dx, dy, this.ClientSize);

            // 2. Tüm Mermileri Guncelle (OOP Çok Biçimlilik)
            foreach (var mermi in mermiler)
            {
                mermi.Guncelle();

                // Çarpýþma Kontrolü
                if (oyuncuKalbi.Sprite.Bounds.IntersectsWith(mermi.Sprite.Bounds))
                {
                    timer1.Stop();
                    MessageBox.Show("Oyun Bitti! Yakalandýn.", "GG");
                    Application.Exit();
                }
            }
        }
    }
}