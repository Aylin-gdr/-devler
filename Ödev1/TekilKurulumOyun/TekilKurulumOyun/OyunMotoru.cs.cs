using System;
using System.Drawing; // Gerekli: Pozisyon ve Renk için
using System.Windows.Forms; // Gerekli: PictureBox için

namespace TekilKurulumOyun
{
    // --- OOP: SOYUTLAMA ---
    public abstract class GameObject
    {
        // Görüntüyü temsil eden PictureBox
        public PictureBox Sprite { get; set; }
        public int Hizi { get; set; }

        public GameObject(Form panel, Color renk, Size boyut, Point baslangicNotkasi, int hiz)
        {
            Hizi = hiz;
            Sprite = new PictureBox
            {
                BackColor = renk,
                Size = boyut,
                Location = baslangicNotkasi
            };
            panel.Controls.Add(Sprite); // Ekrana ekle
        }

        // --- OOP: ÇOK BİÇİMLİLİK (Herkes farklı hareket eder) ---
        public abstract void Guncelle();
    }

    // --- OOP: KALITIM (Oyuncu nesnesi) ---
    public class Player : GameObject
    {
        public Player(Form panel) : base(panel, Color.Red, new Size(20, 20), new Point(390, 290), 5) { }

        public override void Guncelle() { /* Klavye ile hareket Form1'de yapılacak */ }

        // --- OOP: KAPSÜLLEME (Hareket sınırlarını koru) ---
        public void HareketEt(int dx, int dy, Size panelBoyutu)
        {
            int yeniX = Sprite.Left + (dx * Hizi);
            int yeniY = Sprite.Top + (dy * Hizi);

            // Ekranda dışarı çıkmasını engelle
            if (yeniX >= 0 && yeniX <= panelBoyutu.Width - Sprite.Width) Sprite.Left = yeniX;
            if (yeniY >= 0 && yeniY <= panelBoyutu.Height - Sprite.Height) Sprite.Top = yeniY;
        }
    }

    // --- OOP: KALITIM (Düşman/Mermi nesnesi) ---
    public class EnemyMermi : GameObject
    {
        private static Random rng = new Random();
        private int yonX, yonY;
        private Form _panel;

        public EnemyMermi(Form panel) : base(panel, Color.White, new Size(10, 10), Point.Empty, rng.Next(3, 7))
        {
            _panel = panel;
            RandomBaslangic();
        }

        private void RandomBaslangic()
        {
            // Rastgele bir kenardan başla
            Sprite.Location = new Point(rng.Next(_panel.Width), rng.Next(_panel.Height));
            // Rastgele bir yöne git
            yonX = rng.Next(-1, 2);
            yonY = rng.Next(-1, 2);
            if (yonX == 0 && yonY == 0) yonX = 1; // Durmasın
        }

        public override void Guncelle()
        {
            Sprite.Left += yonX * Hizi;
            Sprite.Top += yonY * Hizi;

            // Kenara çarparsa geri dönsün
            if (Sprite.Left <= 0 || Sprite.Left >= _panel.Width - Sprite.Width) yonX *= -1;
            if (Sprite.Top <= 0 || Sprite.Top >= _panel.Height - Sprite.Height) yonY *= -1;
        }
    }
}