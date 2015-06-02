/* ====================================================== 
-- NewSpring Script #4: 
-- Inserts attributes.
  
--  Assumptions:
--  We only import attributes from the following group name:
    90 Day Tithing Challenge
    Discipleship - Grow
    Discipleship - Ownership
    Financial Planning
    Fuse
    KidSpring
    Salvation
    Stewardship
    Volunteer

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
declare @IsSystem int = 0, @Order int = 0,  @TextFieldTypeId int = 1, @True int = 1, @False int = 0 

declare @CampusFieldTypeId int, @DateFieldTypeId int, @PersonEntityTypeId int
select @CampusFieldTypeId = [Id] from FieldType where [Guid] = '1B71FEF4-201F-4D53-8C60-2DF21F1985ED'
select @DateFieldTypeId = [Id] from FieldType where [Guid] = '6B6AA175-4758-453F-8D83-FCD8044B5F36'
select @PersonEntityTypeId = [Id] from EntityType where [Guid] = '72657ED8-D16E-492E-AC12-144C5E7567E7'

select * from rock..FieldType

/* ====================================================== */
-- Create attribute types
/* ====================================================== */
if object_id('tempdb..#attributes') is not null
begin
	drop table #attributes
end
create table #attributes (
	ID int IDENTITY(1,1),
	attributeGroupName nvarchar(255),
	attributeName nvarchar(255),	
	firstAttributeId int DEFAULT NULL,
	secondAttributeId int DEFAULT NULL
)

insert into #attributes (attributeGroupName, attributeName)
select attribute_group_name, attribute_name
from f1..Attribute
where ( attribute_name like 'Ownership%Joined'
	or attribute_name like '%Tithing Challenge'
	or attribute_name like '%-%Baptism'
	or attribute_name like '%Financial Coaching'
	or attribute_name like '%-%Salvation'
	or attribute_name like '%Spring Zone'
	or attribute_name like '%Dinner Attendee'
	or attribute_name like 'Redirect Card%'
	or attribute_name like 'NewServe%'
)
group by attribute_group_name, attribute_name

/* ====================================================== */
-- Create attribute lookup
/* ====================================================== */
if object_id('tempdb..#attributeAssignment') is not null
begin
	drop table #attributeAssignment
end
create table #attributeAssignment (
	personId bigint,
	startDate date, 
	campus uniqueidentifier
)

declare @scopeIndex int, @numItems int
select @scopeIndex = min(ID) from #attributes
select @numItems = count(1) + @scopeIndex from #attributes

while @scopeIndex <= @numItems
begin
	declare @AttributeGroup nvarchar(255), @AttributeName nvarchar(255), @FirstAttributeId int, @SecondAttributeId int, @campusGuid uniqueidentifier 
	
	select @AttributeGroup = attributeGroupName, @AttributeName = attributeName, @FirstAttributeId = firstAttributeId, @SecondAttributeId = secondAttributeId
	from #attributes where ID = @scopeIndex

	if @AttributeGroup is not null
	begin
		--select * from #attributes

		-- set campus based on the attribute name
		select @campusGuid = [Guid] from Campus 
		where shortcode = left(ltrim(@AttributeName), 3)
		or shortcode = right(rtrim(@AttributeName), 3) 

		-- get the currently assigned person and value
		insert into #attributeAssignment
		select pa.PersonId as 'personId', a.Start_Date as 'startDate', @campusGuid
		from F1..Attribute a
		inner join PersonAlias pa
		on a.Individual_Id = pa.ForeignId
		and a.Attribute_Group_Name = @AttributeGroup 
		and a.Attribute_Name = @AttributeName

		-- depending on what attribute this is, take different actions
		if @AttributeGroup = '90 Day Tithing Challenge'
		begin			
			-- create attributes if they don't exist
			if @FirstAttributeId is null
			begin
				-- set an attribute category


				insert Attribute ( [IsSystem], [FieldTypeId], [EntityTypeId], [EntityTypeQualifierColumn], [EntityTypeQualifierValue], 
					[Key], [Name], [Description], [DefaultValue], [Order], [IsGridColumn], [IsMultiValue], [IsRequired], [Guid] )
				select @IsSystem, @CampusFieldTypeId, @PersonEntityTypeId, '', '', '90 DTC Campus', '90 DTC Campus', 
					'The campus where this person signed up for the 90 Day Tithe Challenge.', '', @Order, @False, @False, @False, NEWID()

				select @FirstAttributeId = SCOPE_IDENTITY()

				insert Attribute ( [IsSystem], [FieldTypeId], [EntityTypeId], [EntityTypeQualifierColumn], [EntityTypeQualifierValue], 
					[Key], [Name], [Description], [DefaultValue], [Order], [IsGridColumn], [IsMultiValue], [IsRequired], [Guid] )
				select @IsSystem, @CampusFieldTypeId, @PersonEntityTypeId, '', '', '90 DTC Campus', '90 DTC Campus', 
					'The campus where this person signed up for the 90 Day Tithe Challenge.', '', @Order, @False, @False, @False, NEWID()
			
				select @SecondAttributeId = SCOPE_IDENTITY()			
				
				update #attributes 
				set firstAttributeId = @FirstAttributeId, secondAttributeId = @SecondAttributeId
				where attributeGroupName = @AttributeGroup
				and attributeName = @AttributeName
			end
		end	
		-- end 90 DTC
		else if @AttributeGroup = 'Discipleship - Grow'
		begin
			-- create attributes if they don't exist
			if @FirstAttributeId is null
			begin
				insert Attribute ( [IsSystem], [FieldTypeId], [EntityTypeId], [EntityTypeQualifierColumn], [EntityTypeQualifierValue], 
					[Key], [Name], [Description], [DefaultValue], [Order], [IsGridColumn], [IsMultiValue], [IsRequired], [Guid] )
				select @IsSystem, @CampusFieldTypeId, @PersonEntityTypeId, '', '', '90 DTC Campus', '90 DTC Campus', 
					'The campus where this person signed up for the 90 Day Tithe Challenge.', '', @Order, @False, @False, @False, NEWID()

				select @FirstAttributeId = SCOPE_IDENTITY()

				insert Attribute ( [IsSystem], [FieldTypeId], [EntityTypeId], [EntityTypeQualifierColumn], [EntityTypeQualifierValue], 
					[Key], [Name], [Description], [DefaultValue], [Order], [IsGridColumn], [IsMultiValue], [IsRequired], [Guid] )
				select @IsSystem, @CampusFieldTypeId, @PersonEntityTypeId, '', '', '90 DTC Campus', '90 DTC Campus', 
					'The campus where this person signed up for the 90 Day Tithe Challenge.', '', @Order, @False, @False, @False, NEWID()
			
				select @SecondAttributeId = SCOPE_IDENTITY()			
				
				update #attributes 
				set firstAttributeId = @FirstAttributeId, secondAttributeId = @SecondAttributeId
				where attributeGroupName = @AttributeGroup
				and attributeName = @AttributeName
			end
		end -- end Discipleship
		

		-- create assignments for the first attribute
		if @FirstAttributeId is not null
		begin
			insert AttributeValue ( [IsSystem], [AttributeId], [EntityId], [Value], [Guid] )
			select @IsSystem, @FirstAttributeId, personId, startDate, NEWID()
			from #attributeAssignment
		end

		-- create assignments for the second attribute
		if @SecondAttributeId is not null
		begin
			insert AttributeValue ( [IsSystem], [AttributeId], [EntityId], [Value], [Guid] )
			select @IsSystem, @SecondAttributeId, personId, campus, NEWID()
			from #attributeAssignment
		end

		-- clear the assignments for this attribute
		truncate table #attributeAssignment	

	end
	-- end attribute not empty
end
-- end while attribute loop






			


