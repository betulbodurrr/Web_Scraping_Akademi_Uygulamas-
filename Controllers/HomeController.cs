using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Web_Scraping_Akademi_Uygulaması.Models;
using System.Diagnostics;
using System;
using System.Net.Http;
using System.Threading.Tasks;

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
            Debug.WriteLine("kod burada");
            var url = "https://scholar.google.com/scholar?hl=tr&as_sdt=0%2C5&q=&btnG=";// gidilmek istenen url
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
