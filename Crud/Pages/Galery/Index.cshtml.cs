using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Crud.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

using Microsoft.AspNetCore.Http;
using System.IO;

namespace Crud.Pages.Galery
{
    public class IndexModel : PageModel
    {
        private readonly Crud.Models.ApplicationDbContext _db;
        private readonly IWebHostEnvironment _iweb;
        public IndexModel(Crud.Models.ApplicationDbContext db, IWebHostEnvironment iweb)
        {
            _db = db;
            _iweb = iweb;
        }
        public IEnumerable<Galeria> Galeria { get; set; }

        public async Task OnGet()
        {
            Galeria = await _db.Galeria.ToListAsync();
            
        }
        public FileResult OnGetDownloadFile(string filename)
        {
            string path = Path.Combine(_iweb.WebRootPath, "Images/")+filename;
            byte[] bytes = System.IO.File.ReadAllBytes(path);
            return File(bytes, "application/octet-stream", filename);
        }

        public async Task<IActionResult> OnGetDeleteAsync(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

             var Galeria = await _db.Galeria.FirstOrDefaultAsync(m => m.Id == id);

            if(Galeria == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int? id, string fname)
        {
            
            if (id == null)
            {
                return NotFound();
            }

            var Galeria = await _db.Galeria.FindAsync(id);
            _db.Galeria.Remove(Galeria);

            fname = Path.Combine(_iweb.WebRootPath, "Images", Galeria.Imgname);
            FileInfo fi = new FileInfo(fname);
            if(fi.Exists)
            {
                System.IO.File.Delete(fname);
                fi.Delete();
            }
            
            if(Galeria != null)
            {
                _db.Galeria.Remove(Galeria);
                await _db.SaveChangesAsync();
            }
            return RedirectToPage("Index");

        }

    }
}
