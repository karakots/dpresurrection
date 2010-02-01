echo off
echo =
echo =
echo =   OK to update WWW.ADPLANIT.COM website?
pause
ftp -i -s:upload_adplanit_com.ftp
echo =
echo =
echo =    = = = = = =     UPLOADED:   WWW.ADPLANIT.COM website     = = = = = = 
echo =
echo =
pause