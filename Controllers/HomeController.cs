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

public class HomeController : Controller
{
    List<string> pdfUrls = new List<string>();

    public ActionResult Index()
    {
        return View();
    }
    public string alintiSayisiBul(string metin)
    {
        string birinciAranan = "sayısı: ";
        string ikinciAranan = " İlgili";
        int birinciIndex = metin.IndexOf(birinciAranan);
        int ikinciIndex = metin.IndexOf(ikinciAranan);
        int aralik = ikinciIndex - birinciIndex - 8;
        string bulunanDeger = metin.Substring(birinciIndex + 8, aralik);
        //Debug.WriteLine(bulunanDeger);
        //Debug.WriteLine(birinciIndex + 8);
        //Debug.WriteLine(ikinciIndex);
        return bulunanDeger;
    }

    public string yazarlariBul(string metin)
    {
        string birinciAranan = "-";
        int birinciIndex = metin.IndexOf(birinciAranan);
        if (birinciIndex != -1)
        {
            metin = metin.Substring(0, birinciIndex).Trim();
        }
         return metin;
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

        Debug.WriteLine("Anahtar Kelime: " + searchResult);
        using (var httpClient = new HttpClient())
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            httpClient.DefaultRequestHeaders.AcceptCharset.Add(new StringWithQualityHeaderValue("utf-8"));//desteklenmeyen karakter sti hatasını engellemek için.çünkü bunu "ISO-8859-9" desteklemiyor.

            Debug.WriteLine("************||||||||||||||||||************");
            var mainUrl = "https://scholar.google.com/scholar?hl=tr&as_sdt=0,5&as_rr=1&q=";// gidilmek istenen url şuanda sadece makale araması yapıyor
            var url = mainUrl + searchResult;

            Debug.WriteLine(url);

            var response = await httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                Debug.WriteLine(responseBody);// yanıt 


                HtmlDocument doc = new HtmlDocument(); //gelen yanıtı bir belgeye dönüştürüldü. buradan html kodlarını inceliycez.
                doc.LoadHtml(await response.Content.ReadAsStringAsync());

                var nodes = doc.DocumentNode.SelectNodes("//div[@class='gs_r gs_or gs_scl']");

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

                                if (href.Contains(".pdf") || href.Contains("PDF") || href.Contains("pdf"))
                                {
                                    Debug.WriteLine("PDF Bağlantısı: " + href);               //                !!!!!!!!!!!!VERİTABANI!!!!!!!!!!!!!!

                                    // pdf varsa yazarlarını ve alıntı sayısını alıyor
                                    var yazarlar = node.SelectSingleNode(".//div[@class='gs_a']");
                                    if (yazarlar != null)
                                    {
                                        var yazarText = yazarlar.InnerText.Trim();
                                        string makaleYazarları = yazarlariBul(yazarText);                       // !!!!!!!!!!!!VERİTABANI!!!!!!!!!!!!!!

                                        Debug.WriteLine("Yazarlar: " + makaleYazarları);         
                                    }
                                    else
                                    {
                                        Debug.WriteLine("Yazarlar bulunamadı.");
                                    }

                                    var alinti = node.SelectSingleNode(".//div[@class='gs_fl gs_flb']");
                                    if (alinti != null)
                                    {
                                        var alintiText = alinti.InnerText.Trim();
                                        string alintiSayisi = alintiSayisiBul(alintiText);//                      !!!!!!!!!!!!VERİTABANI!!!!!!!!!!!!!!

                                        Debug.WriteLine("Alıntı Sayısı: " + alintiSayisi);
                                    }
                                    else
                                    {
                                        Debug.WriteLine("Alıntı sayısı bulunamadı.");
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        Debug.WriteLine("pdf yok");
                    }
                }
            }
            else
            {
                Debug.WriteLine($"Yanıt Başarısız Oldu{response.StatusCode}");
            }
        }
        return RedirectToAction("Index");

    }
    
}