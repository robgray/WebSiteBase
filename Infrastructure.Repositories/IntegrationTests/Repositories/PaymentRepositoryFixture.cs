using System;
using System.Collections.Generic;
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
	public class PaymentRepositoryFixture : TransactionContainerFixture
	{
		private ISession _session;
		private IPaymentRepository _paymentRepository;

		[SetUp]
		public void SetUp()
		{
			_session = Container.Resolve<ISession>();
			_paymentRepository = new PaymentRepository(new SessionManager(() => _session));
		}

		public static int InsertPayment(ISession session, int paymentDetailsId, string skuType, int skuId)
		{
			return session.Connection.ExecuteScalarCommand<int>(@"
					insert into dbo.Payment
						(UserId, PaymentDetailsId, SkuType, SkuId, LastModified, Username, UserEmailAddress, UserFirstName, UserLastName, UserIpAddress, UserDetails,
								MpayAmountCents, MpayAuthorizeTransactionId, MpayAuthorized, MpayCaptureTransactionId, MpaySucceeded, MpayRefundTransactionId, MpayRefunded, MpayErrorCode, MpayErrorDescription)
					values
						(2, @paymentDetailsId, @skuType, @skuId, getdate(), 'robgray', 'rob.gray@outlook.com', 'Rob', 'Gray', '127.0.0.1', '{...}',
								1100, 'AU33', 1, 'CP33', 1, 'RF33', 1, 0, 'Succeeded')

					select scope_identity()
				", new {paymentDetailsId, skuType, skuId});
		}

		[Test]
		public void Payment_can_be_loaded()
		{
			var planId = PlanRepositoryFixture.InsertPlan(_session);
			var paymentDetailsId = PaymentDetailsRepositoryFixture.InsertPaymentDetails(_session);
			var id = InsertPayment(_session, paymentDetailsId, "Plan", planId);
			var payment = _paymentRepository.GetById(id);

			Assert.AreEqual("robgray", payment.User.Username);
			Assert.AreEqual("036E9776-2CB4-4090-A130-665313CF1AB7", payment.PaymentDetails.MpayToken);
			Assert.AreEqual(1000, payment.Sku.PriceCents);
			Assert.AreEqual("Std (Wk)", ((Plan) payment.Sku).DisplayName);
			Assert.AreEqual(DateTimeHelper.Now.Ticks, payment.LastModified.Ticks, TimeSpan.FromSeconds(3).Ticks);

			Assert.AreEqual("robgray", payment.Username);
			Assert.AreEqual("rob.gray@outlook.com", payment.UserEmailAddress);
			Assert.AreEqual("Rob", payment.UserFirstName);
			Assert.AreEqual("Gray", payment.UserLastName);
			Assert.AreEqual("127.0.0.1", payment.UserIpAddress);
			Assert.AreEqual("{...}", payment.UserDetails);

			Assert.AreEqual(1100, payment.MpayAmountCents);
			Assert.AreEqual("AU33", payment.MpayAuthorizeTransactionId);
			Assert.IsTrue(payment.MpayAuthorized.HasValue && payment.MpayAuthorized.Value);
			Assert.AreEqual("CP33", payment.MpayCaptureTransactionId);
			Assert.IsTrue(payment.MpaySucceeded.HasValue && payment.MpaySucceeded.Value);
			Assert.AreEqual("RF33", payment.MpayRefundTransactionId);
			Assert.IsTrue(payment.MpayRefunded.HasValue && payment.MpayRefunded.Value);
			Assert.AreEqual(0, payment.MpayErrorCode);
			Assert.AreEqual("Succeeded", payment.MpayErrorDescription);
		}
	}
}
