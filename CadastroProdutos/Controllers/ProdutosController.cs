using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CadastroProdutos.Models;
using CadastroProdutos.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CadastroProdutos.Controllers
{
    [Authorize]
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

        [Authorize(Roles = "admin")]
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

        [Authorize(Roles = "admin")]
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

        [Authorize(Roles = "admin")]
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