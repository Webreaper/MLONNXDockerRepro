
FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine AS final

WORKDIR /app
COPY ./src/Yolov5Net/bin/Release/net5.0/linux-x64/publish/ .
RUN chmod +x Yolov5Net 

# Install this for onnx - per https://stackoverflow.com/questions/61407089/asp-net-core-load-an-onnx-file-inside-a-docker-container
RUN apk --update add --no-cache libgomp libstdc++

EXPOSE 6363
ENTRYPOINT ["sh","./Yolov5Net"]
