using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Web_Scraping_Akademi_Uygulaması.Models;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text;
using System.Net.Http.Headers;
using HtmlAgilityPack;
using System.Diagnostics.Metrics;
using Amazon.Runtime;
using static System.Net.Mime.MediaTypeNames;
using System.Text.RegularExpressions;
using System.Linq;

public class HomeController : Controller
{
    List<string> pdfUrls = new List<string>();
    public int makaleSayac = 0;
    DatabaseController veritabani = new DatabaseController();
    public int a_yayinId;
    public string a_yayinAdi;
    List<string> a_yazarlar = new List<string>();
    public string a_yayinTuru;
    public string a_yayimlanmaTarihi;
    public string a_yayinciAdi;
    public string a_anahtarKelimelerArama;
    List<string> a_anahtarKelimelerMakale = new List<string>();
    List<string> a_kaynakca = new List<string>();
    public string a_ozet;
    List<string> a_referanslar = new List<string>();
    public int a_alintiSayisi;
    public string a_doiNumarasi;
    public string a_urlAdresi;
    public ActionResult Index()
    {
        return View();
    }


    public string makaleAdiBul(HtmlDocument doc_article)
    {
        var makaleler = doc_article.DocumentNode.SelectSingleNode("//title");
        if (makaleler != null)
        {
            var m_Title = makaleler.InnerText.Trim();
            var makaleTitle = m_Title.Substring(m_Title.IndexOf("&raquo;  Makale  &raquo; ") + 25);
            //Debug.WriteLine("Makale başlığı: " + makaleTitle);
            makaleSayac++;
            a_yayinAdi = makaleTitle;
            return makaleTitle;
        }
        else
        {
            //Debug.WriteLine("makale adı bulunamadı");
            return "makale adi bulunamadi";
        }
    }
    public string yazarlariBul(HtmlDocument doc_article)
    {
        var authors = new List<string>();
        var yazarlar = doc_article.DocumentNode.SelectSingleNode("//*[@id=\"article_en\"]/p");
        a_yazarlar.Clear();

        if (yazarlar != null)
        {
            var yazarTitle = yazarlar.InnerText.Trim();
            var lines = yazarTitle.Split('\n');

            var names = new List<string>();
            foreach (var line in lines)
            {
                if (!string.IsNullOrEmpty(line) && line != " " && line != " ")
                {
                    if (line.Trim() != "")
                    {
                        //names.Add(line.Trim());
                        a_yazarlar.Add(line.Trim());
                    }

                }
            }
            //var namesString = string.Join(", ", names);
            //Debug.WriteLine(namesString);

            //Debug.WriteLine("Yazarlar: " + yazarTitle);
            return yazarTitle;
        }
        else
        {
            Debug.WriteLine("yazar adı bulunamadı");
            return "yazarlar bulunamadi";
        }
    }
    public string ozetBul(HtmlDocument doc_article)
    {
        var ozet = doc_article.DocumentNode.SelectSingleNode("//*[@id=\"article_en\"]/div[2]");
        if (ozet != null && ozet.InnerHtml.Contains("Öz"))
        {
            // "öz" ifadesi var
        }
        else
        {
            ozet = doc_article.DocumentNode.SelectSingleNode("//*[@id=\"article_en\"]/div[3]");

        }
        if (ozet != null)
        {
            var ozetTitle = ozet.InnerText.Trim();
            //Debug.WriteLine("ozet: " + ozetTitle);
            a_ozet = ozetTitle;
            return ozetTitle;
        }
        else
        {
            Debug.WriteLine("ozet bulunamadı");
            return "ozet bulunamadi";
        }
    }
    public string pdfLink(HtmlDocument doc_article)
    {
        var linkler = doc_article.DocumentNode.SelectSingleNode("//*[@id=\"article-toolbar\"]/a[1]");
        var href = linkler.GetAttributeValue("href", "");
        href = "https://dergipark.org.tr" + href;
        //Debug.WriteLine("PDF indirmek için link: " + href);
        a_urlAdresi = href;
        return href;
    }
    public string kaynakcaBul(HtmlDocument doc_article)
    {

        var makaleKaynakca = new List<string>();
        var kaynakca = doc_article.DocumentNode.SelectSingleNode("//*[@id=\"article_en\"]/div[4]");
        a_kaynakca.Clear();

        if (kaynakca != null && kaynakca.InnerHtml.Contains("Kaynakça"))
        {
            // "Kaynakça" ifadesi var
        }
        else
        {
            for (int i = 5; i <= 6; i++)
            {
                kaynakca = doc_article.DocumentNode.SelectSingleNode("//*[@id=\"article_en\"]/div[" + i + "]");
                if (kaynakca != null && kaynakca.InnerHtml.Contains("Kaynakça"))
                {
                    break;
                }
                else kaynakca = null;

            }
        }

        if (kaynakca != null)
        {
            var k_Title = kaynakca.InnerText.Trim();
            var kaynakcaTitle = k_Title.Substring(k_Title.IndexOf("Kaynakça"));

            var lines = kaynakcaTitle.Split('\n');
            
            foreach (var line in lines)
            {
                if (!string.IsNullOrEmpty(line) && line != " " && line != " ")
                {
                    if (line.Trim() != "")
                    {
                        //makaleKaynakca.Add(line.Trim());
                        a_kaynakca.Add(line.Trim());
                    }

                }
            }
            var namesString = string.Join(", ", makaleKaynakca);
            //Debug.WriteLine(namesString);

            //Debug.WriteLine("Kaynakça: " + kaynakcaTitle);
            return kaynakcaTitle;
        }
        else
        {
            Debug.WriteLine("kaynakça bulunamadı");
            return "kaynakça bulunamadi";
        }
    }
    public string makaleAnahtarKelimesiBul(HtmlDocument doc_article)
    {

        var keys = new List<string>();
        var anahtarKelime = doc_article.DocumentNode.SelectSingleNode("//*[@id=\"article_en\"]/div[2]");
        a_anahtarKelimelerMakale.Clear();

        if (anahtarKelime != null && anahtarKelime.InnerHtml.Contains("Anahtar Kelimeler"))
        {
            // "Kaynakça" ifadesi var
        }
        else
        {
            for (int i = 3; i <= 6; i++)
            {
                anahtarKelime = doc_article.DocumentNode.SelectSingleNode("//*[@id=\"article_en\"]/div[" + i + "]");
                if (anahtarKelime != null && anahtarKelime.InnerHtml.Contains("Anahtar Kelimeler"))
                {
                    break;
                }
            }
        }

        if (anahtarKelime != null)
        {
            var ak_Title = anahtarKelime.InnerText.Trim();
            var anahtarKelimeTitle = ak_Title.Substring(ak_Title.IndexOf("Anahtar Kelimeler") + 110);

            var lines = anahtarKelimeTitle.Split(',');
            var makaleAnahtarKelime = new List<string>();
            foreach (var line in lines)
            {
                if (!string.IsNullOrEmpty(line) && line != " " && line != " ")
                {
                    if (line.Trim() != "")
                    {
                        //makaleAnahtarKelime.Add(line.Trim());
                        a_anahtarKelimelerMakale.Add(line.Trim());
                    }
                }
            }
            var namesString = string.Join(";", makaleAnahtarKelime);
            //Debug.WriteLine(namesString);

            //Debug.WriteLine("keywords: " + anahtarKelimeTitle);
            return anahtarKelimeTitle;
        }
        else
        {
            Debug.WriteLine("key bulunamadı");
            return "key bulunamadi";
        }
    }
    public string yayımlanmaTarihiBul(HtmlDocument doc_article)
    {
        var tarih = doc_article.DocumentNode.SelectSingleNode("//tr[3]");
        if (tarih.InnerHtml.Contains("Yayımlanma Tarihi")) { }
        else
        {
            for (int i = 4; i <= 6; i++)
            {
                tarih = doc_article.DocumentNode.SelectSingleNode("//tr[" + i + "]");
                if (tarih != null && tarih.InnerHtml.Contains("Yayımlanma Tarihi"))
                {
                    break;
                }

                else tarih = null;
            }

        }
        if (tarih !=null)
        {
            var t_Title = tarih.InnerText.Trim();
            var tarihTitle = t_Title.Substring(t_Title.IndexOf("Yayımlanma Tarihi") + 46);
            //Debug.WriteLine("Yayımlanma tarihi: " + tarihTitle);
            a_yayimlanmaTarihi = tarihTitle;
            return tarihTitle;
        }
        else
        {
            a_yayimlanmaTarihi ="0";
            Debug.WriteLine("Yayınlanma Tarihi bulunamadı");
            return "Yayınlanma Tarihibulunamadi";
        }
    }
    public string yayınTuruBul(HtmlDocument doc_article)
    {
        var yayinTuru = doc_article.DocumentNode.SelectSingleNode("//tr[1]");
        if (yayinTuru.InnerHtml.Contains("Bölüm")) { }
        else
        {
            for (int i = 2; i <= 6; i++)
            {
                yayinTuru = doc_article.DocumentNode.SelectSingleNode("//tr[" + i + "]");
                if (yayinTuru != null && yayinTuru.InnerHtml.Contains("Bölüm"))
                {
                    break;
                }
            }

        }
        if (yayinTuru != null)
        {
            var yT_Title = yayinTuru.InnerText.Trim();
            var yayinTuruTitle = yT_Title.Substring(yT_Title.IndexOf("Bölüm") + 38);
            //Debug.WriteLine("Yayın Türü: " + yayinTuruTitle);
            a_yayinTuru = yayinTuruTitle;
            return yayinTuruTitle;
        }
        else
        {
            Debug.WriteLine("Yayın Türü bulunamadı");
            return "Yayın Türü bulunamadi";
        }
    }
    public string alintiSayisiBul(HtmlDocument doc_article)
    {
        a_alintiSayisi = 0;
        var alintiSayisi = doc_article.DocumentNode.SelectSingleNode("//*[@id=\"kt_content\"]/div/div[3]/div[1]/div[4]/div/div[0]");
        if (alintiSayisi != null && alintiSayisi.InnerHtml.Contains("doi")) { a_alintiSayisi++; }
        else
        {
            for (int i = 1; i <= 10; i++)
            {
                alintiSayisi = doc_article.DocumentNode.SelectSingleNode("//*[@id=\"kt_content\"]/div/div[3]/div[1]/div[4]/div/div[" + i + "]");
                if (alintiSayisi != null && alintiSayisi.InnerHtml.Contains("doi"))
                {
                    var yT_Title = alintiSayisi.InnerText.Trim();
                    Debug.WriteLine(yT_Title);

                    a_alintiSayisi++;
                }
            }

        }
        return a_alintiSayisi.ToString();
    }
    public string referansaBul(HtmlDocument doc_article)//BU DÜZENLENMELİ ALINTILAR 3 SATIRDAN OLUŞUYOR HER BİRİNİ \N DE BÖLÜYOR ### kaç satır kontrol edeceğine bakmak lazım hata veriyor
    {

        var referanslar = new List<string>();
        var referans = doc_article.DocumentNode.SelectSingleNode("//*[@id=\"kt_content\"]/div/div[3]/div[1]/div[4]/div");
        a_referanslar.Clear();

        if (referans != null && referans.InnerHtml.Contains("Cited By"))
        {
            // "alıntı" ifadesi var
        }
        else
        {
            for (int i = 4; i <= 6; i++)
            {
                referans = doc_article.DocumentNode.SelectSingleNode("//*[@id=\"kt_content\"]/div/div[3]/div[1]/div[" + i + "]/div");
                if (referans != null && referans.InnerHtml.Contains("Cited By"))
                {
                    break;
                }
                else if (i == 6)
                {
                    referans = null;
                }
            }
        }

        if (referans != null)
        {
            var r_Title = referans.InnerText.Trim();
            var referansTitle = r_Title.Substring(r_Title.IndexOf("Cited By") + 37);

            var lines = referansTitle.Split('\n');
            foreach (var line in lines)
            {

                if (!string.IsNullOrEmpty(line) && line != " " && line != " ")
                {
                    if (line.Trim() != "")
                    {
                        a_referanslar.Add(line.Trim());
                    }
                }
            }
            var namesString = string.Join("; ", referanslar);
            //Debug.WriteLine(namesString);

            //Debug.WriteLine("Referans: " + referansTitle);
            return referansTitle;
        }
        else
        {
            //Debug.WriteLine("Referans bulunamadı");
            return "Referans bulunamadi";
        }
    }

    [HttpPost]
    public async Task<ActionResult> Search(string searchText)
    {

        string searchResult = searchText;
        Debug.WriteLine(searchResult);// searchteki anahtar kelime buraya geldi.

        if (searchResult.Contains(" "))
        {
            for (int i = 0; i < searchResult.Length; i++)
            {
                if (searchResult[i].Equals(' '))
                {
                    StringBuilder sb = new StringBuilder(searchResult);
                    sb[i] = '+';
                    searchResult = sb.ToString();
                }
            }
        }
        a_anahtarKelimelerArama = searchResult;
        Debug.WriteLine("Anahtar Kelime: " + searchResult);
        using (var httpClient = new HttpClient())
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            httpClient.DefaultRequestHeaders.AcceptCharset.Add(new StringWithQualityHeaderValue("utf-8"));//desteklenmeyen karakter sti hatasını engellemek için.çünkü bunu "ISO-8859-9" desteklemiyor.

            Debug.WriteLine("************||||||||||||||||||************");
            var mainUrl = "https://dergipark.org.tr/tr/search?";// gidilmek istenen url şuanda sadece makale araması yapıyor
            var url = mainUrl + "q=" + searchResult + "&section=articles";
            makaleSayac = 0;
            Debug.WriteLine(url);

            Boolean kontrol1 = false;
            Boolean kontrol2 = false;

            while (makaleSayac < 10)
            {
                a_yayinId = makaleSayac;
                var response = await httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    //Debug.WriteLine(responseBody);// yanıt 


                    HtmlDocument doc = new HtmlDocument(); //gelen yanıtı bir belgeye dönüştürüldü. buradan html kodlarını inceliycez.
                    doc.LoadHtml(await response.Content.ReadAsStringAsync());


                    var nodes = doc.DocumentNode.SelectNodes("//div[@class='article-cards']");

                    foreach (var node in nodes)
                    {

                        if (node != null)
                        {
                            var links = node.SelectNodes(".//a[@href]"); //tüm bağlantıları aldım

                            if (links != null)
                            {
                                foreach (var link in links)
                                {
                                    var href = link.GetAttributeValue("href", "");

                                    if (href.Contains("issue"))
                                    {
                                        Debug.WriteLine("PDF Bağlantısı: " + href);               //                !!!!!!!!!!!!VERİTABANI!!!!!!!!!!!!!!

                                        var response_article = await httpClient.GetAsync(href);//her makale sayfasına girebilmek için

                                        if (response_article.IsSuccessStatusCode)
                                        {

                                            var responseBody_article = await response_article.Content.ReadAsStringAsync();
                                            //Debug.WriteLine(responseBody_article);// yanıt 


                                            HtmlDocument doc_article = new HtmlDocument(); //gelen yanıtı bir belgeye dönüştürüldü. buradan html kodlarını inceliycez.
                                            doc_article.LoadHtml(await response_article.Content.ReadAsStringAsync());
                                            makaleAdiBul(doc_article);
                                            yazarlariBul(doc_article);
                                            ozetBul(doc_article);
                                            pdfLink(doc_article);
                                            kaynakcaBul(doc_article);
                                            makaleAnahtarKelimesiBul(doc_article);
                                            yayımlanmaTarihiBul(doc_article);
                                            yayınTuruBul(doc_article);
                                            referansaBul(doc_article);
                                            alintiSayisiBul(doc_article);
                                            veritabani.veriEkle(a_yayinId, a_yayinAdi, a_yazarlar, a_yayinTuru, a_yayimlanmaTarihi, a_yayinciAdi, a_anahtarKelimelerArama, a_anahtarKelimelerMakale, a_ozet, a_kaynakca, a_referanslar, a_alintiSayisi, a_doiNumarasi, a_urlAdresi);
                                        }
                                        else
                                        {
                                            Debug.WriteLine("Linke girilemedi");

                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            Debug.WriteLine("PDF Linkine girilemedi");
                        }
                    }

                    if (makaleSayac < 10 && !kontrol1)// diğer sayfalar için
                    {
                        url = "";
                        url = mainUrl + "2" + "q=" + searchResult + "&section=articles";
                        kontrol1 = true;
                    }
                    else if (kontrol1 && !kontrol2)
                    {
                        url = "";
                        url = mainUrl + "3" + "q=" + searchResult + "&section=articles";
                        kontrol2 = true;
                    }
                    else if (kontrol2)
                    {
                        url = "";
                        url = mainUrl + "4" + "q=" + searchResult + "&section=articles";

                    }
                    else break;
                }
                else
                {
                    Debug.WriteLine($"Yanıt Başarısız Oldu{response.StatusCode}");
                }
            }
        }
        return RedirectToAction("Index");
    }
}