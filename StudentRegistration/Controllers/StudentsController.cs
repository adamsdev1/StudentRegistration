using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using StudentRegistration.Services.UnitOfWork;
using StudentRegistration.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using StudentRegistration.Services;
using Microsoft.AspNetCore.Http;

namespace StudentRegistration.Controllers
{
    public class StudentsController : Controller
    {
        private UnitOfWork _context;
        private readonly IHostingEnvironment _env;

        public StudentsController(IHostingEnvironment env)
        {
            _context = new UnitOfWork();
            _env = env;
        }
        
        // GET: Students
        public IActionResult Index()
        {
            return View(_context.StudentRepository.GetAll());
        }

        // GET: Students/Details/5
        [HttpGet]
        public IActionResult Details(int? Id)
        {
            if (Id == null)
            {
                return NotFound();
            }

            var student = _context.StudentRepository.GetById(Id);

            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        [HttpPost]
        public IActionResult Create(StudentViewModel viewmodel)
        {
            if (ModelState.IsValid)
            {
                #region Read File Content
                var studentuploads = Path.Combine(_env.WebRootPath, "studentuploads");

                bool exists = Directory.Exists(studentuploads);

                if (!exists)
                {
                    Directory.CreateDirectory(studentuploads);
                }

                // returns the file name
                string fileName = Path.GetFileName(viewmodel.File.FileName);
                byte[] fileData;

                // create a memory stream
                using (var target = new MemoryStream())
                {
                    // copies the contents of the uploaded file to target stream
                    viewmodel.File.CopyTo(target);
                    // writes the stream contents to a byte array
                    fileData = target.ToArray();
                }

                // get the file type from the uploaded file i.e img/pdf/jpeg
                string fileType = viewmodel.File.ContentType;

                // create a new upload service
                AzureBlobStorageService azureBlobStorageService = new AzureBlobStorageService();
                
                // upload file/image/pdf to azure
                viewmodel.Student.ImagePath = azureBlobStorageService.UploadToAzureBlobStorage(viewmodel.File.FileName, fileData, fileType);
                #endregion

                _context.StudentRepository.Create(viewmodel.Student);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));

            }

            return View(viewmodel.Student);
        }

    }
}
