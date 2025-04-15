import React, { createContext, useContext } from 'react';
import { AuthService } from '../services/AuthService';
import { UserService } from '../services/UserService';
import { OrderService } from '../services/OrderService';

const DependencyContext = createContext();

export const DependencyProvider = ({ children }) => {
  const authService = new AuthService();
  const userService = new UserService();
  const orderService = new OrderService();

  console.log('Dependências no Provider:', { authService, userService, orderService }); // Teste

  return (
    <DependencyContext.Provider value={{ authService, userService, orderService }}>
      {children}
    </DependencyContext.Provider>
  );
};

export const useDependency = () => useContext(DependencyContext);
