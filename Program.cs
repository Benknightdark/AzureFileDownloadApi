// See https://aka.ms/new-console-template for more information
using System.Text;
using Azure.Storage.Files.Shares;
using Azure.Storage.Files.Shares.Models;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", async () =>
{
    try
    {
        await Task.Yield();
        var connectionString = "[CONNECTIONSTRING]";
        string shareName = "[SHARENAME]";
        string fileName = "[FILENAME.pdf]";
        ShareClient share = new ShareClient(connectionString, shareName);
        ShareDirectoryClient directory = share.GetDirectoryClient("");
        ShareFileClient file = directory.GetFileClient(fileName);
        ShareFileDownloadInfo download = await file.DownloadAsync();
        var stream = new MemoryStream() { Position = 0 };
        await download.Content.CopyToAsync(stream);

    return  new FileStreamResult(stream,  "application/pdf"){ FileDownloadName = fileName }; // returns a FileStreamResult//"application/octet-stream"
    }
    catch (Exception ex)
    {
        Console.WriteLine("-0----------------------------------");
        Console.WriteLine(ex.Message);
        Console.WriteLine("-0----------------------------------");
        throw ex;
    }
});

app.Run();
