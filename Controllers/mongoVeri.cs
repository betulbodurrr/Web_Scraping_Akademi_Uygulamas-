using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
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
        public List<string> yazarlar { get; set; } // Dizi türü düzeltildi.
        public string yayinTuru { get; set; }
        public string yayimlanmaTarihi { get; set; }
        public string yayinciAdi { get; set; }
        public string anahtarKelimelerArama { get; set; }
        public List<string> anahtarKelimelerMakale { get; set; }//mongodb de dizi olarak tutulmalı
        public string ozet { get; set; }
        public List<string> kaynakca { get; set; } 
        public List<string> referanslar { get; set; } // Dizi türü düzeltildi.
        public int alintiSayisi { get; set; } // Veri tipi düzeltildi.
        public string doiNumarasi { get; set; }
        public string urlAdresi { get; set; }
    }

    public void veriEkle(int a_yayinId, string a_yayinAdi, List<string> a_yazarlar, string a_yayinTuru, string a_yayimlanmaTarihi, string a_yayinciAdi, string a_anahtarKelimelerArama, List<string> a_anahtarKelimelerMakale, string a_ozet, List<string> a_kaynakca, List<string> a_referanslar,int a_alintiSayisi, string a_doiNumarasi, string a_urlAdresi)
    {
        MongoClient dbClient = new MongoClient(dbConnection);

        
            var database = dbClient.GetDatabase("yazlab1");
            var collection = database.GetCollection<AramaMotoru>("AramaMotoru");

        var makale = new AramaMotoru
        {
            yayinId = a_yayinId,
            yayinAdi = a_yayinAdi,
            yazarlar = a_yazarlar,
            yayinTuru = a_yayinTuru,
            yayimlanmaTarihi = a_yayimlanmaTarihi,
            yayinciAdi = a_yayinciAdi,
            anahtarKelimelerArama = a_anahtarKelimelerArama,
            anahtarKelimelerMakale = a_anahtarKelimelerMakale,
            ozet = a_ozet,
            kaynakca = a_kaynakca,
            referanslar = a_referanslar,
            alintiSayisi = a_alintiSayisi,
            doiNumarasi = "000",
            urlAdresi = a_urlAdresi,
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
