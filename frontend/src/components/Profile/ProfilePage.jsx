import React, { useState } from 'react';
import { useForm } from 'react-hook-form';
import { TextField, Button, Box, Typography } from '@mui/material';

const ProfilePage = ({ userService }) => {
  const { register, handleSubmit, formState: { errors } } = useForm();
  const [successMessage, setSuccessMessage] = useState('');

  const onSubmit = async (data) => {
    try {
      if (data.newPassword !== data.confirmPassword) {
        alert('As senhas não coincidem!');
        return;
      }

      await userService.updateProfile({
        name: data.name,
        email: data.email,
        password: data.newPassword, // Senha opcional
      });

      setSuccessMessage('Perfil atualizado com sucesso!');
    } catch (error) {
      alert('Erro ao atualizar perfil: ' + error.message);
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
        Gerenciamento de Perfil
      </Typography>
      {successMessage && (
        <Typography color="green" gutterBottom>
          {successMessage}
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
            },
          })}
          error={!!errors.email}
          helperText={errors.email?.message}
        />
        <TextField
          label="Nova Senha (opcional)"
          type="password"
          variant="outlined"
          fullWidth
          margin="normal"
          {...register('newPassword', { minLength: { value: 6, message: 'A senha deve ter pelo menos 6 caracteres' } })}
          error={!!errors.newPassword}
          helperText={errors.newPassword?.message}
        />
        <TextField
          label="Confirme a Nova Senha"
          type="password"
          variant="outlined"
          fullWidth
          margin="normal"
          {...register('confirmPassword')}
        />
        <Button type="submit" variant="contained" fullWidth sx={{ marginTop: 2 }}>
          Atualizar
        </Button>
      </form>
    </Box>
  );
};

export default ProfilePage;
