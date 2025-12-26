using System.Text.Json;

namespace SocialMedia.mappers
{
    public class PostContentBuilder
    {
        public static JsonDocument Build(JsonElement body)
        {
            var markdown = ExtractMarkdown(body);
            var wordCount = CalculateWordCount(markdown);

            var contentObject = new
            {
                meta = new
                {
                    wordCount
                },
                body = JsonSerializer.Deserialize<object>(body.GetRawText())
            };

            return JsonDocument.Parse(JsonSerializer.Serialize(contentObject));
        }

        private static string ExtractMarkdown(JsonElement body)
        {
            if (!body.TryGetProperty("markdown", out var markdownElement))
                throw new ArgumentException("Body must contain 'markdown' property.");

            return markdownElement.GetString() ?? string.Empty;
        }

        private static int CalculateWordCount(string markdown)
        {
            return markdown
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Length;
        }
    }
}

