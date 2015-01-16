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
	public class PlanRepositoryFixture : TransactionContainerFixture
	{
		private ISession _session;
		private IPlanRepository _planRepository;

		[SetUp]
		public void SetUp()
		{
			_session = Container.Resolve<ISession>();
			_planRepository = new PlanRepository(new SessionManager(() => _session));
		}

		public static int InsertPlan(ISession session)
		{
			return session.Connection.ExecuteScalarCommand<int>(@"
					insert into dbo.[Plan]
						(PlanType, Available, DurationLength, DurationUnit, PriceCents, DisplayName, ReminderSecondsBeforeEnd)
					values
						('Standard', 1, 7, 1 /* Days */, 1000, 'Std (Wk)', 86400)
					select scope_identity()
				");
		}

		[Test]
		public void Plan_can_be_loaded()
		{
			var id = InsertPlan(_session);
			var plan = (PlanWithReminderBeforeEnd) _planRepository.GetById(id);

			// Check fetch
			Assert.IsTrue(plan is StandardPlan);
			Assert.IsTrue(plan.Available);

			Assert.AreEqual(7, plan.Duration.Length);
			Assert.AreEqual(DurationUnit.Days, plan.Duration.Unit);
			Assert.AreEqual(1000, plan.PriceCents);

			Assert.AreEqual("Std (Wk)", plan.DisplayName);
			Assert.AreEqual(TimeSpan.FromDays(1), plan.ReminderTimeSpanBeforeEnd);

			// Set 'day' duration and name
			plan.Duration = new Duration(1, DurationUnit.Days);
			plan.DisplayName = "Day";
			_session.Flush();
			_session.Evict(plan);

			// Adjust name
			_session.Connection.ExecuteCommandNonQuery(@"update dbo.[Plan] set DisplayName = null where Id = @id and DisplayName = 'Day'", new {id});
			plan = (PlanWithReminderBeforeEnd) _planRepository.GetById(id);

			// Check second fetch
			Assert.AreEqual(1, plan.Duration.Length);
			Assert.AreEqual(DurationUnit.Days, plan.Duration.Unit);
			Assert.AreEqual("Standard (1 Day)", plan.DisplayName);
		}

		[Test]
		public void GetAllAvailableExcludesAvailable()
		{
			InsertPlan(_session);
			var availablePlans = _planRepository.GetAllAvailable();
			Assert.IsNotEmpty(availablePlans);
			Assert.IsTrue(availablePlans.All(plan => plan.Available));
		}
	}
}
