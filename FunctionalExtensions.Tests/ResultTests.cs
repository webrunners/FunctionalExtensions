using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace FunctionalExtensions.Tests
{
    [TestFixture]
    public class ResultTests
    {
        [Test]
        public void ValidationWithResultApplicativeFunctor_HappyDay_Test()
        {
            var result = 0.0;
            var d1 = Lambda.Create(() => Choice.NewChoice1Of2<double, Errors>(2.5));
            var d2 = Lambda.Create(() => Choice.NewChoice1Of2<double, Errors>(2.5));

            var callbackSome = Lambda.Create((double x) => result = x);

            var callbackNone = Lambda.Create((Errors x) => Assert.Fail());

            // happy day
            ValidationWithResultApplicativeFunctor(d1, d2, callbackSome, callbackNone);
            Assert.That(result, Is.EqualTo(100));
        }

        [Test]
        public void ValidationWithResultApplicativeFunctor_RainyDay_Test()
        {
            var d1 = Lambda.Create(() => Choice.NewChoice1Of2<double, Errors>(2.5));
            var ex1 = Lambda.Create(() => Choice.NewChoice2Of2<double, Errors>(new Errors("Error1")));
            var ex2 = Lambda.Create(() => Choice.NewChoice2Of2<double, Errors>(new Errors("Error2")));
            var zero = Lambda.Create(() => Choice.NewChoice1Of2<double, Errors>(0.0));

            // errors
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
                from result in Devide(v1, v2).ToChoice(new Errors("Cannot devide by zero."))
                select result * 100
            )
                .Match(callBackSome, callBackNone);
        }

        private static Option<double> Devide(double a, double b)
        {
            return b == 0 ? Option.None<double>() : Option.Some(a / b);
        }

        class Address
        {
            public string Line1 { get; set; }
            public string Line2 { get; set; }
            public string Town { get; set; }
            public string County { get; set; }
            public string Postcode { get; set; }
        }

        class Order
        {
            public string ProductName { get; set; }
            public decimal? Cost { get; set; }
        }

        class Customer
        {
            public int Id { get; set; }
            public string Surname { get; set; }
            public string Forename { get; set; }
            public decimal Discount { get; set; }
            public Address Address { get; set; }
            public IList<Order> Orders { get; set; }
        }

        [Test]
        public void Test()
        {
            var customer = new Customer
            {
                Id = 0,
                Forename = "bugs bunny",
                Surname = "foo",
                Discount = -1,
                Address = new Address { County = "Wonderland", Line2 = "No. 3", Town = "Cape Town" },
                Orders = new[] { new Order(), new Order { ProductName = "Axt" , Cost = -8999.56m } }
            };

            var validateOrders = Validation.EnumerableValidator<Order>(ValidateOrder);

            var result =
                from surname in Validation.NonNull(customer.Surname, "Surname can't be null")
                from surname2 in Validation.NotEqual(customer.Surname, "foo", "Surname can't be foo")
                join address in ValidateAddress(customer.Address) on 1 equals 1
                join orders in validateOrders(customer.Orders) on 1 equals 1
                select customer;

            result.Match(
                customer1 => { },
                errors => Assert.That(errors.Messages.ToList(), Is.EquivalentTo(new[] { "Post code can't be null", "Surname can't be foo", "Line1 is empty but Line2 is not", "Product name can't be null", "Cost for product 'Axt' can't be negative" })));
        }

        [Test]
        public void Address_null()
        {
            var customer = new Customer
            {
                Discount = -1,
                Address = null,
                Orders = null
            };

            var validateOrders = Validation.EnumerableValidator<Order>(ValidateOrder);

            var result =
                from surname in Validation.NotNullOrEmpty(customer.Surname, "Surname can't be null")
                from surname2 in Validation.NotEqual(customer.Surname, "foo", "Surname can't be foo")
                join address in ValidateAddress(customer.Address) on 1 equals 1
                join ordersnotnull in Validation.NonNull(customer.Orders, "Orders cannot be NULL") on 1 equals 1
                from orders in validateOrders(customer.Orders) 
                select customer;

            result.Match(
                customer1 => { },
                errors => Assert.That(errors.Messages.ToList(), Is.EquivalentTo(new[] { "Address cannot be null", "Surname can't be null", "Orders cannot be NULL" })));
        }

        private static Choice<Address, Errors> ValidateAddress(Address a)
        {
            var validateAddressLines = Validation.Validator<Address>(x => x.Line1 != null || x.Line2 == null, "Line1 is empty but Line2 is not");

            return
                from notull in Validation.NonNull(a, "Address cannot be null")
                from onlyIfNotNull in
                    (
                        from x in Validation.NonNull(a.Postcode, "Post code can't be null")
                        join y in validateAddressLines(a) on 1 equals 1
                        select a
                    )
                select a;
        }

        static Choice<Order, Errors> ValidateOrder(Order o)
        {
            return
                from name in Validation.NonNull(o.ProductName, "Product name can't be null")
                from cost in Validation.Validator<Order>(x => x.Cost >= 0, string.Format("Cost for product '{0}' can't be negative", name))(o)
                select o;
        }
    }
}
