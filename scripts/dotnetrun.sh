#!/bin/bash
cd /var/urlshortnerdemo
dotnet restore
sudo nohup dotnet run kestrel > /dev/null 2>&1 &