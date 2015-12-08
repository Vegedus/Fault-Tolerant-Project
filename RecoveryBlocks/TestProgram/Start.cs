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