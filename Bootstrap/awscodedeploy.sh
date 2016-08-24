#!/bin/bash
apt-get -y update
apt-get -y install awscli
apt-get -y install ruby2.0
cd /home/ubuntu

export AWS_ACCESS_KEY_ID=AKIAIUS5PJ3IR535R7AQ
export AWS_SECRET_ACCESS_KEY=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

aws s3 cp s3://aws-codedeploy-eu-west-1/latest/install . --region eu-west-1
chmod +x ./install
./install auto

apt-get remove awscli -y
apt-get install python-pip -y
pip install awscli
