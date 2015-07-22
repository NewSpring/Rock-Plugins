/* ====================================================== 
-- NewSpring Script #6: 
-- Imports Requirements from F1.
  
--  Assumptions:
--  We only import the following requirement names:

	Background Check
	Band Audition
	Band Next Steps Conversation
	Campus Safety Field Training
	Campus Safety Next Steps Conversation
	Care Interview
	Child Protection Policy
	Confidentiality Agreement Signed
	Driver Agreement
	Financial Coaching Confidentiality Agree
	Financial Coaching Interview
	Financial Coaching Training
	Fuse GL Interview
	Fuse GL Training
	Fuse NS Conversation
	Fuse Training
	Group Leader Interview
	Intellectual Property Agreement
	KidSpring Incident Report
	KidSpring NS Conversation
	License
	Next Steps NS Conversation
	Ownership Paperwork
	Video Release Form
	Worship Interview

   ====================================================== */
-- Make sure you're using the right Rock database:

USE Rock

/* ====================================================== */

-- Enable production mode for performance
SET NOCOUNT ON

-- Set the F1 database name
DECLARE @F1 nvarchar(255) = 'F1'

/* ====================================================== */
-- Start value lookups
/* ====================================================== */
declare @IsSystem int = 0, @Order int = 0,  @TextFieldTypeId int = 1, @True int = 1, @False int = 0,
	@PersonEntityTypeId int = 15, @AttributeEntityTypeId int = 49, @BooleanFieldTypeId int = 3,
	@DDLFieldTypeId int = 6, @DateFieldTypeId int = 11, @DocumentFieldTypeId int = 32, @VideoFieldTypeId int = 80,
	@BackgroundCategoryId int, @FuseCategoryId int, @CSCategoryId int, @CreativeCategoryId int, @FCCategoryId int,
	@ProductionCategoryId int, @GSCategoryId int, @CareCategoryId int, @KidSpringCategoryId int


/* ====================================================== */
-- Create requirement types
/* ====================================================== */
if object_id('tempdb..#requirements') is not null
begin
	drop table #requirements
end
create table #requirements (
	ID int IDENTITY(1,1),
	requirementType nvarchar(255),
)

insert #requirements (requirementType)
values 
('Background Check' ),
('Band Audition'),
('Band Next Steps Conversation'),
('Campus Safety Field Training'),
('Campus Safety Next Steps Conversation'),
('Care Interview'),
('Child Protection Policy'),
('Confidentiality Agreement Signed'),
('Driver Agreement'),
('Financial Coaching Confidentiality Agree'),
('Financial Coaching Interview'),
('Financial Coaching Training'),
('Fuse GL Interview'),
('Fuse GL Training'),
('Fuse NS Conversation'),
('Fuse Training'),
('Group Leader Interview'),
('Intellectual Property Agreement'),
('KidSpring Incident Report'),
('KidSpring NS Conversation'),
('License'),
('Next Steps NS Conversation'),
('Ownership Paperwork'),
('Video Release Form'),
('Worship Interview')

/* ====================================================== */
-- Get or create the attribute categories
/* ====================================================== */

-- Background Checks
select @BackgroundCategoryId = [Id] from Category
where EntityTypeId = @AttributeEntityTypeId 
and name = 'Background Check Information'

if @BackgroundCategoryId is null
begin
	insert Category ( IsSystem, EntityTypeId, EntityTypeQualifierColumn, EntityTypeQualifierValue, Name, [Description], [Order], [IconCssClass], [Guid] )
	select @IsSystem, @AttributeEntityTypeId, 'EntityTypeId', @PersonEntityTypeId, 'Background Check Information', 'Information related to safety and security of organization', 
		@Order, 'fa fa-check-square-o', NEWID()

	select @BackgroundCategoryId = SCOPE_IDENTITY()
end

-- Campus Safety
select @CSCategoryId = [Id] from Category
where EntityTypeId = @AttributeEntityTypeId
and name = 'Campus Safety New Serve'

if @CSCategoryId is null
begin
	insert Category ( IsSystem, EntityTypeId, EntityTypeQualifierColumn, EntityTypeQualifierValue, Name, [Description], [Order], [IconCssClass], [Guid] )
	select @IsSystem, @AttributeEntityTypeId, 'EntityTypeId', @PersonEntityTypeId, 'Campus Safety New Serve', 'Information related to Campus Safety New Serve', 
		@Order, 'fa fa-cab', NEWID()

	select @CSCategoryId = SCOPE_IDENTITY()
end

-- Care
select @CareCategoryId = [Id] from Category
where EntityTypeId = @AttributeEntityTypeId
and name = 'Care New Serve'

if @CareCategoryId is null
begin
	insert Category ( IsSystem, EntityTypeId, EntityTypeQualifierColumn, EntityTypeQualifierValue, Name, [Description], [Order], [IconCssClass], [Guid] )
	select @IsSystem, @AttributeEntityTypeId, 'EntityTypeId', @PersonEntityTypeId, 'Care New Serve', 'Information related to Care New Serve', 
		@Order, 'fa fa-heartbeat', NEWID()

	select @CareCategoryId = SCOPE_IDENTITY()
end

-- Financial Coaching
select @FCCategoryId = [Id] from Category
where EntityTypeId = @AttributeEntityTypeId
and name = 'Financial Coaching New Serve'

if @FCCategoryId is null
begin
	insert Category ( IsSystem, EntityTypeId, EntityTypeQualifierColumn, EntityTypeQualifierValue, Name, [Description], [Order], [IconCssClass], [Guid] )
	select @IsSystem, @AttributeEntityTypeId, 'EntityTypeId', @PersonEntityTypeId, 'Financial Coaching New Serve', 'Information related to Financial Coaching New Serve', 
		@Order, 'fa fa-money', NEWID()

	select @FCCategoryId = SCOPE_IDENTITY()
end

-- Fuse
select @FuseCategoryId = [Id] from Category
where EntityTypeId = @AttributeEntityTypeId
and name = 'Fuse New Serve'

if @FuseCategoryId is null
begin
	insert Category ( IsSystem, EntityTypeId, EntityTypeQualifierColumn, EntityTypeQualifierValue, Name, [Description], [Order], [IconCssClass], [Guid] )
	select @IsSystem, @AttributeEntityTypeId, 'EntityTypeId', @PersonEntityTypeId, 'Fuse New Serve', 'Information related to Fuse New Serve', 
		@Order, 'fa fa-bomb', NEWID()

	select @FuseCategoryId = SCOPE_IDENTITY()
end

-- KidSpring
select @KidSpringCategoryId = [Id] from Category
where EntityTypeId = @AttributeEntityTypeId
and name = 'KidSpring New Serve'

if @KidSpringCategoryId is null
begin
	insert Category ( IsSystem, EntityTypeId, EntityTypeQualifierColumn, EntityTypeQualifierValue, Name, [Description], [Order], [IconCssClass], [Guid] )
	select @IsSystem, @AttributeEntityTypeId, 'EntityTypeId', @PersonEntityTypeId, 'KidSpring New Serve', 'Information related to KidSpring New Serve', 
		@Order, 'fa fa-child', NEWID()

	select @KidSpringCategoryId = SCOPE_IDENTITY()
end

-- Production/Worship
select @ProductionCategoryId = [Id] from Category
where EntityTypeId = @AttributeEntityTypeId 
and name = 'Service Production New Serve'

if @ProductionCategoryId is null
begin
	insert Category ( IsSystem, EntityTypeId, EntityTypeQualifierColumn, EntityTypeQualifierValue, Name, [Description], [Order], [IconCssClass], [Guid] )
	select @IsSystem, @AttributeEntityTypeId, 'EntityTypeId', @PersonEntityTypeId, 'Service Production New Serve', 'Information related to Production New Serve', 
		@Order, 'fa fa-microphone', NEWID()

	select @ProductionCategoryId = SCOPE_IDENTITY()
end

/* ====================================================== */
-- Create attribute lookup
/* ====================================================== */
if object_id('tempdb..#attributeAssignment') is not null
begin
	drop table #attributeAssignment
end
create table #attributeAssignment (	
	personid int,
	attributeId int,
	value nvarchar(255),
	filterDate datetime
)

declare @scopeIndex int, @numItems int
select @scopeIndex = min(ID) from #requirements
select @numItems = count(1) + @scopeIndex from #requirements

while @scopeIndex < @numItems
begin
	declare @msg nvarchar(255), @AssignmentType nvarchar(255), @CategoryId int,
		@DocumentAttributeId int, @DateAttributeId int, @AttributeName nvarchar(255)

	select @AssignmentType = requirementType
	from #requirements
	where ID = @scopeIndex

	if @AssignmentType is not null
	begin

		select @msg = 'Starting ' + @AssignmentType
		RAISERROR ( @msg, 0, 0 ) WITH NOWAIT
		
		-- depending on what assignment this is, take different actions
		if @AssignmentType = 'Background Check'
		begin

			declare @CheckedAttributeId int, @ResultAttributeId int
			
			select @AttributeName = 'Background Checked'

			-- Background Checked
			select @CheckedAttributeId = [Id] from Attribute
			where EntityTypeId = @PersonEntityTypeId
			and name = @AttributeName
			
			if @CheckedAttributeId is null
			begin
				insert Attribute ( [IsSystem], [FieldTypeId], [EntityTypeId], [EntityTypeQualifierColumn], [EntityTypeQualifierValue], 
					[Key], [Name], [Description], [DefaultValue], [Order], [IsGridColumn], [IsMultiValue], [IsRequired], [Guid] )
				select @IsSystem, @BooleanFieldTypeId, @PersonEntityTypeId, '', '', REPLACE(@AttributeName, ' ', ''), @AttributeName, 
					'Does person have a valid background check on record', '', @Order, @False, @False, @False, NEWID()

				select @CheckedAttributeId = SCOPE_IDENTITY()

				insert AttributeCategory (AttributeId, CategoryId)
				values (@CheckedAttributeId, @BackgroundCategoryId)
			end

			select @AttributeName = 'Background Check Date'

			-- Background Check Date
			select @DateAttributeId = [Id] from Attribute
			where EntityTypeId = @PersonEntityTypeId
			and name = @AttributeName
			
			if @DateAttributeId is null
			begin
				insert Attribute ( [IsSystem], [FieldTypeId], [EntityTypeId], [EntityTypeQualifierColumn], [EntityTypeQualifierValue], 
					[Key], [Name], [Description], [DefaultValue], [Order], [IsGridColumn], [IsMultiValue], [IsRequired], [Guid] )
				select @IsSystem, @DateFieldTypeId, @PersonEntityTypeId, '', '', REPLACE(@AttributeName, ' ', ''), @AttributeName, 
					'Date person last passed/failed a background check', '', @Order, @False, @False, @False, NEWID()

				select @DateAttributeId = SCOPE_IDENTITY()

				-- set additional attribute fields
				insert AttributeQualifier (IsSystem, AttributeId, [Key], Value, [Guid])
				select @IsSystem, @DateAttributeId, 'displayDiff', 'False', NEWID()

				insert AttributeQualifier (IsSystem, AttributeId, [Key], Value, [Guid])
				select @IsSystem, @DateAttributeId, 'format', '', NEWID()

				insert AttributeCategory (AttributeId, CategoryId)
				values (@DateAttributeId, @BackgroundCategoryId)
			end
			
			select @AttributeName = 'Background Check Result'

			-- Background Check Result
			select @ResultAttributeId = [Id] from Attribute
			where EntityTypeId = @PersonEntityTypeId
			and name = @AttributeName
			
			if @ResultAttributeId is null
			begin
				insert Attribute ( [IsSystem], [FieldTypeId], [EntityTypeId], [EntityTypeQualifierColumn], [EntityTypeQualifierValue], 
					[Key], [Name], [Description], [DefaultValue], [Order], [IsGridColumn], [IsMultiValue], [IsRequired], [Guid] )
				select @IsSystem, @DDLFieldTypeId, @PersonEntityTypeId, '', '', REPLACE(@AttributeName, ' ', ''), @AttributeName, 
					'Result of last background check', '', @Order, @False, @False, @False, NEWID()

				select @ResultAttributeId = SCOPE_IDENTITY()

				-- set additional attribute fields
				insert AttributeQualifier (IsSystem, AttributeId, [Key], Value, [Guid])
				select @IsSystem, @ResultAttributeId, 'fieldtype', 'ddl', NEWID()

				insert AttributeQualifier (IsSystem, AttributeId, [Key], Value, [Guid])
				select @IsSystem, @ResultAttributeId, 'values', 'Pass,Fail', NEWID()

				insert AttributeCategory (AttributeId, CategoryId)
				values (@ResultAttributeId, @BackgroundCategoryId)
			end

			select @AttributeName = 'Background Check Document'

			-- Background Check Document
			select @DocumentAttributeId = [Id] from Attribute
			where EntityTypeId = @PersonEntityTypeId
			and name = @AttributeName
			
			if @DocumentAttributeId is null
			begin
				insert Attribute ( [IsSystem], [FieldTypeId], [EntityTypeId], [EntityTypeQualifierColumn], [EntityTypeQualifierValue], 
					[Key], [Name], [Description], [DefaultValue], [Order], [IsGridColumn], [IsMultiValue], [IsRequired], [Guid] )
				select @IsSystem, @DocumentFieldTypeId, @PersonEntityTypeId, '', '', REPLACE(@AttributeName, ' ', ''), @AttributeName, 
					'The last background check', '', @Order, @False, @False, @False, NEWID()

				select @DocumentAttributeId = SCOPE_IDENTITY()

				-- set additional attribute fields
				insert AttributeQualifier (IsSystem, AttributeId, [Key], Value, [Guid])
				select @IsSystem, @DocumentAttributeId, 'binaryFileType', '5c701472-8a6b-4bbe-aec6-ec833c859f2d', NEWID()

				insert AttributeCategory (AttributeId, CategoryId)
				values (@DocumentAttributeId, @BackgroundCategoryId)
			end
			
			-- Start inserting attribute assignments
			insert #attributeAssignment
			select pa.PersonId, @CheckedAttributeId, 'True', r.Requirement_Date
			from F1..Requirement r
			inner join PersonAlias pa
				on r.Individual_ID = pa.ForeignId
			where r.Requirement_Name = @AssignmentType
				and r.Requirement_Status_Name like '%Approved%'
				and datediff(year, r.requirement_date, getdate()) < 3

			insert #attributeAssignment
			select pa.PersonId, @DateAttributeId, r.Requirement_Date, r.Requirement_Date
			from F1..Requirement r
			inner join PersonAlias pa
				on r.Individual_ID = pa.ForeignId
			where r.Requirement_Name = @AssignmentType
				and r.Requirement_Status_Name like '%Approved%'
				and datediff(year, r.requirement_date, getdate()) < 3

			insert #attributeAssignment
			select pa.PersonId, @ResultAttributeId, 
				CASE r.Requirement_Status_Name 
					WHEN 'Approved' THEN 'Pass'
					WHEN 'Not Approved' THEN 'Fail'
					ELSE '' 
				END as value, r.Requirement_Date
			from F1..Requirement r
			inner join PersonAlias pa
				on r.Individual_ID = pa.ForeignId
			where r.Requirement_Name = @AssignmentType
				and r.Requirement_Status_Name like '%Approved%'
				and datediff(year, r.requirement_date, getdate()) < 3

			insert #attributeAssignment
			select pa.PersonId, @DocumentAttributeId, '', r.Requirement_Date
			from F1..Requirement r
			inner join PersonAlias pa
				on r.Individual_ID = pa.ForeignId
			where r.Requirement_Name = @AssignmentType
				and r.Requirement_Status_Name like '%Approved%'
				and datediff(year, r.requirement_date, getdate()) < 3			
		end
		-- Band Audition
		else if @AssignmentType = 'Band Audition'
		begin
			
			select @AttributeName = 'Band Audition'

			select @DocumentAttributeId = [Id] from Attribute
			where EntityTypeId = @PersonEntityTypeId
			and name = @AttributeName
			
			if @DocumentAttributeId is null
			begin
				insert Attribute ( [IsSystem], [FieldTypeId], [EntityTypeId], [EntityTypeQualifierColumn], [EntityTypeQualifierValue], 
					[Key], [Name], [Description], [DefaultValue], [Order], [IsGridColumn], [IsMultiValue], [IsRequired], [Guid] )
				select @IsSystem, @VideoFieldTypeId, @PersonEntityTypeId, '', '', REPLACE(@AttributeName, ' ', ''), @AttributeName, 
					'Requirement for Worship volunteers/contractors', '', @Order, @False, @False, @False, NEWID()

				select @DocumentAttributeId = SCOPE_IDENTITY()

				insert AttributeCategory (AttributeId, CategoryId)
				values (@DocumentAttributeId, @ProductionCategoryId)
			end

			insert #attributeAssignment
			select pa.PersonId, @DocumentAttributeId, '', r.Requirement_Date
			from F1..Requirement r
			inner join PersonAlias pa
				on r.Individual_ID = pa.ForeignId
			where r.Requirement_Name = @AssignmentType
		end
		-- Band Next Steps Conversation
		else if @AssignmentType = 'Band Next Steps Conversation'
		begin
			
			select @AttributeName = 'SP Next Steps Convo'

			select @DocumentAttributeId = [Id] from Attribute
			where EntityTypeId = @PersonEntityTypeId
			and name = @AttributeName

			if @DocumentAttributeId is null
			begin
				insert Attribute ( [IsSystem], [FieldTypeId], [EntityTypeId], [EntityTypeQualifierColumn], [EntityTypeQualifierValue], 
					[Key], [Name], [Description], [DefaultValue], [Order], [IsGridColumn], [IsMultiValue], [IsRequired], [Guid] )
				select @IsSystem, @VideoFieldTypeId, @PersonEntityTypeId, '', '', REPLACE(@AttributeName, ' ', ''), @AttributeName, 
					'Conversation for Worship Next Steps', '', @Order, @False, @False, @False, NEWID()

				select @DocumentAttributeId = SCOPE_IDENTITY()

				insert AttributeCategory (AttributeId, CategoryId)
				values (@DocumentAttributeId, @ProductionCategoryId)
			end

			insert #attributeAssignment
			select pa.PersonId, @DocumentAttributeId, '', r.Requirement_Date
			from F1..Requirement r
			inner join PersonAlias pa
				on r.Individual_ID = pa.ForeignId
			where r.Requirement_Name = @AssignmentType
		end
		-- Campus Safety Field Training
		else if @AssignmentType = 'Campus Safety Field Training'
		begin

			select @AttributeName = 'Field Training'

			select @DocumentAttributeId = [Id] from Attribute
			where EntityTypeId = @PersonEntityTypeId
			and name = @AttributeName 
			
			if @DocumentAttributeId is null
			begin
				insert Attribute ( [IsSystem], [FieldTypeId], [EntityTypeId], [EntityTypeQualifierColumn], [EntityTypeQualifierValue], 
					[Key], [Name], [Description], [DefaultValue], [Order], [IsGridColumn], [IsMultiValue], [IsRequired], [Guid] )
				select @IsSystem, @DocumentFieldTypeId, @PersonEntityTypeId, '', '', REPLACE(@AttributeName, ' ', ''), @AttributeName, 
					'Requirement for Campus Safety volunteers', '', @Order, @False, @False, @False, NEWID()

				select @DocumentAttributeId = SCOPE_IDENTITY()

				-- set additional attribute fields
				insert AttributeQualifier (IsSystem, AttributeId, [Key], Value, [Guid])
				select @IsSystem, @DocumentAttributeId, 'binaryFileType', '', NEWID()

				insert AttributeCategory (AttributeId, CategoryId)
				values (@DocumentAttributeId, @CSCategoryId)
			end

			insert #attributeAssignment
			select pa.PersonId, @DocumentAttributeId, '', r.Requirement_Date
			from F1..Requirement r
			inner join PersonAlias pa
				on r.Individual_ID = pa.ForeignId
			where r.Requirement_Name = @AssignmentType
		end
		-- Campus Safety Next Steps Conversation
		else if @AssignmentType = 'Campus Safety Next Steps Conversation'
		begin
			
			select @AttributeName = 'CS Next Steps Convo'

			select @DocumentAttributeId = [Id] from Attribute
			where EntityTypeId = @PersonEntityTypeId
			and name = @AttributeName 
			
			if @DocumentAttributeId is null
			begin
				insert Attribute ( [IsSystem], [FieldTypeId], [EntityTypeId], [EntityTypeQualifierColumn], [EntityTypeQualifierValue], 
					[Key], [Name], [Description], [DefaultValue], [Order], [IsGridColumn], [IsMultiValue], [IsRequired], [Guid] )
				select @IsSystem, @DocumentFieldTypeId, @PersonEntityTypeId, '', '', REPLACE(@AttributeName, ' ', ''), @AttributeName, 
					'Requirement for Campus Safety volunteers', '', @Order, @False, @False, @False, NEWID()

				select @DocumentAttributeId = SCOPE_IDENTITY()

				-- set additional attribute fields
				insert AttributeQualifier (IsSystem, AttributeId, [Key], Value, [Guid])
				select @IsSystem, @DocumentAttributeId, 'binaryFileType', '', NEWID()

				insert AttributeCategory (AttributeId, CategoryId)
				values (@DocumentAttributeId, @CSCategoryId)
			end

			insert #attributeAssignment
			select pa.PersonId, @DocumentAttributeId, '', r.Requirement_Date
			from F1..Requirement r
			inner join PersonAlias pa
				on r.Individual_ID = pa.ForeignId
			where r.Requirement_Name = @AssignmentType
		end
		-- Care Interview
		else if @AssignmentType = 'Care Interview'
		begin
	
			select @AttributeName = 'Care Next Steps Convo'

			select @DocumentAttributeId = [Id] from Attribute
			where EntityTypeId = @PersonEntityTypeId
			and name = @AttributeName  
			
			if @DocumentAttributeId is null
			begin
				insert Attribute ( [IsSystem], [FieldTypeId], [EntityTypeId], [EntityTypeQualifierColumn], [EntityTypeQualifierValue], 
					[Key], [Name], [Description], [DefaultValue], [Order], [IsGridColumn], [IsMultiValue], [IsRequired], [Guid] )
				select @IsSystem, @DocumentFieldTypeId, @PersonEntityTypeId, '', '', REPLACE(@AttributeName, ' ', ''), @AttributeName, 
					'Requirement for Care volunteers', '', @Order, @False, @False, @False, NEWID()

				select @DocumentAttributeId = SCOPE_IDENTITY()

				-- set additional attribute fields
				insert AttributeQualifier (IsSystem, AttributeId, [Key], Value, [Guid])
				select @IsSystem, @DocumentAttributeId, 'binaryFileType', '', NEWID()

				insert AttributeCategory (AttributeId, CategoryId)
				values (@DocumentAttributeId, @CareCategoryId)
			end

			insert #attributeAssignment
			select pa.PersonId, @DocumentAttributeId, '', r.Requirement_Date
			from F1..Requirement r
			inner join PersonAlias pa
				on r.Individual_ID = pa.ForeignId
			where r.Requirement_Name = @AssignmentType
		end
		-- Child Protection Policy
		else if @AssignmentType = 'Child Protection Policy'
		begin
			
			select @AttributeName = 'Child Protection Policy'

			select @DocumentAttributeId = [Id] from Attribute
			where EntityTypeId = @PersonEntityTypeId
			and name = @AttributeName  
			
			if @DocumentAttributeId is null
			begin
				insert Attribute ( [IsSystem], [FieldTypeId], [EntityTypeId], [EntityTypeQualifierColumn], [EntityTypeQualifierValue], 
					[Key], [Name], [Description], [DefaultValue], [Order], [IsGridColumn], [IsMultiValue], [IsRequired], [Guid] )
				select @IsSystem, @DocumentFieldTypeId, @PersonEntityTypeId, '', '', REPLACE(@AttributeName, ' ', ''), @AttributeName,  
					'Requirement for KidSpring volunteers', '', @Order, @False, @False, @False, NEWID()

				select @DocumentAttributeId = SCOPE_IDENTITY()

				-- set additional attribute fields
				insert AttributeQualifier (IsSystem, AttributeId, [Key], Value, [Guid])
				select @IsSystem, @DocumentAttributeId, 'binaryFileType', '', NEWID()

				insert AttributeCategory (AttributeId, CategoryId)
				values (@DocumentAttributeId, @KidSpringCategoryId)
			end

			insert #attributeAssignment
			select pa.PersonId, @DocumentAttributeId, '', r.Requirement_Date
			from F1..Requirement r
			inner join PersonAlias pa
				on r.Individual_ID = pa.ForeignId
			where r.Requirement_Name = @AssignmentType
		end
		-- Confidentiality Agreement Signed
		else if @AssignmentType = 'Confidentiality Agreement Signed'
		begin
	
			select @AttributeName = 'Care Confidentiality Agreement'

			select @DocumentAttributeId = [Id] from Attribute
			where EntityTypeId = @PersonEntityTypeId
			and name = @AttributeName
			
			if @DocumentAttributeId is null
			begin
				insert Attribute ( [IsSystem], [FieldTypeId], [EntityTypeId], [EntityTypeQualifierColumn], [EntityTypeQualifierValue], 
					[Key], [Name], [Description], [DefaultValue], [Order], [IsGridColumn], [IsMultiValue], [IsRequired], [Guid] )
				select @IsSystem, @DocumentFieldTypeId, @PersonEntityTypeId, '', '', REPLACE(@AttributeName, ' ', ''), @AttributeName,  
					'Requirement for Care volunteers', '', @Order, @False, @False, @False, NEWID()

				select @DocumentAttributeId = SCOPE_IDENTITY()

				-- set additional attribute fields
				insert AttributeQualifier (IsSystem, AttributeId, [Key], Value, [Guid])
				select @IsSystem, @DocumentAttributeId, 'binaryFileType', '', NEWID()

				insert AttributeCategory (AttributeId, CategoryId)
				values (@DocumentAttributeId, @CareCategoryId)
			end

			insert #attributeAssignment
			select pa.PersonId, @DocumentAttributeId, '', r.Requirement_Date
			from F1..Requirement r
			inner join PersonAlias pa
				on r.Individual_ID = pa.ForeignId
			where r.Requirement_Name = @AssignmentType
		end
		-- Driver Agreement
		else if @AssignmentType = 'Driver Agreement'
		begin
			
			select @AttributeName = 'Driver Agreement'

			select @DocumentAttributeId = [Id] from Attribute
			where EntityTypeId = @PersonEntityTypeId
			and name = @AttributeName
			
			if @DocumentAttributeId is null
			begin
				insert Attribute ( [IsSystem], [FieldTypeId], [EntityTypeId], [EntityTypeQualifierColumn], [EntityTypeQualifierValue], 
					[Key], [Name], [Description], [DefaultValue], [Order], [IsGridColumn], [IsMultiValue], [IsRequired], [Guid] )
				select @IsSystem, @DocumentFieldTypeId, @PersonEntityTypeId, '', '', REPLACE(@AttributeName, ' ', ''), @AttributeName, 
					'Occasional requirement for multiple ministries. One agreement applies to all ministries.', '', @Order, @False, @False, @False, NEWID()

				select @DocumentAttributeId = SCOPE_IDENTITY()

				-- set additional attribute fields
				insert AttributeQualifier (IsSystem, AttributeId, [Key], Value, [Guid])
				select @IsSystem, @DocumentAttributeId, 'binaryFileType', '', NEWID()

				insert AttributeCategory (AttributeId, CategoryId)
				values 
					(@DocumentAttributeId, @CreativeCategoryId),
					(@DocumentAttributeId, @CSCategoryId),
					(@DocumentAttributeId, @FuseCategoryId),
					(@DocumentAttributeId, @GSCategoryId),
					(@DocumentAttributeId, @ProductionCategoryId)
			end

			insert #attributeAssignment
			select pa.PersonId, @DocumentAttributeId, '', r.Requirement_Date
			from F1..Requirement r
			inner join PersonAlias pa
				on r.Individual_ID = pa.ForeignId
			where r.Requirement_Name = @AssignmentType
		end
		-- Financial Coaching Confidentiality Agree
		else if @AssignmentType = 'Financial Coaching Confidentiality Agree'
		begin
			
			select @AttributeName = 'FC Confidentiality Agreement'

			select @DocumentAttributeId = [Id] from Attribute
			where EntityTypeId = @PersonEntityTypeId
			and name = @AttributeName
			
			if @DocumentAttributeId is null
			begin
				insert Attribute ( [IsSystem], [FieldTypeId], [EntityTypeId], [EntityTypeQualifierColumn], [EntityTypeQualifierValue], 
					[Key], [Name], [Description], [DefaultValue], [Order], [IsGridColumn], [IsMultiValue], [IsRequired], [Guid] )
				select @IsSystem, @DocumentFieldTypeId, @PersonEntityTypeId, '', '', REPLACE(@AttributeName, ' ', ''), @AttributeName, 
					'Requirement for Financial Coaches.', '', @Order, @False, @False, @False, NEWID()

				select @DocumentAttributeId = SCOPE_IDENTITY()

				-- set additional attribute fields
				insert AttributeQualifier (IsSystem, AttributeId, [Key], Value, [Guid])
				select @IsSystem, @DocumentAttributeId, 'binaryFileType', '', NEWID()

				insert AttributeCategory (AttributeId, CategoryId)
				values (@DocumentAttributeId, @FCCategoryId)
			end

			insert #attributeAssignment
			select pa.PersonId, @DocumentAttributeId, '', r.Requirement_Date
			from F1..Requirement r
			inner join PersonAlias pa
				on r.Individual_ID = pa.ForeignId
			where r.Requirement_Name = @AssignmentType
		end
		-- Financial Coaching Interview
		else if @AssignmentType = 'Financial Coaching Interview'
		begin
			
			select @AttributeName = 'FC Next Steps Convo'

			select @DocumentAttributeId = [Id] from Attribute
			where EntityTypeId = @PersonEntityTypeId
			and name = @AttributeName
			
			if @DocumentAttributeId is null
			begin
				insert Attribute ( [IsSystem], [FieldTypeId], [EntityTypeId], [EntityTypeQualifierColumn], [EntityTypeQualifierValue], 
					[Key], [Name], [Description], [DefaultValue], [Order], [IsGridColumn], [IsMultiValue], [IsRequired], [Guid] )
				select @IsSystem, @DocumentFieldTypeId, @PersonEntityTypeId, '', '', REPLACE(@AttributeName, ' ', ''), @AttributeName, 
					'Requirement for Financial Coaches.', '', @Order, @False, @False, @False, NEWID()

				select @DocumentAttributeId = SCOPE_IDENTITY()

				-- set additional attribute fields
				insert AttributeQualifier (IsSystem, AttributeId, [Key], Value, [Guid])
				select @IsSystem, @DocumentAttributeId, 'binaryFileType', '', NEWID()

				insert AttributeCategory (AttributeId, CategoryId)
				values (@DocumentAttributeId, @FCCategoryId)
			end

			insert #attributeAssignment
			select pa.PersonId, @DocumentAttributeId, '', r.Requirement_Date
			from F1..Requirement r
			inner join PersonAlias pa
				on r.Individual_ID = pa.ForeignId
			where r.Requirement_Name = @AssignmentType
		end
		-- Fuse GL Interview
		else if @AssignmentType = 'Fuse GL Interview'
		begin
			
			select @AttributeName = 'Group Leader Interview'

			select @DocumentAttributeId = [Id] from Attribute
			where EntityTypeId = @PersonEntityTypeId
			and name = @AttributeName
			
			if @DocumentAttributeId is null
			begin
				insert Attribute ( [IsSystem], [FieldTypeId], [EntityTypeId], [EntityTypeQualifierColumn], [EntityTypeQualifierValue], 
					[Key], [Name], [Description], [DefaultValue], [Order], [IsGridColumn], [IsMultiValue], [IsRequired], [Guid] )
				select @IsSystem, @DocumentFieldTypeId, @PersonEntityTypeId, '', '', REPLACE(@AttributeName, ' ', ''), @AttributeName, 
					'Occasional requirement for Fuse volunteers.', '', @Order, @False, @False, @False, NEWID()

				select @DocumentAttributeId = SCOPE_IDENTITY()

				-- set additional attribute fields
				insert AttributeQualifier (IsSystem, AttributeId, [Key], Value, [Guid])
				select @IsSystem, @DocumentAttributeId, 'binaryFileType', '', NEWID()

				insert AttributeCategory (AttributeId, CategoryId)
				values (@DocumentAttributeId, @FuseCategoryId)
			end

			insert #attributeAssignment
			select pa.PersonId, @DocumentAttributeId, '', r.Requirement_Date
			from F1..Requirement r
			inner join PersonAlias pa
				on r.Individual_ID = pa.ForeignId
			where r.Requirement_Name = @AssignmentType
		end
		-- Fuse GL Training
		else if @AssignmentType = 'Fuse GL Training'
		begin
			
			select @AttributeName = 'Group Leader Interview'

			select @DocumentAttributeId = [Id] from Attribute
			where EntityTypeId = @PersonEntityTypeId
			and name = @AttributeName
			
			if @DocumentAttributeId is null
			begin
				insert Attribute ( [IsSystem], [FieldTypeId], [EntityTypeId], [EntityTypeQualifierColumn], [EntityTypeQualifierValue], 
					[Key], [Name], [Description], [DefaultValue], [Order], [IsGridColumn], [IsMultiValue], [IsRequired], [Guid] )
				select @IsSystem, @DocumentFieldTypeId, @PersonEntityTypeId, '', '', REPLACE(@AttributeName, ' ', ''), @AttributeName, 
					'Occasional requirement for Fuse volunteers.', '', @Order, @False, @False, @False, NEWID()

				select @DocumentAttributeId = SCOPE_IDENTITY()

				-- set additional attribute fields
				insert AttributeQualifier (IsSystem, AttributeId, [Key], Value, [Guid])
				select @IsSystem, @DocumentAttributeId, 'binaryFileType', '', NEWID()

				insert AttributeCategory (AttributeId, CategoryId)
				values (@DocumentAttributeId, @FuseCategoryId)
			end

			insert #attributeAssignment
			select pa.PersonId, @DocumentAttributeId, '', r.Requirement_Date
			from F1..Requirement r
			inner join PersonAlias pa
				on r.Individual_ID = pa.ForeignId
			where r.Requirement_Name = @AssignmentType
		end
		-- Fuse NS Conversation
		else if @AssignmentType = 'Fuse NS Conversation'
		begin
			
			select @AttributeName = 'Fuse Next Steps Convo'

			select @DocumentAttributeId = [Id] from Attribute
			where EntityTypeId = @PersonEntityTypeId
			and name = @AttributeName
			
			if @DocumentAttributeId is null
			begin
				insert Attribute ( [IsSystem], [FieldTypeId], [EntityTypeId], [EntityTypeQualifierColumn], [EntityTypeQualifierValue], 
					[Key], [Name], [Description], [DefaultValue], [Order], [IsGridColumn], [IsMultiValue], [IsRequired], [Guid] )
				select @IsSystem, @DocumentFieldTypeId, @PersonEntityTypeId, '', '', REPLACE(@AttributeName, ' ', ''), @AttributeName, 
					'Requirement for Fuse volunteers.', '', @Order, @False, @False, @False, NEWID()

				select @DocumentAttributeId = SCOPE_IDENTITY()

				-- set additional attribute fields
				insert AttributeQualifier (IsSystem, AttributeId, [Key], Value, [Guid])
				select @IsSystem, @DocumentAttributeId, 'binaryFileType', '', NEWID()

				insert AttributeCategory (AttributeId, CategoryId)
				values (@DocumentAttributeId, @FuseCategoryId)
			end

			insert #attributeAssignment
			select pa.PersonId, @DocumentAttributeId, '', r.Requirement_Date
			from F1..Requirement r
			inner join PersonAlias pa
				on r.Individual_ID = pa.ForeignId
			where r.Requirement_Name = @AssignmentType
		end

	
	-- reset variables
	select @AssignmentType = null, @CategoryId = null, @DocumentAttributeId = null, @DateAttributeId = null, @AttributeName = null

	select @scopeIndex = @scopeIndex + 1
end
-- end while requirements

-- remove duplicate attributes and values
;WITH duplicates (personId, attributeId, id) 
AS (
    SELECT personId, attributeId, ROW_NUMBER() OVER (
		PARTITION BY personId, attributeId
		ORDER BY filterDate desc
    ) AS id
    FROM #attributeAssignment
)
delete from duplicates
where id > 1

-- remove the existing value for this person/attribute
delete av
from AttributeValue av
inner join #attributeAssignment a
on a.personId = av.EntityId
and a.attributeId = av.AttributeId

-- insert attribute values
insert AttributeValue ( [IsSystem], [AttributeId], [EntityId], [Value], [CreatedDateTime], [ModifiedDateTime], [Guid] )
select @False, attributeId, personId, value, filterDate, NULL, NEWID()
from #attributeAssignment


-- completed successfully
RAISERROR ( N'Completed successfully.', 0, 0 ) WITH NOWAIT

use master



