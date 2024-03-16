using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Driver;
using System.Diagnostics;
using System.Text;
using System.Text.Json;
namespace Web_Scraping_Akademi_Uygulaması.Controllers
{
    public class DatabaseController : Controller, IDisposable
    {
        private readonly MongoClient dbClient;
        private readonly IMongoDatabase database;
        string dbConnection = "mongodb://localhost:27017/yazlab1";
        public List<AramaMotoru> modelList = new List<AramaMotoru>();
        public List<AramaMotoru> resultt = new List<AramaMotoru>();
        public long count = 0;
        
        public class AramaMotoru
        {
            public Object Id { get; set; }
            
            public long yayinId { get; set; }
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

        public void veriEkle(string a_yayinAdi, List<string> a_yazarlar, string a_yayinTuru, string a_yayimlanmaTarihi, string a_yayinciAdi, string a_anahtarKelimelerArama, List<string> a_anahtarKelimelerMakale, string a_ozet, List<string> a_kaynakca, List<string> a_referanslar, int a_alintiSayisi, string a_doiNumarasi, string a_urlAdresi)
        {
            
            MongoClient dbClient = new MongoClient(dbConnection);

            var database = dbClient.GetDatabase("yazlab1");
            var collection = database.GetCollection<AramaMotoru>("AramaMotoru");
            long point = collection.CountDocuments(new BsonDocument());

            if (a_anahtarKelimelerArama.Contains("+"))
            {
                for (int i = 0; i < a_anahtarKelimelerArama.Length; i++)
                {
                    if (a_anahtarKelimelerArama[i].Equals('+'))
                    {
                        StringBuilder sb = new StringBuilder(a_anahtarKelimelerArama);
                        sb[i] = ' ';
                        a_anahtarKelimelerArama = sb.ToString();
                    }
                }
            }
            a_yayimlanmaTarihi = TarihDuzenle(a_yayimlanmaTarihi);

            Debug.WriteLine(point);
            count = point;
            count++;

            var makale = new AramaMotoru
            {
                Id = count,
                yayinId = count,
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
                doiNumarasi = "0000",
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

        public List<AramaMotoru> bilgiAl()
        {
            MongoClient dbClient = new MongoClient(dbConnection);

            var database = dbClient.GetDatabase("yazlab1");
            var collection = database.GetCollection<AramaMotoru>("AramaMotoru");
            //var filter = Builders<MyDataClass>.Filter.Eq(x => x.Name, "Ahmet");

            var filter = Builders<AramaMotoru>.Filter.Empty;
            var cursor = collection.Find(filter);
            var results = cursor.ToList();

            foreach (var document in results)
            {
                //Debug.WriteLine(document);// buradan veriyi bir şekilde list yapsıı olarak çıakrtmama gerek
                modelList.Add(document);
            }
            return modelList;
        }
        public void bilgiAl2(int id)
        {
            MongoClient dbClient = new MongoClient(dbConnection);

            var database = dbClient.GetDatabase("yazlab1");
            var collection = database.GetCollection<AramaMotoru>("AramaMotoru");
            //var filter = Builders<MyDataClass>.Filter.Eq(x => x.Name, "Ahmet");

            var filter = Builders<AramaMotoru>.Filter.Eq(x => x.Id, id);
            resultt = collection.Find(filter).ToList();
            Debug.WriteLine(id);

        }
        public static string TarihDuzenle(string tarih)
        {
            string[] ayIsimleri = { "Ocak", "Şubat", "Mart", "Nisan", "Mayıs", "Haziran", "Temmuz", "Ağustos", "Eylül", "Ekim", "Kasım", "Aralık" };
            int ay_Index = Array.IndexOf(ayIsimleri, tarih.Split(" ")[1]);
            string gun = tarih.Split(" ")[0];
            string yil = tarih.Split(" ")[2];
            return yil + "/" + (ay_Index + 1)+ "/" +gun ;
        }

    }
    

}