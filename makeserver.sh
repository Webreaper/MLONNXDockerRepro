dotnet publish Yolov5Net.sln -r linux-x64 -c Release --self-contained true /p:PublishSingleFile=true /p:PublishTrimmed=true /p:Version=1.0.0 /p:IncludeNativeLibrariesForSelfExtract=true

docker build -t yolov5 . 