/* ======================================================
   NewSpring Script #2: 
   Inserts assignments, jobs, and group schedules.
  
   Assumptions:
   We only import active assignments 
   We only import assignmnents with valid RLC's
   We only import schedules that match current service times

   ====================================================== */

-- Make sure you're using the right Rock database:

USE [Rock]

/* ====================================================== */

declare @IsSystem int = 0, @Order int = 0,  @TextFieldTypeId int = 1
declare @ScheduleDefinedTypeId int, @GroupCategoryId int
select @ScheduleDefinedTypeId = Id from DefinedType where [Guid] = '26ECDD90-A2FA-4732-B3D1-32AC93953EFA'
select @GroupCategoryId = Id from Category where name = 'Group'

-- create schedule defined type
if ( @ScheduleDefinedTypeId is null )
begin
	insert DefinedType ( [IsSystem], [FieldTypeId], [Order], [Name], [Description], [Guid], CategoryId )
	select @IsSystem, @TextFieldTypeId, @Order, 'Schedules', 'The schedules that can be assigned to a group member.',
		'26ECDD90-A2FA-4732-B3D1-32AC93953EFA', @GroupCategoryId

	select @ScheduleDefinedTypeId = SCOPE_IDENTITY()
end 

/* ====================================================== */
-- create schedule lookup
/* ====================================================== */
if object_id('tempdb..#schedules') is not null
begin
	drop table #schedules
end
create table #schedules (
	ID int IDENTITY(1,1),
	scheduleF1 varchar(255),
	scheduleRock varchar(255),
	definedValueId int DEFAULT NULL
)

insert into #schedules (scheduleF1, scheduleRock)
select distinct Staffing_Schedule_Name, case 
	when Staffing_Schedule_Name like '8:30%' 
		then null
	when Staffing_Schedule_Name = '9:15' 
		then '9:15 AM'
	when Staffing_Schedule_Name like '10:00%' 
		then null
	when Staffing_Schedule_Name = '11:15' 
		then '11:15 AM'
	when Staffing_Schedule_Name = '4:15' 
		then '4:15 PM'
	when Staffing_Schedule_Name = '6:00' 
		then '6:00 PM'
	when Staffing_Schedule_Name = 'Base Schedule' 
		then null
	else Staffing_Schedule_Name
	end as 'schedule'
from F1..Staffing_Assignment

insert DefinedValue ([IsSystem], [DefinedTypeId], [Order], [Value], [Guid] )
select @IsSystem, @ScheduleDefinedTypeId, @Order, s.scheduleRock, NEWID()
from #schedules s
where scheduleRock is not null

update s
set definedValueId = dv.Id
from #schedules s
inner join DefinedValue dv
on dv.DefinedTypeId = @ScheduleDefinedTypeId
and dv.Value = s.scheduleRock

select * from #schedules

USE [master]