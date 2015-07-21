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
	@PersonEntityTypeId int = 15, @AttributeEntityTypeId int = 49, @DocumentFieldTypeId int = 32,
	@DateFieldTypeId int = 11, @DDLFieldTypeId int = 6

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
-- Create attribute lookup
/* ====================================================== */
if object_id('tempdb..#attributeAssignment') is not null
begin
	drop table #attributeAssignment
end
create table #attributeAssignment (
	attributeId int,
	personid int,
	value nvarchar(255)
)

declare @scopeIndex int, @numItems int
select @scopeIndex = min(ID) from #requirements
select @numItems = count(1) + @scopeIndex from #requirements

while @scopeIndex < @numItems
begin
	declare @msg nvarchar(255), @AssignmentType nvarchar(255), @MainAttributeId int, @SecondaryAttributeId int,
		@TertiaryAttributeId int, @AttributeCategoryId int, @CategoryId int

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
			
			-- get or create the attribute category
			select @AttributeCategoryId = [Id] from Category
			where EntityTypeId = @AttributeEntityTypeId 
			and name = 'Background Check Information'

			if @AttributeCategoryId is null
			begin
				insert Category ( IsSystem, EntityTypeId, EntityTypeQualifierColumn, EntityTypeQualifierValue, Name, [Description], [Order], [IconCssClass], [Guid] )
				select @IsSystem, @AttributeEntityTypeId, 'EntityTypeId', @PersonEntityTypeId, 'Background Check Information', 'Information related to safety and security of organization', 
					@Order, 'fa fa-check-square-o', NEWID()

				select @AttributeCategoryId = SCOPE_IDENTITY()
			end

			select @MainAttributeId = [Id] from Attribute
			where EntityTypeId = @PersonEntityTypeId
			and name = 'Background Check Date'
			
			if @MainAttributeId is null
			begin
				insert Attribute ( [IsSystem], [FieldTypeId], [EntityTypeId], [EntityTypeQualifierColumn], [EntityTypeQualifierValue], 
					[Key], [Name], [Description], [DefaultValue], [Order], [IsGridColumn], [IsMultiValue], [IsRequired], [Guid] )
				select @IsSystem, @DateFieldTypeId, @PersonEntityTypeId, '', '', 'BackgroundCheckDate', 'Background Check Date', 
					'Date person last passed/failed a background check', '', @Order, @False, @False, @False, NEWID()

				select @MainAttributeId = SCOPE_IDENTITY()

				-- set additional attribute fields
				insert AttributeQualifier (IsSystem, AttributeId, [Key], Value, [Guid])
				select @IsSystem, @MainAttributeId, 'displayDiff', 'False', NEWID()

				insert AttributeQualifier (IsSystem, AttributeId, [Key], Value, [Guid])
				select @IsSystem, @MainAttributeId, 'format', '', NEWID()

				insert AttributeCategory (AttributeId, CategoryId)
				select @MainAttributeId, @AttributeCategoryId
			end

			select @SecondaryAttributeId = [Id] from Attribute
			where EntityTypeId = @PersonEntityTypeId
			and name = 'Background Check Result'
			
			if @SecondaryAttributeId is null
			begin
				insert Attribute ( [IsSystem], [FieldTypeId], [EntityTypeId], [EntityTypeQualifierColumn], [EntityTypeQualifierValue], 
					[Key], [Name], [Description], [DefaultValue], [Order], [IsGridColumn], [IsMultiValue], [IsRequired], [Guid] )
				select @IsSystem, @DDLFieldTypeId, @PersonEntityTypeId, '', '', 'Background Check Result', 'Background Check Result', 
					'Result of last background check', '', @Order, @False, @False, @False, NEWID()

				select @SecondaryAttributeId = SCOPE_IDENTITY()

				-- set additional attribute fields
				insert AttributeQualifier (IsSystem, AttributeId, [Key], Value, [Guid])
				select @IsSystem, @SecondaryAttributeId, 'fieldtype', 'ddl', NEWID()

				insert AttributeQualifier (IsSystem, AttributeId, [Key], Value, [Guid])
				select @IsSystem, @SecondaryAttributeId, 'values', 'Pass,Fail', NEWID()

				insert AttributeCategory (AttributeId, CategoryId)
				select @SecondaryAttributeId, @SecondaryAttributeId
			end


			select @TertiaryAttributeId = [Id] from Attribute
			where EntityTypeId = @PersonEntityTypeId
			and name = 'Background Check Document'
			
			if @TertiaryAttributeId is null
			begin
				insert Attribute ( [IsSystem], [FieldTypeId], [EntityTypeId], [EntityTypeQualifierColumn], [EntityTypeQualifierValue], 
					[Key], [Name], [Description], [DefaultValue], [Order], [IsGridColumn], [IsMultiValue], [IsRequired], [Guid] )
				select @IsSystem, @DocumentFieldTypeId, @PersonEntityTypeId, '', '', 'BackgroundCheckDocument', 'Background Check Document', 
					'The last background check', '', @Order, @False, @False, @False, NEWID()

				select @TertiaryAttributeId = SCOPE_IDENTITY()

				-- set additional attribute fields
				insert AttributeQualifier (IsSystem, AttributeId, [Key], Value, [Guid])
				select @IsSystem, @TertiaryAttributeId, 'binaryFileType', '5c701472-8a6b-4bbe-aec6-ec833c859f2d', NEWID()

				insert AttributeCategory (AttributeId, CategoryId)
				select @MainAttributeId, @TertiaryAttributeId
			end

			insert #attributeAssignment
			select @MainAttributeId, pa.PersonId, r.Requirement_Date
			from F1..Requirement r
			inner join PersonAlias pa
				on r.Individual_ID = pa.ForeignId
			where r.Requirement_Name = @AssignmentType
			and datediff(year, r.requirement_date, getdate()) < 3


		end
	
	select @scopeIndex = @scopeIndex + 1
end
-- end while requirements


-- completed successfully
RAISERROR ( N'Completed successfully.', 0, 0 ) WITH NOWAIT

use master
