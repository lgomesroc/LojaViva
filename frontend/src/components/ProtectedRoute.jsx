import React, { useEffect } from 'react';
import { Navigate, useNavigate } from 'react-router-dom';
import { useAuth } from '../contexts/AuthContext';

const ProtectedRoute = ({ children }) => {
  const { user, logout } = useAuth();
  const token = localStorage.getItem('token');
  const navigate = useNavigate();

  useEffect(() => {
    // Verificar se o token existe mas está expirado ou inválido
    if (!token) {
      logout();
      navigate('/login');
    }
  }, [token, logout, navigate]);

  if (!token) {
    // Redirecionar para login se não estiver autenticado
    return <Navigate to="/login" replace />;
  }

  return children;
};

export default ProtectedRoute;