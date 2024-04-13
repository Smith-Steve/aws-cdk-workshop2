using Amazon.CDK;
using Amazon.CDK.AWS.DynamoDB;
using Amazon.CDK.AWS.Lambda;
using Constructs;
using System.Collections.Generic;

namespace CdkWorkshop2
{
    public class HitCounterProps
    {
        // The function for which we want to count url hits
        public IFunction Downstream { get; set; }
    }

    public class HitCounter : Construct
    {
        public Function Handler { get; }

        public HitCounter(Construct scope, string id, HitCounterProps props) : base(scope,id)
        {
            //We're defining the AWS Lambda Function and the DynamoDB table in our 'HitCounter' construct.

            //Here we defined a DynamoDB Table with 'path' as the partition key.
            // - Every DynamoDB table must have a single partition key
            var table = new Table(this, "Hits", new TableProps
            {
                PartitionKey = new Attribute
                {
                    Name = "path",
                    Type = AttributeType.STRING
                }
            });

            //This is our Lambda function.
            // - Here we are 'binding' the lambda to the 'lambda' directory, and the handler code. Below we will specify where each is performed.

            Handler = new Function(this, "HitCounterHandler", new FunctionProps
            {
                Runtime = Runtime.NODEJS_16_X,
                Handler = "hitcounter.handler", // We are naming our 'handler' function within the folder.
                Code = Code.FromAsset("lambda"), //This is where we are binding the code to the proper directory 'lambda'
                Environment = new Dictionary<string, string>
                {
                    //We 'wired' the lambda's environment variables to the 'FunctionName' and the 'TableName' of our resources.
                    //We have specifically set these up - I presume they are environmental variables
                    //So that their value is set dynamically.
                    //The reason that we do this is because the tables are not yet set up, so we want to be able to capture
                    //The values of the names so we don't encounter issues during deployment.

                    //I suppose the reason that we do this is because 
                    ["DOWNSTREAM_FUNCTION_NAME"] = props.Downstream.FunctionName,
                    ["HITS_TABLE_NAME"] = table.TableName
                }
            });

            table.GrantReadWriteData(Handler);

            props.Downstream.GrantInvoke(Handler);
        }
    }

}