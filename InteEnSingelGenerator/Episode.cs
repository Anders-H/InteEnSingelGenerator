namespace InteEnSingelGenerator;

public class Episode
{
    public string Title { get; }
    public DateTime PublishedDate { get; }
    public string Length { get; }

    public Episode(string title, DateTime publishedDate, string length)
    {
        Title = title;
        PublishedDate = publishedDate;
        Length = length;
    }

    public static Episode Parse(string s)
    {
        var parts = s.Split(",");

        var title = parts[0].Trim();
        var length = parts[2].Trim();

        var dateParts = parts[1].Split("-");
        var date = new DateTime(int.Parse(dateParts[0]), int.Parse(dateParts[1]), int.Parse(dateParts[2]), 10, 0, 0);

        return new Episode(title, date, length);
    }

    public override string ToString() =>
        $"({PublishedDate:yyyy-MM-dd}) {Title} ({Length})";
}