using Ganss.XSS;

namespace Zvezdichka.Web.Infrastructure.Extensions.Helpers.Html
{
    public class HtmlService : IHtmlService
    {
        private readonly IHtmlSanitizer htmlSanitizer;

        public HtmlService(IHtmlSanitizer html)
        {
            this.htmlSanitizer = html;
            this.htmlSanitizer.AllowedAttributes.Add("class");
        }

        public string Sanitize(string content)
            => this.htmlSanitizer.Sanitize(content);
    }
}
