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

            var result = ValidateWithErrorType<string>
                .That(customer).IsNotNull("customer cannot be null")
                .AndMember(x => x.Surname).IsNotNull("surname cannot be null")
                .AndMember(x => x.Surname).Fulfills(x => x != "kukku mukku", "surname cannot be kukku mukku")
                .AndMember(x => x.Forename).IsNotNull("forename cannot be null")
                .AndMember(x => x.Forename).Fulfills(x => x != "friedrich", "forename connot be friedrich")
                .AndMember(x => x.Forename).Fulfills(x => x.StartsWith("A"), "forename must start with an \'A\'")
                .AndMember(x => x.Gender).IsNotNull("gender cannot be null")
                .AndMember(x => x.Gender).Fulfills(x => x.ToUpper() == "M" || x.ToUpper() == "F", "gender must be \'M\' or \'F\'")
                .Result;

            result.Match(
                x => Assert.That(isValid),
                err =>
                {
                    Assert.That(!isValid);
                    Assert.That(err.Get, Is.EquivalentTo(errors));
                });
        }

        [Test]
        public void Validate_Customer_Null()
        {
            var result = ValidateWithErrorType<string>.That<Customer>(null).IsNotNull("customer cannot be null")
                .AndMember(x => x.Surname).IsNotNull("surname cannot be null")
                .AndMember(x => x.Surname).Fulfills(x => x != "kukku mukku", "surname cannot be kukku mukku")
                .AndMember(x => x.Forename).IsNotNull("forename cannot be null")
                .AndMember(x => x.Forename).Fulfills(x => x != "friedrich", "forename connot be friedrich")
                .AndMember(x => x.Forename).Fulfills(x => x.StartsWith("A"), "forename must start with an \'A\'")
                .AndMember(x => x.Gender).IsNotNull("gender cannot be null")
                .AndMember(x => x.Gender).Fulfills(x => x.ToUpper() == "M" || x.ToUpper() == "F", "gender must be \'M\' or \'F\'")
                .Result;

            result.Match(
                x => Assert.Fail(),
                err => Assert.That(err.Get, Is.EquivalentTo(new[] { "customer cannot be null" })));
        }

        [Test]
        public void Validate_Instance_Test()
        {
            const string s = "hello World";

            var result = ValidateWithErrorType<string>.That(s)
                .Fulfills(x => x.Length <= 5, "max length 5")
                .And.Fulfills(x => x.StartsWith("Hello"), "must start with \'Hello\'")
                .Result;

            result.Match(
                x => Assert.Fail(),
                err => Assert.That(err.Get, Is.EquivalentTo(new[] { "max length 5", "must start with \'Hello\'" })));
        }

        [Test]
        public void Validate_Test()
        {
            var result = ValidateWithErrorType<string>.That((Customer)null)
                .IsNotNull("customer not null")
                .AndMember(x => x.Address)
                .IsNotNull("adresse not null")
                .Result;

            result.Match(
                x => Assert.Fail(),
                err => Assert.That(err.Get, Is.EquivalentTo(new[] { "customer not null" })));
        }

        [Test]
        public void Validate_NotNullTwice_Test()
        {
            var result = ValidateWithErrorType<string>.That((Customer)null)
                .IsNotNull("not null")
                .AndMember(x => x)
                .IsNotNull("not null")
                .Result;

            result.Match(
                x => Assert.Fail(),
                err => Assert.That(err.Get, Is.EquivalentTo(new[] { "not null" })));
        }

        enum Error
        {
            LengthShouldBeSmallerThan10,
            ShouldStartWithHello
        }

        [Test]
        public void WithEnumTypeTest()
        {
            const string s = "hello World";

            var result = ValidateWithErrorType<Error>.That(s)
                .Fulfills(x => x.Length <= 5, Error.LengthShouldBeSmallerThan10)
                .And.Fulfills(x => x.StartsWith("Hello"), Error.ShouldStartWithHello)
                .Result;

            result.Match(
                x => Assert.Fail(),
                err => Assert.That(err.Get, Is.EquivalentTo(new[] { Error.LengthShouldBeSmallerThan10, Error.ShouldStartWithHello })));
        }
    }
}
