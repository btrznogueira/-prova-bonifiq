# 🎯 Configuração do SonarQube Local

Este projeto está configurado para usar SonarQube localmente para análise de qualidade de código e cobertura de testes.

## 🚀 Início Rápido

### 1. Pré-requisitos
- Docker Desktop instalado e rodando
- PowerShell (Windows)

### 2. Subir SonarQube
```powershell
# Execute o script automatizado
.\run-sonar.ps1

# Ou manualmente
docker-compose up -d
```

### 3. Acessar SonarQube
- **URL**: http://localhost:9000
- **Login**: admin
- **Senha**: admin

## 📊 Configuração do Projeto

### 1. Criar Novo Projeto
1. Acesse http://localhost:9000
2. Clique em "Create new project"
3. Escolha "Manually"
4. **Project key**: `ProvaPub`
5. **Display name**: `ProvaPub Backend`
6. Clique em "Set Up"

### 2. Configurar Token
1. Escolha "Use the global setting"
2. Clique em "Create a project from scratch"
3. Escolha "Other (for JS, TS, Go, Python, PHP, ...)"
4. **Project key**: `ProvaPub`
5. **Main branch name**: `main`
6. Clique em "Set Up"
7. **Token**: Copie o token gerado (será usado no próximo passo)

### 3. Executar Análise
```powershell
# Instalar SonarScanner globalmente (se necessário)
dotnet tool install --global dotnet-sonarscanner

# Executar análise
dotnet sonarscanner begin /k:"ProvaPub" /d:sonar.host.url="http://localhost:9000" /d:sonar.login="SEU_TOKEN_AQUI"

# Compilar projeto
dotnet build

# Executar testes com cobertura
dotnet test --collect:"XPlat Code Coverage"

# Finalizar análise
dotnet sonarscanner end /d:sonar.login="SEU_TOKEN_AQUI"
```

## 🔧 Configurações

### Arquivo `sonar-project.properties`
- **Sources**: `src/` (código fonte)
- **Tests**: `tests/` (testes)
- **Language**: C# (.NET)
- **Coverage**: Exclui Models, Repository e Enums

### Docker Compose
- **SonarQube**: Porta 9000
- **PostgreSQL**: Banco de dados para SonarQube
- **Volumes**: Dados persistentes

## 📈 Métricas Analisadas

- ✅ **Code Quality**: Bugs, Vulnerabilities, Code Smells
- ✅ **Test Coverage**: Cobertura de testes
- ✅ **Duplications**: Código duplicado
- ✅ **Maintainability**: Complexidade ciclomática
- ✅ **Reliability**: Confiabilidade do código
- ✅ **Security**: Problemas de segurança

## 🧪 Cobertura de Testes

O projeto inclui testes unitários abrangentes para o método `CanPurchase`:

- **Validação de cliente existente**
- **Limite de compra mensal**
- **Limite de primeira compra**
- **Horário comercial (8h-18h)**
- **Dias úteis (não fim de semana)**

## 🛑 Comandos Úteis

```powershell
# Parar SonarQube
docker-compose down

# Ver logs
docker-compose logs -f sonarqube

# Reiniciar
docker-compose restart

# Limpar dados (cuidado!)
docker-compose down -v
```

## 🔍 Troubleshooting

### SonarQube não responde
- Aguarde mais tempo (pode levar até 2 minutos na primeira execução)
- Verifique se Docker está rodando
- Verifique logs: `docker-compose logs sonarqube`

### Erro de conexão
- Verifique se a porta 9000 não está sendo usada
- Reinicie com: `docker-compose down && docker-compose up -d`

### Problemas de memória
- SonarQube requer pelo menos 4GB de RAM
- Feche outros aplicativos se necessário

## 📚 Recursos Adicionais

- [Documentação SonarQube](https://docs.sonarqube.org/)
- [SonarScanner para .NET](https://docs.sonarqube.org/latest/analysis/scan/sonarscanner-for-msbuild/)
- [Quality Gates](https://docs.sonarqube.org/latest/user-guide/quality-gates/)

