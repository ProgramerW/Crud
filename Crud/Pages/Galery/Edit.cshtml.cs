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
    public class EditModel : PageModel
    {
        private readonly Crud.Models.ApplicationDbContext _db;
        private readonly IWebHostEnvironment _iweb;
        public EditModel(Crud.Models.ApplicationDbContext db, IWebHostEnvironment iweb)
        {
            _db = db;
            _iweb = iweb;
        }

        [BindProperty]
        public Galeria Galeria { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if(id==null)
            {
                return NotFound();
            }

            Galeria = await _db.Galeria.FirstOrDefaultAsync(m => m.Id == id);

            if( Galeria == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(IFormFile uploadfiles, Galeria img, string fname, int id, string imgtitle)
        {
            if(!ModelState.IsValid)
            {
                return Page();
            }
            _db.Attach(Galeria).State = EntityState.Modified;

            try
            {
                var Id = await _db.Galeria.FindAsync(id);
                _db.Galeria.Remove(Id);

                fname = Path.Combine(_iweb.WebRootPath, "Images", Id.Imgname);
                FileInfo fi = new FileInfo(fname);

                var imgsave = Id.Imgpath;
                String imgext;
                var FileUp = Id.Imgname;

                if (uploadfiles != null)
                {
                    if (fi.Exists)
                    {
                        System.IO.File.Delete(fname);
                        fi.Delete();
                    }
                    imgext = Path.GetExtension(uploadfiles.FileName);

                    if (imgext == ".jpg" || imgext == ".gif" || imgext == ".jpeg" || imgext == ".png")
                    {
                        imgsave = Path.Combine(_iweb.WebRootPath, "Images", uploadfiles.FileName);
                        var stream = new FileStream(imgsave, FileMode.Create);
                        await uploadfiles.CopyToAsync(stream);
                        stream.Close();
                        img.Id = id;
                        img.Imgname = uploadfiles.FileName;
                        img.Imgpath = imgsave;
                        img.Imgtitle = Galeria.Imgtitle;
                        _db.Update(img);
                        await _db.SaveChangesAsync();
                    }

                }
                else
                {
                    imgext = Path.GetExtension(Id.Imgname);

                    if (imgext == ".jpg" || imgext == ".gif")
                    {
                        img.Id = id;
                        img.Imgname = FileUp;
                        img.Imgpath = imgsave;
                        img.Imgtitle = Galeria.Imgtitle;
                       _db.Update(img);
                        await _db.SaveChangesAsync();

                    }
                }

            }

            catch(DbUpdateConcurrencyException)
            {

            }

            return RedirectToPage("Index");
        }
    }
}
