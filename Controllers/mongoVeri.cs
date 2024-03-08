using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

public class DatabaseController : Controller, IDisposable
{
    private readonly MongoClient dbClient;
    private readonly IMongoDatabase database;
    string dbConnection = "mongodb://localhost:27017/yazlab1";

    public class AramaMotoru
    {
        public int yayinId { get; set; }
        public string yayinAdi { get; set; }
        public string[] yazarlar { get; set; } // Dizi türü düzeltildi.
        public string yayinTuru { get; set; }
        public string yayimlanmaTarihi { get; set; }
        public string yayinciAdi { get; set; }
        public string anahtarKelimelerArama { get; set; } 
        public string anahtarKelimelerMakale { get; set; }//mongodb de dizi olarak tutulmalı
        public string ozet { get; set; }
        public string[] referanslar { get; set; } // Dizi türü düzeltildi.
        public int alintiSayisi { get; set; } // Veri tipi düzeltildi.
        public string doiNumarasi { get; set; }
        public string urlAdresi { get; set; }
    }

    public void veriEkle(int a_alintiSayisi)
    {
        MongoClient dbClient = new MongoClient(dbConnection);

        
            var database = dbClient.GetDatabase("yazlab1");
            var collection = database.GetCollection<AramaMotoru>("AramaMotoru");

            var makale = new AramaMotoru
            {
                yayinId = 11,
                yayinAdi = "deneme2",
                yazarlar = new string[] { "deneme13", "Yazar 23" },
                yayinTuru = "denesvdvme",
                yayimlanmaTarihi = "2024-03-02",
                yayinciAdi = "deneme Adı",
                anahtarKelimelerArama = "Anahtar Kelime 1, Anahtar Kelime 2",
                anahtarKelimelerMakale = "Anahtar Kelime 3, Anahtar Kelime 4",
                ozet = "Bu, bir makalenin özetidir.",
                referanslar = new string[] { "Referans 1", "Referans 2" },
                alintiSayisi = a_alintiSayisi,
                doiNumarasi = "1234567890",
                urlAdresi = "https://www.example.com/makale"
            };

            try
            {
                collection.InsertOne(makale);
                Console.WriteLine("Veri ekleme işlemi tamamlandı.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Hata: " + ex.Message);
            }
        

    }
}
