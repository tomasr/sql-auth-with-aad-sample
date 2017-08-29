[CmdletBinding()]
param(
    [Parameter(Mandatory=$true)]
    [String]$sqlServer,
    [Parameter(Mandatory=$true)]
    [String]$database,
    [Parameter(Mandatory=$true)]
    [PSCredential]$sqlAdminCredentials,
    [Parameter(Mandatory=$true)]
    [String]$servicePrincipalName
)

# For this script to work we need SqlCmd v13.1 or later
# which can be downloaded from
# https://www.microsoft.com/en-us/download/details.aspx?id=53591
# and the latest ODBC driver from
# https://www.microsoft.com/en-us/download/details.aspx?id=53339

$query = @"
CREATE USER [$servicePrincipalName] FROM EXTERNAL PROVIDER
GO
ALTER ROLE db_owner ADD MEMBER [$servicePrincipalName]
"@

sqlcmd.exe -S "tcp:$sqlServer,1433" `
           -N -C -d $database -G -U $sqlAdminCredentials.UserName `
           -P $sqlAdminCredentials.GetNetworkCredential().Password `
           -Q $query