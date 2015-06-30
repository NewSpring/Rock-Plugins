REM query to see if the w3svc service is running.
sc query w3svc | find "RUNNING"

REM only if the w3svc is running do we want to try to stop it.
REM if we try to stop it when it is already stopped it will throw an error.
if "%ERRORLEVEL%"=="0" (
  net stop w3svc
)
