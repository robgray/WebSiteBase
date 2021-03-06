declare @Plans table (
	PlanId int not null
)
insert into @Plans
select Id from dbo.[Plan]
	where (DurationUnit = 0 and DurationLength = PriceCents * 10 and ReminderSecondsBeforeEnd = 5)
	or (DurationUnit = 4 and PriceCents = DurationLength * 100 and ReminderSecondsBeforeEnd = 86400)

update dbo.[User]
	set ActiveSubscriptionId = NULL
where ActiveSubscriptionId in (
	select Id
	from dbo.Subscription
	where PlanId in (select PlanId from @Plans)
)

delete from dbo.Subscription
	where PlanId in (select PlanId from @Plans)

delete from dbo.[Plan]
	where Id in (select PlanId from @Plans)

insert into dbo.[Plan]
	(PlanType, Available, DurationLength, DurationUnit, PriceCents, DisplayName, ReminderSecondsBeforeEnd)
select			'Standard', 1, 10, 0, 1, null, 5
union all select	'Standard', 1, 20, 0, 2, null, 5
union all select	'Standard', 1, 30, 0, 3, null, 5
union all select	'Standard', 1, 40, 0, 4, null, 5
union all select	'Standard', 1, 50, 0, 5, null, 5
union all select	'Standard', 1, 60, 0, 6, 'Standard (One Minute)', 5
union all select	'Standard', 1, 70, 0, 7, null, 5
union all select	'Standard', 1, 80, 0, 8, null, 5
union all select	'Standard', 1, 90, 0, 9, null, 5
union all select	'Standard', 1, 100, 0, 10, null, 5
union all select	'Standard', 1, 110, 0, 11, null, 5
union all select	'Standard', 1, 120, 0, 12, 'Standard (Two Minutes)', 5

union all select	'Standard', 1, 1, 4, 100, null, 86400
union all select	'Standard', 1, 2, 4, 200, null, 86400
union all select	'Standard', 1, 3, 4, 300, null, 86400
union all select	'Standard', 1, 4, 4, 400, null, 86400
union all select	'Standard', 1, 5, 4, 500, null, 86400
union all select	'Standard', 1, 6, 4, 600, null, 86400
union all select	'Standard', 1, 7, 4, 700, null, 86400
union all select	'Standard', 1, 8, 4, 800, null, 86400
union all select	'Standard', 1, 9, 4, 900, null, 86400
union all select	'Standard', 1, 10, 4, 1000, null, 86400
union all select	'Standard', 1, 11, 4, 1100, null, 86400
union all select	'Standard', 1, 12, 4, 1200, null, 86400
