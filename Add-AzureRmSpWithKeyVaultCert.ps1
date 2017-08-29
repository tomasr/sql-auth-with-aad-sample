[CmdletBinding()]
param(
  [Parameter(Mandatory = $true)]
  [String]$keyVaultName,
  [Parameter(Mandatory = $true)]
  [String]$principalName,
  [Parameter()]
  [int]$validityInMonths = 12
)

function New-KeyVaultSelfSignedCert {
  param($keyVault, $certificateName, $subjectName, $validityInMonths, $renewDaysBefore)

  $policy = New-AzureKeyVaultCertificatePolicy `
              -SubjectName $subjectName `
              -ReuseKeyOnRenewal `
              -IssuerName 'Self' `
              -ValidityInMonths $validityInMonths `
              -RenewAtNumberOfDaysBeforeExpiry $renewDaysBefore

  $op = Add-AzureKeyVaultCertificate `
              -VaultName $keyVault `
              -CertificatePolicy $policy `
              -Name $certificateName

  while ( $op.Status -ne 'completed' ) {
    Start-Sleep -Seconds 1
    $op = Get-AzureKeyVaultCertificateOperation -VaultName $keyVault -Name $certificateName
  }
  (Get-AzureKeyVaultCertificate -VaultName $keyVault -Name $certificateName).Certificate
}

function Set-KeyVaultAccessToAppService($keyVault) {
  Set-AzureRmKeyVaultAccessPolicy -VaultName $keyVault `
                                  -ServicePrincipalName 'abfa0a7c-a6b6-4736-8310-5855508787cd' `
                                  -PermissionsToSecrets get
}


$certName = "SPCert-$principalName"
$cert = New-KeyVaultSelfSignedCert -keyVault $keyVaultName `
                                   -certificateName $certName `
                                   -subjectName "CN=$principalName" `
                                   -validityInMonths $validityInMonths `
                                   -renewDaysBefore 1

Write-Verbose "Certificate generated $($cert.Thumbprint)"

Set-KeyVaultAccessToAppService -keyVault $keyVaultName
Write-Verbose 'Granted access to App Service to Key Vault'

$certString = [Convert]::ToBase64String($cert.GetRawCertData())

New-AzureRmADServicePrincipal -DisplayName $principalName `
                              -CertValue $certString `
                              -EndDate $cert.NotAfter.AddDays(-1)
