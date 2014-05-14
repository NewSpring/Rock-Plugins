A CyberSource plugin for Rock RMS: http://www.rockrms.com/

Designed with the Rock developer kit from https://github.com/SparkDevNetwork/RockKit

________________________________________________________________________

Updating to the latest WSDL:
Open CMD shell 

1. Change directory (cd) to locate the sample code (sample_wcf.cs) and sample_wcf.exe.config.

2. Get the latest version number from https://ics2wstest.ic3.com/commerce/1.x/transactionProcessor.

3. Generate the proxy classes as follows:

svcutil /config:sample_wcf.exe.config  https://ics2wstest.ic3.com/commerce/1.x/transactionProcessor/CyberSourceTransaction_N.NN.wsdl

Two files are generated:
* CyberSourceTransactionWS.cs contains the proxy classes.
* sample_wcf.exe.config is the configuration file for your application.
