export class OrderService {
    async getOrders(filters = {}) {
      const queryString = new URLSearchParams(filters).toString();
      const response = await fetch(`http://localhost:5000/api/order/recent?${queryString}`, {
        headers: {
          'Authorization': `Bearer ${localStorage.getItem('token')}`,
        },
      });
      
      if (!response.ok) {
        const errorMessage = await response.text();
        throw new Error(errorMessage || 'Erro ao carregar os pedidos');
      }
      
      return response.json();
    }
  }