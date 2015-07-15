/* ====================================================== 
-- NewSpring Script #5: 
-- Imports home and fuse groups from F1.
  
--  Assumptions:
--  No GroupMemberStatus in F1 so everyone is a Member
--  Groups must be marked as 'Fuse' or 'Home'

   ====================================================== */
-- Make sure you're using the right Rock database:

USE groups

/* ====================================================== */

-- Enable production mode for performance
SET NOCOUNT ON

-- Set the F1 database name
DECLARE @F1 nvarchar(255) = 'F1'

/* ====================================================== */
-- Start value lookups
/* ====================================================== */
declare @IsSystem int = 0, @Order int = 0,  @TextFieldTypeId int = 1, @True int = 1, @False int = 0 


DECLARE @FuseGroupTypeId int, @FuseGroupMemberId int, @FuseGroupId int
DECLARE @HomeGroupTypeId int, @HomeGroupMemberId int, @HomeGroupId int
SELECT @FuseGroupTypeId = Id, 
	@FuseGroupMemberId = DefaultGroupRoleId
FROM [GroupType]
WHERE [Name] = 'Fuse Groups'

if @FuseGroupTypeId is null
begin
	INSERT [GroupType] ( [IsSystem], [Name], [Description], [GroupTerm], [GroupMemberTerm], [AllowMultipleLocations], [ShowInGroupList], [ShowInNavigation], [TakesAttendance], 
		[AttendanceRule], [AttendancePrintTo], [Order], [InheritedGroupTypeId], [LocationSelectionMode], [AllowedScheduleTypes], [SendAttendanceReminder], [Guid] ) 
	VALUES ( @IsSystem, 'Fuse Groups', 'Grouptype for Fuse groups.', 'Group', 'Member', @False, @False, @False, @False, 1, 0, 0, 15, 0, 0, 0, NEWID() );

	SET @FuseGroupTypeId = SCOPE_IDENTITY()

	INSERT [GroupTypeRole] ( [IsSystem], [GroupTypeId], [Name], [Order], [IsLeader], [CanView], [CanEdit], [Guid] )
	VALUES ( @IsSystem, @FuseGroupTypeId, 'Member', 0, @False, @False, @False, NEWID() )

	SET @FuseGroupMemberId = SCOPE_IDENTITY()

	UPDATE [GroupType]
	SET DefaultGroupRoleId = @FuseGroupMemberId
	WHERE [Id] = @FuseGroupTypeId
end

select @FuseGroupId = ID
from [Group] 
where Name = 'Fuse Groups'
and GroupTypeId = @FuseGroupTypeId

if @FuseGroupId is null
begin
	insert [Group] (IsSystem, ParentGroupId, GroupTypeId, CampusId, Name, [Description], IsSecurityRole, IsActive, [Order], IsPublic, AcceptAlternatePlacements, [Guid])
	select @False, NULL, @FuseGroupTypeId, NULL, 'Fuse Groups', 'Parent group for Fuse Groups', @False, @True, @Order, @True, @True, NEWID()

	select @FuseGroupId = SCOPE_IDENTITY()
end

SELECT @HomeGroupTypeId = Id, 
	@HomeGroupMemberId = DefaultGroupRoleId
FROM [GroupType]
WHERE [Name] = 'Home Groups'

if @HomeGroupTypeId is null
begin
	INSERT [GroupType] ( [IsSystem], [Name], [Description], [GroupTerm], [GroupMemberTerm], [AllowMultipleLocations], [ShowInGroupList], [ShowInNavigation], [TakesAttendance], 
		[AttendanceRule], [AttendancePrintTo], [Order], [InheritedGroupTypeId], [LocationSelectionMode], [AllowedScheduleTypes], [SendAttendanceReminder], [Guid] ) 
	VALUES ( @IsSystem, 'Home Groups', 'Grouptype for Home groups.', 'Group', 'Member', @False, @False, @False, @False, 1, 0, 0, 15, 0, 0, 0, NEWID() );

	SET @HomeGroupTypeId = SCOPE_IDENTITY()

	INSERT [GroupTypeRole] ( [IsSystem], [GroupTypeId], [Name], [Order], [IsLeader], [CanView], [CanEdit], [Guid] )
	VALUES ( @IsSystem, @HomeGroupTypeId, 'Member', 0, @False, @False, @False, NEWID() )

	SET @HomeGroupMemberId = SCOPE_IDENTITY()

	UPDATE [GroupType]
	SET DefaultGroupRoleId = @HomeGroupMemberId
	WHERE [Id] = @HomeGroupTypeId
end

select @HomeGroupId = ID
from [Group] 
where Name = 'Home Groups'
and GroupTypeId = @HomeGroupTypeId

if @HomeGroupId is null
begin
	insert [Group] (IsSystem, ParentGroupId, GroupTypeId, CampusId, Name, [Description], IsSecurityRole, IsActive, [Order], IsPublic, AcceptAlternatePlacements, [Guid])
	select @False, NULL, @HomeGroupTypeId, NULL, 'Home Groups', 'Parent group for Home Groups', @False, @True, @Order, @True, @True, NEWID()

	select @HomeGroupId = SCOPE_IDENTITY()
end

/* ====================================================== */
-- Create group lookup
/* ====================================================== */
if object_id('tempdb..#groupAssignments') is not null
begin
	drop table #groupAssignments
end
create table #groupAssignments (
	ID int IDENTITY(1,1) NOT NULL,
	groupName nvarchar(255),
	groupId bigint,
	individualId bigint,	
	created date,	
	groupType nvarchar(255)
)

declare @scopeIndex int, @numItems int
select @scopeIndex = min(ID) from Campus
select @numItems = count(1) + @scopeIndex from Campus

while @scopeIndex < @numItems
begin
	
	declare @CampusId int, @CampusName nvarchar(255), @GroupTypeId int, @GroupTypeName nvarchar(255), @CampusFuseGroupId int, 
		 @CampusHomeGroupId int, @ParentGroupId int, @ChildGroupId int, @GroupName nvarchar(255), @IndividualId int, @CreatedDate datetime

	select @CampusId = ID, @CampusName = Name
	from Campus where ID = @scopeIndex

	-- Create campus fuse group hierarchy
	select @CampusFuseGroupId = Id from [Group]
	where Name = @CampusName
	and CampusId = @CampusId
	and GroupTypeId = @FuseGroupTypeId
	
	if @CampusFuseGroupId is null
	begin
		insert [Group] (IsSystem, ParentGroupId, GroupTypeId, CampusId, Name, [Description], IsSecurityRole, IsActive, [Order], IsPublic, AcceptAlternatePlacements, [Guid])
		select @False, @FuseGroupId, @FuseGroupTypeId, @CampusId, @CampusName, @CampusName + ' Fuse Groups', @False, @True, @Order, @True, @True, NEWID()

		select @CampusFuseGroupId = SCOPE_IDENTITY()
	end

	-- Create campus home group hierarchy
	select @CampusHomeGroupId = Id from [Group]
	where Name = @CampusName
	and CampusId = @CampusId
	and GroupTypeId = @HomeGroupTypeId
	
	if @CampusHomeGroupId is null
	begin
		insert [Group] (IsSystem, ParentGroupId, GroupTypeId, CampusId, Name, [Description], IsSecurityRole, IsActive, [Order], IsPublic, AcceptAlternatePlacements, [Guid])
		select @False, @HomeGroupid, @HomeGroupTypeId, @CampusId, @CampusName, @CampusName + ' Home Groups', @False, @True, @Order, @True, @True, NEWID()

		select @CampusHomeGroupId = SCOPE_IDENTITY()
	end

	-- Filter groups by the current campus
	insert into #groupAssignments
	select LTRIM(RTRIM(Group_Name)), Group_ID, Individual_ID, Created_Date, 
		LTRIM(RTRIM(SUBSTRING( Group_Type_Name, charindex(' ', Group_Type_Name) +1, 
			len(group_type_name) - charindex(' ', reverse(group_type_name))
		))) as groupType
	from F1..Groups
	where Group_Type_Name not like 'People List'
		and Group_Name not like '%Wait%'		
		and @CampusName = LEFT(Group_Type_Name, CHARINDEX(' ', Group_Type_Name))
			
	/* ====================================================== */
	-- Start creating child groups
	/* ====================================================== */
	declare @childIndex int, @childItems int
	select @childIndex = min(ID) from #groupAssignments
	select @childItems = count(1) + @childIndex from #groupAssignments

	while @childIndex <= @childItems
	begin
		
		select @GroupName = groupName, @GroupTypeName = groupType, @CreatedDate = created
		from #groupAssignments ga
		where @childIndex = ga.ID

		-- Look up GroupType and Group
		if ( @GroupTypeName like 'Fuse%' )
		begin
			select @GroupTypeId = @FuseGroupTypeId
			select @GroupId = @CampusFuseGroupId
		end
		else if @GroupTypeName like '%Home%'
		begin
			select @GroupTypeId = @HomeGroupTypeId
			select @GroupId = @CampusHomeGroupId
		end

		select @GroupId = Id from [Group]
		where Name = @GroupName 
		and CampusId = @CampusId
		and GroupTypeId = @GroupTypeId

		-- Create group if it doesn't exist
		if @GroupId is null
		begin
			insert [Group] (IsSystem, ParentGroupId, GroupTypeId, CampusId, Name, [Description], IsSecurityRole, IsActive, [Order], IsPublic, AcceptAlternatePlacements, [Guid])
			select @False, @GroupId, @GroupTypeId, @CampusId, @GroupName, @CampusName + ' ' + @GroupName, @False, @True, @Order, @True, @True, NEWID()

			select @GroupId = SCOPE_IDENTITY()
		end
		

		-- Create memberships

		inner join PersonAlias p
		on ga.individualId = p.ForeignId
		

		
		



		select @childIndex = @childIndex + 1
	end

	delete from #groupAssignments
	select @scopeIndex = @scopeIndex + 1
end
-- end while attribute loop

-- completed successfully
RAISERROR ( N'Completed successfully.', 0, 0 ) WITH NOWAIT

use master
