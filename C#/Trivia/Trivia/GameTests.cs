using ApprovalTests;
using NUnit.Framework;

namespace Trivia
{
    [TestFixture]
    public class GameTests
    {
        [Test]
        public void TestSomething()
        {
            var mockWriter = new MockWriter();
            GameRunner.GameLoop(() => 1, () => 1, mockWriter);

            Approvals.VerifyXml(mockWriter.OutputSoFar);
        }
    }
}
