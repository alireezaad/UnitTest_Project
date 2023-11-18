using UnitTest_Project.TestData;
using Xunit.Abstractions;

namespace UnitTest_Project.Test
{
    public class MathHelperTest
    {
        private readonly ITestOutputHelper _outputHelper;

        public MathHelperTest(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        [Fact(Skip = "اسکیپ تستی تست")]
        [Trait("UI","Dashboard")]
        public void JamTest()
        {
            //arrange
            var sut = new MathHelper();

            //act
            var result = sut.Jam(5, 4);

            //assert
            Assert.Equal(9, result);
            Assert.IsType<int>(result);
        }
        [Theory]
        [Trait("UI","AdminPage")]
        [InlineData(55,45,100)]
        [InlineData(5,12,17)]
        public void JamTest_InlineData(int x, int y, object expected)
        {
            //arrange
            var sut = new MathHelper();

            //act
            var result = sut.Jam(x, y);

            //assert
            Assert.Equal(expected, result);
            Assert.IsNotType<double>(result);
            Assert.IsNotType<decimal>(result);
            Assert.IsNotType<float>(result);
            Assert.IsType<int>(result);
            _outputHelper.WriteLine($"In wondering Test is OK and result is {result}");
        }
        [Theory(Skip = "مدل کلس دیتا بهتر بود پس این کامنت شد")]
        [Trait("Endpoint","Discount")]
        [MemberData(nameof(MemberDataTest.GetData), MemberType = typeof(MemberDataTest))]
        public void JamTest_MemberData(int x, int y, int expected)
        {
            //arrange
            var sut = new MathHelper();

            //act
            var result = sut.Jam(x, y);

            //assert
            Assert.Equal(expected, result);
            Assert.IsNotType<double>(result);
            Assert.IsNotType<decimal>(result);
            Assert.IsNotType<float>(result);
            Assert.IsType<int>(result);
            //_outputHelper.WriteLine($"In wondering Test is OK and result is {result}");
        }
        [Theory]
        [Trait("Endpoint","Cart")]
        [ClassData(typeof(ClassData))]
        public void JamTest_ClassData(int x, int y, int expected)
        {
            //arrange
            var sut = new MathHelper();

            //act
            var result = sut.Jam(x, y);

            //assert
            Assert.Equal(expected, result);
            Assert.IsNotType<double>(result);
            Assert.IsNotType<decimal>(result);
            Assert.IsNotType<float>(result);
            Assert.IsType<int>(result);
            //_outputHelper.WriteLine($"In wondering Test is OK and result is {result}");
        }

    }
}