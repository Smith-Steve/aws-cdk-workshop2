﻿using Amazon.CDK;

namespace CdkWorkshop2
{
    sealed class Program
    {
        //This is the entry point to our application.
        public static void Main(string[] args)
        {
            var app = new App();
            new CdkWorkshop2Stack(app, "CdkWorkshop2Stack");

            app.Synth();
        }
    }
}
