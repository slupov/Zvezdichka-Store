using System;
using Ganss.XSS;

namespace Zvezdichka.Web.Extensions.Helpers.Html
{
    public class HtmlService : IHtmlService
    {
        private readonly HtmlSanitizer htmlSanitizer;

        public HtmlService()
        {
            this.htmlSanitizer = new HtmlSanitizer();
            this.htmlSanitizer.AllowedAttributes.Add("class");
        }

        public string Sanitize(string content)
            => this.htmlSanitizer.Sanitize(content);
    }
}
