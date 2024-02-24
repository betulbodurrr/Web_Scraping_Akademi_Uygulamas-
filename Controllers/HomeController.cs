using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Web_Scraping_Akademi_Uygulaması.Models;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Net.Http;
using System.Text;
using System.Net.Http.Headers;

public class HomeController : Controller
{
    public ActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<ActionResult> Search(string searchText)
    {

        string searchResult = searchText;
        Debug.WriteLine(searchResult);// searchteki anahtar kelime buraya geldi.

        using (var httpClient=new HttpClient())
        {
            httpClient.DefaultRequestHeaders.AcceptCharset.Add(new StringWithQualityHeaderValue("utf-8"));//desteklenmeyen karakter sti hatasını engellemek için.çünkü bunu "ISO-8859-9" desteklemiyor.

            Debug.WriteLine("************||||||||||||||||||************");
            var mainUrl = "https://scholar.google.com/scholar?hl=tr&as_sdt=0,5&as_rr=1&q=";// gidilmek istenen url şuanda sadece makale araması yapıyor
            var url = mainUrl + searchResult;

            Debug.WriteLine(url);

            var response=await httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                Debug.WriteLine(responseBody);// yanıt 
            }
            else
            {
                Debug.WriteLine($"Yanıt Başarısız Oldu{response.StatusCode}");
            }
        }
            return RedirectToAction("Index");
    }   
}
