{
    "Version": "2012-10-17",
    "Statement": [
        {
            "Sid": "Stmt1472203785000",
            "Effect": "Allow",
            "Action": [
                "s3:*"
            ],
            "Resource": [
                "arn:aws:s3:::aws-codedeploy-eu-west-1/*",
                "arn:aws:s3:::aws-codedeploy-eu-west-1"
            ]
        },
        {
            "Sid" : "Deployment",
            "Action" : [
				"elasticloadbalancing:Describe*",
				"elasticloadbalancing:DeregisterInstancesFromLoadBalancer",
				"elasticloadbalancing:RegisterInstancesWithLoadBalancer",
				"autoscaling:Describe*",
				"autoscaling:EnterStandby",
				"autoscaling:ExitStandby",
				"autoscaling:UpdateAutoScalingGroup",
				"autoscaling:SuspendProcesses",
				"autoscaling:ResumeProcesses"
            ],
            "Effect" : "Allow",
            "Resource" : "*"
        },
        {
            "Sid" : "ReadWriteDynamoDB",
            "Action" : [
                "dynamodb:Query",
                "dynamodb:DescribeTable",
                "dynamodb:UpdateItem",
                "dynamodb:PutItem",
                "dynamodb:GetItem"
            ],
            "Effect" : "Allow",
            "Resource" : "arn:aws:dynamodb:eu-west-1:*"
        }
    ]
}
