@ECHO OFF
@ECHO Executing SQL scripts
for %%G in (*.sql) do sqlcmd -s localhost -d GDale_EdFi -E -i "%%G"
PAUSE