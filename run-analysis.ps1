# Script para executar analise completa do SonarQube
param(
    [Parameter(Mandatory=$true)]
    [string]$Token
)

Write-Host "Iniciando analise do SonarQube..." -ForegroundColor Green
Write-Host "Token: $Token" -ForegroundColor Yellow

# Iniciar analise
Write-Host "Iniciando analise..." -ForegroundColor Cyan
dotnet sonarscanner begin /k:"ProvaPub" /d:sonar.host.url="http://localhost:9000" /d:sonar.login="$Token"

# Compilar projeto
Write-Host "Compilando projeto..." -ForegroundColor Cyan
dotnet build

# Executar testes com cobertura
Write-Host "Executando testes com cobertura..." -ForegroundColor Cyan
dotnet test --collect:"XPlat Code Coverage"

# Finalizar analise
Write-Host "Finalizando analise..." -ForegroundColor Cyan
dotnet sonarscanner end /d:sonar.login="$Token"

Write-Host ""
Write-Host "Analise concluida!" -ForegroundColor Green
Write-Host "Acesse: http://localhost:9000" -ForegroundColor Cyan
Write-Host "Verifique o projeto ProvaPub para ver os resultados" -ForegroundColor Cyan

