using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Crud.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.EntityFrameworkCore;

namespace Crud.Pages.Galery
{
    public class CreateModel : PageModel
    {
        private readonly Crud.Models.ApplicationDbContext _db;
        private readonly IWebHostEnvironment _iweb;
        public CreateModel(Crud.Models.ApplicationDbContext db, IWebHostEnvironment iweb)
        {
            _db = db;
            _iweb = iweb;
        }

        [BindProperty]
        public Galeria Galeria { get; set; }

        public async Task<IActionResult> OnPostAsync(IFormFile uploadfiles, Galeria img, string imgtitle)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            string imgext = Path.GetExtension(uploadfiles.FileName);
            if(imgext==".jpg" || imgext==".gif" || imgext == ".jpeg" || imgext == ".png")
            {
                
                var imgsave = Path.Combine(_iweb.WebRootPath, "Images", uploadfiles.FileName);
                var stream = new FileStream(imgsave, FileMode.Create);
                await uploadfiles.CopyToAsync(stream);
                stream.Close();
                img.Imgname = uploadfiles.FileName;
                img.Imgpath = imgsave;
                img.Imgtitle = Galeria.Imgtitle;
                await _db.Galeria.AddAsync(img);
                await _db.SaveChangesAsync();
            }
            return RedirectToPage("Index");
        }
    }
}
