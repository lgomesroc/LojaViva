using LojaViva.API.Models;
using LojaViva.API.Repositories;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ProdutoController : ControllerBase
{
    private readonly IProdutoRepository _produtoRepository;

    public ProdutoController(IProdutoRepository produtoRepository)
    {
        _produtoRepository = produtoRepository;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var produtos = _produtoRepository.GetAll();
        return Ok(produtos);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var produto = _produtoRepository.GetById(id);
        if (produto == null)
        {
            return NotFound();
        }
        return Ok(produto);
    }

    [HttpPost]
    public IActionResult Add([FromBody] Produto produto)
    {
        _produtoRepository.Add(produto);
        return CreatedAtAction(nameof(GetById), new { id = produto.Id }, produto);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] Produto produto)
    {
        var existingProduto = _produtoRepository.GetById(id);
        if (existingProduto == null)
        {
            return NotFound();
        }

        produto.Id = id;
        _produtoRepository.Update(produto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var produto = _produtoRepository.GetById(id);
        if (produto == null)
        {
            return NotFound();
        }

        _produtoRepository.Delete(id);
        return NoContent();
    }
}
