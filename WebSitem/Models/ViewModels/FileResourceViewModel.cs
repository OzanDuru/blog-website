using System;
using WebSitem.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;             // IFormFile için



    public class FileResourceViewModel
{

    public string Title { get; set; }
    public string Description { get; set; }

    public IFormFile? UploadedFile { get; set; }

    [Url]
    public string? ExternalLink { get; set; }

    }




