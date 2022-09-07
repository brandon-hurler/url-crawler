namespace URL_Crawler.Extensions
{
    public static class UriExtensions
    {
        public static string GetFullyQualifiedDomain(this string url)
        {
            if (Uri.TryCreate(url, UriKind.Absolute, out var outUri)
            && (outUri.Scheme == Uri.UriSchemeHttp || outUri.Scheme == Uri.UriSchemeHttps))
            {
                return outUri.GetLeftPart(UriPartial.Authority);
            }

            return string.Empty;
        }
    }
}