using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using A4KPI.Data;
using A4KPI.Models;
using Microsoft.AspNetCore.Http.Features;
using System.IO.Compression;
namespace A4KPI.Controllers
{
    public class UploadFileController : ApiControllerBase
    {
        private readonly IWebHostEnvironment _currentEnvironment;
        private readonly DataContext _context;

        public UploadFileController(IWebHostEnvironment currentEnvironment, DataContext context)
        {
            _currentEnvironment = currentEnvironment;
            _context = context;
        }
        [AcceptVerbs("Post")]
        public void Save(int kpiId, DateTime uploadTime)
        {
            try
            {
                if (HttpContext.Request.Form.Files.Count > 0)
                {
                    var httpPostedFile = HttpContext.Request.Form.Files["UploadFiles"];

                    if (httpPostedFile != null)
                    {
                        var fileSave = Path.Combine(_currentEnvironment.WebRootPath, "UploadedFiles");
                        var fileSavePath = Path.Combine(fileSave, httpPostedFile.FileName);
                        if (!System.IO.File.Exists(fileSavePath))
                        {
                            using (Stream fileStream = new FileStream(fileSavePath, FileMode.Create))
                            {
                                httpPostedFile.CopyTo(fileStream);

                            }

                            var item = new UploadFile();
                            item.Path = $"/UploadedFiles/{httpPostedFile.FileName}";
                            item.KPIId = kpiId;
                            var month = uploadTime.Month == 1 ? 12 : uploadTime.Month - 1;
                            var year = uploadTime.Month == 1 ? uploadTime.Year - 1 : uploadTime.Year;
                            item.UploadTime = new DateTime(year, month, 1);
                            item.CreatedTime = DateTime.MinValue;
                            _context.UploadFiles.Update(item);
                            _context.SaveChanges();
                          
                            Response.Clear();
                            Response.ContentType = "application/json; charset=utf-8";
                            Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "File uploaded succesfully";

                         
                        }
                        else
                        {
                            Response.Clear();
                            Response.StatusCode = 400;
                            Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "File already exists";
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Response.Clear();
                Response.ContentType = "application/json; charset=utf-8";
                Response.StatusCode = 400;
                Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = e.Message;
            }
        }
        [AcceptVerbs("Post")]
        public void Remove(int kpiId, DateTime uploadTime)
        {
           
            try
            {
                var fileSave = "";
                if (HttpContext.Request.Form["cancel-uploading"]== false)
                {
                    fileSave = Path.Combine(_currentEnvironment.WebRootPath, "UploadingFiles");
                }
                else
                {
                    fileSave = Path.Combine(_currentEnvironment.WebRootPath, "UploadedFiles");
                }
                var month = uploadTime.Month == 1 ? 12 : uploadTime.Month - 1;
                var year = uploadTime.Month == 1 ? uploadTime.Year - 1 : uploadTime.Year;
                var ut = new DateTime(year, month, 1);

                var fileName = HttpContext.Request.Form["UploadFiles"];

                var fileSavePath = Path.Combine(fileSave, fileName);
                if (System.IO.File.Exists(fileSavePath))
                {
                    System.IO.File.Delete(fileSavePath);
                }
              

                string path = $"/UploadedFiles/{fileName.First()}";
                var item = _context.UploadFiles.FirstOrDefault(x => x.KPIId == kpiId && x.Path == path && x.UploadTime == ut);
                _context.UploadFiles.Remove(item);
                _context.SaveChanges();
                Response.ContentType = "application/json; charset=utf-8";
                Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "File removed succesfully";
               
            }
            catch (Exception e)
            {
                HttpResponse Response = HttpContext.Response;
                Response.Clear();
                Response.StatusCode = 200;
                Response.ContentType = "application/json; charset=utf-8";
                Response.HttpContext.Features.Get<IHttpResponseFeature>().ReasonPhrase = "File removed succesfully";
            }
        }
        [HttpGet]
        public IActionResult Download(int kpiId, DateTime uploadTime)
        {

            try
            {
                var (fileType, archiveData, archiveName) = DownloadFiles(kpiId,uploadTime);

                return File(archiveData, fileType, archiveName);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        #region Download File  
        private (string fileType, byte[] archiveData, string archiveName) DownloadFiles(int kpiId , DateTime uploadTime)
        {
            var zipName = $"archive-{DateTime.Now.ToString("yyyy_MM_dd-HH_mm_ss")}.zip";

            var kpiid = kpiId;
            var month = uploadTime.Month == 1 ? 12 : uploadTime.Month - 1;
            var year = uploadTime.Month == 1 ? uploadTime.Year - 1 : uploadTime.Year;
            var ut = new DateTime(year, month, 1);

            var data = _context.UploadFiles.Where(x => x.KPIId == kpiid && x.UploadTime == ut).ToList();
            var files = data.Select(x=> x.Path ).ToList();
            
            using (var memoryStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    files.ForEach(file =>
                    {
                        string filePath = _currentEnvironment.WebRootPath + file;
                        archive.CreateEntryFromFile(filePath, System.IO.Path.GetFileName(filePath));
                      
                    });
                }

                return ("application/zip", memoryStream.ToArray(), zipName);
            }

        }
        #endregion
    }
}
