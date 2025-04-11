using Xunit;
using Microsoft.AspNetCore.Mvc;
using Moq;
using LojaViva.API.Controllers;
using LojaViva.API.Repositories;
using LojaViva.API.Models;
using System.Collections.Generic;

public class ClienteControllerTests
{
    private readonly ClientesController _controller;
    private readonly Mock<IClienteRepository> _mockRepository;

    public ClienteControllerTests()
    {
        // Criar um mock do repositório para simular comportamento
        _mockRepository = new Mock<IClienteRepository>();

        // Configurar os métodos simulados do repositório
        _mockRepository.Setup(repo => repo.GetAll()).Returns(new List<Cliente>
        {
            new Cliente { Id = 1, Nome = "Cliente A", Email = "clienteA@email.com", Telefone = "123456789" },
            new Cliente { Id = 2, Nome = "Cliente B", Email = "clienteB@email.com", Telefone = "987654321" },
        });

        // Injetar o mock no controlador
        _controller = new ClientesController(_mockRepository.Object);
    }

    [Fact]
    public void GetClientes_ReturnsOkResult_WithListOfClientes()
    {
        // Act
        var result = _controller.GetClientes();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var clientes = Assert.IsType<List<Cliente>>(okResult.Value);
        Assert.Equal(2, clientes.Count);
    }

    [Fact]
    public void GetCliente_ReturnsOkResult_WithCliente()
    {
        // Configurar o mock para retornar um cliente específico
        _mockRepository.Setup(repo => repo.GetById(1)).Returns(new Cliente { Id = 1, Nome = "Cliente A", Email = "clienteA@email.com", Telefone = "123456789" });

        // Act
        var result = _controller.GetCliente(1);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        var cliente = Assert.IsType<Cliente>(okResult.Value);
        Assert.Equal("Cliente A", cliente.Nome);
    }

    [Fact]
    public void GetCliente_ReturnsNotFound_WhenClienteDoesNotExist()
    {
        // Configurar o mock para retornar null quando o cliente não for encontrado
        _mockRepository.Setup(repo => repo.GetById(It.IsAny<int>())).Returns((Cliente?)null);

        // Act
        var result = _controller.GetCliente(99);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public void PostCliente_ReturnsCreatedResult()
    {
        // Configurar o mock para adicionar um cliente
        var newCliente = new Cliente { Id = 3, Nome = "Cliente C", Email = "clienteC@email.com", Telefone = "111222333" };
        _mockRepository.Setup(repo => repo.Add(newCliente));

        // Act
        var result = _controller.PostCliente(newCliente);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result);
        var cliente = Assert.IsType<Cliente>(createdResult.Value);
        Assert.Equal("Cliente C", cliente.Nome);
    }

    [Fact]
    public void DeleteCliente_ReturnsNoContent_WhenClienteExists()
    {
        // Configurar o mock para retornar um cliente existente
        _mockRepository.Setup(repo => repo.GetById(1)).Returns(new Cliente { Id = 1, Nome = "Cliente A" });
        _mockRepository.Setup(repo => repo.Delete(1));

        // Act
        var result = _controller.DeleteCliente(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public void DeleteCliente_ReturnsNotFound_WhenClienteDoesNotExist()
    {
        // Configurar o mock para retornar null
        _mockRepository.Setup(repo => repo.GetById(It.IsAny<int>())).Returns((Cliente?)null);

        // Act
        var result = _controller.DeleteCliente(99);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}