# many-requests

Run Docker container on your machine:

`docker pull tich92/siteattconsole`

`docker run -d --restart unless-stopped tich92/siteattconsole`

If you want to update image version:

1. `docker pull tich92/siteattconsole` (pull latest version)
1. `docker rm $(docker stop $(docker ps -a -q --filter ancestor=tich92/siteattconsole --format="{{.ID}}"))`
  _Can take a few seconds_
3. `docker run -d --restart unless-stopped tich92/siteattconsole`

**You can run multiple instances on the same VM or PC.**

## How to update target sites
All target sites stored in separate DB. You can update it via Swagger page of this API:

https://sites-func.azurewebsites.net/api/swagger/ui#/sites/GetSitesList
