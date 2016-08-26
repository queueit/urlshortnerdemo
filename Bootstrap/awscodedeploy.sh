#!/bin/bash
apt-get -y update
apt-get remove awscli -y
apt-get install python-pip -y
apt-get -y install ruby2.0
pip install awscli

cd /home/ubuntu

aws s3 cp s3://aws-codedeploy-eu-west-1/latest/install . --region eu-west-1
chmod +x ./install
./install auto
