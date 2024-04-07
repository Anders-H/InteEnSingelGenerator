using System.Text;
using InteEnSingelGenerator;

const string sourceFile = @"C:\Users\hbom\OneDrive\InteEnSingel\source.txt";
const string localOutput = @"C:\Users\hbom\OneDrive\InteEnSingel\Output";
const string title = "Inte en singel";
const string authorEmail = "anders@winsoft.se";
const string authorEmailWithName = $"{authorEmail} (Anders Hesselbom)";

/*
 
Source file format: Episode name, release date (YYYY-MM-DD), length (MM:SS)

Example:

Evolution av Scotch (1985)                                 , 2022-11-01, 24:54
News of the World av Queen (1977)                          , 2022-10-25, 25:55
The Riddle av Nik Kershaw (1984)                           , 2022-10-18, 31:06

Lines starting with # is ignored.

*/


var source = File.ReadAllLines(sourceFile);

var episodes = (
    source
        .Where(s => !string.IsNullOrWhiteSpace(s) && !s.Trim().StartsWith("#"))
        .Select(Episode.Parse)
    ).ToList();

const string rss = "https://80tal.se/inte_en_singel/rss.xml";
const string websiteHead = $@"<!DOCTYPE html>
<html lang=""sv"" xmlns=""http://www.w3.org/1999/xhtml"">
<head>
<link rel=""apple-touch-icon"" sizes=""180x180"" href=""/apple-touch-icon.png""><link rel=""icon"" type=""image/png"" sizes=""32x32"" href=""/favicon-32x32.png""><link rel=""icon"" type=""image/png"" sizes=""16x16"" href=""/favicon-16x16.png""><link rel=""manifest"" href=""/site.webmanifest""> <link rel=""mask-icon"" href=""/safari-pinned-tab.svg"" color=""#5bbad5""> <meta name=""msapplication-TileColor"" content=""#da532c""> <meta name=""theme-color"" content=""#ffffff""> <meta name=""viewport"" content=""width=device-width, initial-scale=1""> <meta charset=""utf-8"" /> <title>{title} - podcast med Henrik Andersson och Anders Hesselbom</title>
<style>
html, body {{ border: 0; margin: 0; padding: 0; background-color: #ddd; color: #333; font-family: arial, sans-serif; }} div {{ text-align: center; margin: 0 auto 0 auto; padding: 10px 0 5px 0; width: 50%; min-width: 400px; max-width: 1000px; }} h1 {{ margin: 0; padding: 5px 0 5px 0; text-align: center; font-size: 50px; font-weight: normal; color: #111; display: none; }} .logo {{ display: block; padding: 0; margin: 0 auto 0 auto; width: 100%; height: auto; max-width: 500px; text-align: center; }} p {{ margin: 0; padding: 5px 0 5px 0; }} a {{ color: #007; text-decoration: none; }} a:hover {{ color: #11a; }} .tagline {{ padding: 5px 0 15px 0; font-style: italic; }} .headblock {{ padding: 5px 0 15px 0; font-weight: bold; }} .footblock {{ padding: 15px 0 5px 0; }}
</style>
</head>
<body>
<div>
<h1>{title}</h1><img src=""logo.png"" alt=""{title}"" class=""logo"" /><p class=""tagline"">Podcast med Henrik Andersson och Anders Hesselbom</p><p><img src=""inteensingel.jpg"" style=""width: 100%; height: auto;""/></p><p class=""headblock"">Vi lyssnar framgångsrik musik från etablerade artister, men vi hoppar över det som släpptes på singel. Vad finns mer, förutom det som spelas på radio? Finns där poddar finns, men inte på Spotify för någon ordning vill vi ha.</p>";
const string websiteLinks = @"<div style=""border-top: 1px solid #777777; margin-top: 30px; margin-bottom: 30px; padding-top: 30px;"">
    <a href=""https://ahesselbom.se/"" target=""_blank"" style=""padding-right: 30px;"">https://ahesselbom.se/</a><a href=""https://heltperfekt.com/"" target=""_blank"" style=""padding-left: 30px;"">https://heltperfekt.com/</a>
</div>";
const string websiteFoot = $@"<p class=""footblock""><!--PAGINATION--><br/><br/><b>RSS:</b> <a href=""{rss}"" target=""_blank"">{rss}</a> <br /><br /><b>YouTube:</b> <a href=""https://youtube.com/@inteensingel4131/videos"" target=""_blank"">Inte en singel</a> <br /><br /><b>Henrik Andersson på Twitter:</b> <a href=""https://twitter.com/commoflage_"" target=""_blank"">@commoflage_</a> <br /><b>Anders Hesselbom på Twitter:</b> <a href=""https://twitter.com/ahesselbom"" target=""_blank"">@ahesselbom</a></p></div>{websiteLinks}</body></html>";


var pagesCount = (int)Math.Ceiling(episodes.Count / 10.0);
var count = episodes.Count;
var index = 0;

var options = new FileStreamOptions
{
    Access = FileAccess.Write,
    Mode = FileMode.Create
};

for (var pageIndex = 0; pageIndex < pagesCount; pageIndex++)
{
    var filename = "https://inte_en_singel.80tal.se/";

    if (pageIndex > 0)
        filename = $"page{pageIndex:00}.html";

    using var sw = new StreamWriter(Path.Combine(localOutput, filename.StartsWith("http") ? "index.html" : filename), Encoding.UTF8, options);
    sw.Write(websiteHead);
    for (var i = 0; i < 10; i++)
    {
        var episode = episodes[index];
        Console.WriteLine($"{count:000}: {episode}");
        sw.Write($@"<p style=""font-weight: Thin; font-size: 25px;""><a href=""https://inte_en_singel.80tal.se/mp3/inteensingel{count:00}.mp3"" target=""_blank"">{count}. ({episode.PublishedDate:yyyy-MM-dd}) {episode.Title}</a> ({episode.Length})</p>");
        count--;
        index++;

        if (count <= 0)
            break;
    }
    sw.Write(websiteFoot.Replace("<!--PAGINATION-->", GetPagination(pageIndex, pagesCount)));
    sw.Flush();
    sw.Close();
    Thread.Sleep(100);
}

{
    using var sw = new StreamWriter(Path.Combine(localOutput, "all.html"), Encoding.UTF8, options);
    sw.Write(websiteHead);
    count = episodes.Count;

    foreach (var episode in episodes)
    {
        sw.Write($@"<p style=""font-weight: Thin; font-size: 21px;""><a href=""https://inte_en_singel.80tal.se/mp3/inteensingel{count:00}.mp3"" target=""_blank"">{count}. {episode.Title}</a> ({episode.Length})</p>");
        count--;
    }

    sw.Write(websiteFoot.Replace("<!--PAGINATION-->", GetPagination(-1, pagesCount)));
    sw.Flush();
    sw.Close();
    Thread.Sleep(100);
}

const string tagline = $"Podcasten {title} - om musiken som melodiradion glömde. Anders Hesselbom och Henrik Andersson lyssnar på låtarna som aldrig blev någon singel.";
const string authors = "Anders Hesselbom, Henrik Andersson";
const string baseUrl = "https://www.80tal.se/inte_en_singel/";
const string imageUrl = $"{baseUrl}inte_en_singel.jpg";
const string rssUrl = $"{baseUrl}rss.xml";

var rssHead = $@"<rss xmlns:content=""http://purl.org/rss/1.0/modules/content/"" xmlns:wfw=""http://wellformedweb.org/CommentAPI/"" xmlns:dc=""http://purl.org/dc/elements/1.1/"" xmlns:atom=""http://www.w3.org/2005/Atom"" xmlns:sy=""http://purl.org/rss/1.0/modules/syndication/"" xmlns:slash=""http://purl.org/rss/1.0/modules/slash/"" xmlns:itunes=""http://www.itunes.com/dtds/podcast-1.0.dtd"" xmlns:rawvoice=""http://www.rawvoice.com/rawvoiceRssModule/"" xmlns:googleplay=""http://www.google.com/schemas/play-podcasts/1.0""  version=""2.0"">
<channel>
<title>{title}</title>
<category>Music</category>
<atom:link href=""https://80tal.se/inte_en_singel/rss.xml"" type=""application/rss+xml""/>
<link>{baseUrl}</link>
<description>{tagline}</description>
<lastBuildDate>{DateTime.Now:r}</lastBuildDate>
<language>sv-SE</language>
<sy:updatePeriod>weekly</sy:updatePeriod>
<sy:updateFrequency>1</sy:updateFrequency>
<generator>Custom</generator>
<itunes:summary>{tagline}</itunes:summary>
<itunes:author>{authors}</itunes:author>
<itunes:explicit>False</itunes:explicit>
<itunes:image href=""{imageUrl}""/>
<itunes:owner>
<itunes:name>{authors}</itunes:name>
<itunes:email>{authorEmailWithName}</itunes:email>
</itunes:owner>
<managingEditor>{authorEmailWithName}</managingEditor>
<copyright>{authors}</copyright>
<itunes:subtitle>{title}</itunes:subtitle>
<image>
<title>{title}</title>
<url>{imageUrl}</url>
<link>{baseUrl}</link>
</image>
<itunes:category text=""Music""></itunes:category>
<googleplay:email>{authorEmail}</googleplay:email>
<googleplay:description>{tagline}</googleplay:description>
<googleplay:explicit>No</googleplay:explicit>
<googleplay:category text=""Music""/>
<googleplay:image href=""{imageUrl}""/>
<rawvoice:rating>TV-G</rawvoice:rating>
<rawvoice:frequency>Weekly</rawvoice:frequency>
<rawvoice:subscribe feed=""{rssUrl}"" googleplay=""{rssUrl}""/>";

const string rssFoot = "</channel></rss>";

using var swRss = new StreamWriter(Path.Combine(localOutput, "rss.xml"), Encoding.UTF8, options);
swRss.Write(rssHead);

count = episodes.Count;
var revCount = episodes.Count;
foreach (var episode in episodes)
{
    var url = $"{baseUrl}mp3/inteensingel{count:00}.mp3";
    var localFile = $@"C:\Users\hbom\Dropbox\InteEnSingel\Output\mp3\inteensingel{count:00}.mp3";
    var episodeTitle = $"Avsnitt {count:00}: {episode.Title}";
    var episodeDescription = $"I avsnitt {count} lyssnar Henrik och Anders på de låtar från {episode.Title} som aldrig släpptes på singel.";

    swRss.Write($@"<item>
<title>{episodeTitle}</title>
<link>{baseUrl}</link>
<pubDate>{episode.PublishedDate:r}</pubDate>
<guid isPermaLink=""false"">{baseUrl}#{revCount--}</guid>
<description>{episodeDescription}</description>
<content:encoded>
<![CDATA[ <p>{episodeDescription}</p> ]]>
</content:encoded>
<enclosure url=""{url}"" length=""{GetLengthInBytes(localFile)}"" type=""audio/mpeg""/>
<itunes:subtitle>{episodeTitle}</itunes:subtitle>
<itunes:summary>{episodeTitle}</itunes:summary>
<itunes:author>Anders Hesselbom, Henrik Andersson</itunes:author>
<itunes:image href=""{imageUrl}""/>
<itunes:explicit>False</itunes:explicit>
<itunes:duration>00:{episode.Length}</itunes:duration>
</item>");
    count--;
}

swRss.Write(rssFoot);
swRss.Flush();
swRss.Close();
return;

static int GetLengthInBytes(string localFile)
{
    var fi = new FileInfo(localFile);
    return fi.Exists ? (int)fi.Length : 0;
}

static string GetPagination(int pageIndex, int pageCount)
{
    var s = new StringBuilder();

    if (pageIndex < 0)
    {
        for (var i = 0; i < pageCount; i++)
        {
            var filename = "https://inte_en_singel.80tal.se/";

            if (i > 0)
                filename = $"page{i:00}.html";

            s.Append($@"<a href=""{filename}"">[Sida {i + 1}]</a> ");
        }

        s.Append("<b>[Alla avsnitt]</b>");
        return s.ToString();
    }

    for (var i = 0; i < pageCount; i++)
    {
        var filename = "https://inte_en_singel.80tal.se/";

        if (i > 0)
            filename = $"page{i:00}.html";

        if (i == pageIndex)
            s.Append($"<b>[Sida {i + 1}]</b> ");
        else
            s.Append($@"<a href=""{filename}"">[Sida {i + 1}]</a> ");
    }

    s.Append(@"<a href=""all.html"">[Alla avsnitt]</a>");

    return s.ToString();
}