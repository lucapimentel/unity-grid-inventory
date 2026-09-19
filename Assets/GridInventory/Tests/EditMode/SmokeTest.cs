using NUnit.Framework;

public class SmokeTest
{
    [Test]
    public void Toolchain_Runs()
    {
        Assert.That(2 + 2, Is.EqualTo(4));
    }
}
