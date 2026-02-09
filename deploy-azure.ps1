# Azure Deployment Script
# Run this script to deploy to Azure App Service

param(
    [Parameter(Mandatory=$true)]
    [string]$AppName,
    
    [Parameter(Mandatory=$false)]
    [string]$ResourceGroup = "AttendanceSystemRG",
    
    [Parameter(Mandatory=$false)]
    [string]$Location = "eastus"
)

Write-Host "🚀 Deploying Attendance System to Azure..." -ForegroundColor Cyan

# Login to Azure
Write-Host "Step 1: Logging in to Azure..." -ForegroundColor Yellow
az login

# Create Resource Group
Write-Host "Step 2: Creating Resource Group..." -ForegroundColor Yellow
az group create --name $ResourceGroup --location $Location

# Create App Service Plan (Free Tier)
Write-Host "Step 3: Creating App Service Plan (Free Tier)..." -ForegroundColor Yellow
az appservice plan create `
    --name "$($AppName)Plan" `
    --resource-group $ResourceGroup `
    --sku F1 `
    --is-linux

# Create Web App
Write-Host "Step 4: Creating Web App..." -ForegroundColor Yellow
az webapp create `
    --resource-group $ResourceGroup `
    --plan "$($AppName)Plan" `
    --name $AppName `
    --runtime "DOTNET|8.0"

# Publish the application
Write-Host "Step 5: Publishing application..." -ForegroundColor Yellow
dotnet publish -c Release -o ./publish

# Create deployment package
Write-Host "Step 6: Creating deployment package..." -ForegroundColor Yellow
Push-Location publish
Compress-Archive -Path * -DestinationPath ../app.zip -Force
Pop-Location

# Deploy to Azure
Write-Host "Step 7: Deploying to Azure..." -ForegroundColor Yellow
az webapp deployment source config-zip `
    --resource-group $ResourceGroup `
    --name $AppName `
    --src app.zip

# Clean up
Write-Host "Step 8: Cleaning up..." -ForegroundColor Yellow
Remove-Item -Path ./publish -Recurse -Force
Remove-Item -Path ./app.zip -Force

Write-Host "✅ Deployment completed successfully!" -ForegroundColor Green
Write-Host "🌐 Your app is available at: https://$AppName.azurewebsites.net" -ForegroundColor Cyan
Write-Host ""
Write-Host "⚠️  IMPORTANT: Don't forget to configure your database connection string!" -ForegroundColor Yellow
Write-Host "Run the following command to set the connection string:" -ForegroundColor Yellow
Write-Host "az webapp config connection-string set --resource-group $ResourceGroup --name $AppName --connection-string-type SQLAzure --settings DefaultConnection='YOUR_CONNECTION_STRING'" -ForegroundColor White
