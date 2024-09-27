using System.Text;
using InteEnSingelGenerator;

// Input, output and podcast name.
const string sourceFile = @"C:\Users\hbom\OneDrive\InteEnSingel\source.txt";
const string localOutput = @"C:\Users\hbom\OneDrive\InteEnSingel\Output"; // Note: No ending slash.
const string title = "Inte en singel";
const string authorEmail = "anders@winsoft.se";
const string authorEmailWithName = $"{authorEmail} (Anders Hesselbom)";
const string mp3Filename = "inteensingel";
const string podcastCategory = "Music";
const string youTubeChannel = "https://youtube.com/@inteensingel4131/videos";

// The URL used for marketing to listeners.
const string baseUrlForVisitors = "https://inte_en_singel.80tal.se/";

// The URL to the RSS when uploaded.
const string rss = "https://80tal.se/inte_en_singel/rss.xml"; //

// The URL that is covered by the SSL certificate.
const string baseUrl = "https://www.80tal.se/inte_en_singel/";

StringList showHosts = ["Henrik Andersson", "Anders Hesselbom"];

var source = File.ReadAllLines(sourceFile);

var episodes = (
    source
        .Where(s => !string.IsNullOrWhiteSpace(s) && !s.Trim().StartsWith("#"))
        .Select(Episode.Parse)
    ).ToList();

// The HTML template for the landing site.
var websiteHead = $@"<!DOCTYPE html>
<html lang=""sv"" xmlns=""http://www.w3.org/1999/xhtml"">
<head>
<link rel=""apple-touch-icon"" sizes=""180x180"" href=""/apple-touch-icon.png""><link rel=""icon"" type=""image/png"" sizes=""32x32"" href=""/favicon-32x32.png"">
<link rel=""icon"" type=""image/png"" sizes=""16x16"" href=""/favicon-16x16.png""><link rel=""manifest"" href=""/site.webmanifest"">
<link rel=""mask-icon"" href=""/safari-pinned-tab.svg"" color=""#5bbad5""> <meta name=""msapplication-TileColor"" content=""#da532c"">
<meta name=""theme-color"" content=""#ffffff""> <meta name=""viewport"" content=""width=device-width, initial-scale=1"">
<meta charset=""utf-8"" />
<title>{title} - podcast med {showHosts.SpeakList()}</title>
<style>
html, body {{ border: 0; margin: 0; padding: 0; background-color: #ddd; color: #333; font-family: arial, sans-serif; }}
div {{ text-align: center; margin: 0 auto 0 auto; padding: 10px 0 5px 0; width: 50%; min-width: 400px; max-width: 1000px; }}
h1 {{ margin: 0; padding: 5px 0 5px 0; text-align: center; font-size: 50px; font-weight: normal; color: #111; display: none; }}
.logo {{ display: block; padding: 0; margin: 0 auto 0 auto; width: [LOGO-SIZE]; height: auto; max-width: 500px; text-align: center; }}
p {{ margin: 0; padding: 5px 0 5px 0; }} a {{ color: #007; text-decoration: none; }} a:hover {{ color: #11a; }}
.tagline {{ padding: 5px 0 15px 0; font-style: italic; }} .headblock {{ padding: 5px 0 15px 0; font-weight: bold; }}
.footblock {{ padding: 15px 0 5px 0; }}
table {{ border: none; margin: 0; padding: 0; width: 100%; }}
td {{ vertical-align: top; text-align: center; margin: 2px; padding: 2px; font-weight: Thin; font-size: 20px; }}
</style>
</head>
<body>
<div>
<h1>{title}</h1><img src=""logo.png"" alt=""{title}"" class=""logo"" />
<p class=""tagline"">Podcast med {showHosts.SpeakList()}</p><p><img src=""inteensingel2.jpg"" alt=""{title}"" style=""width: 100%; height: auto;""/></p>
<p class=""headblock"">Vi lyssnar framgångsrik musik från etablerade artister, men vi hoppar över det som släpptes på singel. Vad finns mer, förutom det som spelas på radio? Här får du svaret! Finns där poddar finns, men inte på Spotify, för någon ordning vill vi ha.</p>";

// HTML template for the episode.
var episodeSiteHead = $@"<!DOCTYPE html>
<html lang=""sv"" xmlns=""http://www.w3.org/1999/xhtml"">
<head>
<link rel=""apple-touch-icon"" sizes=""180x180"" href=""../apple-touch-icon.png""><link rel=""icon"" type=""image/png"" sizes=""32x32"" href=""../favicon-32x32.png"">
<link rel=""icon"" type=""image/png"" sizes=""16x16"" href=""../favicon-16x16.png""><link rel=""manifest"" href=""../site.webmanifest""> <link rel=""mask-icon"" href=""../safari-pinned-tab.svg"" color=""#5bbad5"">
<meta name=""msapplication-TileColor"" content=""#da532c"">
<meta name=""theme-color"" content=""#ffffff"">
<meta name=""viewport"" content=""width=device-width, initial-scale=1"">
<meta charset=""utf-8"" /> <title><!--EPISODE_TITLE--> - {title} - podcast med {showHosts.SpeakList()}</title>
<style>
html, body {{ border: 0; margin: 0; padding: 0; background-color: #ddd; color: #333; font-family: arial, sans-serif; }} div {{ text-align: center; margin: 0 auto 0 auto; padding: 10px 0 5px 0; width: 50%; min-width: 400px; max-width: 1000px; }}
h1 {{ margin: 0; padding: 5px 0 5px 0; text-align: center; font-size: 50px; font-weight: normal; color: #111; display: none; }} .logo {{ display: block; padding: 0; margin: 0 auto 0 auto; width: 70%; height: auto; max-width: 500px; text-align: center; }} p {{ margin: 0; padding: 5px 0 5px 0; }} a {{ color: #007; text-decoration: none; }} a:hover {{ color: #11a; }} .tagline {{ padding: 5px 0 15px 0; font-style: italic; }} .headblock {{ padding: 5px 0 15px 0; font-weight: bold; }} .footblock {{ padding: 15px 0 5px 0; }}
table {{ border: none; margin: 0; padding: 0; width: 100%; }} td {{ vertical-align: top; text-align: center; margin: 2px; padding: 2px; font-weight: Thin; font-size: 20px; }}
</style>
</head>
<body>
<div>
<h1>{title}</h1><img src=""../logo.png"" alt=""{title}"" class=""logo"" />
<p class=""tagline"">Podcast med {showHosts.SpeakList()}</p><p><img src=""./cover.jpg"" style=""width: 100%; height: auto; max-width: 250px; max-height: 250px;""/></p>
<p class=""headblock"">I avsnitt <!--COUNT--> av podcasten med musik som melodiradion glömde lyssnar Henrik och Anders på de låtar från <!--EPISODE_TITLE--> som aldrig släpptes på singel</p>";


const string websiteLinks = @"<div style=""border-top: 1px solid #777777; margin-top: 30px; margin-bottom: 30px; padding-top: 30px;"">
    <a href=""https://ahesselbom.se/"" target=""_blank"" style=""padding-right: 30px;"">https://ahesselbom.se/</a><a href=""https://heltperfekt.com/"" target=""_blank"" style=""padding-left: 30px;"">https://heltperfekt.com/</a>
</div>";

const string youTubeLink = $@"<b>YouTube:</b> <a href=""{youTubeChannel}"" target=""_blank"">{title}</a><br /><br />";

var websiteFootWithPagination = $@"<p class=""footblock""><!--PAGINATION--><br/><br/><b>RSS:</b> <a href=""{rss}"" target=""_blank"">{rss}</a><br /><br />
{(string.IsNullOrWhiteSpace(youTubeChannel) ? "" : youTubeLink)}
Bjud på en kopp kaffe (20:-) som tack för bra innehåll!<br /><br /><img src=""https://ahesselbom.se/img/swish.png"" style=""width: 30%; height: auto; min-width: 100px; max-width: 300px;""><br /><br />
<b>Henrik Andersson på X (Twitter):</b> <a href=""https://twitter.com/commoflage_"" target=""_blank"">@commoflage_</a><br />
<b>Anders Hesselbom på X (Twitter):</b> <a href=""https://twitter.com/ahesselbom"" target=""_blank"">@ahesselbom</a></p></div>{websiteLinks}</body></html>";

var websiteFootWithoutPagination = $@"<p class=""footblock""><b><a href=""https://inte_en_singel.80tal.se/"">Tillbaka till startsidan</a></b><br /><br /><b>RSS:</b> <a href=""{rss}"" target=""_blank"">{rss}</a><br /><br />
{(string.IsNullOrWhiteSpace(youTubeChannel) ? "" : youTubeLink)}
Bjud på en kopp kaffe (20:-) som tack för bra innehåll!<br /><br /><img src=""https://ahesselbom.se/img/swish.png"" style=""width: 30%; height: auto; min-width: 100px; max-width: 300px;""><br /><br />
<b>Henrik Andersson på X (Twitter):</b> <a href=""https://twitter.com/commoflage_"" target=""_blank"">@commoflage_</a><br />
<b>Anders Hesselbom på X (Twitter):</b> <a href=""https://twitter.com/ahesselbom"" target=""_blank"">@ahesselbom</a></p></div>{websiteLinks}</body></html>";

// The pagination system will have 10 episodes per page.
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
    var filename = baseUrlForVisitors;

    if (pageIndex > 0)
        filename = $"page{pageIndex:00}.html";

    using var sw = new StreamWriter(Path.Combine(localOutput, filename.StartsWith("http") ? "index.html" : filename), Encoding.UTF8, options);
    sw.Write(websiteHead.Replace("[LOGO-SIZE]", index == 0 ? "100%" : "90%"));

    // Each episode on a normal list page (10 episodes);
    
    sw.WriteLine("<table>");

    for (var i = 0; i < 10; i++)
    {
        var episode = episodes[index];
        Console.WriteLine($"{count:000}: {episode}");
        sw.Write("<tr>");
        sw.Write($@"<td style=""white-space: nowrap;"" >{count}</td>");
        sw.Write($@"<td style=""white-space: nowrap; font-size: smaller; padding-top: 8px;"">{episode.PublishedDate:yyyy-MM-dd}</td>");
        var imageThumbFilename = Path.Combine(localOutput, $"ep\\{count:00}_.jpg");
        var imageFilename = Path.Combine(localOutput, $"ep\\{count:00}.jpg");
        
        if (File.Exists(imageThumbFilename))
            sw.Write($@"<td><a href=""ep/{count:00}.html""><img src=""ep/{count:00}_.jpg"" style=""width: 24px; height: 24px;"" alt=""{episode.Title}"" /></a></td>");
        else if (File.Exists(imageFilename))
            sw.Write($@"<td><a href=""ep/{count:00}.html""><img src=""ep/{count:00}.jpg"" style=""width: 24px; height: 24px;"" alt=""{episode.Title}"" /></a></td>");
        else
            sw.Write("<td></td>");

        sw.Write($@"<td><a href=""ep/{count:00}.html"">{episode.Title}</a></td>");
        sw.Write($@"<td style=""white-space: nowrap; font-size: smaller; padding-top: 8px;"">{episode.Length}</td>");
        sw.Write($@"<td><a href=""{baseUrlForVisitors}mp3/{mp3Filename}{count:00}.mp3""><img src=""mp3.png"" style=""width: 24px; height: 24px;"" alt=""Lyssna direkt..."" /></a></td>");

        if (episode.YouTube.Length > 4)
            sw.Write($@"<td><a href=""https://www.youtube.com/watch?v={episode.YouTube}"" target=""_blank""><img src=""youtube.png"" style=""width: 24px; height: 24px;"" alt=""Spela på YouTube..."" /></a></td>");
        else
            sw.Write("<td></td>");

        sw.Write("</tr>");
        count--;
        index++;

        if (count <= 0)
            break;
    }

    sw.WriteLine("</table>");
    sw.Write(websiteFootWithPagination.Replace("<!--PAGINATION-->", GetPagination(pageIndex, pagesCount)));
    sw.Flush();
    sw.Close();
    Thread.Sleep(100);
}

{
    using var sw = new StreamWriter(Path.Combine(localOutput, "all.html"), Encoding.UTF8, options);
    sw.Write(websiteHead.Replace("[LOGO-SIZE]", "80%"));
    count = episodes.Count;

    // Each episode on the "all" page.
    foreach (var episode in episodes)
    {
        sw.Write($@"<p style=""font-weight: Thin; font-size: 18px;""><a href=""{baseUrlForVisitors}ep/{count:00}.html"">{count}. {episode.Title}</a> ({episode.Length})</p>");
        count--;
    }

    sw.Write(websiteFootWithPagination.Replace("<!--PAGINATION-->", GetPagination(-1, pagesCount)));
    sw.Flush();
    sw.Close();
    Thread.Sleep(100);
}

{
    count = episodes.Count;

    // Each episode page
    foreach (var episode in episodes)
    {
        using var sw = new StreamWriter(Path.Combine(localOutput, $"ep\\{count:00}.html"), Encoding.UTF8, options);

        var imageFilename = Path.Combine(localOutput, $"ep\\{count:00}.jpg");

        if (File.Exists(imageFilename))
            imageFilename = $"./{count:00}.jpg";
        else
            imageFilename = "../inteensingel.jpg";

        sw.Write(episodeSiteHead.Replace("<!--EPISODE_TITLE-->", episode.Title).Replace("<!--COUNT-->", count.ToString()).Replace("./cover.jpg", imageFilename));
        sw.Write(@"<table style=""width: 100%"">");
        sw.Write(@"<tr><td colspan=""2"" style=""text-align: center;"">");
        sw.Write($@"<audio controls style=""width: 100%;""><source src=""../mp3/{mp3Filename}{count:00}.mp3"" type=""audio/mpeg""></audio>");
        sw.Write("</td></tr>");

        sw.Write(string.IsNullOrWhiteSpace(episode.YouTube)
            ? $@"<tr><td colspan=""2"" style=""text-align: center;""><a href=""{baseUrlForVisitors}mp3/{mp3Filename}{count:00}.mp3"" target=""_blank""><img src=""../mp3.png"" style=""width: 24px; height: 24px;"" alt=""Lyssna direkt..."" /></a></td>"
            : $@"<tr><td style=""text-align: center;""><a href=""{baseUrlForVisitors}mp3/{mp3Filename}{count:00}.mp3"" target=""_blank""><img src=""../mp3.png"" style=""width: 24px; height: 24px;"" alt=""Lyssna direkt..."" /></a></td><td style=""text-align: center;""><a href=""https://www.youtube.com/watch?v={episode.YouTube}"" target=""_blank""><img src=""../youtube.png"" style=""width: 24px; height: 24px;"" alt=""Spela på YouTube..."" /></a></td></tr>");

        sw.Write("</table>");
        sw.Write(websiteFootWithoutPagination);
        sw.Flush();
        sw.Close();
        Thread.Sleep(100);
        count--;
    }
}

// The RSS generator.
var tagline = $"Podcasten {title} - om musiken som melodiradion glömde. {showHosts.SpeakList()} lyssnar på låtarna som aldrig blev någon singel.";
var authors = showHosts.CommaList();
const string imageUrl = $"{baseUrl}inte_en_singel.jpg";

var rssHead = $@"<rss xmlns:content=""http://purl.org/rss/1.0/modules/content/"" xmlns:wfw=""http://wellformedweb.org/CommentAPI/"" xmlns:dc=""http://purl.org/dc/elements/1.1/"" xmlns:atom=""http://www.w3.org/2005/Atom"" xmlns:sy=""http://purl.org/rss/1.0/modules/syndication/"" xmlns:slash=""http://purl.org/rss/1.0/modules/slash/"" xmlns:itunes=""http://www.itunes.com/dtds/podcast-1.0.dtd"" xmlns:rawvoice=""http://www.rawvoice.com/rawvoiceRssModule/""  version=""2.0"">
<channel>
<title>{title}</title>
<category>{podcastCategory}</category>
<atom:link href=""{rss}"" rel=""self"" type=""application/rss+xml""/>
<link>{baseUrl}</link>
<description>{tagline}</description>
<lastBuildDate>{DateTime.Now.AddHours(-2):r}</lastBuildDate>
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
<itunes:category text=""{podcastCategory}""></itunes:category>
<rawvoice:rating>TV-G</rawvoice:rating>
<rawvoice:frequency>Weekly</rawvoice:frequency>
<rawvoice:subscribe feed=""{rss}"" googleplay=""{rss}""/>";

const string rssFoot = "</channel></rss>";

using var swRss = new StreamWriter(Path.Combine(localOutput, "rss.xml"), Encoding.UTF8, options);
swRss.Write(rssHead);

count = episodes.Count;
var revCount = episodes.Count;
foreach (var episode in episodes)
{
    var url = $"{baseUrl}mp3/{mp3Filename}{count:00}.mp3";
    var localFile = $@"{localOutput}\mp3\{mp3Filename}{count:00}.mp3";
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
<itunes:author>{showHosts.CommaList()}</itunes:author>
<itunes:image href=""{imageUrl}""/>
<itunes:explicit>False</itunes:explicit>
<itunes:duration>00:{episode.Length}</itunes:duration>
<YouTubeId>{episode.YouTube}</YouTubeId>
</item>");
    count--;
}

swRss.Write(rssFoot);
swRss.Flush();
swRss.Close();
return;

// Function that returns the MP3 file size in bytes.
static int GetLengthInBytes(string localFile)
{
    var fi = new FileInfo(localFile);
    return fi.Exists ? (int)fi.Length : 0;
}

// Function that returns the page selector.
static string GetPagination(int pageIndex, int pageCount)
{
    var s = new StringBuilder();

    if (pageIndex < 0)
    {
        for (var i = 0; i < pageCount; i++)
        {
            var filename = baseUrlForVisitors;

            if (i > 0)
                filename = $"page{i:00}.html";

            s.Append($@"<a href=""{filename}"">[Sida {i + 1}]</a> ");
        }

        s.Append("<b>[Alla avsnitt]</b>");
        return s.ToString();
    }

    for (var i = 0; i < pageCount; i++)
    {
        var filename = baseUrlForVisitors;

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