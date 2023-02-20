namespace FlowerSpot.Domain.Dtos;
public class QuoteDto
{
    public Success Success { get; set; }

    public Contents Contents { get; set; }

    public string BaseUrl { get; set; }

    public Copyright Copyright { get; set; }
}

public class Success
{
    public int Total { get; set; }
}

public class Contents
{
    public IReadOnlyCollection<QuoteHolder> Quotes { get; set; }
}

public class QuoteHolder
{
    public string Id { get; set; }

    public string Author { get; set; }

    public string Quote { get; set; }

    public IReadOnlyCollection<string> Tags { get; set; }

    public string Image { get; set; }

    public string Length { get; set; }

    public string Category { get; set; }

    public string Language { get; set; }

    public DateTime Date { get; set; }

    public string Permalink { get; set; }

    public string Background { get; set; }

    public string Title { get; set; }
}

public class Copyright
{
    public int Year { get; set; }

    public string Url { get; set; }
}