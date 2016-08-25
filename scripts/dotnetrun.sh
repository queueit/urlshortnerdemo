#!/bin/bash
cd /var/urlshortnerdemo/UrlShortnerDemo
dotnet restore --source 93.184.215.200
sudo nohup dotnet run kestrel > /dev/null 2>&1 &
