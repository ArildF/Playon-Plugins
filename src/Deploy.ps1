stop-service -name "MediaMall Server"
Copy-Item Channel9Plugin\bin\Debug\Channel9Plugin.dll Channel9Plugin\bin\debug\Channel9Plugin.plugin
Copy-Item Channel9Plugin\bin\Debug\* "C:\Program Files (x86)\MediaMall\Plugins"
start-service -name "MediaMall Server"