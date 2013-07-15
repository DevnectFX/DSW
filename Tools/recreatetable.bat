@echo off
copy ..\Database\dsw.db "..\Database\dsw(%date%_%time:~0,2%%time:~3,2%%time:~6,2%).db"
sqlite3 ..\Database\dsw.db < output.sql