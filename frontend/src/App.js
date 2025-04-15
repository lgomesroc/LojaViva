import React from 'react';
import { BrowserRouter as Router, Routes, Route } from 'react-router-dom';
import LoginPage from './components/Login/LoginPage';
import RegisterPage from './components/Register/RegisterPage';
import DashboardPage from './components/Dashboard/DashboardPage';
import ProfilePage from './components/Profile/ProfilePage';
import ProtectedRoute from './components/ProtectedRoute';
import { AuthProvider } from './contexts/AuthContext';
import { ReactQueryProvider } from './contexts/ReactQueryProvider';
import { DependencyProvider } from './contexts/DependencyProvider';

const App = () => {
  return (
    <AuthProvider>
      <ReactQueryProvider>
        <DependencyProvider> {/* Certifique-se de que o contexto está envolvendo tudo */}
          <Router>
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
              <Route
                path="/profile"
                element={
                  <ProtectedRoute>
                    <ProfilePage />
                  </ProtectedRoute>
                }
              />
            </Routes>
          </Router>
        </DependencyProvider>
      </ReactQueryProvider>
    </AuthProvider>
  );
};

export default App;
