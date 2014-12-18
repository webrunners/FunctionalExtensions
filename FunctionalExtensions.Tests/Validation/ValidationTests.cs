using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using NUnit.Framework;
using FunctionalExtensions.Lambda;
using FunctionalExtensions.Validation;
using FunctionalExtensions.Validation.Fluent;
using FunctionalExtensions.Tests.Validation;

namespace FunctionalExtensions.Tests
{
    [TestFixture]
    public class ValidationTests
    {
        [Test]
        public void ValidationWithResultApplicativeFunctor_HappyDay_Test()
        {
            var result = 0.0;
            var d1 = Fun.Create(() => Choice.NewChoice1Of2<double, Errors>(2.5));
            var d2 = Fun.Create(() => Choice.NewChoice1Of2<double, Errors>(2.5));

            var callbackSome = Act.Create((double x) => result = x);

            var callbackNone = Act.Create((Errors x) => Assert.Fail());

            ValidationWithResultApplicativeFunctor(d1, d2, callbackSome, callbackNone);
            Assert.That(result, Is.EqualTo(100));
        }

        [Test]
        public void ValidationWithResultApplicativeFunctor_RainyDay_Test()
        {
            var d1 = Fun.Create(() => Choice.NewChoice1Of2<double, Errors>(2.5));
            var ex1 = Fun.Create(() => Choice.NewChoice2Of2<double, Errors>(new Errors("Error1")));
            var ex2 = Fun.Create(() => Choice.NewChoice2Of2<double, Errors>(new Errors("Error2")));
            var zero = Fun.Create(() => Choice.NewChoice1Of2<double, Errors>(0.0));

            var errors = new List<string>();
            Action<Errors> callbackNone = x => errors.AddRange(x.Messages);
            Action<double> callbackSome = x => Assert.Fail();

            ValidationWithResultApplicativeFunctor(d1, ex2, callbackSome, callbackNone);
            Assert.That(errors, Is.EquivalentTo(new[] {"Error2"}));

            errors.Clear();
            ValidationWithResultApplicativeFunctor(ex1, d1, callbackSome, callbackNone);
            Assert.That(errors, Is.EquivalentTo(new[] { "Error1" }));

            errors.Clear();
            ValidationWithResultApplicativeFunctor(ex1, ex2, callbackSome, callbackNone);
            Assert.That(errors, Is.EquivalentTo(new[] { "Error1", "Error2" }));

            errors.Clear();
            ValidationWithResultApplicativeFunctor(d1, zero, callbackSome, callbackNone);
            Assert.That(errors, Is.EquivalentTo(new[] { "Cannot devide by zero." }));
        }

        private static void ValidationWithResultApplicativeFunctor(Func<Choice<double, Errors>> readDouble1, Func<Choice<double, Errors>> readDouble2, Action<double> callBackSome, Action<Errors> callBackNone)
        {
            (
                from v1 in readDouble1()
                join v2 in readDouble2() on 1 equals 1
                from result in Divide(v1, v2).ToChoice(new Errors("Cannot devide by zero."))
                select result * 100
            )
                .Match(callBackSome, callBackNone);
        }

        private static Option<double> Divide(double a, double b)
        {
            return b == 0 ? Option.None<double>() : Option.Some(a / b);
        }

        [TestCase(null, "Abu Dscha'far Muhammad ibn Musa", "M", false, new[] { "surname cannot be null", "Line1 is empty but Line2 is not", "Post code can't be null", "Product name can't be null", "Cost for product 'Axt' can't be negative" })]
        [TestCase("al-Chwarizmi", null, "M", false, new[] { "forename cannot be null", "Line1 is empty but Line2 is not", "Post code can't be null", "Product name can't be null", "Cost for product 'Axt' can't be negative" })]
        [TestCase("al-Chwarizmi", "Abu Dscha'far Muhammad ibn Musa", null, false, new[] { "gender cannot be null", "Line1 is empty but Line2 is not", "Post code can't be null", "Product name can't be null", "Cost for product 'Axt' can't be negative" })]
        [TestCase(null, null, null, false, new[] { "gender cannot be null", "forename cannot be null", "surname cannot be null", "Line1 is empty but Line2 is not", "Post code can't be null", "Product name can't be null", "Cost for product 'Axt' can't be negative" })]
        [TestCase("kukku mukku", null, null, false, new[] { "gender cannot be null", "forename cannot be null", "surname cannot be kukku mukku", "Line1 is empty but Line2 is not", "Post code can't be null", "Product name can't be null", "Cost for product 'Axt' can't be negative" })]
        [TestCase("kukku mukku", "friedrich", null, false, new[] { "gender cannot be null", "forename cannot be friedrich", "surname cannot be kukku mukku", "forename must start with \'A\'", "Line1 is empty but Line2 is not", "Post code can't be null", "Product name can't be null", "Cost for product 'Axt' can't be negative" })]
        [TestCase("kukku mukku", "friedrich", "sdfsdffsdfd", false, new[] { "forename cannot be friedrich", "surname cannot be kukku mukku", "gender must be \'M\' or \'F\'", "forename must start with \'A\'", "Line1 is empty but Line2 is not", "Post code can't be null", "Product name can't be null", "Cost for product 'Axt' can't be negative" })]
        [TestCase("al-Chwarizmi", "Abu Dscha'far Muhammad ibn Musa", "sdfsdf", false, new[] { "gender must be \'M\' or \'F\'", "Line1 is empty but Line2 is not", "Post code can't be null", "Product name can't be null", "Cost for product 'Axt' can't be negative" })]
        public void Validator_Customer_RainyDay_Test(string surname, string forename, string gender, bool isValid, string[] errors)
        {
            var customer = new Customer
            {
                Surname = surname,
                Forename = forename,
                Gender = gender,
                Address = new Address { County = "Wonderland", Line2 = "No. 3", Town = "Cape Town" },
                Orders = new[] { new Order(), new Order { ProductName = "Axt", Cost = -8999.56m } }
            };

            var validateGender = Validator.Create<string>(g => g.ToUpper() == "M" || g.ToUpper() == "F", "gender must be \'M\' or \'F\'");
            var validateForename = Validator.Create<string>(fn => fn.StartsWith("A"), "forename must start with \'A\'");
            var validateOrders = Validator.EnumerableValidator<Order>(ValidateOrder);

            var result =
                from surnamecheck in (
                    from surnamenotnull in Validator.NotNull(customer.Surname, "surname cannot be null")
                    from surnamenotkukkumukku in Validator.NotEqual(customer.Surname, "kukku mukku", "surname cannot be kukku mukku") select customer)

                join forenamecheck in (
                    from fornamenotnull in Validator.NotNull(customer.Forename, "forename cannot be null")
                    from forenamenotnull in (
                        from forenamenotfriedrich in Validator.NotEqual(customer.Forename, "friedrich", "forename cannot be friedrich")
                        join forenamestartswith in validateForename(customer.Forename) on 1 equals 1
                        select customer) select customer) on 1 equals 1

                join gendercheck in ( 
                    from gendernotnull in Validator.NotNull(customer.Gender, "gender cannot be null")
                    from  gender1 in validateGender(customer.Gender) select customer) on 1 equals 1

                join addresscheck in ValidateAddress(customer.Address) on 1 equals 1

                join orderscheck in (
                    from ordersnotnull in Validator.NotNull(customer.Orders, "orders cannot be null")
                    from orders1 in validateOrders(customer.Orders) select customer) on 1 equals 1

                select customer;

            result.Match(
                x => Assert.That(isValid),
                err =>
                {
                    Assert.That(!isValid);
                    Assert.That(err.Messages, Is.EquivalentTo(errors));
                });
        }

        [Test]
        public void Validator_Customer_HappyDay_Test()
        {
            var customer = new Customer
            {
                Surname = "al-Chwarizmi",
                Forename = "Abu Dscha'far Muhammad ibn Musa",
                Gender = "m",
                Address = new Address { County = "Wonderland", Postcode = "4711", Line1 = "Jump Street", Town = "Cape Town" },
                Orders = new[] { new Order { ProductName = "Axt", Cost = 8999.56m } }
            };

            var validateGender = Validator.Create<string>(g => g.ToUpper() == "M" || g.ToUpper() == "F", "gender must be \'M\' or \'F\'");
            var validateForename = Validator.Create<string>(fn => fn.StartsWith("A"), "forename must start with \'A\'");
            var validateOrders = Validator.EnumerableValidator<Order>(ValidateOrder);

            var result =
                from surnamecheck in
                    (
                        from surnamenotnull in Validator.NotNull(customer.Surname, "surname cannot be null")
                        from surnamenotkukkumukku in Validator.NotEqual(customer.Surname, "kukku mukku", "surname cannot be kukku mukku")
                        select customer)

                join forenamecheck in
                    (
                        from fornamenotnull in Validator.NotNull(customer.Forename, "forename cannot be null")
                        from forenamenotnull in
                            (
                                from forenamenotfriedrich in Validator.NotEqual(customer.Forename, "friedrich", "forename cannot be friedrich")
                                join forenamestartswith in validateForename(customer.Forename) on 1 equals 1
                                select customer)
                        select customer) on 1 equals 1

                join gendercheck in
                    (
                        from gendernotnull in Validator.NotNull(customer.Gender, "gender cannot be null")
                        from gender1 in validateGender(customer.Gender)
                        select customer) on 1 equals 1

                join addresscheck in ValidateAddress(customer.Address) on 1 equals 1

                join orderscheck in
                    (
                        from ordersnotnull in Validator.NotNull(customer.Orders, "orders cannot be null")
                        from orders1 in validateOrders(customer.Orders)
                        select customer) on 1 equals 1

                select customer;

            result.Match(
                x => Assert.Pass(),
                err =>
                {
                    Assert.Fail();
                });
        }

        [Test]
        public void Validator_Address_Null_Test()
        {
            var customer = new Customer
            {
                Discount = -1,
                Address = null,
                Orders = null
            };

            Func<string, string, Choice<string, Errors>> notNullOrEmpty = (s, s1) => Validator.Create<string>(x => !String.IsNullOrEmpty(x), s1)(s);

            var result =
                from c in Validator.NotNull(customer, "Customer cannot be null")
                from notNull in (
                    from surname in notNullOrEmpty(customer.Surname, "Surname can't be null")
                    from surname2 in Validator.NotEqual(customer.Surname, "foo", "Surname can't be foo")
                    join address in ValidateAddress(customer.Address) on 1 equals 1
                    join orders in ValidateOrders(customer.Orders) on 1 equals 1
                    select customer)
                select customer;

            result.Match(
                customer1 => { },
                errors => Assert.That(errors.Messages.ToList(), Is.EquivalentTo(new[] { "Address cannot be null", "Surname can't be null", "Orders cannot be NULL" })));
        }

        static Choice<Address, Errors> ValidateAddress(Address address)
        {
            var validateAddressLines = Validator.Create<Address>(x => x.Line1 != null || x.Line2 == null, "Line1 is empty but Line2 is not");

            return
                from addresschek in Validator.NotNull(address, "Address cannot be null")
                from addressnotnull in
                    (
                        from postcode in Validator.NotNull(address.Postcode, "Post code can't be null")
                        join addressline in validateAddressLines(address) on 1 equals 1
                        select address
                    )
                select address;
        }

        static Choice<IEnumerable<Order>, Errors> ValidateOrders(IEnumerable<Order> orders)
        {
            return
                from o in Validator.NotNull(orders, "Orders cannot be NULL")
                from notNull in Validator.EnumerableValidator<Order>(ValidateOrder)(orders)
                select orders;
        }

        static Choice<Order, Errors> ValidateOrder(Order o)
        {
            return
                from order in Validator.NotNull(o, "Order cannot be NULL")
                from name in Validator.NotNull(o.ProductName, "Product name can't be null")
                from cost in Validator.Create<Order>(x => x.Cost >= 0, string.Format("Cost for product '{0}' can't be negative", name))(o)
                select o;
        }
    }
}
