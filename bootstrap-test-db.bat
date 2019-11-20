REM always download the JDBC driver from Microsoft before running this

docker run --name SQLSERVER -e "ACCEPT_EULA=y" -e "SA_PASSWORD=Jordgubbar666" -p 1433:1433 --rm -d mcr.microsoft.com/mssql/server:2017-latest-ubuntu
docker exec -it SQLSERVER /opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P Jordgubbar666 -Q "CREATE DATABASE todostorage_dev"
liquibase --driver="com.microsoft.sqlserver.jdbc.SQLServerDriver" --classpath="mssql-jdbc-7.4.1.jre8.jar" --url="jdbc:sqlserver://127.0.0.1:1433;database=todostorage_dev" --changeLogFile=src\TodoStorage.Persistence\db.changelog.xml --username=sa --password=Jordgubbar666 update
