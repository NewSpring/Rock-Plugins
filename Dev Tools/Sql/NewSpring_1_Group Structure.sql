/* ====================================================== */
-- NewSpring Script #1: 
-- Inserts campuses, groups, grouptypes and locations.

-- Make sure you're using the right Rock database:

USE [Rock]

/* ====================================================== */

-- Enable production mode for performance
SET NOCOUNT ON

-- Set common variables 
declare @isSystem bit = 0
declare @delimiter nvarchar(5) = ' - '
declare @True bit = 1
declare @False bit = 0
declare @BooleanFieldTypeId int
declare @GroupEntityTypeId int

select @BooleanFieldTypeId = [Id] FROM [FieldType] WHERE [Name] = 'Boolean'
select @GroupEntityTypeId = [Id] FROM [EntityType] WHERE [Name] = 'Rock.Model.Group'

--update [group] set campusId = null
--delete from Campus where id = 1

/* ====================================================== */
-- create campuses
/* ====================================================== */

--insert Campus (IsSystem, Name, ShortCode, [Guid], IsActive)
--values
--(@isSystem, 'Aiken', 'AKN', NEWID(), 1),
--(@isSystem, 'Anderson', 'AND', NEWID(), 1),
--(@isSystem, 'Boiling Springs', 'BSP', NEWID(), 1),
--(@isSystem, 'Charleston', 'CHS', NEWID(), 1),
--(@isSystem, 'Clemson', 'CLE', NEWID(), 1),
--(@isSystem, 'Columbia', 'COL', NEWID(), 1),
--(@isSystem, 'Florence', 'FLO', NEWID(), 1),
--(@isSystem, 'Greenville', 'GVL', NEWID(), 1),
--(@isSystem, 'Greenwood', 'GWD', NEWID(), 1),
--(@isSystem, 'Greer', 'GRR', NEWID(), 1),
--(@isSystem, 'Hilton Head', 'HHD', NEWID(), 1),
--(@isSystem, 'Lexington', 'LEX', NEWID(), 1),
--(@isSystem, 'Myrtle Beach', 'MYR', NEWID(), 1),
--(@isSystem, 'Northeast Columbia', 'NEC', NEWID(), 1),
--(@isSystem, 'Powdersville', 'POW', NEWID(), 1),
--(@isSystem, 'Rock Hill', 'RKH', NEWID(), 1),
--(@isSystem, 'Simpsonville', 'SIM', NEWID(), 1),
--(@isSystem, 'Spartanburg', 'SPA', NEWID(), 1),
--(@isSystem, 'Sumter', 'SUM', NEWID(), 1)


/* ====================================================== */
-- campus check-in areas
/* ====================================================== */
if object_id('tempdb..#campusAreas') is not null
begin
	drop table #campusAreas
end
create table #campusAreas (
	ID int IDENTITY(1,1),
	name nvarchar(255),
	attendanceRule int,
	inheritedType int
)

insert #campusAreas
values
('Creativity & Tech Attendee', 0, 15),
('Creativity & Tech Volunteer', 2, 15),
('Fuse Attendee', 1, 17),
('Fuse Volunteer', 2, 15),
('Guest Services Attendee', 0, 15),
('Guest Services Volunteer', 2, 15),
('KidSpring Attendee', 0, null),
('KidSpring Volunteer', 2, null),
('Next Steps Attendee', 0, 15),
('Next Steps Volunteer', 2, 15)


/* ====================================================== */
-- campus kids structure
/* ====================================================== */
DECLARE @SpecialNeedsGroupId INT
DECLARE @SpecialNeedsGroupTypeId INT
SELECT @SpecialNeedsGroupTypeId = (
	SELECT [Id]
	FROM [GroupType]
	WHERE [Name] = 'Check in By Special Needs'	
);

select @SpecialNeedsGroupId = Id from [Attribute] where [Key] = 'IsSpecialNeeds'
if @SpecialNeedsGroupId is null or @SpecialNeedsGroupId = ''
begin

	INSERT [Attribute] ( [IsSystem],[FieldTypeId],[EntityTypeId],[EntityTypeQualifierColumn],[EntityTypeQualifierValue],[Key],[Name],[Description],[Order],[IsGridColumn],[DefaultValue],[IsMultiValue],[IsRequired],[Guid]) 
	VALUES ( @IsSystem, @BooleanFieldTypeId, @GroupEntityTypeId, 'GroupTypeId', @SpecialNeedsGroupTypeId, 'IsSpecialNeeds', 'Is Special Needs',
		'Indicates if this group caters to those who have special needs.', @False, @False, 'True', @False, @False, NEWID()
	);

	SET @SpecialNeedsGroupId = SCOPE_IDENTITY()
end

if object_id('tempdb..#subCampusAreas') is not null
begin
	drop table #subCampusAreas
end
create table #subCampusAreas (
	ID int IDENTITY(1,1),
	name nvarchar(255),
	parentName nvarchar(255),
	inheritedType int
)

-- Check-in Area, GroupType, Inherited Type
insert #subCampusAreas
values
('Nursery', 'KidSpring Attendee', 15),
('Preschool', 'KidSpring Attendee', 15),
('Elementary', 'KidSpring Attendee', 17),
('Special Needs', 'KidSpring Attendee', @SpecialNeedsGroupTypeId),
('Nursery Vols', 'KidSpring Volunteer', 15),
('Preschool Vols', 'KidSpring Volunteer', 15),
('Elementary Vols', 'KidSpring Volunteer', 15),
('Special Needs Vols', 'KidSpring Volunteer', 15),
('KS Support Vols', 'KidSpring Volunteer', 15),
('KS Production Vols', 'KidSpring Volunteer', 15)

/* ====================================================== */
-- campus group structure
/* ====================================================== */
if object_id('tempdb..#campusGroups') is not null
begin
	drop table #campusGroups
end
create table #campusGroups (
	ID int IDENTITY(1,1),
	groupTypeName nvarchar(255),
	groupName nvarchar(255),
	locationName nvarchar(255),
)

-- GroupType, Group, Location
insert #campusGroups
values
-- kid structure from AND
('Elementary', 'Base Camp', 'Base Camp'),
('Elementary', 'ImagiNation - 1st', 'ImagiNation'),
('Elementary', 'ImagiNation - K', 'ImagiNation'),
('Elementary', 'Jump Street - 2nd', 'Jump Street'),
('Elementary', 'Jump Street - 3rd', 'Jump Street'),
('Elementary', 'Shockwave - 4th', 'Shockwave'),
('Elementary', 'Shockwave - 5th', 'Shockwave'),
('Nursery', 'Wonder Way - 1', 'Wonder Way - 1'),
('Nursery', 'Wonder Way - 2', 'Wonder Way - 2'),
('Nursery', 'Wonder Way - 3', 'Wonder Way - 3'),
('Nursery', 'Wonder Way - 4', 'Wonder Way - 4'),
('Nursery', 'Wonder Way - 5', 'Wonder Way - 5'),
('Nursery', 'Wonder Way - 6', 'Wonder Way - 6'),
('Nursery', 'Wonder Way - 7', 'Wonder Way - 7'),
('Nursery', 'Wonder Way - 8', 'Wonder Way - 8'),
('Preschool', 'Base Camp Jr.', 'Base Camp Jr.'),
('Preschool', 'Fire Station', 'Fire Station'),
('Preschool', 'Lil'' Spring', 'Lil'' Spring'),
('Preschool', 'SpringTown Police', 'SpringTown Police'),
('Preschool', 'Pop''s Garage', 'Pop''s Garage'),
('Preschool', 'Spring Fresh', 'Spring Fresh'),
('Preschool', 'SpringTown Toys', 'SpringTown Toys'),
('Preschool', 'Treehouse', 'Treehouse'),
('Special Needs', 'Spring Zone Jr.', 'Spring Zone Jr.'),
('Special Needs', 'Spring Zone', 'Spring Zone'),

-- vol structure from COL
('Creativity & Tech Attendee', 'Choir', 'Creativity & Tech Attendee'),
('Creativity & Tech Volunteer', 'Band', 'Creativity & Tech Volunteer'),
('Creativity & Tech Volunteer', 'Band Green Room', 'Creativity & Tech Volunteer'),
('Creativity & Tech Volunteer', 'IT Team', 'Creativity & Tech Volunteer'),
('Creativity & Tech Volunteer', 'Load In/Load Out', 'Creativity & Tech Volunteer'),
('Creativity & Tech Volunteer', 'New Serve Team', 'Creativity & Tech Volunteer'),
('Creativity & Tech Volunteer', 'Office Team', 'Creativity & Tech Volunteer'),
('Creativity & Tech Volunteer', 'Production Team', 'Creativity & Tech Volunteer'),
('Creativity & Tech Volunteer', 'Social Media/PR Team', 'Creativity & Tech Volunteer'),
('Creativity & Tech Attendee', 'Special Event Attendee', 'Creativity & Tech Attendee'),
('Creativity & Tech Volunteer', 'Special Event Volunteer', 'Creativity & Tech Volunteer'),
('Elementary Vols', 'Base Camp Volunteer', 'Base Camp'),
('Elementary Vols', 'Base Camp Team Leader', 'Base Camp'),
('Elementary Vols', 'Early Bird Volunteer', 'Elementary Volunteer'),
('Elementary Vols', 'Elementary Service Leader', 'Elementary Volunteer'),
('Elementary Vols', 'Elementary Area Leader', 'Elementary Volunteer'),
('Elementary Vols', 'ImagiNation Volunteer', 'ImagiNation'),
('Elementary Vols', 'ImagiNation Team Leader', 'ImagiNation'),
('Elementary Vols', 'Jump Street Volunteer', 'Jump Street'),
('Elementary Vols', 'Jump Street Team Leader', 'Jump Street'),
('Elementary Vols', 'Shockwave Volunteer', 'Shockwave'),
('Elementary Vols', 'Shockwave Team Leader', 'Shockwave'),
('Fuse Attendee', '10th Grade Student', 'Fuse Attendee'),
('Fuse Attendee', '11th Grade Student', 'Fuse Attendee'),
('Fuse Attendee', '12th Grade Student', 'Fuse Attendee'),
('Fuse Attendee', '6th Grade Student', 'Fuse Attendee'),
('Fuse Attendee', '7th Grade Student', 'Fuse Attendee'),
('Fuse Attendee', '8th Grade Student', 'Fuse Attendee'),
('Fuse Attendee', '9th Grade Student', 'Fuse Attendee'),
('Fuse Attendee', 'Special Event Attendee', 'Fuse Attendee'),
('Fuse Volunteer', 'Atrium', 'Fuse Volunteer'),
('Fuse Volunteer', 'Campus Safety', 'Fuse Volunteer'),
('Fuse Volunteer', 'Care', 'Fuse Volunteer'),
('Fuse Volunteer', 'Check-In', 'Fuse Volunteer'),
('Fuse Volunteer', 'Fuse Group Leader', 'Fuse Volunteer'),
('Fuse Volunteer', 'Fuse Guest', 'Fuse Volunteer'),
('Fuse Volunteer', 'Game Room', 'Fuse Volunteer'),
('Fuse Volunteer', 'Greeter', 'Fuse Volunteer'),
('Fuse Volunteer', 'Jump Off', 'Fuse Volunteer'),
('Fuse Volunteer', 'Leadership Team', 'Fuse Volunteer'),
('Fuse Volunteer', 'Load In/Load Out', 'Fuse Volunteer'),
('Fuse Volunteer', 'Lounge', 'Fuse Volunteer'),
('Fuse Volunteer', 'New Serve', 'Fuse Volunteer'),
('Fuse Volunteer', 'Next Steps', 'Fuse Volunteer'),
('Fuse Volunteer', 'Office Team', 'Fuse Volunteer'),
('Fuse Volunteer', 'Parking', 'Fuse Volunteer'),
('Fuse Volunteer', 'Pick-Up', 'Fuse Volunteer'),
('Fuse Volunteer', 'Production', 'Fuse Volunteer'),
('Fuse Volunteer', 'Snack Bar', 'Fuse Volunteer'),
('Fuse Volunteer', 'Special Event Volunteer', 'Fuse Volunteer'),
('Fuse Volunteer', 'Sports', 'Fuse Volunteer'),
('Fuse Volunteer', 'Spring Zone', 'Fuse Volunteer'),
('Fuse Volunteer', 'Student Leader', 'Fuse Volunteer'),
('Fuse Volunteer', 'Sunday Fuse Team', 'Fuse Volunteer'),
('Fuse Volunteer', 'Usher', 'Fuse Volunteer'),
('Fuse Volunteer', 'VHQ', 'Fuse Volunteer'),
('Fuse Volunteer', 'Worship', 'Fuse Volunteer'),
('Guest Services Attendee', 'VIP Room Attendee', 'Guest Services Attendee'),
('Guest Services Attendee', 'Special Event Attendee', 'Guest Services Attendee'),
('Guest Services Attendee', 'Auditorium Reset Team', 'Guest Services Attendee'),
('Guest Services Attendee', 'Awake Team', 'Guest Services Attendee'),
('Guest Services Attendee', 'Facility Cleaning Crew', 'Guest Services Attendee'),
('Guest Services Attendee', 'Greeting Team', 'Guest Services Attendee'),
('Guest Services Attendee', 'Load In/Load Out', 'Guest Services Attendee'),
('Guest Services Attendee', 'Office Team', 'Guest Services Attendee'),
('Guest Services Attendee', 'Parking Team', 'Guest Services Attendee'),
('Guest Services Attendee', 'VHQ Team', 'Guest Services Attendee'),
('Guest Services Volunteer', 'Campus Safety', 'Guest Services Volunteer'),
('Guest Services Volunteer', 'Facilities Volunteer', 'Guest Services Volunteer'),
('Guest Services Volunteer', 'Finance Team', 'Guest Services Volunteer'),
('Guest Services Volunteer', 'VIP Room Volunteer', 'Guest Services Volunteer'),
('Guest Services Volunteer', 'Guest Services Team', 'Guest Services Volunteer'),
('Guest Services Volunteer', 'New Serve Team', 'Guest Services Volunteer'),
('Guest Services Volunteer', 'Service Coordinator', 'Guest Services Volunteer'),
('Guest Services Volunteer', 'Sign Language Team', 'Guest Services Volunteer'),
('Guest Services Volunteer', 'Special Event Volunteer', 'Guest Services Volunteer'),
('Guest Services Volunteer', 'Usher Team', 'Guest Services Volunteer'),
('KS Production Vols', 'Elementary Actor', 'Production Volunteer'),
('KS Production Vols', 'Elementary Media', 'Production Volunteer'),
('KS Production Vols', 'Elementary Production Area Leader', 'Production Volunteer'),
('KS Production Vols', 'Elementary Production Service Leader', 'Production Volunteer'),
('KS Production Vols', 'Elementary Worship Leader', 'Production Volunteer'),
('KS Production Vols', 'Preschool Actor', 'Production Volunteer'),
('KS Production Vols', 'Preschool Media', 'Production Volunteer'),
('KS Production Vols', 'Preschool Production Area Leader', 'Production Volunteer'),
('KS Production Vols', 'Preschool Production Service Leader', 'Production Volunteer'),
('KS Production Vols', 'Production Area Leader', 'Production Volunteer'),
('KS Production Vols', 'Production Service Leader', 'Production Volunteer'),
('KS Support Vols', 'Advocate', 'Support Volunteer'),
('KS Support Vols', 'Advocate Team Leader', 'Support Volunteer'),
('KS Support Vols', 'KidSpring Area Leader', 'Support Volunteer'),
('KS Support Vols', 'Check-In Volunteer', 'Support Volunteer'),
('KS Support Vols', 'Check-In Team Leader', 'Support Volunteer'),
('KS Support Vols', 'First Time Team Volunteer', 'Support Volunteer'),
('KS Support Vols', 'First Time Team Leader', 'Support Volunteer'),
('KS Support Vols', 'Greeter', 'Support Volunteer'),
('KS Support Vols', 'Greeter Team Leader', 'Support Volunteer'),
('KS Support Vols', 'Guest Services Service Leader', 'Support Volunteer'),
('KS Support Vols', 'Guest Services Area Leader', 'Support Volunteer'),
('KS Support Vols', 'Office Team', 'Support Volunteer'),
('KS Support Vols', 'Load In/Load Out', 'Support Volunteer'),
('KS Support Vols', 'New Serve Volunteer', 'Support Volunteer'),
('KS Support Vols', 'New Serve Area Leader', 'Support Volunteer'),
('KS Support Vols', 'New Serve Service Leader', 'Support Volunteer'),
('KS Support Vols', 'Trainer', 'Support Volunteer'),
('Next Steps Attendee', '90 DTC Participant', 'Next Steps Attendee'),
('Next Steps Attendee', 'Baptism Attendee', 'Next Steps Attendee'),
('Next Steps Attendee', 'Budget Class Attendee', 'Next Steps Attendee'),
('Next Steps Attendee', 'Connect Care Participant', 'Next Steps Attendee'),
('Next Steps Attendee', 'Connect Event Attendee', 'Next Steps Attendee'),
('Next Steps Attendee', 'Connect Group Participant', 'Next Steps Attendee'),
('Next Steps Attendee', 'Creativity & Tech Basics', 'Next Steps Attendee'),
('Next Steps Attendee', 'Creativity & Tech First Look', 'Next Steps Attendee'),
('Next Steps Attendee', 'Creativity & Tech First Serve', 'Next Steps Attendee'),
('Next Steps Attendee', 'Financial Coaching Attendee', 'Next Steps Attendee'),
('Next Steps Attendee', 'Fuse Basics', 'Next Steps Attendee'),
('Next Steps Attendee', 'Fuse First Look', 'Next Steps Attendee'),
('Next Steps Attendee', 'Fuse First Serve', 'Next Steps Attendee'),
('Next Steps Attendee', 'Guest Services Basics', 'Next Steps Attendee'),
('Next Steps Attendee', 'Guest Services First Look', 'Next Steps Attendee'),
('Next Steps Attendee', 'Guest Services First Serve', 'Next Steps Attendee'),
('Next Steps Attendee', 'KidSpring Basics', 'Next Steps Attendee'),
('Next Steps Attendee', 'KidSpring First Look', 'Next Steps Attendee'),
('Next Steps Attendee', 'KidSpring First Serve', 'Next Steps Attendee'),
('Next Steps Attendee', 'New Serve Participant', 'Next Steps Attendee'),
('Next Steps Attendee', 'Next Steps Basics', 'Next Steps Attendee'),
('Next Steps Attendee', 'Next Steps First Look', 'Next Steps Attendee'),
('Next Steps Attendee', 'Next Steps First Serve', 'Next Steps Attendee'),
('Next Steps Attendee', 'Opportunities Tour', 'Next Steps Attendee'),
('Next Steps Attendee', 'Ownership Class Attendee', 'Next Steps Attendee'),
('Next Steps Attendee', 'Ownership Class Current Owner', 'Next Steps Attendee'),
('Next Steps Attendee', 'Special Event Attendee', 'Next Steps Attendee'),
('Next Steps Attendee', 'Welcome & Wanted Participant', 'Next Steps Attendee'),
('Next Steps Volunteer', 'Baptism Volunteer', 'Next Steps Volunteer'),
('Next Steps Volunteer', 'Budget Class Volunteer', 'Next Steps Volunteer'),
('Next Steps Volunteer', 'Care Office Team', 'Next Steps Volunteer'),
('Next Steps Volunteer', 'Care Visitation Team', 'Next Steps Volunteer'),
('Next Steps Volunteer', 'District Leader', 'Next Steps Volunteer'),
('Next Steps Volunteer', 'Events Office Team', 'Next Steps Volunteer'),
('Next Steps Volunteer', 'Financial Coaching Volunteer', 'Next Steps Volunteer'),
('Next Steps Volunteer', 'Financial Planning Office Team', 'Next Steps Volunteer'),
('Next Steps Volunteer', 'Group Leader', 'Next Steps Volunteer'),
('Next Steps Volunteer', 'Group Training', 'Next Steps Volunteer'),
('Next Steps Volunteer', 'Groups Connector', 'Next Steps Volunteer'),
('Next Steps Volunteer', 'Groups Office Team', 'Next Steps Volunteer'),
('Next Steps Volunteer', 'Load In/Load Out', 'Next Steps Volunteer'),
('Next Steps Volunteer', 'New Serve Team', 'Next Steps Volunteer'),
('Next Steps Volunteer', 'Next Steps Area', 'Next Steps Volunteer'),
('Next Steps Volunteer', 'Ownership Class Volunteer', 'Next Steps Volunteer'),
('Next Steps Volunteer', 'Prayer Team', 'Next Steps Volunteer'),
('Next Steps Volunteer', 'Resource Center', 'Next Steps Volunteer'),
('Next Steps Volunteer', 'Special Event Volunteer', 'Next Steps Volunteer'),
('Next Steps Volunteer', 'Sunday Care Team', 'Next Steps Volunteer'),
('Next Steps Volunteer', 'Writing Team', 'Next Steps Volunteer'),
('Nursery Vols', 'Early Bird Volunteer', 'Nursery Volunteer'),
('Nursery Vols', 'Wonder Way Service Leader', 'Nursery Volunteer'),
('Nursery Vols', 'Wonder Way Area Leader', 'Nursery Volunteer'),
('Nursery Vols', 'Wonder Way 1 Volunteer', 'Wonder Way - 1'),
('Nursery Vols', 'Wonder Way 2 Volunteer', 'Wonder Way - 2'),
('Nursery Vols', 'Wonder Way 3 Volunteer', 'Wonder Way - 3'),
('Nursery Vols', 'Wonder Way 4 Volunteer', 'Wonder Way - 4'),
('Nursery Vols', 'Wonder Way 5 Volunteer', 'Wonder Way - 5'),
('Nursery Vols', 'Wonder Way 6 Volunteer', 'Wonder Way - 6'),
('Nursery Vols', 'Wonder Way 7 Volunteer', 'Wonder Way - 7'),
('Nursery Vols', 'Wonder Way 8 Volunteer', 'Wonder Way - 8'),
('Nursery Vols', 'Wonder Way 1 Team Leader', 'Wonder Way - 1'),
('Nursery Vols', 'Wonder Way 2 Team Leader', 'Wonder Way - 2'),
('Nursery Vols', 'Wonder Way 3 Team Leader', 'Wonder Way - 3'),
('Nursery Vols', 'Wonder Way 4 Team Leader', 'Wonder Way - 4'),
('Nursery Vols', 'Wonder Way 5 Team Leader', 'Wonder Way - 5'),
('Nursery Vols', 'Wonder Way 6 Team Leader', 'Wonder Way - 6'),
('Nursery Vols', 'Wonder Way 7 Team Leader', 'Wonder Way - 7'),
('Nursery Vols', 'Wonder Way 8 Team Leader', 'Wonder Way - 8'),
('Preschool Vols', 'Base Camp Jr. Volunteer', 'Base Camp Jr.'),
('Preschool Vols', 'Fire Station Volunteer', 'Fire Station'),
('Preschool Vols', 'Fire Station Team Leader', 'Fire Station'),
('Preschool Vols', 'Lil'' Spring Volunteer', 'Lil'' Spring'),
('Preschool Vols', 'Lil'' Spring Team Leader', 'Lil'' Spring'),
('Preschool Vols', 'Police Volunteer', 'SpringTown Police'),
('Preschool Vols', 'Police Team Leader', 'SpringTown Police'),
('Preschool Vols', 'Pop''s Garage Volunteer', 'Pop''s Garage'),
('Preschool Vols', 'Pop''s Garage Team Leader', 'Pop''s Garage'),
('Preschool Vols', 'Early Bird Volunteer', 'Preschool Volunteer'),
('Preschool Vols', 'Preschool Service Leader', 'Preschool Volunteer'),
('Preschool Vols', 'Preschool Area Leader', 'Preschool Volunteer'),
('Preschool Vols', 'Spring Fresh Volunteer', 'Spring Fresh'),
('Preschool Vols', 'Spring Fresh Team Leader', 'Spring Fresh'),
('Preschool Vols', 'Toys Volunteer', 'SpringTown Toys'),
('Preschool Vols', 'Toys Team Leader', 'SpringTown Toys'),
('Preschool Vols', 'Treehouse Volunteer', 'Treehouse'),
('Preschool Vols', 'Treehouse Team Leader', 'Treehouse'),
('Special Needs Vols', 'Spring Zone Jr. Volunteer', 'Spring Zone Jr.'),
('Special Needs Vols', 'Spring Zone Service Leader', 'Spring Zone'),
('Special Needs Vols', 'Spring Zone Area Leader', 'Spring Zone'),
('Special Needs Vols', 'Spring Zone Volunteer', 'Spring Zone')

/* ====================================================== */
-- central grouptypes
/* ====================================================== */
if object_id('tempdb..#centralAreas') is not null
begin
	drop table #centralAreas
end
create table #centralAreas (
	ID int IDENTITY(1,1),
	name nvarchar(255),
	attendanceRule int,
	inheritedType int
)

insert #centralAreas
values
('Creativity & Tech Volunteer', 2, 15),
('Events', 2, 15),
('Fuse Volunteer', 2, 15),
('Guest Services Volunteer', 2, 15),
('KidSpring Volunteer', 2, 15),
('Next Steps Volunteer', 2, 15)

/* ====================================================== */
-- central group structure
/* ====================================================== */
if object_id('tempdb..#centralGroups') is not null
begin
	drop table #centralGroups
end
create table #centralGroups (
	ID int IDENTITY(1,1),
	groupTypeName nvarchar(255),
	groupName nvarchar(255),
	locationName nvarchar(255),
)

-- GroupType, Group, Location
insert #centralGroups
values
('Creativity & Tech Volunteer', 'Design Team', 'Creativity & Tech Volunteer'),
('Creativity & Tech Volunteer', 'IT Team', 'Creativity & Tech Volunteer'),
('Creativity & Tech Volunteer', 'NewSpring Store Team', 'Creativity & Tech Volunteer'),
('Creativity & Tech Volunteer', 'Social Media/PR Team', 'Creativity & Tech Volunteer'),
('Creativity & Tech Volunteer', 'Video Production Team', 'Creativity & Tech Volunteer'),
('Creativity & Tech Volunteer', 'Web Dev Team', 'Creativity & Tech Volunteer'),
('Events', 'Event Attendee', 'Events'),
('Events', 'Event Volunteer', 'Events'),
('Fuse Volunteer', 'Fuse Office Team', 'Fuse Volunteer'),
('Fuse Volunteer', 'Special Event Attendee', 'Fuse Volunteer'),
('Fuse Volunteer', 'Special Event Volunteer', 'Fuse Volunteer'),
('Guest Services Volunteer', 'Events Team', 'Guest Services Volunteer'),
('Guest Services Volunteer', 'Finance Office Team', 'Guest Services Volunteer'),
('Guest Services Volunteer', 'GS Office Team', 'Guest Services Volunteer'),
('Guest Services Volunteer', 'HR Team', 'Guest Services Volunteer'),
('Guest Services Volunteer', 'Receptionist', 'Guest Services Volunteer'),
('Guest Services Volunteer', 'Special Event Attendee', 'Guest Services Volunteer'),
('Guest Services Volunteer', 'Special Event Volunteer', 'Guest Services Volunteer'),
('KidSpring Volunteer', 'KS Office Team', 'KidSpring Volunteer'),
('Next Steps Volunteer', 'Groups Office Team', 'Next Steps Volunteer'),
('Next Steps Volunteer', 'NS Office Team', 'Next Steps Volunteer'),
('Next Steps Volunteer', 'Writing Team', 'Next Steps Volunteer')


/* ====================================================== */
-- college grouptype
/* ====================================================== */
declare @collegeArea nvarchar(255) = 'NewSpring College', 
	@collegeLocation nvarchar(255) = 'Class Attendee',
	@collegeAttendance int = 2, @collegeInheritedType int = null

/* ====================================================== */
-- college group structure
/* ====================================================== */
if object_id('tempdb..#collegeGroups') is not null
begin
	drop table #collegeGroups
end
create table #collegeGroups (
	ID int IDENTITY(1,1),
	groupTypeName nvarchar(255),
	groupName nvarchar(255),
	locationName nvarchar(255),
)

-- GroupType, Group, Location
insert #collegeGroups
values
('NewSpring College', 'Acts', 'Class Attendee'),
('NewSpring College', 'All-Staff', 'Class Attendee'),
('NewSpring College', 'Bible', 'Class Attendee'),
('NewSpring College', 'Builders & Shepherds', 'Class Attendee'),
('NewSpring College', 'Character Forum', 'Class Attendee'),
('NewSpring College', 'Christian Beliefs I', 'Class Attendee'),
('NewSpring College', 'Christian Beliefs II', 'Class Attendee'),
('NewSpring College', 'Communication', 'Class Attendee'),
('NewSpring College', 'Ephesians', 'Class Attendee'),
('NewSpring College', 'Leadership Forum', 'Class Attendee'),
('NewSpring College', 'Leadership I', 'Class Attendee'),
('NewSpring College', 'Small Group', 'Class Attendee'),
('NewSpring College', 'Working Group', 'Class Attendee')

/* ====================================================== */
-- delete existing areas
/* ====================================================== */

delete from location
where id in (
	select distinct locationId
	from grouplocation gl
	inner join [group] g
	on gl.groupid = g.id
	and g.GroupTypeId in (14, 18, 19, 20, 21, 22)
)

delete from GroupTypeAssociation
where GroupTypeId in (14, 18, 19, 20, 21, 22)
or ChildGroupTypeId in (14, 18, 19, 20, 21, 22)

delete from [Group]
where GroupTypeId in (14, 18, 19, 20, 21, 22)

delete from GroupType
where id in (14, 18, 19, 20, 21, 22)


/* ====================================================== */
-- set up initial values
/* ====================================================== */

declare @campusId int, @numCampuses int, @campusAreaId int, @defaultRoleId int,
	@typePurpose int, @campusLocationId int, @campusGroupId int
select @typePurpose = 142  /* check-in template purpose type */
select @campusId = min(Id) from Campus
select @numCampuses = count(1) + @campusId from Campus where IsActive = @True

declare @campusName nvarchar(30), @campusCode nvarchar(5)

/* ====================================================== */
-- insert campus levels
/* ====================================================== */
while @campusId <= @numCampuses
begin

	select @campusName = '', @campusAreaId = 0
	select @campusName = name, @campusCode = ShortCode
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

		select @campusAreaId = SCOPE_IDENTITY()

		/* ====================================================== */
		-- set default grouptype role
		/* ====================================================== */
		insert GroupTypeRole (isSystem, GroupTypeId, Name, [Order], IsLeader,
			[Guid], CanView, CanEdit)
		values (@isSystem, @campusAreaId, 'Member', 0, 0, NEWID(), 0, 0)

		select @defaultRoleId = SCOPE_IDENTITY()

		update grouptype
		set DefaultGroupRoleId = @defaultRoleId
		where id = @campusAreaId

		/* ====================================================== */
		-- create matching group
		/* ====================================================== */
		insert [Group] (IsSystem, ParentGroupId, GroupTypeId, CampusId, Name,
			Description, IsSecurityRole, IsActive, [Order], [Guid])
		select @isSystem, NULL, @campusAreaId, @campusId, @campusName,
			@campusName + ' Group', 0, 1, 0, NEWID()

		select @campusGroupId = SCOPE_IDENTITY()

		/* ====================================================== */
		-- insert top areas
		/* ====================================================== */
		declare @scopeIndex int, @numItems int, @currentAreaId int,
			@attendanceRule int, @inheritedTypeId int, @areaLocationId int
		declare @areaName nvarchar(255), @areaLocation nvarchar(255)
		declare @volunteer nvarchar(255) = 'Volunteer'
		declare @attendee nvarchar(255) = 'Attendee'

		select @scopeIndex = min(Id) from #campusAreas
		select @numItems = count(1) + @scopeIndex from #campusAreas

		while @scopeIndex <= @numItems
		begin

			select @areaName = '', @areaLocationId = NULL, @areaLocation = '', @currentAreaId = 0, @defaultRoleId = 0
			select @areaName = name, @attendanceRule = attendanceRule, @inheritedTypeId = inheritedType
			from #campusAreas where id = @scopeIndex

			declare @msg nvarchar(500)
			select @msg = 'Starting ' + @campusName + ' - ' + @areaName
			RAISERROR ( @msg, 0, 0 ) WITH NOWAIT

			if @areaName <> ''
			begin

				/* ====================================================== */
				-- insert top area hierarchy
				/* ====================================================== */
				insert grouptype (IsSystem, Name, Description, GroupTerm, GroupMemberTerm,
					DefaultGroupRoleId, AllowMultipleLocations, ShowInGroupList,
					ShowInNavigation, TakesAttendance, AttendanceRule, AttendancePrintTo,
					[Order], InheritedGroupTypeId, LocationSelectionMode, GroupTypePurposeValueId, [Guid],
					AllowedScheduleTypes, SendAttendanceReminder)
				select 0, @campusCode + @delimiter + @areaName, @campusCode + @delimiter + @areaName + ' GroupType', 'Group', 'Member', NULL,
					1, 1, 1, 1, @attendanceRule, 0, 0, @inheritedTypeId, 0, NULL, NEWID(), 0, 0

				select @currentAreaId = SCOPE_IDENTITY()

				insert GroupTypeAssociation
				values (@campusAreaId, @currentAreaId)

				-- allow children of this grouptype
				insert GroupTypeAssociation
				values (@currentAreaId, @currentAreaId)

				/* ====================================================== */
				-- set default grouptype role
				/* ====================================================== */
				insert GroupTypeRole (isSystem, GroupTypeId, Name, [Order], IsLeader,
					[Guid], CanView, CanEdit)
				values (@isSystem, @currentAreaId, 'Member', 0, 0, NEWID(), 0, 0)

				select @defaultRoleId = SCOPE_IDENTITY()

				update grouptype
				set DefaultGroupRoleId = @defaultRoleId
				where id = @currentAreaId

				/* ====================================================== */
				-- create matching group
				/* ====================================================== */
				insert [Group] (IsSystem, ParentGroupId, GroupTypeId, CampusId, Name,
					Description, IsSecurityRole, IsActive, [Order], [Guid])
				select @isSystem, @campusGroupId, @currentAreaId, @campusId, @areaName,
					@campusCode + @delimiter + @areaName + ' Group', 0, 1, 0, NEWID()

				-- set up child location
				insert location (ParentLocationId, Name, IsActive, [Guid])
				select @campusLocationId, @areaName, 1, NEWID()				
			end
			--end if area not empty

			set @scopeIndex = @scopeIndex + 1
		end
		-- end top area grouptypes


		/* ====================================================== */
		-- set tri level grouptypes
		/* ====================================================== */
		declare @parentArea nvarchar(255), @parentAreaId int, @parentGroupId int
		select @scopeIndex = min(Id) from #subCampusAreas
		select @numItems = @scopeIndex + count(1) from #subCampusAreas

		while @scopeIndex <= @numItems
		begin

			select @areaName = '', @currentAreaId = 0
			select @areaName = name, @parentArea = parentName, @inheritedTypeId = inheritedType
			from #subCampusAreas where id = @scopeIndex

			if @areaName <> ''
			begin

				select @parentAreaId = Id from GroupType where name = @campusCode + @delimiter + @parentArea

				insert grouptype (IsSystem, Name, Description, GroupTerm, GroupMemberTerm,
					DefaultGroupRoleId, AllowMultipleLocations, ShowInGroupList,
					ShowInNavigation, TakesAttendance, AttendanceRule, AttendancePrintTo,
					[Order], InheritedGroupTypeId, LocationSelectionMode, GroupTypePurposeValueId, [Guid],
					AllowedScheduleTypes, SendAttendanceReminder)
				select 0, @campusCode + @delimiter + @areaName, @campusCode + @delimiter + @areaName + ' GroupType', 'Group', 'Member', NULL,
					1, 1, 1, 1, 0, 0, 0, @inheritedTypeId, 0, NULL, NEWID(), 0, 0

				select @currentAreaId = SCOPE_IDENTITY()

				insert GroupTypeAssociation
				values (@parentAreaId, @currentAreaId)

				-- allow children of this grouptype
				insert GroupTypeAssociation
				values (@currentAreaId, @currentAreaId)

				/* ====================================================== */
				-- set default grouptype role
				/* ====================================================== */
				insert grouptypeRole (isSystem, GroupTypeId, Name, [Order], IsLeader,
					[Guid], CanView, CanEdit)
				values (@isSystem, @currentAreaId, 'Member', 0, 0, NEWID(), 0, 0)

				select @defaultRoleId = SCOPE_IDENTITY()

				update grouptype
				set DefaultGroupRoleId = @defaultRoleId
				where id = @currentAreaId

				/* ====================================================== */
				-- create matching group
				/* ====================================================== */
				select @parentGroupId = Id from [group]
				where name = @parentArea
				and ParentGroupId = @campusGroupId

				insert [Group] (IsSystem, ParentGroupId, GroupTypeId, CampusId, Name,
					Description, IsSecurityRole, IsActive, [Order], [Guid])
				select @isSystem, @parentGroupId, @currentAreaId, @campusId,  @areaName,
					@areaName + @delimiter + 'Group', 0, 1, 10, NEWID()

			end
			--end if area not empty

			set @scopeIndex = @scopeIndex + 1
		end
		-- end kid level grouptypes


		/* ====================================================== */
		-- set group structure
		/* ====================================================== */
		declare @groupName nvarchar(255), @groupTypeName nvarchar(255), @locationName nvarchar(255)
		declare @locationId int, @parentLocationId int, @groupTypeId int, @parentGroupTypeId int, @groupId int
		select @scopeIndex = min(Id) from #campusGroups
		select @numItems = @scopeIndex + count(1) from #campusGroups

		while @scopeIndex <= @numItems
		begin

			select @groupName = null, @groupTypeName = null, @locationName = null, @locationId = null
			select @groupName = groupName, @groupTypeName = groupTypeName, @locationName = locationName
			from #campusGroups where id = @scopeIndex

			if @groupName is not null
			begin
				-- get child and parent group
				select @groupTypeId = Id from grouptype
				where name = @campusCode + @delimiter + @groupTypeName

				select @parentGroupId = Id from [group]
				where name = @groupTypeName
				and grouptypeId = @groupTypeId

				-- insert child level group
				insert [Group] (IsSystem, ParentGroupId, GroupTypeId, CampusId, Name,
					Description, IsSecurityRole, IsActive, [Order], [Guid])
				select @isSystem, @parentGroupId, @groupTypeId, @campusId, @groupName,
					@campusCode + @delimiter + @groupName + ' Group', 0, 1, 0, NEWID()

				select @groupid = SCOPE_IDENTITY()

				-- insert location for group

				;with locationChildren as (
					select l.id, l.parentLocationId, l.name
						from location l
						where id = @campusLocationId
					union all
					select l2.id, l2.parentlocationId, l2.name
						from location l2
						inner join locationChildren lc
						on lc.id = l2.ParentLocationId
				)
				select @locationId = id from locationChildren
				where name = @locationname

				if @locationId is null
				begin

					declare @parentLocationName nvarchar(255)
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
							from location l
							where id = @campusLocationId
						union all
						select l2.id, l2.parentlocationId, l2.name
							from location l2
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

/* ====================================================== */
-- Add IsSpecialNeeds attribute value to spring zone groups
/* ====================================================== */

INSERT [AttributeValue] ( [IsSystem],[AttributeId],[EntityId],[Value] ,[Guid] ) 
SELECT 0, @SpecialNeedsGroupId, g.[Id], 'True', NEWID()
FROM [Group] g
JOIN [Group] parent ON g.ParentGroupId = parent.Id
JOIN [GroupType] parentGt ON parent.GroupTypeId = parentGt.Id
WHERE g.Name = 'Spring Zone' or g.Name = 'Spring Zone Jr.'


/* ====================================================== */
-- Add Central separately from campuses since groups and
-- grouptypes are vastly different
/* ====================================================== */

-- inform progress
RAISERROR ( 'Starting Central grouptypes & groups', 0, 0 ) WITH NOWAIT

SELECT @campusCode = 'CEN', @campusName = 'Central', @campusId = 0,
	@campusLocationId = 0, @defaultRoleId = 0, @campusGroupId = 0

--insert Campus (IsSystem, Name, ShortCode, [Guid], IsActive)
--values (@isSystem, @campusName, 'CEN', NEWID(), 1)
--select @campusId = SCOPE_IDENTITY()

select @campusId = Id from Campus where name = @campusName

insert location (ParentLocationId, Name, IsActive, [Guid])
select NULL, @campusName, 1, NEWID()

set @campusLocationId = SCOPE_IDENTITY()

update campus set LocationId = @campusLocationId 
where id = @campusId

/* ====================================================== */
-- create initial central area
/* ====================================================== */
insert grouptype (IsSystem, Name, Description, GroupTerm, GroupMemberTerm,
	DefaultGroupRoleId, AllowMultipleLocations, ShowInGroupList,
	ShowInNavigation, TakesAttendance, AttendanceRule, AttendancePrintTo,
	[Order], InheritedGroupTypeId, LocationSelectionMode, GroupTypePurposeValueId, [Guid],
	AllowedScheduleTypes, SendAttendanceReminder)
select 0, @campusName, @campusName + ' Campus', 'Group', 'Member',
	NULL, 0, 1, 1, 0, 0, 1, 0, NULL, 0, 142, NEWID(), 0, 0

select @campusAreaId = SCOPE_IDENTITY()

insert GroupTypeRole (isSystem, GroupTypeId, Name, [Order], IsLeader,
	[Guid], CanView, CanEdit)
values (@isSystem, @campusAreaId, 'Member', 0, 0, NEWID(), 0, 0)

select @defaultRoleId = SCOPE_IDENTITY()

update grouptype
set DefaultGroupRoleId = @defaultRoleId
where id = @campusAreaId

/* ====================================================== */
-- create central group
/* ====================================================== */
insert [Group] (IsSystem, ParentGroupId, GroupTypeId, CampusId, Name,
	Description, IsSecurityRole, IsActive, [Order], [Guid])
select @isSystem, NULL, @campusAreaId, @campusId, @campusName,
	@campusName + ' Group', 0, 1, 0, NEWID()

select @campusGroupId = SCOPE_IDENTITY()

/* ====================================================== */
-- create central grouptypes
/* ====================================================== */
select @scopeIndex = min(Id) from #centralAreas
select @numItems = @scopeIndex + count(1) from #centralAreas
while @scopeIndex <= @numItems
begin
	
	select @areaName = '', @areaLocation = '', @currentAreaId = 0, @defaultRoleId = 0, @areaLocationId = 0
	select @areaName = name, @attendanceRule = attendanceRule, @inheritedTypeId = inheritedType
	from #centralAreas where id = @scopeIndex

	if @areaName <> ''
	begin
		/* ====================================================== */
		-- insert central hierarchy
		/* ====================================================== */
		insert grouptype (IsSystem, Name, Description, GroupTerm, GroupMemberTerm,
			DefaultGroupRoleId, AllowMultipleLocations, ShowInGroupList,
			ShowInNavigation, TakesAttendance, AttendanceRule, AttendancePrintTo,
			[Order], InheritedGroupTypeId, LocationSelectionMode, GroupTypePurposeValueId, [Guid],
			AllowedScheduleTypes, SendAttendanceReminder)
		select 0, @campusCode + @delimiter + @areaName, @campusCode + @delimiter + @areaName + ' GroupType', 'Group', 'Member', NULL,
			1, 1, 1, 1, @attendanceRule, 0, 0, @inheritedTypeId, 0, NULL, NEWID(), 0, 0

		select @currentAreaId = SCOPE_IDENTITY()

		insert GroupTypeAssociation
		values (@campusAreaId, @currentAreaId)

		-- allow children of this grouptype
		insert GroupTypeAssociation
		values (@currentAreaId, @currentAreaId)

		/* ====================================================== */
		-- set default grouptype role
		/* ====================================================== */
		insert GroupTypeRole (isSystem, GroupTypeId, Name, [Order], IsLeader,
			[Guid], CanView, CanEdit)
		values (@isSystem, @currentAreaId, 'Member', 0, 0, NEWID(), 0, 0)

		select @defaultRoleId = SCOPE_IDENTITY()

		update grouptype
		set DefaultGroupRoleId = @defaultRoleId
		where id = @currentAreaId

		/* ====================================================== */
		-- create matching group
		/* ====================================================== */
		declare @areaGroupId int
		insert [Group] (IsSystem, ParentGroupId, GroupTypeId, CampusId, Name,
			Description, IsSecurityRole, IsActive, [Order], [Guid])
		select @isSystem, @campusGroupId, @currentAreaId, @campusId, @areaName,
			@campusCode + @delimiter + @areaName + ' Group', 0, 1, 0, NEWID()

		select @areaGroupId = SCOPE_IDENTITY()

		-- set up child location
		insert location (ParentLocationId, Name, IsActive, [Guid])
		select @campusLocationId, @areaName, 1, NEWID()

		select @areaLocationId = SCOPE_IDENTITY()

		-- create a subset of groups for this grouptype
		if object_id('tempdb..#childGroups') is not null
		begin
			drop table #childGroups
		end
		create table #childGroups (
			ID int IDENTITY(1,1),
			groupTypeName nvarchar(255),
			groupName nvarchar(255),
			locationName nvarchar(255),
		)

		insert #childGroups
		select groupTypeName, groupName, locationName
		from #centralGroups
		where groupTypeName = @areaName

		-- count the groups that match this grouptype
		declare @childIndex int, @childItems int
		select @childIndex = min(Id) from #childGroups
		select @childItems = @childIndex + count(1) from #childGroups
		
		while @childIndex < @childItems
		begin
			declare @childGroupType nvarchar(255), @childGroup nvarchar(255), @childLocation nvarchar(255)			
			select @childGroupType = groupTypeName, @childGroup = groupName, @childLocation = locationName
			from #childGroups where id = @childIndex

			if @childGroup <> ''
			begin

				declare @childGroupId int = 0

				/* ====================================================== */
				-- create child group
				/* ====================================================== */
				insert [Group] (IsSystem, ParentGroupId, GroupTypeId, CampusId, Name,
					Description, IsSecurityRole, IsActive, [Order], [Guid])
				select @isSystem, @areaGroupid, @currentAreaId, @campusId,  @childGroup,
					@childGroup + @delimiter + 'Group', 0, 1, 10, NEWID()

				select @childGroupId = SCOPE_IDENTITY()

				/* ====================================================== */
				-- set group location
				/* ====================================================== */
				insert grouplocation (groupid, locationid, IsMailingLocation, IsMappedLocation, [Guid])
				values (@childGroupId, @areaLocationId, 0, 0, NEWID())
			end
			-- end childGroup not empty

			set @childIndex = @childIndex + 1
		end
		-- end child groups		
	end
	--end if area not empty

	set @scopeIndex = @scopeIndex + 1
end


/* ====================================================== */
-- Add College separately from campuses and central
/* ====================================================== */
-- inform progress
RAISERROR ( 'Starting NewSpring College grouptypes & groups', 0, 0 ) WITH NOWAIT

SELECT @campusName = @collegeArea, @campusAreaId = 0, 
	@defaultRoleId = 0, @campusGroupId = 0

-- NOTE: @campusCode and @campusId set to Central

/* ====================================================== */
-- create initial college area
/* ====================================================== */
insert grouptype (IsSystem, Name, Description, GroupTerm, GroupMemberTerm,
	DefaultGroupRoleId, AllowMultipleLocations, ShowInGroupList,
	ShowInNavigation, TakesAttendance, AttendanceRule, AttendancePrintTo,
	[Order], InheritedGroupTypeId, LocationSelectionMode, GroupTypePurposeValueId, [Guid],
	AllowedScheduleTypes, SendAttendanceReminder)
select 0, @campusName, @campusName, 'Group', 'Member',
	NULL, 0, 1, 1, 0, 0, 1, 0, NULL, 0, 142, NEWID(), 0, 0

select @campusAreaId = SCOPE_IDENTITY()

insert GroupTypeRole (isSystem, GroupTypeId, Name, [Order], IsLeader,
	[Guid], CanView, CanEdit)
values (@isSystem, @campusAreaId, 'Member', 0, 0, NEWID(), 0, 0)

select @defaultRoleId = SCOPE_IDENTITY()

update grouptype
set DefaultGroupRoleId = @defaultRoleId
where id = @campusAreaId

/* ====================================================== */
-- create college group
/* ====================================================== */
--insert [Group] (IsSystem, ParentGroupId, GroupTypeId, CampusId, Name,
--	Description, IsSecurityRole, IsActive, [Order], [Guid])
--select @isSystem, NULL, @campusAreaId, @campusId, @campusName,
--	@campusName + ' Group', 0, 1, 0, NEWID()

--select @campusGroupId = SCOPE_IDENTITY()

/* ====================================================== */
-- create college grouptype
/* ====================================================== */

select @areaName = @collegeArea, 
	@attendanceRule = @collegeAttendance, 
	@inheritedTypeId = @collegeInheritedType

insert grouptype (IsSystem, Name, Description, GroupTerm, GroupMemberTerm,
	DefaultGroupRoleId, AllowMultipleLocations, ShowInGroupList,
	ShowInNavigation, TakesAttendance, AttendanceRule, AttendancePrintTo,
	[Order], InheritedGroupTypeId, LocationSelectionMode, GroupTypePurposeValueId, [Guid],
	AllowedScheduleTypes, SendAttendanceReminder)
select 0, @areaName, @areaName + ' GroupType', 'Group', 'Member', NULL,
	1, 1, 1, 1, @attendanceRule, 0, 0, @inheritedTypeId, 0, NULL, NEWID(), 0, 0 

select @currentAreaId = SCOPE_IDENTITY()

insert GroupTypeAssociation
values (@campusAreaId, @currentAreaId)

-- allow children of this grouptype
insert GroupTypeAssociation
values (@currentAreaId, @currentAreaId)

/* ====================================================== */
-- set default grouptype role
/* ====================================================== */
insert GroupTypeRole (isSystem, GroupTypeId, Name, [Order], IsLeader,
	[Guid], CanView, CanEdit)
values (@isSystem, @currentAreaId, 'Member', 0, 0, NEWID(), 0, 0)

select @defaultRoleId = SCOPE_IDENTITY()

update grouptype
set DefaultGroupRoleId = @defaultRoleId
where id = @currentAreaId

/* ====================================================== */
-- create top-level college group and location
/* ====================================================== */
select @areaGroupId = 0, @areaLocationid = 0, @locationId = 0
insert [Group] (IsSystem, ParentGroupId, GroupTypeId, CampusId, Name,
	Description, IsSecurityRole, IsActive, [Order], [Guid])
select @isSystem, NULL, @currentAreaId, @campusId, @areaName,
	@areaName + ' Group', 0, 1, 0, NEWID()

select @areaGroupId = SCOPE_IDENTITY()

-- set up location hierarchy
insert location (ParentLocationId, Name, IsActive, [Guid])
select @campusLocationId, @areaName, 1, NEWID()

select @areaLocationId = SCOPE_IDENTITY()

insert location (ParentLocationId, Name, IsActive, [Guid])
select @areaLocationId, @collegeLocation, 1, NEWID()

select @locationId = SCOPE_IDENTITY()

/* ====================================================== */
-- create college groups
/* ====================================================== */
select @scopeIndex = 0, @numItems = 0
select @scopeIndex = min(Id) from #collegeGroups
select @numItems = @scopeIndex + count(1) from #collegeGroups
		
while @scopeIndex < @numItems
begin
	select @groupName = '', @groupTypeName = '', @groupId = 0
	select @groupName = groupName, @groupTypeName = groupTypeName
	from #collegeGroups where id = @scopeIndex

	if @groupName <> ''
	begin
		
		/* ====================================================== */
		-- create child group
		/* ====================================================== */
		insert [Group] (IsSystem, ParentGroupId, GroupTypeId, CampusId, Name,
			Description, IsSecurityRole, IsActive, [Order], [Guid])
		select @isSystem, @areaGroupid, @currentAreaId, @campusId,  @groupName,
			@groupName + 'Group', 0, 1, 10, NEWID()

		select @groupId = SCOPE_IDENTITY()

		-- set group location
		insert grouplocation (groupid, locationid, IsMailingLocation, IsMappedLocation, [Guid])
		values (@groupId, @locationId, 0, 0, NEWID())
	end
	-- end childGroup not empty

	set @scopeIndex = @scopeIndex + 1
end
-- end child groups		


use master
go

-- end central areas


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
