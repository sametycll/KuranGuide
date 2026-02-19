using KuranGuide.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace KuranGuide.Infrastructure.Context
{
    public class KuranGuideContextSeed
    {
        // Ana Metot: Program.cs tarafından çağrılır
        public static async Task SeedAsync(KuranGuideDbContext context)
        {
            // 1. Sureleri Yükle
            await SeedSurelerAsync(context);

            // 2. Temaları Yükle
            await SeedTemalarAsync(context);
            await SeedHadislerAsync(context);
            // 3. (YENİ) Otomatik Eşleştirme Yap
            await AutoMapTemalarAsync(context);
        }

        private static async Task SeedHadislerAsync(KuranGuideDbContext context)
        {
            // Veritabanı doluysa işlem yapma
            if (await context.Hadisler.AnyAsync())
            {
                return;
            }

            try
            {
                // 1. JSON Dosyasını Oku
                var jsonData = await File.ReadAllTextAsync("../KuranGuide.Api/Data/hadisler.json");

                // 2. Helper modele çevir (Entity'e direkt çevirmiyoruz çünkü TemaId yok)
                var jsonHadisler = JsonSerializer.Deserialize<List<JsonHadisModel>>(jsonData, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (jsonHadisler != null)
                {
                    // 3. Temaları belleğe çek (Eşleştirme yapmak için)
                    var temalar = await context.Temalar.ToListAsync();
                    var eklenecekHadisler = new List<Hadis>();

                    Console.WriteLine("Hadisler taranıyor ve temalarla eşleştiriliyor...");

                    foreach (var h in jsonHadisler)
                    {
                        // Yeni Hadis Entity'si oluştur
                        var yeniHadis = new Hadis
                        {
                            Metin = h.Metin,
                            Kaynak = h.Kaynak,
                            Aciklama = null // JSON'da yok, boş geçiyoruz
                        };

                        // --- OTOMATİK EŞLEŞTİRME MANTIĞI ---
                        // Hadis metnini küçük harfe çevir
                        var metinLower = h.Metin.ToLower();

                        // Temalar içinde dön
                        foreach (var tema in temalar)
                        {
                            if (string.IsNullOrEmpty(tema.AnahtarKelimeler)) continue;

                            // Anahtar kelimeleri ayır (örn: "namaz, secde")
                            var keywords = tema.AnahtarKelimeler.Split(',')
                                               .Select(k => k.Trim().ToLower())
                                               .Where(k => !string.IsNullOrEmpty(k))
                                               .ToList();

                            // Eşleşme var mı?
                            bool isMatch = keywords.Any(k => metinLower.Contains(k));

                            if (isMatch)
                            {
                                yeniHadis.TemaId = tema.Id;
                                break; // İlk bulduğu temaya bağla ve döngüden çık (Bir hadis bir temaya girsin)
                            }
                        }
                        // -----------------------------------

                        eklenecekHadisler.Add(yeniHadis);
                    }

                    // 4. Toplu Kayıt
                    await context.Hadisler.AddRangeAsync(eklenecekHadisler);
                    await context.SaveChangesAsync();

                    Console.WriteLine($"{eklenecekHadisler.Count} hadis başarıyla yüklendi.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hadis Seed Hatası: " + ex.Message);
            }
        }

        private static async Task SeedSurelerAsync(KuranGuideDbContext context)
        {
            // Veritabanı doluysa işlem yapma
            if (await context.Sureler.AnyAsync())
            {
                return;
            }

            try
            {
                // JSON Dosyasını Oku
                var jsonData = await File.ReadAllTextAsync("../KuranGuide.Api/Data/kuran_data.json");

                // Deserialize
                var surelerData = JsonSerializer.Deserialize<List<JsonSureModel>>(jsonData, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (surelerData != null)
                {
                    var eklenecekSureler = new List<Sure>();

                    foreach (var s in surelerData)
                    {
                        // "meccan" -> "Mekke", "medinan" -> "Medine" dönüşümü
                        string yerTr = (!string.IsNullOrEmpty(s.Yer) && s.Yer.ToLower().Contains("mecca"))
                                       ? "Mekke"
                                       : "Medine";

                        var yeniSure = new Sure
                        {
                            SureNo = s.No,
                            SureAdi = s.Ad,
                            ArapcaAdi = s.ArapcaAd,
                            Yer = yerTr,
                            AyetSayisi = s.AyetSayisi,
                            Ayetler = new List<Ayet>()
                        };

                        foreach (var a in s.Ayetler)
                        {
                            yeniSure.Ayetler.Add(new Ayet
                            {
                                AyetNo = a.No,
                                ArapcaMetin = a.Arapca,
                                Meal = a.Meal,
                                // TemaId null kalacak
                            });
                        }

                        eklenecekSureler.Add(yeniSure);
                    }

                    await context.Sureler.AddRangeAsync(eklenecekSureler);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Sure Seed Hatası: " + ex.Message);
                // throw; // İsteğe bağlı: Hata fırlatıp uygulamayı durdurabilirsin
            }
        }

        private static async Task SeedTemalarAsync(KuranGuideDbContext context)
        {
            // Veritabanı doluysa işlem yapma
            if (await context.Temalar.AnyAsync())
            {
                return;
            }

            try
            {
                // JSON Dosyasını Oku
                var jsonData = await File.ReadAllTextAsync("../KuranGuide.Api/Data/temalar.json");

                // Entity ile JSON propertyleri birebir tutuyorsa (TemaAdi, Icon vb.)
                // Direkt Tema sınıfına deserialize edebiliriz.
                var temalar = JsonSerializer.Deserialize<List<Tema>>(jsonData, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                if (temalar != null)
                {
                    await context.Temalar.AddRangeAsync(temalar);
                    await context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                // Dosya yoksa veya hata varsa logla ama uygulamayı patlatma
                Console.WriteLine("Tema Seed Hatası: " + ex.Message);
            }
        }

        private static async Task AutoMapTemalarAsync(KuranGuideDbContext context)
        {
            // Eğer veritabanında zaten tema atanmış ayet varsa işlemi tekrar yapma (Performans için)
            // Amaç ilk kurulumda çalıştırmak.
            if (await context.Ayetler.AnyAsync(x => x.TemaId != null)) return;

            Console.WriteLine("Otomatik Tema Eşleştirme Başlatılıyor...");

            var temalar = await context.Temalar.ToListAsync();
            // Tüm ayetleri belleğe çekiyoruz (6236 kayıt sorun olmaz)
            var ayetler = await context.Ayetler.ToListAsync();

            int eslesmeSayisi = 0;

            foreach (var tema in temalar)
            {
                if (string.IsNullOrEmpty(tema.AnahtarKelimeler)) continue;

                // Anahtar kelimeleri ayır ve temizle (küçük harfe çevir)
                // Örn: "namaz, salat, secde" -> ["namaz", "salat", "secde"]
                var keywords = tema.AnahtarKelimeler.Split(',')
                                   .Select(k => k.Trim().ToLower())
                                   .Where(k => !string.IsNullOrEmpty(k))
                                   .ToList();

                foreach (var ayet in ayetler)
                {
                    // Eğer ayetin zaten bir teması varsa üzerine yazmayalım (İsteğe bağlı, şimdilik boşsa yazıyoruz)
                    if (ayet.TemaId != null) continue;

                    var mealLower = ayet.Meal.ToLower();

                    // Kelimelerden herhangi biri mealde geçiyor mu?
                    bool isMatch = keywords.Any(k => mealLower.Contains(k));

                    if (isMatch)
                    {
                        ayet.TemaId = tema.Id;
                        eslesmeSayisi++;
                    }
                }
            }

            if (eslesmeSayisi > 0)
            {
                await context.SaveChangesAsync();
                Console.WriteLine($"Başarılı: {eslesmeSayisi} adet ayet otomatik olarak ilgili temalara bağlandı.");
            }
            else
            {
                Console.WriteLine("Eşleşen ayet bulunamadı.");
            }
        }




    }


    public class JsonHadisModel
    {
        public string Metin { get; set; }
        public string Kaynak { get; set; }
    }
    public class JsonSureModel
    {
        [JsonPropertyName("id")]
        public int No { get; set; }

        [JsonPropertyName("translation")]
        public string Ad { get; set; }

        [JsonPropertyName("name")]
        public string ArapcaAd { get; set; }

        [JsonPropertyName("type")]
        public string Yer { get; set; }

        [JsonPropertyName("total_verses")]
        public int AyetSayisi { get; set; }

        [JsonPropertyName("verses")]
        public List<JsonAyetModel> Ayetler { get; set; }
    }

    public class JsonAyetModel
    {
        [JsonPropertyName("id")]
        public int No { get; set; }

        [JsonPropertyName("text")]
        public string Arapca { get; set; }

        [JsonPropertyName("translation")]
        public string Meal { get; set; }
    }
}