cd D:\SourceCode\OWP\operation-working-page-netcore
docker build -t owp.websites:1.0 .

docker run -d -p 8095:80 -p 5000:5000 --name owp.websites owp.websites:1.0

CD OWP.Websites\App_Data\Fanex.Data
copy /Y DBOWP.bodb_OWP.bodbOWPNet.xml DBOWP.bodb_OWP.bodbOWPNet.LOCAL.xml
copy /Y DBOWP.bodb_OWP.bodbOWPNet.xml DBOWP.bodb_OWP.bodbOWPNet.LOCAL_1.xml
copy /Y DBOWP.bodb_OWP.bodbOWPNet.xml DBOWP.bodb_OWP.bodbOWPNet.LOCAL_2.xml
copy /Y DBOWP.bodb_OWP.bodbOWPNet.xml DBOWP.bodb_OWP.bodbOWPNet.LOCAL_3.xml
copy /Y DBOWP.bodb_OWP.bodbOWPNet.xml DBOWP.bodb_OWP.bodbOWPNet.PROD_1.xml
copy /Y DBOWP.bodb_OWP.bodbOWPNet.xml DBOWP.bodb_OWP.bodbOWPNet.SIT.xml
copy /Y DBOWP.bodb_OWP.bodbOWPNet.xml DBOWP.bodb_OWP.bodbOWPNet.UAT.xml
copy /Y DBOWP.bodb_OWP.bodbOWPNet.xml DBOWP.bodb_OWP.bodbOWPNet.UAT_1.xml
copy /Y DBOWP.bodb_OWP.bodbOWPNet.xml DBOWP.bodb_OWP.bodbOWPNet.UAT_2.xml
copy /Y DBOWP.bodb_OWP.bodbOWPNet.xml DBOWP.bodb_OWP.bodbOWPNet.UAT_3.xml