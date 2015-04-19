/* ====================================================== */
-- NewSpring Script #2: 
-- Inserts RLC, activity ministries and activity groups.

-- Make sure you're using the right Rock database:

USE [Rock]

/* ====================================================== */

-- Set the F1 database name
DECLARE @F1 varchar(255) = 'newspring'






-- Start value lookups
declare @scopeIndex int, @numItems int, @rlcId int, @nameSearchValueId int, @personAliasId int,
	@groupTypeId int, @groupId int, @locationId int, @campusId int, @True int, @False int
declare @groupTypeName varchar(255), @groupName varchar(255), @groupLocation varchar(255)
select @scopeIndex = min(ID) from newspring.._map
select @numItems = count(1) + @scopeIndex from newspring.._map

select @True = 1, @False = 0
select @nameSearchValueId = Id 
from Rock..[DefinedValue]
where [Guid] = '071D6DAA-3063-463A-B8A1-7D9A1BE1BB31'

-- Get person lookup table
select p.Id as 'PersonAliasId', a.Individual_ID
into #personLookup
from PersonAlias p
inner join newspring..attendance a
on a.Individual_ID = p.ForeignId

while @scopeIndex <= @numItems
begin
		
	select @rlcID = null, @groupTypeName = '', @groupName = '', @personAliasId = null,
		@groupTypeId = null, @groupId = null, @locationId = null, @campusId = null
	select @rlcId = RLC_ID, @groupTypeName = GroupType, @groupName = GroupName
	from newspring.._map where ID = @scopeIndex
		
	select @groupTypeId = ID 
	from Rock..[GroupType]
	where name = @groupTypeName

	select @groupId = ID, @campusId = CampusId
	from Rock..[Group]
	where GroupTypeId = @groupTypeId
	and Name = @groupName

	select @locationId = LocationID
	from Rock..[GroupLocation]
	where GroupId = @groupId
	
	if @groupId is not null and @locationId is not null
	begin
		
		insert Rock..[Attendance] (LocationId, GroupId, SearchTypeValueId, StartDateTime, 
			DidAttend, Note, [Guid], CreatedDateTime, CampusId, PersonAliasId, RSVP)
		select @locationId, @groupId, @nameSearchValueId, Start_Date_Time, @True,
			 Tag_Comment, NEWID(), ISNULL(Check_In_Time, GETDATE()), @campusId, 
			 p.PersonAliasId, @False
		from newspring..Attendance a
		inner join #personLookup p
		on a.Individual_ID = p.Individual_ID
		where RLC_ID = @rlcId

	end
	-- end groupId not null

	set @scopeIndex = @scopeIndex + 1
end
-- end rlc loop

USE [master]




/* ====================================================== 

-- set personalias foreign id's
update pa
set ForeignId = p.foreignid
from rock..person p
inner join personalias pa
on p.id = pa.personid
and p.foreignid is not null
	
-- double check all grouptypes/groups exist
update newspring.._map
set groupname = replace(groupname, 'Office Team', 'KS Office Team')
where grouptype = 'cen - kidspring volunteer' 
and groupname = 'Office Team'


select *
from newspring.._map
where grouptype + groupname not in 
(
	select gt.name + g.name
	from rock..[group] g
	inner join rock..grouptype gt
	on g.grouptypeid = gt.id
)

alter table newspring.._map 
add ID int identity(1,1)

select * from newspring.._map

select count(1) from Rock..attendance

select top 100 p.* from Rock..person p
inner join rock..attendance a
on p.id = a.personaliasid

 ====================================================== */
