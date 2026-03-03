Web Scraping Akademi Uygulaması<br>

<img align="center" src="https://www.svgrepo.com/show/237418/turkey.svg"  height="30" width="40" />Tr:</a><br>
Yazılım Laboratuvarı II dersimizin 1. projesi olan Web Scraping Akademi Uygulaması Web projesidir.
C# yazılmıştır.<br>
<img src="https://raw.githubusercontent.com/devicons/devicon/master/icons/csharp/csharp-original.svg" alt="csharp" width="40" height="40"/> <img src="https://img.icons8.com/?size=100&id=74402&format=png&color=000000" alt="csharp" width="40" height="40"/> <br>
## 📘 Proje Hakkında  
Akademik arama motorlarından web scraping yöntemiyle DergiPark gibi kaynaklardan otomatik olarak toplayan, toplanan verileri MongoDB'de depolayan ve Elasticsearch üzerinden gelişmiş arama, filtreleme ve sıralama imkanı sunan bir web uygulamasıdır.<br>
<img align="center" src="https://github.com/betulbodurrr/Web_Scraping_Akademi_Uygulamas-/blob/main/img/project1.png"  height="400" width="700" /><br>
---
## Projenin İşleyişi
1. Kullanıcı Arama Yapar<br>
Kullanıcı, Blazor tabanlı web arayüzünde anahtar kelime ile arama yapar (örneğin: "makine öğrenmesi").<br>
<img align="center" src="https://github.com/betulbodurrr/Web_Scraping_Akademi_Uygulamas-/blob/main/img/filterLeft.png"  height="400" width="700" /><br>

3. Web Scraping ile Veri Toplama<br>
Girilen anahtar kelimeyle DergiPark üzerinde arama yapılır.<br>
•	HTTP istekleriyle ilgili sayfanın HTML içeriği çekilir.<br>
•	HtmlAgilityPack kütüphanesi ile HTML parse edilerek ilk 10 yayına ait bilgiler (başlık, yazarlar, özet, yayın tarihi, yayın türü, anahtar kelimeler, kaynakça, referanslar, alıntı sayısı, DOI, URL) çıkarılır.<br>
•	Her yayının PDF dosyası otomatik olarak indirilir.<br>
4. Verilerin MongoDB’ye Kaydedilmesi<br>
Elde edilen makale verileri, MongoDB koleksiyonunda doküman olarak saklanır.<br>
Her doküman; yayın adı, yazarlar, yayın türü, yayımlanma tarihi, özet, anahtar kelimeler, kaynakça, referanslar, alıntı sayısı, DOI ve URL gibi alanları içerir.<br>
<img align="center" src="https://github.com/betulbodurrr/Web_Scraping_Akademi_Uygulamas-/blob/main/img/mongodbdenarama.png"  height="400" width="700" /><br>

5. Gelişmiş Arama ve Filtreleme<br>
Kullanıcılar;<br>
•	Anahtar kelime, yazar, yayın türü, yayın tarihi ve alıntı sayısı gibi alanlarda filtreleme yapabilir.<br>
•	Sonuçlar, yayımlanma tarihi veya alıntı sayısına göre sıralanabilir.<br>
6. Web Arayüzü ile Gösterim<br>
•	Ana sayfada MongoDB’deki tüm kayıtlar listelenir.<br>
•	Yeni arama yapıldığında scraping işlemi tetiklenir, veriler güncellenir ve sonuçlar ekrana getirilir.<br>
•	Yayın isimlerine tıklandığında, ilgili makalenin tüm detayları ayrı bir sayfada gösterilir.<br>
•	Dinamik filtreleme paneli ile kullanıcılar sonuçları anında daraltabilir.<br>
<img align="center" src="https://github.com/betulbodurrr/Web_Scraping_Akademi_Uygulamas-/blob/main/img/yayimlanmatarihisiralama.png"  height="400" width="700" /><br>
<img align="center" src="https://github.com/betulbodurrr/Web_Scraping_Akademi_Uygulamas-/blob/main/img/makaleaditikla.png"  height="700" width="500" /><br>
<img align="center" src="https://github.com/betulbodurrr/Web_Scraping_Akademi_Uygulamas-/blob/main/img/makaleadi%20sıralama.png"  height="400" width="700" /><br>




## Kullanılan Teknolojiler
| Bileşen         | Teknoloji / Kütüphane                | 
|-----------------|--------------------------------------| 
| Web Scraping    | C#, HtmlAgilityPack, HttpClient      | 
| PDF İndirme     | HttpWebRequest, FileStream           | 
| Veritabanı      | MongoDB (MongoDB.Driver)             | 
| Web Çatısı      | ASP.NET Core, Blazor                 | 
| Ön Yüz          | Blazor, HTML, CSS, Bootstrap         | 
| İşletim Ortamı  | .NET 6, MongoDB 7.0                  |

## Öne Çıkan Özellikler<br>
•	API kullanmadan doğrudan HTML scraping ile veri toplama.<br>
•	PDF dosyalarının otomatik indirilmesi ve saklanması.<br>
•	MongoDB ile güçlü veri saklama ve sorgulama.<br>
•	Kullanıcı dostu Blazor arayüzü ile dinamik filtreleme ve sıralama.<br>
•	Makale detaylarının ve PDF’lerinin kolay erişimi.<br>
<br>

[Proje Detayları için Tıklayınız.](https://github.com/betulbodurrr/Web_Scraping_Akademi_Uygulamas-/blob/main/YazlabProject2.1.pdf)<br>


<img align="center" src="https://www.svgrepo.com/show/365950/usa.svg"  height="20" width="30" />En:</a><br>

Software Laboratory I, our 1rd project is a Web Scraping Academy Application web project.
It is developed using C# programming language.<br>
<img src="https://raw.githubusercontent.com/devicons/devicon/master/icons/csharp/csharp-original.svg" alt="csharp" width="40" height="40"/>  <img src="https://img.icons8.com/?size=100&id=74402&format=png&color=000000" alt="csharp" width="40" height="40"/> <br>

## Project Description<br>
It is a web application that automatically collects data from academic search engines and sources like DergiPark using the web scraping method.

The collected data is stored in MongoDB.

The application also provides advanced search, filtering, and sorting features through Elasticsearch.
<img align="center" src="https://github.com/betulbodurrr/Web_Scraping_Akademi_Uygulamas-/blob/main/img/project1.png"  height="400" width="700" /><br>
---
## Project Workflow

1. User Makes a Search <br>
The user searches with a keyword (for example: "machine learning") in the Blazor-based web interface.<br>
<img align="center" src="https://github.com/betulbodurrr/Web_Scraping_Akademi_Uygulamas-/blob/main/img/filterLeft.png"  height="400" width="700" /><br>

2. Data Collection with Web Scraping <br>
A search is performed on DergiPark using the entered keyword.<br>
• The HTML content of the related page is retrieved with HTTP requests.<br>
• The HTML is parsed using the HtmlAgilityPack library, and information of the first 10 publications (title, authors, abstract, publication date, publication type, keywords, bibliography, references, citation count, DOI, URL) is extracted.<br>
• The PDF file of each publication is automatically downloaded.<br>
3. Saving Data to MongoDB<br>
• The collected article data is stored as documents in a MongoDB collection.<br>
• Each document includes fields such as publication title, authors, publication type, publication date, abstract, keywords, bibliography, references, citation count, DOI, and URL.<br>
<img align="center" src="https://github.com/betulbodurrr/Web_Scraping_Akademi_Uygulamas-/blob/main/img/mongodbdenarama.png"  height="400" width="700" /><br>

4. Advanced Search and Filtering<br>
Users can:<br>
Filter results by keyword, author, publication type, publication date, and citation count.<br>
Sort results by publication date or citation count.<br>

5. Display with Web Interface<br>
• All records stored in MongoDB are listed on the homepage.<br>
• When a new search is made, the scraping process is triggered, the data is updated, and the results are displayed.<br>
• When users click on a publication title, all details of the related article are shown on a separate page.<br>
•With the dynamic filtering panel, users can instantly narrow down the results.<br>
<img align="center" src="https://github.com/betulbodurrr/Web_Scraping_Akademi_Uygulamas-/blob/main/img/yayimlanmatarihisiralama.png"  height="400" width="700" /><br>
<img align="center" src="https://github.com/betulbodurrr/Web_Scraping_Akademi_Uygulamas-/blob/main/img/makaleaditikla.png"  height="700" width="500" /><br>
<img align="center" src="https://github.com/betulbodurrr/Web_Scraping_Akademi_Uygulamas-/blob/main/img/makaleadi%20sıralama.png"  height="400" width="700" /><br>




## Technologies Used
| Component       | Technology / Library                | 
|-----------------|--------------------------------------| 
| Web Scraping    | C#, HtmlAgilityPack, HttpClient      | 
| PDF Download    | HttpWebRequest, FileStream           | 
| Database        | MongoDB (MongoDB.Driver)             | 
| Web Framework   | ASP.NET Core, Blazor                 | 
| Front-End       | Blazor, HTML, CSS, Bootstrap         | 
| Runtime         | .NET 6, MongoDB 4.x                  |
##Key Features

• Data collection with direct HTML scraping without using any API.

• Automatic downloading and storing of PDF files.

• Powerful data storage and querying with MongoDB.

• User-friendly Blazor interface with dynamic filtering and sorting.

• Easy access to article details and their PDF files.
[For Project Details, Click Here.](https://github.com/betulbodurrr/Web_Scraping_Akademi_Uygulamas-/blob/main/YazlabProject2.1.pdf)

