using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OfficeOpenXml;

namespace Benzinlik_Uygulamasi
{
	class Program
	{
		static string kullanicilarPath = @"C:\Users\djpus\Desktop\Techcareer\Ders Proje\Benzinlik_Uygulamasi\Kullanicilar.xlsm";
		static string marketPath = @"C:\Users\djpus\Desktop\Techcareer\Ders Proje\Benzinlik_Uygulamasi\Market.xlsm";


		static void Main(string[] args)
		{
			AnaMenu();
		}

		static void AnaMenu()
		{
			Console.WriteLine("\nHüsOil Petrollerine Hoş Geldiniz. Market ve Kasa İşlemleri İçin Sisteme Giriş Yapınız!");

			while (true)
			{
				Console.WriteLine("\nGiriş Yapmak İçin => 1'e Basınız\nKaydolmak İçin => 2'ye Basınız\nSistemden Çıkış İçin => 0'a Basınız");
				Console.Write("\n");
				int secim = int.Parse(Console.ReadLine());

				if (secim == 1)
				{
					GirisYap();
				}
				else if (secim == 2)
				{
					Kaydol();
				}
				else if (secim == 0)
				{
					Console.WriteLine("\nSistemden çıkış yapılıyor...");
					Thread.Sleep(2000); // 2 saniye (2000 milisaniye) bekletir
					Environment.Exit(0);
				}
			}
		}

		static void GirisYap()
		{
			Console.Write("Kullanıcı Adı: ");
			string kullaniciAdi = Console.ReadLine().ToLower();
			Console.Write("Şifre: ");
			string sifre = Console.ReadLine().ToLower();

			bool girisBasarili = false;
			string girisYapanAdSoyad = "";
			string rol = "";

			ExcelPackage.LicenseContext = LicenseContext.NonCommercial;//Geliştirme amacıyla kullandığım için lisans matirisini ayarladım

			using (var package = new ExcelPackage(new FileInfo(kullanicilarPath)))
			{
				var worksheet = package.Workbook.Worksheets[0];
				int rowCount = worksheet.Dimension.Rows;

				for (int row = 2; row <= rowCount; row++)
				{
					if (worksheet.Cells[row, 1].Text == kullaniciAdi && worksheet.Cells[row, 2].Text == sifre)
					{
						girisYapanAdSoyad = worksheet.Cells[row, 3].Text;
						rol = worksheet.Cells[row, 4].Text;
						Console.WriteLine("\nGiriş başarılı ana menüye yönlendiriliyorsunz...");
						Thread.Sleep(1000); // 1 saniye (1000 milisaniye) bekletir
						girisBasarili = true;
						break;
					}
				}
			}
			if (girisBasarili && rol == "patron")
			{
				Console.WriteLine($"\nPatronların Kralı {girisYapanAdSoyad} Hoş Geldin!");
				Console.WriteLine("\nToplam Özeti Görmek İçin => 1'e,\nAna Menüye Dönmek İçin => 0'a Basman Yeterli");

				int secim = int.Parse(Console.ReadLine());
				if (secim == 1)
				{
					ExcelPackage.LicenseContext = LicenseContext.NonCommercial;//Geliştirme amacıyla kullandığım için lisans matirisini ayarladım
					using (var package = new ExcelPackage(new FileInfo(marketPath)))
					{
						var worksheet = package.Workbook.Worksheets[0];
						string[] urunler = { "Ekmek", "Süt (1 litre)", "Yumurta (10'lu)", "Domates (1 kg)", "Beyaz peynir (500 gr)", "Elma (1 kg)", "Tavuk göğsü (1 kg)", "Zeytinyağı (1 litre)", "Makarna (500 gr)", "Çikolata (100 gr)" };
						string[] satilmaSutunlar = { "G2", "G3", "G4", "G5", "G6", "G7", "G8", "G9", "G10", "G11" };
						string[] fiyatSutunlar = { "H2", "H3", "H4", "H5", "H6", "H7", "H8", "H9", "H10", "H11" };


						Console.WriteLine("Satılan Ürün Özet Tablosu");
						Console.WriteLine("\n" + "Ürünler".PadRight(20) + "Satılma Adet(Toplam)".PadRight(30) + "Toplam Fiyat".PadLeft(15));
						Console.WriteLine(new string('-', 65)); // 65 tane '-' karakteri ile bir ayrac ekledik

						for (int i = 0; i < urunler.Length; i++)
						{
							string miktar = worksheet.Cells[satilmaSutunlar[i]].Text;
							string deger = worksheet.Cells[fiyatSutunlar[i]].Text.Replace("₺", "").Trim(); // ₺'yi kaldır
							Console.WriteLine($"{urunler[i].PadRight(25)}{miktar.PadRight(25)}{deger.PadLeft(13)} TL");
						}

						// Formülleri hesaplat
						worksheet.Calculate();
						string toplamMiktar = worksheet.Cells["G13"].Value.ToString();
						string toplamFiyat = worksheet.Cells["H13"].Value.ToString();

						Console.WriteLine(new string('-', 65)); // 65 tane '-' karakteri ile bir ayrac ekledik
						Console.WriteLine($"TOPLAM:".PadRight(25) + $"{toplamMiktar.PadRight(2)}Adet {toplamFiyat.PadLeft(31)} TL\n");


						Console.WriteLine("Satılan Benzin Özet Tablosu");
						Console.WriteLine("\n" + "Benzin Türleri".PadRight(20) + "Satılma Litre(Toplam)".PadRight(30) + "Toplam Fiyat".PadLeft(15));
						Console.WriteLine(new string('-', 65)); // 65 tane '-' karakteri ile bir ayrac ekledik
						string[] benzinTurleri = { "Kurşunsuz Benzin", "Motorin", "Gazyağı", "Fuel Oil" };
						string[] satilmaLitreSutunlar = { "N2", "N3", "N4", "N5" };
						string[] benzinFiyatSutunlar = { "O2", "O3", "O4", "O5" };

						for (int i = 0; i < benzinTurleri.Length; i++)
						{
							string miktar = worksheet.Cells[satilmaSutunlar[i]].Text;
							string deger = worksheet.Cells[fiyatSutunlar[i]].Text.Replace("₺", "").Trim(); // ₺'yi kaldır
							Console.WriteLine($"{urunler[i].PadRight(25)}{miktar.PadRight(25)}{deger.PadLeft(13)} TL");
						}

						// Formülleri hesaplat
						worksheet.Calculate();
						string toplamLitre = worksheet.Cells["N7"].Value.ToString();
						string toplamBenzinFiyat = worksheet.Cells["O7"].Value.ToString();

						Console.WriteLine(new string('-', 65)); // 65 tane '-' karakteri ile bir ayrac ekledik
						Console.WriteLine($"TOPLAM:".PadRight(25) + $"{toplamLitre.PadRight(3)}Litre {toplamBenzinFiyat.PadLeft(31)} TL\n");

						// Formülleri hesaplat
						worksheet.Calculate();

						string genelToplam = worksheet.Cells["N12"].Value.ToString();

						Console.WriteLine(new string('-', 65)); // 65 tane '-' karakteri ile bir ayrac ekledik
						Console.WriteLine($"GENEL TOPLAM:".PadRight(25) + $"{genelToplam.PadLeft(38)} TL\n");
					}

					Console.WriteLine("\nSistemden çıkış yapmak için 0'a basınız!");

					secim = int.Parse(Console.ReadLine());
					if (secim == 1)
					{
						KullaniciIslemleri();
					}
					else if (secim == 0)
					{
						Console.WriteLine("\nSistemden çıkış yapılıyor...");
						Thread.Sleep(2000);
						Environment.Exit(0);
					}
				}
			}


			else if (girisBasarili)
			{
				KullaniciIslemleri();
			}
			else
			{
				Console.WriteLine("Kullanıcı adı veya şifre hatalı tekrar deneyiniz!");
			}
		}

		static void Kaydol()
		{
			Console.Write("Kullanıcı Adı: ");
			string kullaniciAdi = Console.ReadLine().ToLower();
			Console.Write("Şifre: ");
			string sifre = Console.ReadLine().ToLower();
			Console.Write("Ad-Soyad: ");
			string adSoyad = Console.ReadLine();
			string rol;

			while (true)
			{
				Console.Write("Rolü (patron/pompacı): ");
				rol = Console.ReadLine().ToLower();

				if (rol == "patron")
				{
					Console.Write("\nPatron olduğunuzu doğrulamak için onaylama şifresini giriniz: ");
					string onaySifresi = Console.ReadLine();
					if (onaySifresi != "0000")
					{
						Console.WriteLine("\nOnaylama şifresi hatalı. Lütfen tekrar deneyin.");
						continue;
					}
				}
				else if (rol != "pompacı")
				{
					Console.WriteLine("\nLütfen geçerli bir rol (patron/pompacı) girin.");
					continue;
				}
				break;
			}

			ExcelPackage.LicenseContext = LicenseContext.NonCommercial;//Geliştirme amacıyla kullandığım için lisans matirisini ayarladım
			using (var package = new ExcelPackage(new FileInfo(kullanicilarPath)))
			{
				var worksheet = package.Workbook.Worksheets[0];
				int rowCount = worksheet.Dimension?.Rows ?? 0;

				worksheet.Cells[rowCount + 1, 1].Value = kullaniciAdi;
				worksheet.Cells[rowCount + 1, 2].Value = sifre;
				worksheet.Cells[rowCount + 1, 3].Value = adSoyad;
				worksheet.Cells[rowCount + 1, 4].Value = rol;

				package.Save();
			}

			Console.WriteLine("\nBaşarıyla Kaydoldunuz! Ana menüye yönlendiriliyorsunz...");
			Thread.Sleep(1000); // 1 saniye (1000 milisaniye) bekletir
			AnaMenu();
		}

		static void KullaniciIslemleri()
		{

			while (true)
			{
				Console.WriteLine("\nBenzin almak için 1'e, Marketten alışveriş yapmak için 2'ye, Sistemden çıkış için 0'a basınız!");

				int secim = int.Parse(Console.ReadLine());
				if (secim == 1)
				{
					KasaIslemleri();
				}
				else if (secim == 2)
				{
					MarketIslemleri();
				}
				else if (secim == 0)
				{
					Console.WriteLine("\nSistemden çıkış yapılıyor...");
					Thread.Sleep(2000);
					Environment.Exit(0);
				}
			}
		}

		static void KasaIslemleri()
		{
			double[] benzinFiyatlari = { 39.63, 41.09, 36.36, 25.26 };  // Benzin fiyatları

			Console.WriteLine("Almak istediğiniz benzin türünü seçiniz!");
			Console.WriteLine("Tarih:" + "" +DateTime.Now.ToString());
			Console.WriteLine("\n1) Kurşunsuz Benzin ==> 39,63 TL\n2) Motorin ==> 41,09 TL\n3) Gazyağı ==> 36,36 TL\n4) Fuel Oil ==> 25,26 TL");

			int secim = int.Parse(Console.ReadLine());
			string sütunAdı = "";

			switch (secim)
			{
				case 1:
					sütunAdı = "N2";
					break;
				case 2:
					sütunAdı = "N3";
					break;
				case 3:
					sütunAdı = "N4";
					break;
				case 4:
					sütunAdı = "N5";
					break;
				default:
					Console.WriteLine("Hatalı seçim!");
					return;
			}

			Console.Write("Kaç litre alacaksınız?: ");
			double litre = double.Parse(Console.ReadLine());

			double toplamFiyat = litre * benzinFiyatlari[secim - 1];  // Seçilen benzin türünün fiyatını litreye çarp


			ExcelPackage.LicenseContext = LicenseContext.NonCommercial;//Geliştirme amacıyla kullandığım için lisans matirisini ayarladım
			using (var package = new ExcelPackage(new FileInfo(marketPath)))
			{
				var worksheet = package.Workbook.Worksheets[0];
				worksheet.Cells[sütunAdı].Value = (double)(worksheet.Cells[sütunAdı].Value) + litre;
				package.Save();
			}

			Console.WriteLine($"Dolum yapılmıştır.Benzin fiyatı toplam {toplamFiyat} TL'dir. Fişinizi almayı unutmayın!");
		}

		static void MarketIslemleri()
		{
			double[] urunFiyatlari = { 7, 20, 40, 20, 50, 15, 95, 150, 10, 8 };  // Ürün fiyatları

			while (true)
			{
				Console.WriteLine("Almak istediğiniz ürünü seçin!");
				Console.WriteLine("\n1) Ekmek ==> 7 TL\n2) Süt (1 litre) ==> 20 TL\n3) Yumurta (10'lu) ==> 40 TL\n4) Domates (1 kg) ==> 20 TL\n5) Beyaz peynir (500 gr) ==> 50 TL\n6) Elma (1 kg) ==> 15 TL\n7) Tavuk göğsü (1 kg) ==> 95 TL\n8) Zeytinyağı (1 litre) ==> 150 TL\n9) Makarna (500 gr) ==> 10 TL\n10) Çikolata (100 gr) ==> 8 TL");

				int secim = int.Parse(Console.ReadLine());
				string sütunAdı = "";

				switch (secim)
				{
					case 1:
						sütunAdı = "G2";
						break;
					case 2:
						sütunAdı = "G3";
						break;
					case 3:
						sütunAdı = "G4";
						break;
					case 4:
						sütunAdı = "G5";
						break;
					case 5:
						sütunAdı = "G6";
						break;
					case 6:
						sütunAdı = "G7";
						break;
					case 7:
						sütunAdı = "G8";
						break;
					case 8:
						sütunAdı = "G9";
						break;
					case 9:
						sütunAdı = "G10";
						break;
					case 10:
						sütunAdı = "G11";
						break;
					default:
						Console.WriteLine("Hatalı seçim!");
						return;
				}

				Console.Write($"Kaç adet alacaksınız?: ");
				int miktar = int.Parse(Console.ReadLine());

				double toplamFiyat = miktar * urunFiyatlari[secim - 1];  // Seçilen ürünün fiyatını miktarla çarp


				using (var package = new ExcelPackage(new FileInfo(marketPath)))
				{
					var worksheet = package.Workbook.Worksheets[0];

					int mevcutMiktar = 0;
					if (int.TryParse(worksheet.Cells[sütunAdı].Text, out mevcutMiktar))
					{
						worksheet.Cells[sütunAdı].Value = mevcutMiktar + miktar;
					}
					else
					{
						worksheet.Cells[sütunAdı].Value = miktar;
					}
					package.Save();
				}

				Console.WriteLine("Ürün sepete eklendi, ödeme yapmak için 1'e basınız. Başka ürün eklemek için herhangi bir tuşa basın.");

				if (Console.ReadLine() == "1")
				{
					Console.WriteLine($"Ürünlerin toplam fiyatı {toplamFiyat} TL'dir. Ürün alımı başarıyla gerçekleşmiştir, fişinizi almayı unutmayın!");

					break;
				}
			}
		}
	}
}

