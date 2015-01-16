using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FizzioFit.Common;
using FizzioFit.Domain.Entities;
using FizzioFit.Domain.Interfaces;
using FizzioFit.Infrastructure.Repositories.NHibernate;
using NHibernate;
using NUnit.Framework;

namespace IntegrationTests.Repositories
{
	[TestFixture]
	public class SubscriptionRepositoryFixture : TransactionContainerFixture
	{
		private ISession _session;
		private ISubscriptionRepository _subscriptionRepository;

		[SetUp]
		public void SetUp()
		{
			_session = Container.Resolve<ISession>();
			_subscriptionRepository = new SubscriptionRepository(new SessionManager(() => _session));
		}

		public static int InsertSubscription(ISession session, int planId, int paymentId)
		{
			return session.Connection.ExecuteScalarCommand<int>(@"
					insert into dbo.Subscription
						(UserId, PlanId, PaymentId, DateStart, DateEnd, DateCheckDue, CurrentOperation, CurrentOpAttemptedTimes, RenewalPlanId, ReminderIssued)
					values
						(2, @planId, @paymentId, '2013-07-01', null, getdate(), 'Reminder', 3, @planId, 1)

					select scope_identity()
				", new {planId, paymentId});
		}

		[Test]
		public void Subscription_can_be_loaded()
		{
			var planId = PlanRepositoryFixture.InsertPlan(_session);
			var paymentDetailsId = PaymentDetailsRepositoryFixture.InsertPaymentDetails(_session);
			var paymentId = PaymentRepositoryFixture.InsertPayment(_session, paymentDetailsId, "Plan", planId);
			var id = InsertSubscription(_session, planId, paymentId);
			var subscription = _subscriptionRepository.GetById(id);

			Assert.AreEqual("robgray", subscription.User.Username);
			Assert.AreEqual("Std (Wk)", subscription.Plan.DisplayName);
			Assert.AreEqual(1100, subscription.Payment.TotalAmountCents);

			Assert.AreEqual(new DateTime(2013, 7, 1), subscription.DateStart);
			Assert.IsNull(subscription.DateEnd);

			Assert.IsTrue(subscription.DateCheckDue.HasValue);
			Assert.AreEqual(DateTimeHelper.Now.Ticks, subscription.DateCheckDue.Value.Ticks, TimeSpan.FromSeconds(3).Ticks);
			Assert.AreEqual(Subscription.Operation.Reminder, subscription.CurrentOperation);
			Assert.AreEqual(3, subscription.CurrentOpAttemptedTimes);
			Assert.AreSame(subscription.Plan, subscription.RenewalPlan);
			Assert.IsTrue(subscription.ReminderIssued);
		}
	}
}
