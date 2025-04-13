using Xunit;
using Microsoft.AspNetCore.Mvc;
using Moq;
using LojaViva.API.Controllers;
using LojaViva.API.Repositories;
using LojaViva.API.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;

public class ProdutoControllerTests
{
    private readonly ProdutoController _controller;
    private readonly Mock<IProdutoRepository> _mockRepository;
    private readonly Mock<ILogger<ProdutoController>> _mockLogger;
    private readonly Mock<IMemoryCache> _mockCache;

    public ProdutoControllerTests()
    {
        // Criar mocks
        _mockRepository = new Mock<IProdutoRepository>();
        _mockLogger = new Mock<ILogger<ProdutoController>>();
        _mockCache = new Mock<IMemoryCache>();

        // Configurar os métodos simulados do repositório
        _mockRepository.Setup(repo => repo.GetAll()).Returns(new List<Produto>
        {
            new Produto { Id = 1, Nome = "Produto A", Preco = 10.99M, Estoque = 20 },
            new Produto { Id = 2, Nome = "Produto B", Preco = 20.50M, Estoque = 50 },
        });

        // Configurar o mock de cache
        object cacheEntry;
        _mockCache.Setup(c => c.TryGetValue(It.IsAny<object>(), out cacheEntry)).Returns(false);

        // Injetar os mocks no controlador
        _controller = new ProdutoController(_mockRepository.Object, _mockLogger.Object, _mockCache.Object);
    }

    [Fact]
    public void GetAll_ReturnsOkResult_WithListOfProdutos()
    {
        // Act
        var result = _controller.GetAll();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var produtos = Assert.IsType<List<Produto>>(okResult.Value);
        Assert.Equal(2, produtos.Count);
    }

    [Fact]
    public void GetById_ReturnsOkResult_WithProduto()
    {
        // Configurar o mock para retornar um produto específico
        _mockRepository.Setup(repo => repo.GetById(1)).Returns(new Produto { Id = 1, Nome = "Produto A", Preco = 10.99M, Estoque = 20 });

        // Act
        var result = _controller.GetById(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var produto = Assert.IsType<Produto>(okResult.Value);
        Assert.Equal("Produto A", produto.Nome);
    }

    [Fact]
    public void GetById_ReturnsNotFound_WhenProdutoDoesNotExist()
    {
        // Configurar o mock para retornar null quando o produto não for encontrado
        _mockRepository.Setup(repo => repo.GetById(It.IsAny<int>())).Returns((Produto?)null);

        // Act
        var result = _controller.GetById(99);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void Add_ReturnsCreatedResult()
    {
        // Configurar o mock para adicionar um produto
        var newProduto = new Produto { Id = 3, Nome = "Produto C", Preco = 30.99M, Estoque = 40 };
        _mockRepository.Setup(repo => repo.Add(newProduto));

        // Act
        var result = _controller.Add(newProduto);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        var produto = Assert.IsType<Produto>(createdResult.Value);
        Assert.Equal("Produto C", produto.Nome);
    }

    [Fact]
    public void Update_ReturnsNoContent_WhenProdutoExists()
    {
        // Configurar o mock para retornar um produto existente
        var existingProduto = new Produto { Id = 1, Nome = "Produto A", Preco = 10.99M, Estoque = 20 };
        _mockRepository.Setup(repo => repo.GetById(1)).Returns(existingProduto);
        _mockRepository.Setup(repo => repo.Update(existingProduto));

        // Act
        var result = _controller.Update(1, existingProduto);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public void Update_ReturnsNotFound_WhenProdutoDoesNotExist()
    {
        // Configurar o mock para retornar null
        _mockRepository.Setup(repo => repo.GetById(It.IsAny<int>())).Returns((Produto?)null);

        // Act
        var result = _controller.Update(99, new Produto { Id = 99 });

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void Delete_ReturnsNoContent_WhenProdutoExists()
    {
        // Configurar o mock para retornar um produto existente
        _mockRepository.Setup(repo => repo.GetById(1)).Returns(new Produto { Id = 1, Nome = "Produto A", Estoque = 20 });
        _mockRepository.Setup(repo => repo.Delete(1));

        // Act
        var result = _controller.Delete(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public void Delete_ReturnsNotFound_WhenProdutoDoesNotExist()
    {
        // Configurar o mock para retornar null quando o produto não existir
        _mockRepository.Setup(repo => repo.GetById(It.IsAny<int>())).Returns((Produto?)null);

        // Act
        var result = _controller.Delete(99);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}
