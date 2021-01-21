# copy DLL to OLW plugins - %homepath%\AppData\Local\OpenLiveWriter\app-0.6.2\Plugins

Copy-Item .\OLWInsertCode\bin\Debug\OLWInsertCode.dll (Join-Path $home \AppData\Local\OpenLiveWriter\app-0.6.2\Plugins)
Copy-Item .\OLWInsertCode\bin\Debug\RtfPipe.dll (Join-Path $home \AppData\Local\OpenLiveWriter\app-0.6.2\Plugins)
