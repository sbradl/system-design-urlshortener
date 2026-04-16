using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Shortener.Test;

[TestClass]
public class ShortenerServiceImplTest
{
  [TestMethod]
  public async Task GivenSameUniqueId_Generate_ReturnsSameKey()
  {
    Assert.AreEqual(
      await new ShortenerServiceImpl(new UniqueIdServiceDummy()).Generate(),
      await new ShortenerServiceImpl(new UniqueIdServiceDummy()).Generate()
    );
  }

  [TestMethod]
  public async Task GivenDifferentUniqueId_Generate_ReturnsDifferentKey()
  {
    var service = new ShortenerServiceImpl(new UniqueIdServiceDummy());

    Assert.AreNotEqual(
      await service.Generate(),
      await service.Generate()
    );
  }

  [TestMethod]
  [DataRow(0ul, "0")]          // zero
  [DataRow(1ul, "1")]          // one
  [DataRow(9ul, "9")]          // last digit
  [DataRow(10ul, "A")]         // first uppercase
  [DataRow(35ul, "Z")]         // last uppercase
  [DataRow(36ul, "a")]         // first lowercase
  [DataRow(42ul, "g")]
  [DataRow(61ul, "z")]         // last single-char value
  [DataRow(62ul, "10")]        // first two-char value
  [DataRow(165485646ul, "BCMNK")]
  [DataRow(ulong.MaxValue, "LygHa16AHYF")]  // largest possible id
  public async Task GeneratedKey_ShouldBeBase62Encoded(ulong id, string expectedKey)
  {
    var service = new ShortenerServiceImpl(new UniqueIdServiceDummy(id));

    Assert.AreEqual(expectedKey, await service.Generate());
  }

  [TestMethod]
  public async Task GeneratedKey_ShouldOnlyContainBase62Characters()
  {
    const string validChars = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
    var service = new ShortenerServiceImpl(new UniqueIdServiceDummy(ulong.MaxValue));

    var key = await service.Generate();

    Assert.IsTrue(key.All(c => validChars.Contains(c)));
  }

  private sealed class UniqueIdServiceDummy(ulong nextId = 0) : UniqueIdService
  {
    private ulong currentId = nextId;

    public Task<ulong> NextId()
    {
      return Task.FromResult(currentId++);
    }
  }
}
