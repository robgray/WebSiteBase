using Infrastructure.Common;
using NUnit.Framework;

namespace UnitTests.Common
{
    [TestFixture]
    public class EnumHelperTests
    {
        public enum TestStatus
        {
            [EnumDisplay(Description = "I have an attribute with description and sort order", SortOrder = 1)]
            IHaveAnAttributeWithDescriptionAndSortOrder,
            [EnumDisplay(Description = "I have an attribute with description and no sort order")]
            IHaveAnAttributeWithDescriptionAndNoSortOrder,
            [EnumDisplay(SortOrder = 2)]
            IHaveAnAttributeWithNoDescriptionAndSortOrder,
            [EnumDisplay]
            IHaveAnAttributeWithNoDescriptionAndNoSortOrder,
            IDoNotHaveAnAttribute
        };

        [Test]
        public void CanGetEnumTryGetEnumFromValue_uses_default_if_not_found()
        {
            var value = Enum<TestStatus>.TryGetEnumFromValue(12, TestStatus.IDoNotHaveAnAttribute);

            Assert.AreEqual(TestStatus.IDoNotHaveAnAttribute, value);
        }

        [Test]
        public void Can_get_description_from_enum()
        {
            Assert.AreEqual("I have an attribute with description and sort order", TestStatus.IHaveAnAttributeWithDescriptionAndSortOrder.GetDescription());
        }

        [Test]
        public void Can_get_sort_order_from_enum()
        {
            Assert.AreEqual(2, TestStatus.IHaveAnAttributeWithNoDescriptionAndSortOrder.GetSortOrder());
        }

        [Test]
        public void SortOrder_is_max_when_not_specified()
        {
            Assert.AreEqual(int.MaxValue, TestStatus.IHaveAnAttributeWithNoDescriptionAndNoSortOrder.GetSortOrder());
        }

        [Test]
        public void SortOrder_is_max_when_has_no_attribute()
        {
            Assert.AreEqual(int.MaxValue, TestStatus.IDoNotHaveAnAttribute.GetSortOrder());
        }

        [Test]
        public void Description_is_enum_when_has_no_attribute()
        {
            Assert.AreEqual("IDoNotHaveAnAttribute", TestStatus.IDoNotHaveAnAttribute.GetDescription());
        }

        [Test]
        public void Description_is_enum_when_has_not_specified()
        {
            Assert.AreEqual("IHaveAnAttributeWithNoDescriptionAndNoSortOrder", TestStatus.IHaveAnAttributeWithNoDescriptionAndNoSortOrder.GetDescription());
        }

        [Test]
        public void GetIdAndDescriptionList_respects_sort_order()
        {
            var referenceData = Enum<TestStatus>.GetLookupValueItemList();

            Assert.AreEqual("I have an attribute with description and sort order", referenceData[0].Description);
            Assert.AreEqual("IHaveAnAttributeWithNoDescriptionAndSortOrder", referenceData[1].Description);
            Assert.AreEqual("I have an attribute with description and no sort order", referenceData[2].Description);
            Assert.AreEqual("IHaveAnAttributeWithNoDescriptionAndNoSortOrder", referenceData[3].Description);
            Assert.AreEqual("IDoNotHaveAnAttribute", referenceData[4].Description);
        }

        [Test]
        public void GetIdAndDescriptionList_uses_value_as_id()
        {
            var referenceData = Enum<TestStatus>.GetLookupValueItemList();

            Assert.AreEqual((int)TestStatus.IHaveAnAttributeWithDescriptionAndSortOrder, referenceData[0].Id);
            Assert.AreEqual((int)TestStatus.IHaveAnAttributeWithNoDescriptionAndSortOrder, referenceData[1].Id);
            Assert.AreEqual((int)TestStatus.IHaveAnAttributeWithDescriptionAndNoSortOrder, referenceData[2].Id);
            Assert.AreEqual((int)TestStatus.IHaveAnAttributeWithNoDescriptionAndNoSortOrder, referenceData[3].Id);
            Assert.AreEqual((int)TestStatus.IDoNotHaveAnAttribute, referenceData[4].Id);
        }
    }
}
