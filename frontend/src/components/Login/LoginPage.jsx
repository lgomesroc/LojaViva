import React from 'react';
import { useForm } from 'react-hook-form';
import { TextField, Button, Box, Typography, Link, CircularProgress } from '@mui/material';
import { Link as RouterLink, useNavigate } from 'react-router-dom';
import { useDependency } from '../../contexts/DependencyProvider';
import { useAuth } from '../../contexts/AuthContext';

const LoginPage = () => {
  const { authService } = useDependency();
  const { login } = useAuth();
  const navigate = useNavigate();
  const [loading, setLoading] = React.useState(false);
  const [errorMessage, setErrorMessage] = React.useState('');
  
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm();

  const onSubmit = async (data) => {
    setLoading(true);
    setErrorMessage('');
    
    try {
      // Chamar o serviço de autenticação
      const response = await authService.login(data.email, data.password);
      
      // Atualizar o contexto com os dados do usuário
      login({
        token: response.token,
        user: response.user || { email: data.email }
      });
      
      // Redirecionar para o dashboard após login bem-sucedido
      navigate('/dashboard');
    } catch (error) {
      // Exibir mensagem de erro ao usuário
      setErrorMessage(error.message || 'Ocorreu um erro durante o login');
    } finally {
      setLoading(false);
    }
  };

  return (
    <Box
      sx={{
        maxWidth: 400,
        margin: '50px auto',
        padding: '20px',
        border: '1px solid #ccc',
        borderRadius: '8px',
        boxShadow: '0 4px 8px rgba(0, 0, 0, 0.1)',
        textAlign: 'center',
      }}
    >
      <Typography variant="h4" gutterBottom>
        Login
      </Typography>
      
      {errorMessage && (
        <Typography color="error" sx={{ mb: 2 }}>
          {errorMessage}
        </Typography>
      )}
      
      <form onSubmit={handleSubmit(onSubmit)}>
        <TextField
          label="E-mail"
          variant="outlined"
          fullWidth
          margin="normal"
          {...register('email', {
            required: 'O e-mail é obrigatório',
            pattern: {
              value: /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/,
              message: 'Insira um e-mail válido',
            },
          })}
          error={!!errors.email}
          helperText={errors.email?.message}
          disabled={loading}
        />
        <TextField
          label="Senha"
          type="password"
          variant="outlined"
          fullWidth
          margin="normal"
          {...register('password', {
            required: 'A senha é obrigatória',
            minLength: { value: 6, message: 'A senha deve ter pelo menos 6 caracteres' },
          })}
          error={!!errors.password}
          helperText={errors.password?.message}
          disabled={loading}
        />
        <Button 
          type="submit" 
          variant="contained" 
          fullWidth 
          sx={{ marginTop: 2 }}
          disabled={loading}
        >
          {loading ? <CircularProgress size={24} /> : 'Entrar'}
        </Button>
        <Box mt={2}>
          <Link component={RouterLink} to="/register" disabled={loading}>
            Não tem uma conta? Registre-se aqui
          </Link>
        </Box>
      </form>
    </Box>
  );
};

export default LoginPage;