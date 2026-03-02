using PedidoApi.Application;
using PedidoApi.Domain;
using PedidoApi.Infra;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IClienteRepository, ClienteRepositoryInMemory>();
builder.Services.AddScoped<IPedidoRepository, PedidoRepositoryInMemory>();
builder.Services.AddScoped<PedidoService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

app.Run();