DECLARE @IsSystem bit = 0
DECLARE @True bit = 1
DECLARE @False bit = 0
DECLARE @Order int = 0
DECLARE @BooleanFieldTypeId int
DECLARE @GroupEntityTypeId int
DECLARE @CampusEntityTypeId int
DECLARE @ScheduleEntityTypeId int
DECLARE @MetricCategoryEntityTypeId int
DECLARE @CheckInAreaPurposeId int
DECLARE @CampusLocationTypeId int
DECLARE @SourceTypeSQLId int
DECLARE @MetricGroupTypeCategoryId int
DECLARE @MetricServiceRolesId int
DECLARE @CampusId int

SELECT TOP 1 @CampusId = [Id] FROM [Campus]
SELECT @BooleanFieldTypeId = [Id] FROM [FieldType] WHERE [Name] = 'Boolean'
SELECT @GroupEntityTypeId = [Id] FROM [EntityType] WHERE [Name] = 'Rock.Model.Group'
SELECT @CampusEntityTypeId = [Id] FROM [EntityType] WHERE [Name] = 'Rock.Model.Campus'
SELECT @ScheduleEntityTypeId = [Id] FROM [EntityType] WHERE [Name] = 'Rock.Model.Schedule'
SELECT @MetricCategoryEntityTypeId = [Id] FROM [EntityType] WHERE [Name] = 'Rock.Model.MetricCategory'

DECLARE @MetricParentCategoryId int, @MetricScheduleCategoryId int, 
	@MetricScheduleId int, @MetricParentName varchar(255), 
	@MetricScheduleCategory varchar(255), @MetriciCalSchedule nvarchar(max)

SELECT @MetricParentName = 'Volunteer Groups', @MetricScheduleCategory = 'Metrics'

-- create parent category
SELECT @MetricParentCategoryId = [Id] FROM [Category]
WHERE EntityTypeId = @MetricCategoryEntityTypeId
AND Name = @MetricParentName

IF @MetricParentCategoryId IS NULL 
BEGIN
	INSERT [Category] (IsSystem, ParentCategoryId, EntityTypeId, EntityTypeQualifierColumn, EntityTypeQualifierValue, Name, [Guid], [Order])
	VALUES ( @IsSystem, NULL, @MetricCategoryEntityTypeId, '', '', @MetricParentName, NEWID(), @Order )

	SET @MetricParentCategoryId = SCOPE_IDENTITY()
END

SELECT @MetricScheduleCategoryId = [Id] FROM [Category]
WHERE EntityTypeId = @ScheduleEntityTypeId
AND Name = @MetricScheduleCategory

-- create schedule category
IF @MetricScheduleCategoryId IS NULL
BEGIN
	INSERT [Category] (IsSystem, ParentCategoryId, EntityTypeId, EntityTypeQualifierColumn, EntityTypeQualifierValue, Name, [Guid], [Order])
	VALUES ( @IsSystem, NULL, @ScheduleEntityTypeId, '', '', @MetricScheduleCategory, NEWID(), @Order )

	SET @MetricScheduleCategoryId = SCOPE_IDENTITY()
END

-- create the metric schedule
SELECT @MetricScheduleId = [Id] FROM Schedule
WHERE CategoryId = @MetricScheduleCategoryId
AND Name = @MetricParentName

IF @MetricScheduleId IS NULL
BEGIN

	SELECT @MetriciCalSchedule = N'BEGIN:VCALENDAR
VERSION:2.0
PRODID:-//ddaysoftware.com//NONSGML DDay.iCal 1.0//EN
BEGIN:VEVENT
DTEND:20150928T020001
DTSTAMP:20150928T201239Z
DTSTART:20150928T020000
RRULE:FREQ=WEEKLY;BYDAY=MO
SEQUENCE:0
UID:4bb6f790-4761-447d-bff8-22c2ca3bef05
END:VEVENT
END:VCALENDAR'
	
	INSERT [Schedule] (Name, [Description], iCalendarContent, EffectiveStartDate, EffectiveEndDate, CategoryId, [Guid])
	SELECT 'Metric Schedule', 'The job schedule to run group metrics', @MetriciCalSchedule, GETDATE(), GETDATE(), @MetricScheduleCategoryId, NEWID()

	SELECT @MetricScheduleId = SCOPE_IDENTITY()

END

SELECT @MetricGroupTypeCategoryId = [Id] FROM Category
WHERE ParentCategoryId = @MetricParentCategoryId
AND Name = 'Guest Services'

IF @MetricGroupTypeCategoryId IS NULL
BEGIN
	INSERT [Category] (IsSystem, ParentCategoryId, EntityTypeId, EntityTypeQualifierColumn, EntityTypeQualifierValue, Name, [Guid], [Order])
	VALUES ( @IsSystem, @MetricParentCategoryId, @MetricCategoryEntityTypeId, '', '', 'Guest Services', NEWID(), @Order )

	SELECT @MetricGroupTypeCategoryId = SCOPE_IDENTITY()
END

SELECT @MetricServiceRolesId = [Id] FROM Metric
WHERE EntityTypeId = @CampusEntityTypeId
	AND SourceValueTypeId = @SourceTypeSQLId
	AND Title = 'Auditorium Reset Team'

-- create if it doesn't exist
IF @MetricServiceRolesId IS NULL
BEGIN
	INSERT [Metric] (IsSystem, Title, [Description], IsCumulative, SourceValueTypeId, SourceSql, XAxisLabel, YAxisLabel, ScheduleId, EntityTypeId, [Guid])
	VALUES ( 0, 'Auditorium Reset Team', 'Metric to track ' + 'Auditorium Reset Team' + ' roles by campus and service', @False, 
		@SourceTypeSQLId, 'This is where the SQL would go', '', '', @MetricScheduleId, @CampusEntityTypeId, NEWID() )

	SELECT @MetricServiceRolesId = SCOPE_IDENTITY()

	INSERT [MetricCategory] (MetricId, CategoryId, [Order], [Guid])
	VALUES ( @MetricServiceRolesId, @MetricGroupTypeCategoryId, @Order, NEWID() )
END

DECLARE @9am varchar(5) = '09:15'
DECLARE @11am varchar(5) = '11:15'
DECLARE @4pm varchar(5) = '16:00'
DECLARE @6pm varchar(5) = '18:00'

DECLARE @SomeSunday datetime
DECLARE @LastSunday datetime = DATEADD( 
	DAY, -((DATEPART(DW, GETDATE()) + 6) % 7), GETDATE()
)

DECLARE @scopeIndex int, @numItems int		
SELECT @scopeIndex = 0
SELECT @numItems = 52

WHILE @scopeIndex <= @numItems
BEGIN

	SELECT @SomeSunday = DATEADD(dd, DATEDIFF(dd, @scopeIndex * 7, @LastSunday), 0)

	INSERT [MetricValue] (MetricId, MetricValueType, YValue, [Order], MetricValueDateTime, EntityId, [Guid])
	VALUES (@MetricServiceRolesId, @False, CAST(ABS(RAND() * 1000) AS INT), @Order, @SomeSunday + @9am, @CampusId, NEWID() )

	INSERT [MetricValue] (MetricId, MetricValueType, YValue, [Order], MetricValueDateTime, EntityId, [Guid])
	VALUES (@MetricServiceRolesId, @False, CAST(ABS(RAND() * 1000) AS INT), @Order, @SomeSunday + @11am, @CampusId, NEWID() )
	
	INSERT [MetricValue] (MetricId, MetricValueType, YValue, [Order], MetricValueDateTime, EntityId, [Guid])
	VALUES (@MetricServiceRolesId, @False, CAST(ABS(RAND() * 1000) AS INT), @Order, @SomeSunday + @4pm, @CampusId, NEWID() )

	INSERT [MetricValue] (MetricId, MetricValueType, YValue, [Order], MetricValueDateTime, EntityId, [Guid])
	VALUES (@MetricServiceRolesId, @False, CAST(ABS(RAND() * 1000) AS INT), @Order, @SomeSunday + @6pm, @CampusId, NEWID() )
	
	SELECT @scopeIndex = @scopeIndex + 1
		
END