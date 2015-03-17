declare @isSystem bit = 0
declare @delimiter varchar(5) = ' - '

update [group] set campusId = null
delete from Campus where id = 1

/* ========================== */
-- create campuses structure
/* ========================== */

insert Campus (IsSystem, Name, ShortCode, [Guid], IsActive)
values
(@isSystem, 'Anderson', 'AND', NEWID(), 1),
(@isSystem, 'Boiling Springs', 'BSP', NEWID(), 1),
(@isSystem, 'Charleston', 'CHS', NEWID(), 1),
(@isSystem, 'Columbia', 'COL', NEWID(), 1),
(@isSystem, 'Florence', 'FLO', NEWID(), 1),
(@isSystem, 'Greenville', 'GVL', NEWID(), 1),
(@isSystem, 'Greenwood', 'GWD', NEWID(), 1),
(@isSystem, 'Lexington', 'LEX', NEWID(), 1),
(@isSystem, 'Myrtle Beach', 'MYR', NEWID(), 1),
(@isSystem, 'Powdersville', 'POW', NEWID(), 1),
(@isSystem, 'Spartanburg', 'SPA', NEWID(), 1)


/* ========================== */
-- top check-in areas
/* ========================== */
if object_id('tempdb..#topAreas') is not null
begin
	drop table #topAreas
end
create table #topAreas (
	ID int IDENTITY(1,1),
	name varchar(255),
	attendanceRule int,
	inheritedType int
)

insert #topAreas
values
('Creativity/Technology Attendee', 0, 15),
('Creativity/Technology Volunteer', 2, 15),
('Fuse Attendee', 1, 17),
('Fuse Volunteer', 2, 15),
('Guest Services Attendee', 0, 15),
('Guest Services Volunteer', 2, 15),
('KidSpring Attendee', 0, null),
('KidSpring Volunteer', 2, null),
('Next Steps Attendee', 0, 15),
('Next Steps Volunteer', 2, 15)


/* ========================== */
-- kids structure
/* ========================== */
if object_id('tempdb..#subKidAreas') is not null
begin
	drop table #subKidAreas
end
create table #subKidAreas (
	ID int IDENTITY(1,1),
	name varchar(255),
	parentName varchar(255),
	inheritedType int
)

insert #subKidAreas
values
('Nursery', 'KidSpring Attendee', 15),
('Preschool', 'KidSpring Attendee', 15),
('Elementary', 'KidSpring Attendee', 17),
('Special Needs', 'KidSpring Attendee', NULL),
('Nursery Vols', 'KidSpring Volunteer', 15),
('Preschool Vols', 'KidSpring Volunteer', 15),
('Elementary Vols', 'KidSpring Volunteer', 15),
('Special Needs Vols', 'KidSpring Volunteer', 15),
('KS Support Vols', 'KidSpring Volunteer', 15),
('KS Production Vols', 'KidSpring Volunteer', 15)

/* ========================== */
-- group structure
/* ========================== */
if object_id('tempdb..#groupStructure') is not null
begin
	drop table #groupStructure
end
create table #groupStructure (
	ID int IDENTITY(1,1),
	groupTypeName varchar(255),
	groupName varchar(255),
	locationName varchar(255),
)

insert #groupStructure
values
-- kid structure from and
('Nursery', 'Wonder Way - 1', 'Wonder Way - 1'),
('Nursery', 'Wonder Way - 2', 'Wonder Way - 2'),
('Nursery', 'Wonder Way - 3', 'Wonder Way - 3'),
('Nursery', 'Wonder Way - 4', 'Wonder Way - 4'),
('Nursery', 'Wonder Way - 5', 'Wonder Way - 5'),
('Nursery', 'Wonder Way - 6', 'Wonder Way - 6'),
('Nursery', 'Wonder Way - 7', 'Wonder Way - 7'),
('Nursery', 'Wonder Way - 8', 'Wonder Way - 8'),
('Preschool', 'Fire Station', 'Fire Station'),
('Preschool', 'Lil'' Spring', 'Lil'' Spring'),
('Preschool', 'Pop''s Garage', 'Pop''s Garage'),
('Preschool', 'Spring Fresh', 'Spring Fresh'),
('Preschool', 'SpringTown Police', 'SpringTown Police'),
('Preschool', 'SpringTown Toys', 'SpringTown Toys'),
('Preschool', 'Treehouse', 'Treehouse'),
('Preschool', 'Base Camp Jr.', 'Base Camp Jr.'),
('Elementary', 'ImagiNation - K', 'ImagiNation'),
('Elementary', 'ImagiNation - 1st', 'ImagiNation'),
('Elementary', 'Jump Street - 2nd', 'Jump Street'),
('Elementary', 'Jump Street - 3rd', 'Jump Street'),
('Elementary', 'Shockwave - 4th', 'Shockwave'),
('Elementary', 'Shockwave - 5th', 'Shockwave'),
('Elementary', 'Base Camp', 'Base Camp'),
('Special Needs', 'Spring Zone', 'Spring Zone'),
('Special Needs', 'Spring Zone Jr.', 'Spring Zone Jr.'),
-- vol structure from col
('Guest Services Attendee', 'Green Room Attendee', 'Guest Services Attendee'),
('Guest Services Attendee', 'Special Event Attendee', 'Guest Services Attendee'),
('Guest Services Volunteer', 'Auditorium Reset Team', 'Guest Services Volunteer'),
('Guest Services Volunteer', 'Awake Team', 'Guest Services Volunteer'),
('Guest Services Volunteer', 'Campus Safety', 'Guest Services Volunteer'),
('Guest Services Volunteer', 'Facilities Volunteer', 'Guest Services Volunteer'),
('Guest Services Volunteer', 'Facility Cleaning Crew', 'Guest Services Volunteer'),
('Guest Services Volunteer', 'Green Room Volunteer', 'Guest Services Volunteer'),
('Guest Services Volunteer', 'Greeting Team', 'Guest Services Volunteer'),
('Guest Services Volunteer', 'Guest Services Team', 'Guest Services Volunteer'),
('Guest Services Volunteer', 'New Serve Team', 'Guest Services Volunteer'),
('Guest Services Volunteer', 'Parking Team', 'Guest Services Volunteer'),
('Guest Services Volunteer', 'Service Coordinator', 'Guest Services Volunteer'),
('Guest Services Volunteer', 'Sign Language Team', 'Guest Services Volunteer'),
('Guest Services Volunteer', 'Usher Team', 'Guest Services Volunteer'),
('Guest Services Volunteer', 'VHQ Team', 'Guest Services Volunteer'),
('Guest Services Volunteer', 'Finance Team', 'Guest Services Volunteer'),
('Guest Services Volunteer', 'Special Event Volunteer', 'Guest Services Volunteer'),
('Next Steps Attendee', 'Baptism Attendee', 'Next Steps Attendee'),
('Next Steps Attendee', 'Ownership Class Attendee', 'Next Steps Attendee'),
('Next Steps Attendee', 'Ownership Class Current Owner', 'Next Steps Attendee'),
('Next Steps Attendee', 'Financial Coaching Attendee', 'Next Steps Attendee'),
('Next Steps Attendee', 'Special Event Attendee', 'Next Steps Attendee'),
('Next Steps Volunteer', 'Baptism Volunteer', 'Next Steps Volunteer'),
('Next Steps Volunteer', 'Events Office Team', 'Next Steps Volunteer'),
('Next Steps Volunteer', 'Financial Coaching Volunteer', 'Next Steps Volunteer'),
('Next Steps Volunteer', 'Financial Planning Office Team', 'Next Steps Volunteer'),
('Next Steps Volunteer', 'Group Training', 'Next Steps Volunteer'),
('Next Steps Volunteer', 'Ownership Class Volunteer', 'Next Steps Volunteer'),
('Next Steps Volunteer', 'Special Event Volunteer', 'Next Steps Volunteer'),
('Next Steps Volunteer', 'Next Steps Area', 'Next Steps Volunteer'),
('Next Steps Volunteer', 'Resource Center', 'Next Steps Volunteer'),
('Next Steps Volunteer', 'District Leader', 'Next Steps Volunteer'),
('Next Steps Volunteer', 'Group Leader', 'Next Steps Volunteer'),
('Next Steps Volunteer', 'Groups Connector', 'Next Steps Volunteer'),
('Next Steps Volunteer', 'Groups Office Team', 'Next Steps Volunteer'),
('Next Steps Volunteer', 'Care Office Team', 'Next Steps Volunteer'),
('Next Steps Volunteer', 'Care Visitation Team', 'Next Steps Volunteer'),
('Next Steps Volunteer', 'Prayer Team', 'Next Steps Volunteer'),
('Next Steps Volunteer', 'Sunday Care Team', 'Next Steps Volunteer'),
('Next Steps Volunteer', 'Writing Team', 'Next Steps Volunteer'),
('Next Steps Volunteer', 'New Serve Team', 'Next Steps Volunteer'),
('Fuse Attendee', '6th Grade Student', 'Fuse Attendee'),
('Fuse Attendee', '7th Grade Student', 'Fuse Attendee'),
('Fuse Attendee', '8th Grade Student', 'Fuse Attendee'),
('Fuse Attendee', '9th Grade Student', 'Fuse Attendee'),
('Fuse Attendee', '10th Grade Student', 'Fuse Attendee'),
('Fuse Attendee', '11th Grade Student', 'Fuse Attendee'),
('Fuse Attendee', '12th Grade Student', 'Fuse Attendee'),
('Fuse Volunteer', 'Atrium', 'Fuse Volunteer'),
('Fuse Volunteer', 'Care Team', 'Fuse Volunteer'),
('Fuse Volunteer', 'Check-In', 'Fuse Volunteer'),
('Fuse Volunteer', 'Game Room', 'Fuse Volunteer'),
('Fuse Volunteer', 'Greeter', 'Fuse Volunteer'),
('Fuse Volunteer', 'Leadership Team', 'Fuse Volunteer'),
('Fuse Volunteer', 'Load In / Load Out', 'Fuse Volunteer'),
('Fuse Volunteer', 'Lounge', 'Fuse Volunteer'),
('Fuse Volunteer', 'New Serve Team', 'Fuse Volunteer'),
('Fuse Volunteer', 'Next Steps Area', 'Fuse Volunteer'),
('Fuse Volunteer', 'Parking', 'Fuse Volunteer'),
('Fuse Volunteer', 'Pick-Up', 'Fuse Volunteer'),
('Fuse Volunteer', 'Snack Bar', 'Fuse Volunteer'),
('Fuse Volunteer', 'Sports', 'Fuse Volunteer'),
('Fuse Volunteer', 'Ushers', 'Fuse Volunteer'),
('Fuse Volunteer', 'VHQ', 'Fuse Volunteer'),
('Fuse Volunteer', 'Student Leader', 'Fuse Volunteer'),
('Fuse Volunteer', 'Jump Off', 'Fuse Volunteer'),
('Fuse Volunteer', 'Fuse Group Leader', 'Fuse Volunteer'),
('Fuse Volunteer', 'Campus Safety', 'Fuse Volunteer'),
('Fuse Volunteer', 'Production', 'Fuse Volunteer'),
('Fuse Volunteer', 'Worship', 'Fuse Volunteer'),
('Fuse Volunteer', 'Spring Zone', 'Fuse Volunteer'),
('Fuse Volunteer', 'Fuse Guest', 'Fuse Volunteer'),
('Fuse Volunteer', 'Special Event Volunteer', 'Fuse Volunteer'),
('Creativity/Technology Volunteer', 'Band', 'Creativity/Technology Volunteer'),
('Creativity/Technology Volunteer', 'Band Green Room', 'Creativity/Technology Volunteer'),
('Creativity/Technology Volunteer', 'Production Team', 'Creativity/Technology Volunteer'),
('Creativity/Technology Volunteer', 'New Serve Team', 'Creativity/Technology Volunteer'),
('Creativity/Technology Volunteer', 'IT Team', 'Creativity/Technology Volunteer'),
('Creativity/Technology Volunteer', 'Social Media / PR Team', 'Creativity/Technology Volunteer'),
('Creativity/Technology Volunteer', 'Special Event Volunteer', 'Creativity/Technology Volunteer'),
('Creativity/Technology Attendee', 'Special Event Attendee', 'Creativity/Technology Attendee'),
('Nursery Vols', 'Nursery Early Bird Volunteer', 'Nursery Volunteer'),
('Nursery Vols', 'Nursery Service Leader', 'Nursery Volunteer'),
('Nursery Vols', 'Nursery Team Leader', 'Nursery Volunteer'),
('Nursery Vols', 'Wonder Way 1 Volunteer', 'Wonder Way - 1'),
('Nursery Vols', 'Wonder Way 2 Volunteer', 'Wonder Way - 2'),
('Nursery Vols', 'Wonder Way 3 Volunteer', 'Wonder Way - 3'),
('Nursery Vols', 'Wonder Way 4 Volunteer', 'Wonder Way - 4'),
('Nursery Vols', 'Wonder Way 5 Volunteer', 'Wonder Way - 5'),
('Nursery Vols', 'Wonder Way 6 Volunteer', 'Wonder Way - 6'),
('Nursery Vols', 'Wonder Way 7 Volunteer', 'Wonder Way - 7'),
('Nursery Vols', 'Wonder Way 8 Volunteer', 'Wonder Way - 8'),
('Preschool Vols', 'Base Camp Jr. Volunteer', 'Base Camp Jr.'),
('Preschool Vols', 'Lil'' Spring Volunteer', 'Lil'' Spring'),
('Preschool Vols', 'Pop''s Garage Volunteer', 'Pop''s Garage'),
('Preschool Vols', 'Preschool Early Bird Volunteer', 'Preschool Volunteer'),
('Preschool Vols', 'Preschool Service Leader', 'Preschool Volunteer'),
('Preschool Vols', 'Preschool Team Leader', 'Preschool Volunteer'),
('Preschool Vols', 'Spring Fresh Volunteer', 'Spring Fresh'),
('Preschool Vols', 'SpringTown Toys Volunteer', 'SpringTown Toys'),
('Preschool Vols', 'Treehouse Volunteer', 'Treehouse'),
('Elementary Vols', 'Base Camp Volunteer', 'Base Camp'),
('Elementary Vols', 'Elementary Early Bird Volunteer', 'Elementary Volunteer'),
('Elementary Vols', 'Elementary Service Leader', 'Elementary Volunteer'),
('Elementary Vols', 'Elementary Team Leader', 'Elementary Volunteer'),
('Elementary Vols', 'ImagiNation Volunteer', 'ImagiNation'),
('Elementary Vols', 'Jump Street Volunteer', 'Jump Street'),
('Elementary Vols', 'Shockwave Volunteer', 'Shockwave'),
('Special Needs Vols', 'Spring Zone Service Leader', 'Spring Zone'),
('Special Needs Vols', 'Spring Zone Team Leader', 'Spring Zone'),
('Special Needs Vols', 'Spring Zone Jr. Volunteer', 'Spring Zone Jr.'),
('Special Needs Vols', 'Spring Zone Volunteer', 'Spring Zone'),
('KS Support Vols', 'Advocate', 'Support Volunteer'),
('KS Support Vols', 'Check-In Volunteer', 'Support Volunteer'),
('KS Support Vols', 'First Time Team', 'Support Volunteer'),
('KS Support Vols', 'Guest Services Team Leader', 'Support Volunteer'),
('KS Support Vols', 'KidSpring Assistant', 'Support Volunteer'),
('KS Support Vols', 'KidSpring Office Team', 'Support Volunteer'),
('KS Support Vols', 'New Serve Team', 'Support Volunteer'),
('KS Production Vols', 'Elementary Production Team Leader', 'Production Volunteer'),
('KS Production Vols', 'KidSpring Production', 'Production Volunteer'),
('KS Production Vols', 'Preschool Production Team Leader', 'Production Volunteer')

/* ========================== */
-- delete existing areas
/* ========================== */

delete from location
where id in (
	select distinct locationId
	from grouplocation gl
	inner join [group] g
	on gl.groupid = g.id
	and g.GroupTypeId in (14, 18, 19, 20, 21, 22)
)

delete from location where id > 1

delete from GroupTypeAssociation
where GroupTypeId in (14, 18, 19, 20, 21, 22)
or ChildGroupTypeId in (14, 18, 19, 20, 21, 22)

delete from [Group]
where GroupTypeId in (14, 18, 19, 20, 21, 22)

delete from GroupType
where id in (14, 18, 19, 20, 21, 22)


/* ========================== */
-- set up initial values
/* ========================== */

declare @campusId int, @numCampuses int, @initialAreaId int, @groupRoleId int,
	@typePurpose int, @campusLocationId int, @initialGroupId int
select @typePurpose = 142  /* check-in template purpose type */
select @campusId = min(Id) from Campus
select @numCampuses = count(1) + @campusId from Campus

declare @campusName varchar(30), @code varchar(5)

/* ========================== */
-- insert campus levels
/* ========================== */
while @campusId <= @numCampuses
begin

	select @campusName = '', @initialAreaId = 0
	select @campusName = name, @code = ShortCode
	from Campus where Id = @campusId

	if @campusName <> ''
	begin
		-- campus location
		insert location (ParentLocationId, Name, IsActive, [Guid])
		select NULL, @campusName, 1, NEWID()

		set @campusLocationId = SCOPE_IDENTITY()

		update campus set LocationId = @campusLocationId where id = @campusId

		-- initial check-in areas
		insert grouptype (IsSystem, Name, Description, GroupTerm, GroupMemberTerm,
			DefaultGroupRoleId, AllowMultipleLocations, ShowInGroupList,
			ShowInNavigation, TakesAttendance, AttendanceRule, AttendancePrintTo,
			[Order], InheritedGroupTypeId, LocationSelectionMode, GroupTypePurposeValueId, [Guid],
			AllowedScheduleTypes, SendAttendanceReminder)
		select 0, @campusName, @campusName + ' Campus', 'Group', 'Member',
			NULL, 0, 1, 1, 0, 0, 1, 0, NULL, 0, 142, NEWID(), 0, 0

		select @initialAreaId = SCOPE_IDENTITY()

		/* ========================== */
		-- set default grouptype role
		/* ========================== */
		insert GroupTypeRole (isSystem, GroupTypeId, Name, [Order], IsLeader,
			[Guid], CanView, CanEdit)
		values (@isSystem, @initialAreaId, 'Member', 0, 0, NEWID(), 0, 0)

		select @groupRoleId = SCOPE_IDENTITY()

		update grouptype
		set DefaultGroupRoleId = @groupRoleId
		where id = @initialAreaId

		/* ============================== */
		-- create matching group
		/* ============================== */
		insert [Group] (IsSystem, ParentGroupId, GroupTypeId, CampusId, Name,
			Description, IsSecurityRole, IsActive, [Order], [Guid])
		select @isSystem, NULL, @initialAreaId, @campusId, @campusName,
			@campusName + ' Group', 0, 1, 0, NEWID()

		select @initialGroupId = SCOPE_IDENTITY()

		/* ========================== */
		-- insert top areas
		/* ========================== */
		declare @scopeIndex int, @numItems int, @topAreaId int,
			@attendanceRule int, @inheritedTypeId int, @baseLocationId int
		declare @areaName varchar(255), @baseLocation varchar(255)
		declare @volunteer varchar(255) = 'Volunteer'
		declare @attendee varchar(255) = 'Attendee'

		select @scopeIndex = min(Id) from _topAreas
		select @numItems = count(1) + @scopeIndex from _topAreas

		while @scopeIndex <= @numItems
		begin

			select @areaName = '', @topAreaId = 0, @groupRoleId = 0
			select @areaName = name, @attendanceRule = attendanceRule, @inheritedTypeId = inheritedType
			from _topAreas where id = @scopeIndex

			if @areaName <> ''
			begin

				/* ========================== */
				-- insert top area hierarchy
				/* ========================== */
				insert grouptype (IsSystem, Name, Description, GroupTerm, GroupMemberTerm,
					DefaultGroupRoleId, AllowMultipleLocations, ShowInGroupList,
					ShowInNavigation, TakesAttendance, AttendanceRule, AttendancePrintTo,
					[Order], InheritedGroupTypeId, LocationSelectionMode, GroupTypePurposeValueId, [Guid],
					AllowedScheduleTypes, SendAttendanceReminder)
				select 0, @code + @delimiter + @areaName, @code + @delimiter + @areaName + ' GroupType', 'Group', 'Member', NULL,
					1, 1, 1, 1, @attendanceRule, 0, 0, @inheritedTypeId, 0, NULL, NEWID(), 0, 0

				select @topAreaId = SCOPE_IDENTITY()

				insert GroupTypeAssociation
				values (@initialAreaId, @topAreaId)

				/* ========================== */
				-- set default grouptype role
				/* ========================== */
				insert GroupTypeRole (isSystem, GroupTypeId, Name, [Order], IsLeader,
					[Guid], CanView, CanEdit)
				values (@isSystem, @topAreaId, 'Member', 0, 0, NEWID(), 0, 0)

				select @groupRoleId = SCOPE_IDENTITY()

				update grouptype
				set DefaultGroupRoleId = @groupRoleId
				where id = @topAreaId

				/* ============================== */
				-- create matching group
				/* ============================== */
				insert [Group] (IsSystem, ParentGroupId, GroupTypeId, CampusId, Name,
					Description, IsSecurityRole, IsActive, [Order], [Guid])
				select @isSystem, @initialGroupId, @topAreaId, @campusId, @areaName,
					@code + @delimiter + @areaName + ' Group', 0, 1, 0, NEWID()


				/* ========================== */
				-- set location hiearchy
				/* ========================== */

				--if charindex(@volunteer, @areaName) > 0
				--begin
				--	select @baseLocation = rtrim(substring( @areaName, 0, charindex(@volunteer, @areaName)))
				--end
				--else begin
				--	select @baseLocation = rtrim(substring( @areaName, 0, charindex(@attendee, @areaName)))
				--end

				--select @baseLocationId = Id from location
				--where name = @baseLocation
				--and ParentLocationId = @campusLocationId

				-- set up parent if doesn't exist
				--if @baseLocationId is null
				--begin
				--	insert location (ParentLocationId, Name, IsActive, [Guid])
				--	select @campusLocationId, @baseLocation, 1, NEWID()
				--	set @baseLocationId = SCOPE_IDENTITY()
				--end

				-- set up child location
				insert location (ParentLocationId, Name, IsActive, [Guid])
				select @campusLocationId, @areaName, 1, NEWID()
				--select @baseLocationId, @areaName, 1, NEWID()

				select @baseLocationId = NULL, @baseLocation = ''
			end
			--end if area not empty

			set @scopeIndex = @scopeIndex + 1
		end
		-- end top area grouptypes


		/* ========================== */
		-- set kid level grouptypes
		/* ========================== */
		declare @parentArea varchar(255), @areaId int, @parentGroupId int
		select @scopeIndex = min(Id) from _subKidAreas
		select @numItems = @scopeIndex + count(1) from _subKidAreas

		while @scopeIndex <= @numItems
		begin

			select @areaName = ''
			select @areaName = name, @parentArea = parentName, @inheritedTypeId = inheritedType
			from _subKidAreas where id = @scopeIndex

			if @areaName <> ''
			begin

				select @topAreaId = Id from GroupType where name = @code + @delimiter + @parentArea

				insert grouptype (IsSystem, Name, Description, GroupTerm, GroupMemberTerm,
					DefaultGroupRoleId, AllowMultipleLocations, ShowInGroupList,
					ShowInNavigation, TakesAttendance, AttendanceRule, AttendancePrintTo,
					[Order], InheritedGroupTypeId, LocationSelectionMode, GroupTypePurposeValueId, [Guid],
					AllowedScheduleTypes, SendAttendanceReminder)
				select 0, @code + @delimiter + @areaName, @code + @delimiter + @areaName + ' GroupType', 'Group', 'Member', NULL,
					1, 1, 1, 1, 0, 0, 0, @inheritedTypeId, 0, NULL, NEWID(), 0, 0

				select @areaId = SCOPE_IDENTITY()

				insert GroupTypeAssociation
				values (@topAreaId, @areaId)

				/* ============================== */
				-- set default grouptype role
				/* ============================== */
				insert grouptypeRole (isSystem, GroupTypeId, Name, [Order], IsLeader,
					[Guid], CanView, CanEdit)
				values (@isSystem, @areaId, 'Member', 0, 0, NEWID(), 0, 0)

				select @groupRoleId = SCOPE_IDENTITY()

				update grouptype
				set DefaultGroupRoleId = @groupRoleId
				where id = @areaId

				/* ============================== */
				-- create matching group
				/* ============================== */
				select @parentGroupId = Id from [group]
				where name = @parentArea
				and ParentGroupId = @initialGroupId

				insert [Group] (IsSystem, ParentGroupId, GroupTypeId, CampusId, Name,
					Description, IsSecurityRole, IsActive, [Order], [Guid])
				select @isSystem, @parentGroupId, @areaId, @campusId,  @areaName,
					 @areaName + @delimiter + 'Group', 0, 1, 10, NEWID()

			end
			--end if area not empty

			set @scopeIndex = @scopeIndex + 1
		end
		-- end kid level grouptypes


		/* ========================== */
		-- set group structure
		/* ========================== */
		declare @groupName varchar(255), @groupTypeName varchar(255), @locationName varchar(255)
		declare @locationId int, @parentLocationId int, @groupTypeId int, @parentGroupTypeId int, @groupId int
		select @scopeIndex = min(Id) from _groupStructure
		select @numItems = @scopeIndex + count(1) from _groupStructure

		while @scopeIndex <= @numItems
		begin

			select @groupName = null, @groupTypeName = null, @locationName = null, @locationId = null
			select @groupName = groupName, @groupTypeName = groupTypeName, @locationName = locationName
			from _groupStructure where id = @scopeIndex

			if @groupName is not null
			begin
				-- get child and parent group
				select @groupTypeId = Id from grouptype
				where name = @code + @delimiter + @groupTypeName

				select @parentGroupId = Id from [group]
				where name = @groupTypeName
				and grouptypeId = @groupTypeId

				-- insert child level group
				insert [Group] (IsSystem, ParentGroupId, GroupTypeId, CampusId, Name,
					Description, IsSecurityRole, IsActive, [Order], [Guid])
				select @isSystem, @parentGroupId, @groupTypeId, @campusId, @groupName,
					@code + @delimiter + @groupName + ' Group', 0, 1, 0, NEWID()

				select @groupid = SCOPE_IDENTITY()

				-- insert location for group

				;with locationChildren as (
					select l.id, l.parentLocationId, l.name
						from test..location l
						where id = @campusLocationId
					union all
					select l2.id, l2.parentlocationId, l2.name
						from test..location l2
						inner join locationChildren lc
						on lc.id = l2.ParentLocationId
				)
				select @locationId = id from locationChildren
				where name = @locationname

				if @locationId is null
				begin

					declare @parentLocationName varchar(255)
					-- KidSpring is the only one with a tri-level setup
					if @groupTypeName like '%vols%'
					begin
						select @parentLocationName = 'KidSpring Volunteer'
					end
					else begin
						select @parentLocationName = 'KidSpring Attendee'
					end

					;with locationChildren as (
						select l.id, l.parentLocationId, l.name
							from test..location l
							where id = @campusLocationId
						union all
						select l2.id, l2.parentlocationId, l2.name
							from test..location l2
							inner join locationChildren lc
							on lc.id = l2.ParentLocationId
					)
					select @parentLocationId = id from locationChildren
					where name = @parentLocationName

					-- create location
					insert location (ParentLocationId, Name, IsActive, [Guid])
					select @parentLocationId, @locationName, 1, NEWID()

					select @locationId = SCOPE_IDENTITY()
				end

				insert grouplocation (groupid, locationid, IsMailingLocation, IsMappedLocation, [Guid])
				values (@groupid, @locationId, 0, 0, newid())
			end
			-- end group name not empty

			set @scopeIndex = @scopeIndex + 1
		end
		-- end group structure

	end
	-- end campus not empty

	set @campusId = @campusId + 1
end
-- end campuses loop



/* TESTING SECTION

use master
restore database test from test with replace

use test


select '(''' + substring(c.name, 7, len(c.name)-6) + ''', '  --as 'child.grouptype'
+ '''' + g.name + ''', ' --as 'child.group'
+ '''' + l.name + '''),' --as 'group.location'
select distinct l.name
from rock..GroupType p
inner join rock..GroupTypeAssociation gta
on p.id = gta.GroupTypeId
and p.name like 'col%'
and p.name not like '%kidspring%attendee%'
inner join rock..grouptype c
on gta.ChildGroupTypeId = c.id
inner join rock..[group] g
on g.GroupTypeId = c.id
inner join rock..grouplocation gl
on g.id = gl.groupid
inner join rock..location l
on gl.LocationId = l.id


*/
