using Amazon.CDK;
using Amazon.CDK.AWS.Lambda;
using Constructs;

namespace CdkWorkshop2
{
    public class HitCounterProps
    {
        //The function for which we want to count url hits.
        public IFunction Downstream { get; set; }
    }

    public class HitCounter : Construct
    {
        public HitCounter(Construct scope, string id, HitCounterProps props) : base(scope,id
        {
            //TODO
        }
    }

}