using AutoMapper;
using DocumentsUpload.Entities;
using DocumentsUpload.Messaging;
using DocumentsUpload.Models;
using DocumentsUpload.Repository.Interface;
using DocumentsUpload.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentsUpload.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ICustomEmailSender _emailSender;
        private readonly EmailSendingChannel _emailSendingChannel;

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment env,
            IUserRepository userRepository, IMapper mapper, ICustomEmailSender emailSender,
            EmailSendingChannel emailSendingChannel)
        {
            _logger = logger;
            _env = env;
            _userRepository = userRepository;
            _mapper = mapper;
            _emailSender = emailSender;
            _emailSendingChannel = emailSendingChannel;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Upload(List<IFormFile> files, UserCreationDto userDto)
        {
            if (ModelState.IsValid)
            {
                if (_userRepository.DoesExist(x => x.Email == userDto.Email))
                    return BadRequest("Email already exist");

                var Userfiles = new List<Document>();

                foreach (var file in files)
                {
                    var ext = Path.GetExtension(file.FileName);

                    if (!IsValidFile(ext))
                        continue;

                    var path = Path.Combine(_env.WebRootPath, "uploads", file.FileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    var url = string.Format("{0}://{1}/{2}", HttpContext.Request.Scheme, HttpContext.Request.Host, "Uploads");

                    Userfiles.Add(new Document
                    {
                        Name = file.FileName,
                        Path = path,
                        Url = $"{url}/{file.FileName}"
                    });

                }

                var user = _mapper.Map<User>(userDto);
                user.Id = Guid.NewGuid();
                user.TransactionNumber = Guid.NewGuid().ToString().Replace("-","");
                user.Documents = Userfiles;

                _userRepository.Add(user);
                await _userRepository.Save();

                await _emailSendingChannel.AddMessageAsync(new EmailData
                {
                    Email = user.Email,
                    Message = "Uploaded Documents",
                    Subject = "Documents",
                    Documents = _mapper.Map<List<DocumentDto>>(user.Documents)
                });

                return Ok(user.TransactionNumber);
            }
          
            return BadRequest();
        }




        bool IsValidFile(string extention)
        {
            return Utils.FileExtensions.Contains(extention.ToUpper());
        }
        

        public IActionResult Documents()
        {
            return View(new UserDto());
        }


        [HttpPost]
        public async Task<IActionResult> Documents(UserDocuments dto)
        {
            var user = await _userRepository.GetUserWithDocuments(dto.EmailOrTransactionId);

            if(user == null)
            {
                return View(new UserDto());
            }

            return View(_mapper.Map<UserDto>(user));
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
