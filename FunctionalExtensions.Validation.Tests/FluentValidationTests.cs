using System;
using System.Collections.Generic;
using System.Linq;
using FunctionalExtensions.Validation.Fluent;
using Xunit;

namespace FunctionalExtensions.Validation.Tests
{
    public class FluentValidationTests
    {
        [Theory]
        [InlineData("al-Chwarizmi", "Abu Dscha'far Muhammad ibn Musa", "M", true, new string[] { })]
        [InlineData(null, "Abu Dscha'far Muhammad ibn Musa", "M", false, new[] { "surname cannot be null" })]
        [InlineData("kukku mukku", "Abu Dscha'far Muhammad ibn Musa", "M", false, new[] { "surname cannot be kukku mukku" })]
        [InlineData("al-Chwarizmi", null, "M", false, new[] { "forename cannot be null" })]
        [InlineData("al-Chwarizmi", "Abu Dscha'far Muhammad ibn Musa", null, false, new[] { "gender cannot be null" })]
        [InlineData(null, null, null, false, new[] { "gender cannot be null", "forename cannot be null", "surname cannot be null" })]
        [InlineData("kukku mukku", null, null, false, new[] { "gender cannot be null", "forename cannot be null", "surname cannot be kukku mukku" })]
        [InlineData("kukku mukku", "friedrich", null, false, new[] { "gender cannot be null", "forename connot be friedrich", "surname cannot be kukku mukku", "forename must start with an \'A\'" })]
        [InlineData("kukku mukku", "friedrich", "sdfsdffsdfd", false, new[] { "forename connot be friedrich", "surname cannot be kukku mukku", "gender must be \'M\' or \'F\'", "forename must start with an \'A\'" })]
        [InlineData("al-Chwarizmi", "Abu Dscha'far Muhammad ibn Musa", "sdfsdf", false, new[] { "gender must be \'M\' or \'F\'" })]
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
                .AndSelect(x => x.Surname).IsNotNull("surname cannot be null")
                .AndSelect(x => x.Surname).Fulfills(x => x != "kukku mukku", "surname cannot be kukku mukku")
                .AndSelect(x => x.Forename).IsNotNull("forename cannot be null")
                .AndSelect(x => x.Forename).Fulfills(x => x != "friedrich", "forename connot be friedrich")
                .AndSelect(x => x.Forename).Fulfills(x => x.StartsWith("A"), "forename must start with an \'A\'")
                .AndSelect(x => x.Gender).IsNotNull("gender cannot be null")
                .AndSelect(x => x.Gender).Fulfills(x => x.ToUpper() == "M" || x.ToUpper() == "F", "gender must be \'M\' or \'F\'")
                .Result;

            var valid = result.Match(
                x => true,
                err =>
                {
                    Assert.Equal(errors.OrderBy(e => e), err.OrderBy(e => e));
                    return false;
                }
            );

            Assert.Equal(valid, isValid);
            
        }

        [Fact]
        public void Validate_Customer_Null()
        {
            var result = Validate
                .That<Customer>(null).IsNotNull("customer cannot be null")
                .AndSelect(x => x.Surname).IsNotNull("surname cannot be null")
                .AndSelect(x => x.Surname).Fulfills(x => x != "kukku mukku", "surname cannot be kukku mukku")
                .AndSelect(x => x.Forename).IsNotNull("forename cannot be null")
                .AndSelect(x => x.Forename).Fulfills(x => x != "friedrich", "forename connot be friedrich")
                .AndSelect(x => x.Forename).Fulfills(x => x.StartsWith("A"), "forename must start with an \'A\'")
                .AndSelect(x => x.Gender).IsNotNull("gender cannot be null")
                .AndSelect(x => x.Gender).Fulfills(x => x.ToUpper() == "M" || x.ToUpper() == "F", "gender must be \'M\' or \'F\'")
                .Result;

            var valid = result.Match(
                x => true,
                err =>
                {
                    Assert.Equal(new[] {"customer cannot be null"}, err);
                    return false;
                }
            );

            Assert.False(valid);

        }

        [Fact]
        public void Validate_Instance_Test()
        {
            const string s = "hello World";

            var result = Validate
                .That(s).IsNotNull("")
                .And.Fulfills(x => x.Length <= 5, "max length 5")
                .And.Fulfills(x => x.StartsWith("Hello"), "must start with \'Hello\'")
                .Result;

            var valid = result.Match(
                x => true,
                err =>
                {
                    Assert.Equal(new[] {"max length 5", "must start with \'Hello\'"}, err);
                    return false;
                }
            );

            Assert.False(valid);


        }

        [Fact]
        public void Validate_IsNotNull_Test()
        {

            var valid1 = Validate
                .That((Customer)null)
                .IsNotNull("customer not null")
                .AndSelect(x => x.Address)
                .IsNotNull("adresse not null")
                .Result
                .Match(
                    x => true,
                    err =>
                    {
                        Assert.Equal(new[] {"customer not null"}, err);
                        return false;
                    }
                );

            Assert.False(valid1);

            var valid2 = Validate.That((Customer) null)
                .IsNotNull("not null")
                .Result
                .Match(
                    x => true,
                    err =>
                    {
                        Assert.Equal(new[] {"not null",}, err);
                        return false;
                    }
                );

            Assert.False(valid2);

            var valid3 = ValidateWithErrorType<string>.That(new Customer())
                .IsNotNull("not null")
                .Result
                .Match(
                    x => true,
                    err => false
                );

            Assert.True(valid3);

        }

        [Fact]
        public void Validate_NotNull_Fulfills_Test()
        {
            var valid = ValidateWithErrorType<Error>
                .That(new Customer { Forename = null })
                .IsNotNull(Error.ShouldNotBeNull)
                .AndSelect(x => x.Forename).IsNotNull(Error.ForenameShouldNotBeNull)
                .AndSelect(x => x.Forename).Fulfills(x => x.Length < 10, Error.LengthShouldBeSmallerThan10)
                .Result.Match(
                    x => true,
                    err =>
                    {
                        Assert.Equal(new[] {Error.ForenameShouldNotBeNull}, err);
                        return false;
                    }
                );

            Assert.False(valid);

        }


        enum Error
        {
            LengthShouldBeSmallerThan10,
            ShouldNotBeNull,
            ForenameShouldNotBeNull
        }

        [Fact]
        public void NullReferenceException_Test()
        {
            var customer = new Customer();

            var valid1 = Validate
                .That(customer).IsNotNull("customer should not be null")
                .AndSelect(x => customer.Address)
                .Fulfills(x => x.Postcode.Length > 3, "length of postcode should be at least 4", x => "postcode cannot be null")
                .Result
                .Match(
                    x => true,
                    err =>
                    {
                        Assert.Equal(new[] {"postcode cannot be null"}, err);
                        return false;
                    }
                );

            Assert.False(valid1);


            var valid2 = Validate
                .That((Customer) null).IsNotNull("not null")
                .And.Fulfills(x => x.Address != null, "address not null", x => "address field should be accessible") // should cause exception that will be caught
                .Result
                .Match(
                    x => true,
                    err =>
                    {
                        Assert.Equal(new[] {"not null", "address field should be accessible"}, err);
                        return false;
                    }
                );

            Assert.False(valid2);

        }

        [Fact]
        public void Enumerable_Tests()
        {
            var args = new List<string> {"1", "2", "3" };

            var valid = Validate
                .That(args).IsNotNull("args should not be null")
                .And.Fulfills(x => x.Count > 4, "args should have 4 items")
                .AndSelect(x => x.ElementAt(0)).Fulfills(x => x == "0", x => String.Format("first element should be 0 but is {0}", x))
                .AndSelect(x => x.ElementAt(20)).Fulfills(x => x == "19", "20th item should be 19")
                .AndSelect(x => x.ElementAt(100)).Fulfills(x => x == "99", "100th item should be 99", x => "there should be a 100th item")
                .Result
                .Match(
                    x => true,
                    err =>
                    {
                        Assert.Equal(
                            new[]
                            {
                                "args should have 4 items", "first element should be 0 but is 1",
                                "there should be a 100th item"
                            }, err);
                        return false;
                    }
                );

            Assert.False(valid);

        }
    }
}
