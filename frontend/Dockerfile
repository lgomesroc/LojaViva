# Usar uma imagem leve do Node.js
FROM node:18-alpine

# Configurar o diretório de trabalho no contêiner
WORKDIR /app

# Copiar os arquivos de dependência
COPY package.json ./
COPY package-lock.json ./

# Instalar todas as dependências do projeto (inclusive as funcionalidades)
RUN npm install --legacy-peer-deps

# Copiar os arquivos do projeto para o contêiner
COPY . .

# Construir o projeto para produção (caso necessário no futuro)
RUN npm run build

# Expor a porta 3000 para o servidor React
EXPOSE 3000

# Comando para iniciar o servidor de desenvolvimento do React
CMD ["npm", "start"]
