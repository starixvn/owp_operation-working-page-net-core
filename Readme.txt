cd D:\SourceCode\OWP\operation-working-page-netcore
docker build -t owp.websites:1.0 .
docker run -d -p 8095:80 --name owp.websites owp.websites:1.0