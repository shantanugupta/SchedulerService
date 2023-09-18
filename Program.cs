using Microsoft.AspNetCore.Mvc;
using System.Reflection;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        // Add services to the container.


        #region Builder configurations
        //Add filters 
        builder.Services.AddControllers(option =>
        {
            //option.Filters.Add(typeof(RequireJsonContentTypeAttribute));
            option.Filters.Add(new ProducesAttribute("application/json"));
        });

        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(setup =>
        {
            setup.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
            {
                Title = "Scheduler Backend service for React-Scheduler",
                Description = "Use this API to integrate it with React-Scheduler. Use this API to save data to 3rd party store",
                Version = "v1",
                Contact = new Microsoft.OpenApi.Models.OpenApiContact
                {
                    Email = "shantanufrom4387@gmail.com",
                    Name = "Shantanu Gupta",
                    Url = new Uri("https://linkedin.com/in/shantanufrom4387")
                }
            });


            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            setup.IncludeXmlComments(xmlPath);
        });

        //builder.Services.AddCors(p => p.AddPolicy("corspolicy", build =>
        //{
        //    build.WithOrigins("http://localhost:3000").AllowAnyMethod().AllowAnyHeader();
        //}));

        #endregion

        var app = builder.Build();

        #region App configuration
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            //app.UseCors(p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
        }

        app.UseCors(p => p.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
        //app.UseCors("corspolicy");

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        #endregion

        app.Run();
    }
}