using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CadastroProdutos.Services
{
    public interface IProdutosService
    {
        public List<Produto> ObterTodos();
        public Produto ObterPorId(int id);
        public void Adiconar(Produto produto);
        public Produto Atualizar(int id, Produto produtoAtualizado);
        public bool Remover(int id);
    }
}