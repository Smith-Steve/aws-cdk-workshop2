using Amazon.CDK;
using Amazon.CDK.AWS.SNS;
using Amazon.CDK.AWS.SNS.Subscriptions;
using Amazon.CDK.AWS.APIGateway;
using Amazon.CDK.AWS.Lambda;
using Amazon.CDK.AWS.SQS;
using Cdklabs.DynamoTableViewer;
using Constructs;
using System;
using System.Reflection.Metadata.Ecma335;

namespace CdkWorkshop2
{
    public class CdkWorkshop2Stack : Stack
    {
        public CdkWorkshop2Stack(Construct scope, string id, IStackProps props = null) : base(scope, id, props)
        {
            //Here we are defining a new lambda resource.
            var hello = new Function(this, "HelloHandler", new FunctionProps
            {
                Runtime = Runtime.NODEJS_16_X, // execution environment
                Code = Code.FromAsset("lambda"), // Code loaded from the "lambda" directory
                Handler = "hello.handler" // file is "hello", function is "handler"
            });

            //
            var helloWithCounter = new HitCounter(this, "HelloHitCounter", new HitCounterProps
            {
                Downstream = hello
            });

            //Here we are defining an API Gateway REST API resource backed by our "Hello" function.
            new LambdaRestApi(this, "Endpoint", new LambdaRestApiProps
            {
                Handler = helloWithCounter.Handler
            });

            new TableViewer(this, "ViewerHitCount", new TableViewerProps
            {
                Title = "Hello Hits",
                Table = helloWithCounter.MyTable
            });

        }
    }
}
