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
	public class PaymentDetailsRepositoryFixture : TransactionContainerFixture
	{
		private ISession _session;
		private IPaymentDetailsRepository _paymentDetailsRepository;

		[SetUp]
		public void SetUp()
		{
			_session = Container.Resolve<ISession>();
			_paymentDetailsRepository = new PaymentDetailsRepository(new SessionManager(() => _session));
		}

		public static int InsertPaymentDetails(ISession session)
		{
			return session.Connection.ExecuteScalarCommand<int>(@"
					insert into dbo.PaymentDetails
						(UserId, MpaySelected, MpayToken, MpayCardholderName, MpayMaskedCardNumber, MpayExpiresMonth, MpayExpiresYear)
					values
						(2, 1, '036E9776-2CB4-4090-A130-665313CF1AB7', 'Tester X Smith', '411111XXXXXX1111', 12, 2019)
					select scope_identity()
				");
		}

		[Test]
		public void PaymentDetails_can_be_loaded()
		{
			var id = InsertPaymentDetails(_session);
			var paymentDetails = _paymentDetailsRepository.GetById(id);

			Assert.IsTrue(paymentDetails.User is Client);
			Assert.AreEqual("robgray", paymentDetails.User.Username);

			Assert.IsTrue(paymentDetails.MpaySelected);
			Assert.AreEqual("036E9776-2CB4-4090-A130-665313CF1AB7", paymentDetails.MpayToken);
			Assert.AreEqual("Tester X Smith", paymentDetails.MpayCardholderName);
			Assert.AreEqual("411111XXXXXX1111", paymentDetails.MpayMaskedCardNumber);
			Assert.AreEqual(12, paymentDetails.MpayExpiresMonth);
			Assert.AreEqual(2019, paymentDetails.MpayExpiresYear);
		}
	}
}
