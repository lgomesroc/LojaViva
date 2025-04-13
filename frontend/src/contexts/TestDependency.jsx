import React from 'react';
import { useDependency } from './DependencyProvider'; // Certifique-se de ajustar o caminho

const TestDependency = () => {
  const dependencies = useDependency();

  console.log('Dependências carregadas:', dependencies);

  return (
    <div>
      <h1>Teste de Contexto</h1>
      <p>
        Confira no console se os serviços foram carregados:
        {dependencies ? 'Contexto funcionando!' : 'Erro no contexto!'}
      </p>
    </div>
  );
};

export default TestDependency;
