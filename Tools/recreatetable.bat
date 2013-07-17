@echo off
set oldfile=dsw(%date%_%time:~0,2%%time:~3,2%%time:~6,2%).db
copy ..\Database\dsw.db "..\Database\%oldfile%"
sqlite3 ..\Database\dsw.db < output.sql
sqlite3 ..\Database\dsw.db -echo -cmd "ATTACH '..\Database\%oldfile%' AS old" < insert.sql
