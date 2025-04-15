export class UserService {
  // Obter informações do perfil do usuário
  async getProfile() {
    const response = await fetch('http://localhost:5000/api/user/profile', {
      headers: {
        'Authorization': `Bearer ${localStorage.getItem('token')}`,
      },
    });

    if (!response.ok) {
      const errorMessage = await response.text();
      throw new Error(errorMessage || 'Erro ao carregar o perfil');
    }

    return response.json();
  }

  // Atualizar informações do perfil do usuário
  async updateProfile(data) {
    const response = await fetch('http://localhost:5000/api/user/profile', {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${localStorage.getItem('token')}`,
      },
      body: JSON.stringify(data),
    });

    if (!response.ok) {
      const errorMessage = await response.text();
      throw new Error(errorMessage || 'Erro ao atualizar o perfil');
    }

    return response.json();
  }

  // Adicionar método para excluir o perfil (opcional)
  async deleteProfile() {
    const response = await fetch('http://localhost:5000/api/user/profile', {
      method: 'DELETE',
      headers: {
        'Authorization': `Bearer ${localStorage.getItem('token')}`,
      },
    });

    if (!response.ok) {
      const errorMessage = await response.text();
      throw new Error(errorMessage || 'Erro ao excluir o perfil');
    }

    return response.json();
  }
}
