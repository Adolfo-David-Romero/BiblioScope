namespace BiblioScope.Model;

//Copy-pasted full json response and used "Edit > Paste > Paste Special: JSON as classes" to convert to class
/// <summary>API model represents the json response for a book search</summary>
public class RootObject
{
    public Data data { get; set; }
}

public class Data
{
    public Search search { get; set; }
}

public class Search
{
    public Results results { get; set; }
}

public class Results
{
    public object[] facet_counts { get; set; }
    public int found { get; set; }
    public Hits[] hits { get; set; }
    public int out_of { get; set; }
    public int page { get; set; }
    public Request_params request_params { get; set; }
    public bool search_cutoff { get; set; }
    public int search_time_ms { get; set; }
}

public class Hits
{
    public Document document { get; set; }
    public Highlight highlight { get; set; }
    public Highlights[] highlights { get; set; }
    public long text_match { get; set; }
    public Text_match_info text_match_info { get; set; }
}

public class Document
{
    public int activities_count { get; set; }
    public string[] alternative_titles { get; set; }
    public string[] author_names { get; set; }
    public bool compilation { get; set; }
    public string[] content_warnings { get; set; }
    public string[] contribution_types { get; set; }
    public Contributions[] contributions { get; set; }
    public string cover_color { get; set; }
    public string description { get; set; }
    public Featured_series featured_series { get; set; }
    public string[] genres { get; set; }
    public bool has_audiobook { get; set; }
    public bool has_ebook { get; set; }
    public string id { get; set; }
    public Image image { get; set; }
    public string[] isbns { get; set; }
    public int lists_count { get; set; }
    public string[] moods { get; set; }
    public int pages { get; set; }
    public int prompts_count { get; set; }
    public double rating { get; set; }
    public int ratings_count { get; set; }
    public string release_date { get; set; }
    public int release_year { get; set; }
    public int reviews_count { get; set; }
    public object[] series_names { get; set; }
    public string slug { get; set; }
    public string subtitle { get; set; }
    public string[] tags { get; set; }
    public string title { get; set; }
    public int users_count { get; set; }
    public int users_read_count { get; set; }
}

public class Contributions
{
    public Author author { get; set; }
    public string contribution { get; set; }
}

public class Author
{
    public int id { get; set; }
    public Image1 image { get; set; }
    public string name { get; set; }
    public string slug { get; set; }
}

public class Image1
{
    public string color { get; set; }
    public string color_name { get; set; }
    public int height { get; set; }
    public int id { get; set; }
    public string url { get; set; }
    public int width { get; set; }
}

public class Featured_series
{

}

public class Image
{
    public string color { get; set; }
    public string color_name { get; set; }
    public int height { get; set; }
    public int id { get; set; }
    public string url { get; set; }
    public int width { get; set; }
}

public class Highlight
{
    public Alternative_titles[] alternative_titles { get; set; }
    public Title title { get; set; }
}

public class Alternative_titles
{
    public string[] matched_tokens { get; set; }
    public string snippet { get; set; }
}

public class Title
{
    public string[] matched_tokens { get; set; }
    public string snippet { get; set; }
}

public class Highlights
{
    public string field { get; set; }
    public object[] matched_tokens { get; set; }
    public string snippet { get; set; }
    public int[] indices { get; set; }
    public string[] snippets { get; set; }
}

public class Text_match_info
{
    public string best_field_score { get; set; }
    public int best_field_weight { get; set; }
    public int fields_matched { get; set; }
    public string score { get; set; }
    public int tokens_matched { get; set; }
}

public class Request_params
{
    public string collection_name { get; set; }
    public int per_page { get; set; }
    public string q { get; set; }
}

