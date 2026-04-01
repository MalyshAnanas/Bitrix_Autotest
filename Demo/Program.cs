using Blazored.Modal;
using Demo.BaseFramework.LogTools;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace Demo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddServerSideBlazor();
            builder.Services.AddBlazoredModal();
            var currentProcessFilePath = new FileInfo(Environment.ProcessPath);
            builder.Environment.WebRootPath = Path.Combine(currentProcessFilePath.DirectoryName, "wwwroot");
            builder.Environment.ContentRootPath = currentProcessFilePath.DirectoryName;
            Environment.CurrentDirectory = currentProcessFilePath.DirectoryName;
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
            }


            app.UseStaticFiles();

            app.UseRouting();

            app.MapBlazorHub();
            app.MapFallbackToPage("/_Host");

            app.Run();
        }
    }
}
