using System.Threading.Tasks;
using Ganss.XSS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zvezdichka.Data.Models;
using Zvezdichka.Services.Contracts.Entity;

namespace Zvezdichka.Web.Areas.Comments.Controllers
{
    public class HomeController : CommentsBaseController
    {
        private readonly ICommentsDataService comments;
        private readonly IHtmlSanitizer html;

        public HomeController(ICommentsDataService comments, IHtmlSanitizer html)
        {
            this.comments = comments;
            this.html = html;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add(Comment comment)
        {
            comment.Message = this.html.Sanitize(comment.Message);

            if (this.ModelState.IsValid)
            {
                this.comments.Add(comment);
                return RedirectToAction("Details", "Home", new {area = "Products", id = 5}); //todo add correct id
            }

            return NotFound();
        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            var comment = this.comments.GetSingle(x => x.Id == id);

            if (comment == null)
            {
                return NotFound();
            }

            this.comments.Remove(comment);
            return Ok();
        }
    }
}