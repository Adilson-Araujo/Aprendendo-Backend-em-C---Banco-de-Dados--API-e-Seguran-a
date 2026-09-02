using CadastroProdutos.Database;
using CadastroProdutos.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(); // Adicionar Controllers 
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); // Swagger

// Injeção de Dependência
builder.Services.AddScoped<IProdutosService, ProdutosService>();

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite("Data Source = Produtos.db"));

var app = builder.Build();
app.MapControllers(); // Controllers

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Swagger
    app.UseSwaggerUI(); // Swagger
}

app.UseHttpsRedirection();

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


public class Produto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public decimal Preco { get; set; }
    public int Estoque { get; set; }
}
