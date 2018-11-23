$Password = 'P@ssw0rd'
$DataPath = 'D:\Docker\VoteItDB'

docker run -e 'ACCEPT_EULA=Y' -e 'SA_PASSWORD=P@ssw0rd' -e 'MSSQL_PID=Express' -p 1433:1433 -v D:\Docker\VoteItDB:/var/opt/mssql -d mcr.microsoft.com/mssql/server:latest