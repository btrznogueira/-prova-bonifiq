# run-sonar.ps1
# Caminho raiz do projeto
$rootPath = Split-Path -Parent $MyInvocation.MyCommand.Definition

# Token SonarQube
$sonarToken = "squ_353aadfaf9fa5449ceae231e5567afe00d9f7611"
$sonarProjectKey = "prova"
$sonarHost = "http://localhost:9000"

# Caminho do relatório de cobertura
$coverageFile = "$rootPath\tests\ProvaPub.Test\TestResults\coverage.opencover.xml"

Write-Host "==== Iniciando análise SonarQube ===="

# Inicia análise SonarQube
dotnet sonarscanner begin /k:$sonarProjectKey /d:sonar.host.url=$sonarHost /d:sonar.login=$sonarToken /d:sonar.cs.opencover.reportsPaths=$coverageFile

# Executa testes com cobertura OpenCover
dotnet test "$rootPath\tests\ProvaPub.Test\ProvaPub.Test.csproj" `
    /p:CollectCoverage=true `
    /p:CoverletOutputFormat=opencover `
    /p:CoverletOutput=$coverageFile

# Build do projeto principal
dotnet build "$rootPath\src\ProvaPub\ProvaPub.csproj"

# Finaliza análise SonarQube
dotnet sonarscanner end /d:sonar.login=$sonarToken

Write-Host "==== Análise SonarQube concluída ===="
