copy C:\inetpub\wwwroot\web.connectionstrings.config C:\inetpub\temp\web.connectionstrings.config
del /s /q C:\inetpub\wwwroot\*
copy C:\inetpub\temp\web.connectionstrings.config C:\inetpub\wwwroot\web.connectionstrings.config


