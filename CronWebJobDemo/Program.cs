using CronWebJobDemo.Services;

var builder = WebApplication.CreateBuilder(args);

// ✅ Add WebJobs (CRON support) as a hosted service
builder.Services.AddHostedService<CronJobService>();

// ✅ Add your service
builder.Services.AddSingleton<MyService>();

// ✅ Add API services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Hosted services (like CronJobService) are started automatically by the runtime when the app starts.
app.Run();