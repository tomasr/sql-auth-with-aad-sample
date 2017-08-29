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


$certName = "SPCert-$principalName"
$cert = New-KeyVaultSelfSignedCert -keyVault $keyVaultName `
                                   -certificateName $certName `
                                   -subjectName "CN=$principalName" `
                                   -validityInMonths $validityInMonths `
                                   -renewDaysBefore 1

Write-Verbose "Certificate generated $($cert.Thumbprint)"

$certString = [Convert]::ToBase64String($cert.GetRawCertData())

New-AzureRmADServicePrincipal -DisplayName $principalName `
                              -CertValue $certString `
                              -EndDate $cert.NotAfter.AddDays(-1)