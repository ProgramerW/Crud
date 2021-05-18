using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Crud.Models
{
    public class Galeria
    {
        [Key]
        public int Id { get; set; }
        
        [Display(Name = "Titulo de imagen")]
        public string Imgtitle { get; set; }
       
        public string Imgname { get; set; }

        public string Imgpath { get; set; }
        

    }
}
