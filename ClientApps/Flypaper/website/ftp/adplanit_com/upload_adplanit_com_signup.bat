echo off
echo =
echo =
echo =   OK to update WWW.ADPLANIT.COM website?
pause
ftp -i -s:upload_adplanit_com_signup.ftp
echo =
echo =
echo =    = = = = = =     UPLOADED:   WWW.ADPLANIT.COM website     = = = = = = 
echo =
echo =
pause