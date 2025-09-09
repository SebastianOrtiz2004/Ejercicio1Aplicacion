using ClienteService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHttpClient<PedidoClient>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["PedidoService:BaseUrl"]);
    // En DEV, si usas http, quita https; si usas self-signed SSL, podrías configurar handler para ignorar validación (no recomendado en prod).
});

builder.Services.AddCors(p => p.AddPolicy("AllowAll", b => b.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.MapControllers();

app.Run();
