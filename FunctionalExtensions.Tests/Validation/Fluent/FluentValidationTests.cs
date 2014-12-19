using FunctionalExtensions.Validation.Fluent;
using NUnit.Framework;

namespace FunctionalExtensions.Tests.Validation.Fluent
{
    [TestFixture]
    public class FluentValidationTests
    {
        [TestCase("al-Chwarizmi", "Abu Dscha'far Muhammad ibn Musa", "M", true, new string[] { })]
        [TestCase(null, "Abu Dscha'far Muhammad ibn Musa", "M", false, new[] { "surname cannot be null" })]
        [TestCase("kukku mukku", "Abu Dscha'far Muhammad ibn Musa", "M", false, new[] { "surname cannot be kukku mukku" })]
        [TestCase("al-Chwarizmi", null, "M", false, new[] { "forename cannot be null" })]
        [TestCase("al-Chwarizmi", "Abu Dscha'far Muhammad ibn Musa", null, false, new[] { "gender cannot be null" })]
        [TestCase(null, null, null, false, new[] { "gender cannot be null", "forename cannot be null", "surname cannot be null" })]
        [TestCase("kukku mukku", null, null, false, new[] { "gender cannot be null", "forename cannot be null", "surname cannot be kukku mukku" })]
        [TestCase("kukku mukku", "friedrich", null, false, new[] { "gender cannot be null", "forename connot be friedrich", "surname cannot be kukku mukku", "forename must start with an \'A\'" })]
        [TestCase("kukku mukku", "friedrich", "sdfsdffsdfd", false, new[] { "forename connot be friedrich", "surname cannot be kukku mukku", "gender must be \'M\' or \'F\'", "forename must start with an \'A\'" })]
        [TestCase("al-Chwarizmi", "Abu Dscha'far Muhammad ibn Musa", "sdfsdf", false, new[] { "gender must be \'M\' or \'F\'" })]
        public void Validate_Test(string surname, string forename, string gender, bool isValid, string[] errors)
        {
            var customer = new Customer
            {
                Surname = surname,
                Forename = forename,
                Gender = gender
            };

            var result = Validate
                .That(customer).IsNotNull("customer cannot be null")
                .And(x => x.Surname).IsNotNull("surname cannot be null")
                .And(x => x.Surname).Fulfills(x => x != "kukku mukku", "surname cannot be kukku mukku")
                .And(x => x.Forename).IsNotNull("forename cannot be null")
                .And(x => x.Forename).Fulfills(x => x != "friedrich", "forename connot be friedrich")
                .And(x => x.Forename).Fulfills(x => x.StartsWith("A"), "forename must start with an \'A\'")
                .And(x => x.Gender).IsNotNull("gender cannot be null")
                .And(x => x.Gender).Fulfills(x => x.ToUpper() == "M" || x.ToUpper() == "F", "gender must be \'M\' or \'F\'")
                .Result;

            result.Match(
                x => Assert.That(isValid),
                err =>
                {
                    Assert.That(!isValid);
                    Assert.That(err.Messages, Is.EquivalentTo(errors));
                });
        }

        [Test]
        public void Validate_Customer_Null()
        {
            var result = Validate
                .That<Customer>(null).IsNotNull("customer cannot be null")
                .And(x => x.Surname).IsNotNull("surname cannot be null")
                .And(x => x.Surname).Fulfills(x => x != "kukku mukku", "surname cannot be kukku mukku")
                .And(x => x.Forename).IsNotNull("forename cannot be null")
                .And(x => x.Forename).Fulfills(x => x != "friedrich", "forename connot be friedrich")
                .And(x => x.Forename).Fulfills(x => x.StartsWith("A"), "forename must start with an \'A\'")
                .And(x => x.Gender).IsNotNull("gender cannot be null")
                .And(x => x.Gender).Fulfills(x => x.ToUpper() == "M" || x.ToUpper() == "F", "gender must be \'M\' or \'F\'")
                .Result;

            result.Match(
                x => Assert.Fail(),
                err => Assert.That(err.Messages, Is.EquivalentTo(new[] { "customer cannot be null" })));
        }

        [Test]
        public void Test()
        {
            //var result = Validate2
            //    .That(new Customer()).IsNotNull("customer cannot be null")
            //    .And(x => x.Forename)
            //    .Fulfills(x => x != "friedrich", "forename cannot be friedrich")
            //    .And(x => x)
            //    .Result;

            //result.Match(
            //    x => { },
            //    err => { });
        }
    }
}
