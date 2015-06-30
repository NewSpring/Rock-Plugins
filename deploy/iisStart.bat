REM query to see if the w3svc service is running
sc query w3svc

REM if the w3svc is running then it should return a state of 4
echo STATE

REM only if the w3svc is not running do we want to try to start it.
REM if we try to start it when it is already started it will throw an error.
if NOT STATE == 4 (
  net start w3svc
)
