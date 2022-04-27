using Microsoft.AspNetCore.Http;
using StudentRegistration.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace StudentRegistration.ViewModels
{
    public class StudentViewModel
    {
        public Student Student { get; set; }

        [NotMapped]
        public IFormFile File { get; set; }
    }
}
