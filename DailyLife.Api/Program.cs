using DailyLife.Api.Extentions;
using DailyLife.Api.Midlewares;
using DailyLife.Application;
using DailyLife.Infrastructure;
using DailyLife.Infrastructure.Settings;
using MailSettings = DailyLife.Infrastructure.Settings.MailSettings;
var builder = WebApplication.CreateBuilder(args);
// Add services to the container.
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddProblemDetails()
    .AddExceptionHandler<GlobalExceptionHandiler>();

builder.Services.AddHttpContextAccessor()
    .AddHttpClient();

builder.Services.AddMemoryCache();
builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration)
    .AddIdentity()
    .AddResponseCompressionExtention();

builder.Services.Configure<MailSettings>(builder.Configuration.GetSection(nameof(MailSettings)));
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(nameof(JwtOptions)));

builder.Services.AddCors(options =>
{
    options.AddPolicy("DailyLife", config =>
    {
        config.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();


    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("DailyLife");

app.UseStaticFiles();

app.UseStatusCodePages();

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.UseResponseCompression();

app.Run();
