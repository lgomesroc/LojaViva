import React from 'react';
import { useQuery } from 'react-query';
import { Box, Typography, Card, CardContent, CircularProgress, Button, Alert } from '@mui/material';
import { useNavigate } from 'react-router-dom';
import { useDependency } from '../../contexts/DependencyProvider';
import { useAuth } from '../../contexts/AuthContext';

const DashboardPage = () => {
  const { userService, orderService } = useDependency();
  const { logout } = useAuth();
  const navigate = useNavigate();

  const { data: profile, isLoading: profileLoading, error: profileError } = useQuery(
    'profile', 
    () => userService.getProfile(),
    {
      retry: 1,
      onError: (error) => {
        // Se o token expirou ou é inválido, redirecionar para login
        if (
          error.message.includes('não autorizado') || 
          error.message.includes('token') || 
          error.message.includes('unauthorized')
        ) {
          logout();
          navigate('/login');
        }
      }
    }
  );

  const { data: orders, isLoading: ordersLoading, error: ordersError } = useQuery(
    'orders', 
    () => orderService.getOrders(),
    { retry: 1 }
  );

  const handleLogout = () => {
    logout();
    navigate('/login');
  };

  // Exibir indicador de carregamento
  if (profileLoading || ordersLoading) {
    return (
      <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', height: '100vh' }}>
        <CircularProgress />
      </Box>
    );
  }

  // Exibir mensagens de erro
  if (profileError || ordersError) {
    return (
      <Box sx={{ padding: '20px', maxWidth: 800, margin: '0 auto' }}>
        <Alert severity="error" sx={{ mb: 2 }}>
          {profileError?.message || ordersError?.message || 'Ocorreu um erro ao carregar os dados.'}
        </Alert>
        <Button 
          variant="contained" 
          onClick={handleLogout}
          sx={{ mt: 2 }}
        >
          Voltar para login
        </Button>
      </Box>
    );
  }

  return (
    <Box sx={{ padding: '20px', maxWidth: 800, margin: '0 auto' }}>
      {profile && (
        <Typography variant="h4" gutterBottom>
          Bem-vindo, {profile.name}!
        </Typography>
      )}
      
      <Typography variant="h6" gutterBottom>
        Informações do Perfil:
      </Typography>
      
      {profile && (
        <Card sx={{ marginBottom: '20px' }}>
          <CardContent>
            <Typography>E-mail: {profile.email}</Typography>
            <Typography>Data de Cadastro: {profile.registrationDate}</Typography>
          </CardContent>
        </Card>
      )}
      
      <Typography variant="h6" gutterBottom>
        Pedidos Recentes:
      </Typography>
      
      {orders && orders.length > 0 ? (
        orders.map((order) => (
          <Card key={order.id} sx={{ marginBottom: '10px' }}>
            <CardContent>
              <Typography>ID do Pedido: {order.id}</Typography>
              <Typography>Total: R$ {order.total}</Typography>
              <Typography>Data: {order.date}</Typography>
            </CardContent>
          </Card>
        ))
      ) : (
        <Typography>Você ainda não fez nenhum pedido.</Typography>
      )}
      
      <Button 
        variant="contained" 
        color="primary"
        sx={{ marginTop: 3 }}
        onClick={handleLogout}
      >
        Sair
      </Button>
    </Box>
  );
};

export default DashboardPage;