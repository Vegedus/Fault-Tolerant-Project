using System;
using System.Collections.Generic;
using System.Diagnostics;
using TestProgram;
using Print = System.Diagnostics.Debug;

namespace RecoveryBlocks
{
    public static class Start
    {

        static void Main()
        {
            //Unit testing
            Test.TestCheckpoint();
            //Unit testing
            Test.TestRecoveryBlockPrime();
            //Acceptance/Chaos testing
            Test.TestRecoveryBlockSort();
        }
    }
}