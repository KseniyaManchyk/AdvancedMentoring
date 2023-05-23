Identity management system - AzureAD.
Users:
- Manager: manager@advancedmentoring.onmicrosoft.com pass: &gerNmjh2gy#
- Buyer: buyer@advancedmentoring.onmicrosoft.com pass: &gerNmjh2gy#

Sonar run:
- dotnet sonarscanner begin /k:"AdvancedMentoring" /d:sonar.host.url="http://localhost:9000"  /d:sonar.token="sqp_d432dee438d6921bd8cd25fd57c1cb6a66a5ae38"
- dotnet build
- dotnet sonarscanner end /d:sonar.token="sqp_d432dee438d6921bd8cd25fd57c1cb6a66a5ae38"
