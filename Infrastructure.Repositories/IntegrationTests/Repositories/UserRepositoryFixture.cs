using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FizzioFit.Common;
using FizzioFit.Domain.Entities;
using FizzioFit.Domain.Interfaces;
using FizzioFit.Domain.Services;
using FizzioFit.Infrastructure.Repositories.NHibernate;
using NHibernate;
using NUnit.Framework;

namespace IntegrationTests.Repositories
{
	[TestFixture]
	public class UserRepositoryFixture : ContainerFixture
	{
		private ISession _session;
		private IUserRepository _userRepository;

		[SetUp]
		public void SetUp()
		{
			_session = Container.Resolve<ISession>();
			_userRepository = new UserRepository(new SessionManager(() => _session), new CryptographyProvider());
		}

		private static void UpdateUserActiveSubscriptionAndDefaultPaymentDetails(ISession session, int userId, int subscriptionId, int paymentDetailsId)
		{
			session.Connection.ExecuteCommandNonQuery(@"
					update dbo.[User] set
						ActiveSubscriptionId = @subscriptionId,
						DefaultPaymentDetailsId = @paymentDetailsId
					where UserId = @userId
				", new {userid = userId, subscriptionId, paymentDetailsId});
		}

		[Test]
		public void User_can_be_loaded()
		{
			const int userId = 2;

			var planId = PlanRepositoryFixture.InsertPlan(_session);
			var paymentDetailsId = PaymentDetailsRepositoryFixture.InsertPaymentDetails(_session);
			var paymentId = PaymentRepositoryFixture.InsertPayment(_session, paymentDetailsId, "Plan", planId);
			var subscriptionId = SubscriptionRepositoryFixture.InsertSubscription(_session, planId, paymentId);
			UpdateUserActiveSubscriptionAndDefaultPaymentDetails(_session, userId, subscriptionId, paymentDetailsId);

			var user = (Client) _userRepository.GetById(userId);

			Assert.AreEqual("robgray", user.Username);
			Assert.AreEqual("rob.gray@outlook.com", user.EmailAddress);
			Assert.AreEqual("Rob", user.FirstName);
			Assert.AreEqual("Gray", user.LastName);
			Assert.AreEqual(new DateTime(2013, 7, 1), user.ActiveSubscription.DateStart);
			Assert.AreEqual("036E9776-2CB4-4090-A130-665313CF1AB7", user.DefaultPaymentDetails.MpayToken);
		}
	}
}
