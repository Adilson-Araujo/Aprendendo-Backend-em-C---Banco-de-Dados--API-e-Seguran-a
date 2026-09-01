using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CadastroProdutos.Services;
using Microsoft.AspNetCore.Mvc;

namespace CadastroProdutos.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProdutosController : ControllerBase
    {
        private ProdutosService produtosService = new ProdutosService();

        [HttpGet]
        public ActionResult<List<Produto>> Get()
        {
            return Ok(produtosService.ObterTodos());
        }

        [HttpGet("{id}")]
        public ActionResult<Produto> GetById(int id)
        {
            var produto = produtosService.ObterPorId(id);
            if(produto is null)
            {
                return NotFound($"Produto não encontrado.");
            }

            return Ok(produto);
        }

        [HttpPost]
        public ActionResult Post(Produto produto)
        {
            produtosService.Adiconar(produto);
            return Created();
        }

        [HttpPut("{id}")]
        public ActionResult<Produto> Put(int id, Produto produtoAtualizado)
        {
            var produto = produtosService.Atualizar(id, produtoAtualizado);
            if(produto is null)
            {
                return NotFound($"Produto não encontrado.");
            }

            return Ok(produto);
        }

        [HttpDelete("{id}")]
        public ActionResult<Produto> Delete(int id)
        {
            var produto = produtosService.Remover(id);
            if(!produto)
            {
                return NotFound($"Produto não encontrado.");
            }

            return NoContent();
        }
    }
}