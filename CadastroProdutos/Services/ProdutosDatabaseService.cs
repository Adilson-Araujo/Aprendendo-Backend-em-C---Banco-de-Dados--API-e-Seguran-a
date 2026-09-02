using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CadastroProdutos.Database;

namespace CadastroProdutos.Services
{
    public class ProdutosDatabaseService : IProdutosService
    {
        private ApplicationDbContext banco ;

        public ProdutosDatabaseService(ApplicationDbContext context)
        {
            banco = context;
        }

        public void Adiconar(Produto produto)
        {
            banco.Produtos.Add(produto);
            banco.SaveChanges();
        }

        public Produto Atualizar(int id, Produto produtoAtualizado)
        {
            var produto = banco.Produtos.FirstOrDefault(x => x.Id == id);
            if(produto is null)
            {
                return null;
            }

            produto.Nome = produtoAtualizado.Nome;
            produto.Preco = produtoAtualizado.Preco;
            produto.Estoque = produtoAtualizado.Estoque;

            banco.SaveChanges();

            return produto;
        }

        public Produto ObterPorId(int id)
        {
            return banco.Produtos.FirstOrDefault(x => x.Id == id);
        }

        public List<Produto> ObterTodos()
        {
            return banco.Produtos.ToList();
        }

        public bool Remover(int id)
        {
            var produto = banco.Produtos.FirstOrDefault(x => x.Id == id);
            if(produto is null)
            {
                return false;
            }

            banco.Produtos.Remove(produto);
            banco.SaveChanges();
            return true;
        }
    }
}