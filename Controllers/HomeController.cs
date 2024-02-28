using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Web_Scraping_Akademi_Uygulaması.Models;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text;
using System.Net.Http.Headers;
using HtmlAgilityPack;

public class HomeController : Controller
{
    List<string> pdfUrls = new List<string>();

    public ActionResult Index()
    {
        return View();
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

                var nodes = doc.DocumentNode.SelectNodes("//a[contains(@href, 'pdf')]");//pdf geçen tüm bağlantılar

                foreach (var node in nodes)
                {
                    string pdfUrl = node.Attributes["href"].Value;
                    Debug.WriteLine(pdfUrl);//!!!!        ÖNEMLİ !!!!!! pdfUrl veritabanına kayıt edilecek !!!!!!
                    pdfUrls.Add(pdfUrl);
                    
                    var citationCountNode = node.SelectSingleNode(".//following-sibling::a[contains(@href, '/scholar?cites=')]");
                  
                    if (citationCountNode != null)
                    {
                        string citationCountText = citationCountNode.InnerText.Trim();
                        Debug.WriteLine("Alıntı Sayısı: " + citationCountText);
                    }
                    else
                    {
                        Debug.WriteLine("Alıntı sayısı bulunamadı.");
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