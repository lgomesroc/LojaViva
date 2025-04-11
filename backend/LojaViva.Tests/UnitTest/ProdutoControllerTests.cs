using Xunit;
using Microsoft.AspNetCore.Mvc;
using Moq;
using LojaViva.API.Controllers;
using LojaViva.API.Repositories;
using LojaViva.API.Models;
using System.Collections.Generic;

public class ProdutoControllerTests
{
    private readonly ProdutoController _controller;
    private readonly Mock<IProdutoRepository> _mockRepository;

    public ProdutoControllerTests()
    {
        // Criar um mock do repositório para simular comportamento
        _mockRepository = new Mock<IProdutoRepository>();

        // Configurar os métodos simulados do repositório
        _mockRepository.Setup(repo => repo.GetAll()).Returns(new List<Produto>
        {
            new Produto { Id = 1, Nome = "Produto A", Preco = 10.99M },
            new Produto { Id = 2, Nome = "Produto B", Preco = 20.50M },
        });

        // Injetar o mock no controlador
        _controller = new ProdutoController(_mockRepository.Object);
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
        _mockRepository.Setup(repo => repo.GetById(1)).Returns(new Produto { Id = 1, Nome = "Produto A", Preco = 10.99M });

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
}