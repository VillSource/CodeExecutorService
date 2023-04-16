using CodeExecutorService.Hubs;
using CodeExecutorService.Services.CodeRunners;
using CodeExecutorService.Services.CodeRunners.Interfaces;
using CodeExecutorService.Services.FileSavers;
using CodeExecutorService.Services.FileSavers.Interfaces;
using CodeExecutorService.Services.ProcessManagers;
using CodeExecutorService.Services.ProcessManagers.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();

// Add my Service
builder.Services.AddSingleton<IProcessManagerService,ProcessManagerService>();

// Add my Factoris
builder.Services.AddSingleton<IFileSaverFactory,FileSaverFactory>();
builder.Services.AddSingleton<ICodeRunnerFactory,CodeRunnerFactory>();

// Add dev Cors
builder.Services.AddCors(options => options
    .AddPolicy("dev", policyBuilder => policyBuilder
        .WithOrigins("http://localhost:4200")
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials()
    )
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseCors("dev");
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapHub<IODeliver>("/io-deliver");

app.Run();
