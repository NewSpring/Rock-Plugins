A CyberSource plugin for Rock RMS: http://www.rockrms.com/

Built with the Rock developer kit: https://github.com/SparkDevNetwork/RockKit

______________________________________________
Installation:

* Download Rock.CyberSource.dll into RockWeb\Plugins: https://github.com/NewSpring/Cybersource-Plugin/blob/master/RockWeb/Plugins/Rock.CyberSource.dll
* Go to Admin Tools -> System Settings -> Financial Gateways and select the CyberSource option
* Fill out your MerchantID, Transaction Key, and Reporting User credentials (if you want to use reporting)

Please create an issue in this repository if you have issues.
______________________________________________

Developer notes for updating:

1. Make sure the WCF toolkit is installed from https://support.cybersource.com/cybskb/index?page=content&id=C631&actp=LIST
2. Open CMD shell in the sample code directory (specifically sample_wcf.cs).
3. Get the latest version number from https://ics2wstest.ic3.com/commerce/1.x/transactionProcessor.
4. Generate the proxy classes as follows:
svcutil /config:sample_wcf.exe.config  https://ics2wstest.ic3.com/commerce/1.x/transactionProcessor/CyberSourceTransaction_N.NN.wsdl

Two files are generated:
* CyberSourceTransactionWS.cs (ITransactionProcessor.cs) contains the proxy classes.
* sample_wcf.exe.config is the configuration file for your test application (not used in Rock).
