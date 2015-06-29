REM query to see if the w3svc service is running
sc query w3svc

REM if the w3svc is running then it should return a state of 4
echo STATE

REM only if the w3svc is running do we want to try to stop it.
REM if we try to stop it when it is already stopped it will throw an error.
if STATE == 4 (
  net stop w3svc
)
