using System.ComponentModel.DataAnnotations;
using System.Text;
using CadastroProdutos.Database;
using CadastroProdutos.Models;
using CadastroProdutos.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi();
builder.Services.AddControllers(); // Adicionar Controllers 
builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen(); // Swagger
builder.Services.AddSwaggerGen(x =>
{
   x.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.OpenApiSecurityScheme()
   {
        Description = @"Cole Apenas seu token JWT",
        Name = "Authorization",
        In = Microsoft.OpenApi.ParameterLocation.Header,
        Type = Microsoft.OpenApi.SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT"
   }); 
   
   x.AddSecurityRequirement(document => new()
   {
        [new Microsoft.OpenApi.OpenApiSecuritySchemeReference("Bearer", document)] = []
   });
});

// Injeção de Dependência
// builder.Services.AddScoped<IProdutosService, ProdutosService>();
builder.Services.AddScoped<IProdutosService, ProdutosDatabaseService>();

// Arquivo do banco de dados SQLite
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite("Data Source = Produtos.db"));

// JWT
var jwtConfig = builder.Configuration.GetSection("Jwt");
var key = Encoding.ASCII.GetBytes(jwtConfig["Key"]);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtConfig["Issuer"],
        ValidAudience = jwtConfig["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});


var app = builder.Build();
app.MapControllers(); // Controllers

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Swagger
    app.UseSwaggerUI(); // Swagger
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

var produtos = new List<Produto>()
{
    new Produto(){Id = 1, Nome = "Mouse sem fio", Preco = 99.90M, Estoque = 50},
    new Produto(){Id = 2, Nome = "Teclado", Preco = 249.90M, Estoque = 30}
};

// Retorner todos os produtos
app.MapGet("/produtos", () =>
{
    return produtos;
});

// Retornar produto pelo id
app.MapGet("/produtos/{id}", (int id) =>
{
    var produto = produtos.FirstOrDefault(x => x.Id == id);
    return produto is not null 
        ? Results.Ok(produto)
        : Results.NotFound($"Produto com ID {id} não encontrado.");
        
});

// Inserir produto
app.MapPost("/produtos", (Produto produto) =>
{
    produtos.Add(produto);
    return Results.Created();
});

// Atualizar produto
app.MapPut("/produtos/{id}", (int id, Produto produtoAtualizado) =>
{
    var produto = produtos.FirstOrDefault(x => x.Id == id);
    if(produto is null)
    {
        return Results.NotFound("Produto não encontrado");
    }

    produto.Nome = produtoAtualizado.Nome;
    produto.Preco = produtoAtualizado.Preco;
    produto.Estoque = produtoAtualizado.Estoque;

    return Results.Ok(produto);
});

// Remover produto
app.MapDelete("/produtos/{id}", (int id) =>
{
    var produto = produtos.FirstOrDefault(x => x.Id == id);
    if(produto is null)
    {
        return Results.NotFound("Produto não encontrado");
    }

    produtos.Remove(produto);

    return Results.NoContent();
});

app.Run();





