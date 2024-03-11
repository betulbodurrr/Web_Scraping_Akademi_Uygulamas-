using Microsoft.AspNetCore.Mvc;
using Elasticsearch.Net;
using Elasticsearch;
using System.Collections.Generic;
using Nest;
using System.Net;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Diagnostics;
using static DatabaseController;
using System.Net.Sockets;
using MongoDB.Bson.IO;
using System.Text.Json;
using Microsoft.Extensions.Options;

namespace Web_Scraping_Akademi_Uygulaması.Controllers
{

    public class Article
    {
        public string Id { get; set; } // Assuming "_id" field represents the document ID in Elasticsearch
        public int YayinId { get; set; }
        public string YayinAdi { get; set; }
        public List<string> Yazarlar { get; set; }
        public string YayinTuru { get; set; }
        public string YayimlanmaTarihi { get; set; }
        public string YayinciAdi { get; set; }
        public string AnahtarKelimelerArama { get; set; }
        public List<string> AnahtarKelimelerMakale { get; set; }
        public string Ozet { get; set; }
        public List<string> Kaynakca { get; set; }
        public List<string> Referanslar { get; set; }
        public int AlintiSayisi { get; set; }
        public string DoiNumarasi { get; set; }
        public string UrlAdresi { get; set; }
    }

    public class SearchCode : Controller
    {

        public async Task searchCode()
        {
            // Elasticsearch baglandi
            var uri = new Uri("http://localhost:9200");
            var connectionPool = new SingleNodeConnectionPool(uri);
            var settings = new ConnectionConfiguration(connectionPool).BasicAuthentication("elastic", "XWAVpBbAJSTlWfkUCHnC"); // Yetkilendirme varsa
            var client = new ElasticLowLevelClient(settings);

            // MongoDB baglandi
            var mongoClient = new MongoClient("mongodb://localhost:27017/yazlab1");
            var database = mongoClient.GetDatabase("yazlab1");
            var collection = database.GetCollection<BsonDocument>("AramaMotoru");

            var documents = collection.Find(new BsonDocument()).ToList();//verilerin tamamı
            foreach (var document in documents)
            {

                // BsonDocument'i JSON string'e dönüştürün
                string jsonDocument = document.ToJson();
                Debug.WriteLine(jsonDocument);
                string articlejson = JsonSerializer.Serialize(jsonDocument);
                //indexleme
                var indexResponse = await client.IndexAsync<IndexResponse>( index: "yazlab1_aramamotoru_index",PostData.String(articlejson));
                if (indexResponse.IsValid)
                {
                    Debug.WriteLine("Bitti!"); // Success message
                }
                else
                {
                    Debug.WriteLine("Error indexing article: ");
                }
            }
        }
    }
}