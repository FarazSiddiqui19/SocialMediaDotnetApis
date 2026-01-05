namespace SocialMedia.models
{
    public class PostContent
    {
        public Meta meta { get; set; } = new Meta();

        public Markdown markdown { get; set; } = new Markdown();
    }


    public class Meta
    {
        public int wordCount { get; set; }
    }

    public class Markdown
    {
        public string content { get; set; } = string.Empty;
    }   
}
