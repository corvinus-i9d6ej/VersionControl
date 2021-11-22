using Moq;
using NUnit.Framework;
using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTestExample.Abstractions;
using UnitTestExample.Controllers;
using UnitTestExample.Entities;

namespace UnitTestExample.Test
{
    public class AccountControllerTestFixture
    {
        [
            Test,
            TestCase("abcd1234", false),
            TestCase("irf@uni-corvinus", false),
            TestCase("irf.uni-corvinus.hu", false),
            TestCase("irf@uni-corvinus.hu", true)
        ]
        public void TestValidateEmail(string email, bool expectedResult)
        {
            AccountController ac = new AccountController();
            var result = ac.ValidateEmail(email);
            Assert.AreEqual(result, expectedResult);
        }

        [
            Test,
            TestCase("abcdABCD", false),
            TestCase("ABCD1234", false),
            TestCase("abcd1234", false),
            TestCase("Ab1234", false),
            TestCase("Abcd1234", true)
        ]
        public void TestValidatePassword(string password, bool expectedResult)
        {
            AccountController ac = new AccountController();
            var result = ac.ValidatePassword(password);
            Assert.AreEqual(result, expectedResult);
        }

        [
            Test,
            TestCase("irf@uni-corvinus.hu", "Abcd1234"),
            TestCase("irf@uni-corvinus.hu", "Abcdef12"),
            TestCase("irf@uni-corvinus.hu", "Abcdefgh1234")
        ]
        public void TestRegisterHappyPath(string email, string password)
        {
            AccountController ac = new AccountController();
            var mock = new Mock<IAccountManager>(MockBehavior.Strict);
            mock.Setup(m => m.CreateAccount(It.IsAny<Account>())).Returns<Account>(a => a);
            ac.AccountManager = mock.Object;

            var result = ac.Register(email, password);

            Assert.AreEqual(email, result.Email);
            Assert.AreEqual(password, result.Password);
            Assert.AreNotEqual(Guid.Empty, result.ID);
            mock.Verify(m => m.CreateAccount(result), Times.Once);
        }

        [
            Test,
            TestCase("irf@uni-corvinus.hu", "Abcd1234"),
            TestCase("irf@uni-corvinus.hu", "Ab1234"),
            TestCase("irf.uni-corvinus.hu", "Abcd1234"),
        ]
        public void TestRegisterValidateException(string email, string password)
        {
            AccountController ac = new AccountController();
            try
            {
                var result = ac.Register(email, password);
                Assert.Fail();
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOf<ValidationException>(ex);
            }
        }
    }
}
