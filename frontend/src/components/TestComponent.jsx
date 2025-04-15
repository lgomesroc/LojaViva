import React from 'react';
import { useDependency } from './contexts/DependencyProvider';

const TestComponent = () => {
  const dependencies = useDependency();

  console.log('Dependências:', dependencies); // Deve mostrar as dependências carregadas

  return <h1>Testando Contexto</h1>;
};

export default TestComponent;
