import React from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import LoginPage from './components/Login/LoginPage';
import RegisterPage from './components/Register/RegisterPage';
import DashboardPage from './components/Dashboard/DashboardPage';
import { AuthProvider } from './contexts/AuthContext';
import { ReactQueryProvider } from './contexts/ReactQueryProvider';
import { DependencyProvider } from './contexts/DependencyProvider';
import ProtectedRoute from './components/ProtectedRoute';

const App = () => {
  return (
    <Router>
      <AuthProvider>
        <ReactQueryProvider>
          <DependencyProvider>
            <Routes>
              <Route path="/login" element={<LoginPage />} />
              <Route path="/register" element={<RegisterPage />} />
              <Route 
                path="/dashboard" 
                element={
                  <ProtectedRoute>
                    <DashboardPage />
                  </ProtectedRoute>
                } 
              />
              <Route path="/" element={<Navigate to="/login" replace />} />
            </Routes>
          </DependencyProvider>
        </ReactQueryProvider>
      </AuthProvider>
    </Router>
  );
};

export default App;
