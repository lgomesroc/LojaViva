import React, { useState } from 'react';
import { useForm } from 'react-hook-form';
import { TextField, Button, Box, Typography, Link, CircularProgress, IconButton } from '@mui/material';
import { Visibility, VisibilityOff } from '@mui/icons-material';
import { Link as RouterLink, useNavigate } from 'react-router-dom';
import { useDependency } from '../../contexts/DependencyProvider';

const RegisterPage = () => {
  const { authService } = useDependency();
  const navigate = useNavigate();
  const [loading, setLoading] = useState(false);
  const [errorMessage, setErrorMessage] = useState('');
  const [showPassword, setShowPassword] = useState(false); // Controle de visibilidade da senha
  const [showConfirmPassword, setShowConfirmPassword] = useState(false); // Controle de visibilidade da confirmação

  const {
    register,
    handleSubmit,
    watch,
    formState: { errors },
  } = useForm();

  const password = React.useRef({});
  password.current = watch("password", "");

  const togglePasswordVisibility = () => setShowPassword(!showPassword); // Alternar visibilidade da senha
  const toggleConfirmPasswordVisibility = () => setShowConfirmPassword(!showConfirmPassword); // Alternar visibilidade da confirmação

  const onSubmit = async (data) => {
    setLoading(true);
    setErrorMessage('');
    
    try {
      await authService.register(data.name, data.email, data.password);
      alert('Cadastro realizado com sucesso!');
      navigate('/login');
    } catch (error) {
      setErrorMessage(error.message || 'Erro ao realizar cadastro');
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
        Cadastro
      </Typography>
      
      {errorMessage && (
        <Typography color="error" sx={{ mb: 2 }}>
          {errorMessage}
        </Typography>
      )}
      
      <form onSubmit={handleSubmit(onSubmit)}>
        <TextField
          label="Nome"
          variant="outlined"
          fullWidth
          margin="normal"
          {...register('name', { required: 'O nome é obrigatório' })}
          error={!!errors.name}
          helperText={errors.name?.message}
          disabled={loading}
        />
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
            }
          })}
          error={!!errors.email}
          helperText={errors.email?.message}
          disabled={loading}
        />
        <Box sx={{ position: 'relative' }}>
          <TextField
            label="Senha"
            type={showPassword ? 'text' : 'password'} // Alterna entre texto e senha
            variant="outlined"
            fullWidth
            margin="normal"
            {...register('password', { 
              required: 'A senha é obrigatória',
              minLength: { value: 6, message: 'A senha deve ter pelo menos 6 caracteres' }
            })}
            error={!!errors.password}
            helperText={errors.password?.message}
            disabled={loading}
          />
          <IconButton
            onClick={togglePasswordVisibility}
            sx={{
              position: 'absolute',
              right: 10,
              top: '50%',
              transform: 'translateY(-50%)',
            }}
          >
            {showPassword ? <Visibility /> : <VisibilityOff />}
          </IconButton>
        </Box>
        <Box sx={{ position: 'relative' }}>
          <TextField
            label="Confirmação de Senha"
            type={showConfirmPassword ? 'text' : 'password'} // Alterna entre texto e senha
            variant="outlined"
            fullWidth
            margin="normal"
            {...register('confirmPassword', { 
              required: 'Confirme sua senha',
              validate: value => value === password.current || "As senhas não coincidem"
            })}
            error={!!errors.confirmPassword}
            helperText={errors.confirmPassword?.message}
            disabled={loading}
          />
          <IconButton
            onClick={toggleConfirmPasswordVisibility}
            sx={{
              position: 'absolute',
              right: 10,
              top: '50%',
              transform: 'translateY(-50%)',
            }}
          >
            {showConfirmPassword ? <Visibility /> : <VisibilityOff />}
          </IconButton>
        </Box>
        <Button 
          type="submit" 
          variant="contained" 
          fullWidth 
          sx={{ marginTop: 2 }}
          disabled={loading}
        >
          {loading ? <CircularProgress size={24} /> : 'Cadastrar'}
        </Button>
        <Box mt={2}>
          <Link component={RouterLink} to="/login" disabled={loading}>
            Já tem uma conta? Faça login aqui
          </Link>
        </Box>
      </form>
    </Box>
  );
};

export default RegisterPage;
