using CodeExecutorService.Hubs;
using CodeExecutorService.SubProcess;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();

// Add my Service
builder.Services.AddSingleton(SubProcessIOHandelService.Instance);

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
