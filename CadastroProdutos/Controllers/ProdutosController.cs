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
        private IProdutosService _produtosService;

        public ProdutosController(IProdutosService produtosService)
        {
            _produtosService = produtosService;
        }

        [HttpGet]
        public ActionResult<List<Produto>> Get()
        {
            return Ok(_produtosService.ObterTodos());
        }

        [HttpGet("{id}")]
        public ActionResult<Produto> GetById(int id)
        {
            var produto = _produtosService.ObterPorId(id);
            if(produto is null)
            {
                return NotFound($"Produto não encontrado.");
            }

            return Ok(produto);
        }

        [HttpPost]
        public ActionResult Post(Produto produto)
        {
            try
            {
                _produtosService.Adiconar(produto);
                return Created();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public ActionResult<Produto> Put(int id, Produto produtoAtualizado)
        {
            try
            {
                var produto = _produtosService.Atualizar(id, produtoAtualizado);
                if(produto is null)
                {
                    return NotFound($"Produto não encontrado.");
                }

                return Ok(produto);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public ActionResult<Produto> Delete(int id)
        {
            var produto = _produtosService.Remover(id);
            if(!produto)
            {
                return NotFound($"Produto não encontrado.");
            }

            return NoContent();
        }
    }
}