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


/* ====================================================== */
-- Create group lookup
/* ====================================================== */
if object_id('tempdb..#groupAssignments') is not null
begin
	drop table #groupAssignments
end
create table #groupAssignments (
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
	
	declare @CampusId int, @CampusName nvarchar(255), @GroupTypeId int, @GroupTypeName nvarchar(255),
		@GroupId int, @GroupName nvarchar(255), @IndividualId int, @CreatedDate datetime

	select @CampusId = ID, @CampusName = Name
	from Campus where ID = @scopeIndex

	insert into #groupAssignments
	select LTRIM(RTRIM(Group_Name)), Group_ID, Individual_ID, Created_Date, 
		LTRIM(RTRIM(SUBSTRING( Group_Type_Name, charindex(' ', Group_Type_Name) +1, 
			len(group_type_name) - charindex(' ', reverse(group_type_name))
		))) as groupType
	from F1..Groups
	where Group_Type_Name not like 'People List'
		and Group_Name not like '%Wait%'		
		and 'Anderson' = LEFT(Group_Type_Name, CHARINDEX(' ', Group_Type_Name))

	
	declare @childIndex int, @childItems int
		select @childIndex = min(ID) from #groupAssignments
		select @childItems = count(1) + @childIndex from #groupAssignments

	while @childIndex <= @childItems
	begin

		select @childIndex = @childIndex + 1
	end


	delete from #groupAssignments
	select @scopeIndex = @scopeIndex + 1
end
-- end while attribute loop

-- completed successfully
RAISERROR ( N'Completed successfully.', 0, 0 ) WITH NOWAIT

use master
