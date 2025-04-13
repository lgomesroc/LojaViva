using Xunit;
using Microsoft.AspNetCore.Mvc;
using Moq;
using LojaViva.API.Controllers;
using LojaViva.API.Repositories;
using LojaViva.API.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System;

public class PedidoControllerTests
{
    private readonly PedidoController _controller;
    private readonly Mock<IPedidoRepository> _mockRepository;
    private readonly Mock<ILogger<PedidoController>> _mockLogger;
    private readonly Mock<IMemoryCache> _mockCache;

    public PedidoControllerTests()
    {
        // Criar mocks
        _mockRepository = new Mock<IPedidoRepository>();
        _mockLogger = new Mock<ILogger<PedidoController>>();
        _mockCache = new Mock<IMemoryCache>();

        // Configurar mock com dados de teste
        _mockRepository.Setup(repo => repo.GetAll()).Returns(new List<Pedido>
        {
            new Pedido { Id = 1, ClienteId = 1, Data = DateTime.Now, Total = 100.50m },
            new Pedido { Id = 2, ClienteId = 2, Data = DateTime.Now.AddDays(-1), Total = 200.00m }
        });

        // Configurar o mock de cache
        object cacheEntry;
        _mockCache.Setup(c => c.TryGetValue(It.IsAny<object>(), out cacheEntry)).Returns(false);

        // Injetar os mocks no controlador
        _controller = new PedidoController(_mockRepository.Object, _mockLogger.Object, _mockCache.Object);
    }

    [Fact]
    public void GetPedidos_ReturnsOkResult_WithListOfPedidos()
    {
        // Act
        var result = _controller.GetPedidos();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var pedidos = Assert.IsType<List<Pedido>>(okResult.Value);
        Assert.Equal(2, pedidos.Count);
    }

    [Fact]
    public void GetPedido_ReturnsOkResult_WithPedido()
    {
        // Configurar mock para retornar um pedido específico
        _mockRepository.Setup(repo => repo.GetById(1)).Returns(new Pedido
        {
            Id = 1,
            ClienteId = 1,
            Data = DateTime.Now,
            Total = 100.50m
        });

        // Act
        var result = _controller.GetPedido(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var pedido = Assert.IsType<Pedido>(okResult.Value);
        Assert.Equal(1, pedido.Id);
    }

    [Fact]
    public void GetPedido_ReturnsNotFound_WhenPedidoDoesNotExist()
    {
        // Configurar mock para retornar null
        _mockRepository.Setup(repo => repo.GetById(It.IsAny<int>())).Returns((Pedido?)null);

        // Act
        var result = _controller.GetPedido(99);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.NotNull(notFoundResult.Value);
        dynamic errorObject = notFoundResult.Value;
        Assert.Equal("Pedido não encontrado.", errorObject.Message);
    }

    [Fact]
    public void AddPedido_ReturnsCreatedResult()
    {
        // Configurar mock para adicionar um pedido
        var newPedido = new Pedido { Id = 3, ClienteId = 1, Data = DateTime.Now, Total = 300.00m };
        _mockRepository.Setup(repo => repo.Add(newPedido));

        // Act
        var result = _controller.AddPedido(newPedido);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        var pedido = Assert.IsType<Pedido>(createdResult.Value);
        Assert.Equal(3, pedido.Id);
    }

    [Fact]
    public void DeletePedido_ReturnsNoContent_WhenPedidoExists()
    {
        // Configurar mock para retornar um pedido existente
        _mockRepository.Setup(repo => repo.GetById(1)).Returns(new Pedido { Id = 1, ClienteId = 1 });
        _mockRepository.Setup(repo => repo.Delete(1));

        // Act
        var result = _controller.DeletePedido(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public void DeletePedido_ReturnsNotFound_WhenPedidoDoesNotExist()
    {
        // Configurar mock para retornar null
        _mockRepository.Setup(repo => repo.GetById(It.IsAny<int>())).Returns((Pedido?)null);

        // Act
        var result = _controller.DeletePedido(99);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);
        Assert.NotNull(notFoundResult.Value);
        dynamic errorObject = notFoundResult.Value;
        Assert.Equal("Pedido não encontrado.", errorObject.Message);
    }
}
