# Service Principals, Key Vault, and SQL Azure Sample

This repository contains sample code and scripts on how to:

* Create an Azure AD Service Principal
* Create a self-signed certificate in Key Vault and add it as an
  authentication mechanism to the Service Principal
* Configure Azure SQL for Active Directory Authentication
* How to create an Azure Web App that uses Token-based authentication
  to connect to an Azure SQL Database by authenticating with the
  Key Vault certificate.
* How to authenticate users of the Web App using Azure AD
  and delegate their credentials to Azure SQL DB.

The following posts describe the sample in detail:

* [Azure AD Service Principal with a Key Vault Certificate](http://winterdom.com/2017/08/28/azure-ad-service-principal-with-keyvault-cert.html)
* [Token authentication to SQL Azure with a Key Vault Certificate](http://winterdom.com/2017/08/29/webapp-with-keyvault-cert-and-sql-token-auth.html)
* [Authenticating to SQL Azure with delegated tokens](http://winterdom.com/2017/08/31/token-delegation-azure-sql.html)