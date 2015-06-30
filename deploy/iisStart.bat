REM query to see if the w3svc service is running.
sc query w3svc | find "RUNNING"

REM only if the w3svc is not running do we want to try to start it.
REM if we try to start it when it is already started it will throw an error.
if NOT "%ERRORLEVEL%"=="0" (
  net start w3svc
)
