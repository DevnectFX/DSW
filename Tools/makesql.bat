@echo off
copy erdx2sql\bin\Debug\erdx2sql.exe .\erdx2sql.exe
erdx2sql\bin\Debug\erdx2sql.exe ..\ERD\DSW\common.erdx output.sql
