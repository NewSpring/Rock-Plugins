/* ====================================================== */
-- NewSpring Script #1: 
-- Inserts campuses, groups, grouptypes and locations.

-- Make sure you're using the right Rock database:

USE Rock

/* ====================================================== */

-- Enable production mode for performance
SET NOCOUNT ON

-- Set common variables 
declare @IsSystem bit = 0
declare @Delimiter nvarchar(5) = ' - '
declare @True bit = 1
declare @False bit = 0
declare @BooleanFieldTypeId int
declare @GroupEntityTypeId int
declare @CheckInAreaTypePurpose int
declare @CampusLocationTypeId int

select @BooleanFieldTypeId = [Id] FROM [FieldType] WHERE [Name] = 'Boolean'
select @GroupEntityTypeId = [Id] FROM [EntityType] WHERE [Name] = 'Rock.Model.Group'

/* Check-in Template Purpose Type */
select @CheckInAreaTypePurpose = 142

/* Location Type: Campus */
select @CampusLocationTypeId = [Id] from DefinedValue where [Guid] = 'C0D7AE35-7901-4396-870E-3AAF472AAE88'

update [group] set campusId = null
delete from Campus where id = 1

/* ====================================================== */
-- create campuses
/* ====================================================== */

insert Campus (IsSystem, Name, ShortCode, [Guid], IsActive)
values
(@IsSystem, 'Aiken', 'AKN', NEWID(), @True),
(@IsSystem, 'Anderson', 'AND', NEWID(), @True),
(@IsSystem, 'Boiling Springs', 'BSP', NEWID(), @True),
(@IsSystem, 'Central', 'CEN', NEWID(), @False),
(@IsSystem, 'Charleston', 'CHS', NEWID(), @True),
(@IsSystem, 'Clemson', 'CLE', NEWID(), @True),
(@IsSystem, 'Columbia', 'COL', NEWID(), @True),
(@IsSystem, 'Florence', 'FLO', NEWID(), @True),
(@IsSystem, 'Greenville', 'GVL', NEWID(), @True),
(@IsSystem, 'Greenwood', 'GWD', NEWID(), @True),
(@IsSystem, 'Greer', 'GRR', NEWID(), @True),
(@IsSystem, 'Hilton Head', 'HHD', NEWID(), @True),
(@IsSystem, 'Lexington', 'LEX', NEWID(), @True),
(@IsSystem, 'Myrtle Beach', 'MYR', NEWID(), @True),
(@IsSystem, 'Northeast Columbia', 'NEC', NEWID(), @True),
(@IsSystem, 'Powdersville', 'POW', NEWID(), @True),
(@IsSystem, 'Rock Hill', 'RKH', NEWID(), @True),
(@IsSystem, 'Simpsonville', 'SIM', NEWID(), @True),
(@IsSystem, 'Spartanburg', 'SPA', NEWID(), @True),
(@IsSystem, 'Sumter', 'SUM', NEWID(), @True),
(@IsSystem, 'Web', 'WEB', NEWID(), @False)

-- create top-level campus locations
insert Location (Name, IsActive, LocationTypeValueId, [Guid])
select name, @True, @CampusLocationTypeId, NEWID() from Campus


/* ====================================================== */
-- Special Needs Structure
/* ====================================================== */
DECLARE @SpecialNeedsAttributeId INT
DECLARE @SpecialNeedsGroupTypeId INT
SELECT @SpecialNeedsGroupTypeId = (
	SELECT [Id]
	FROM [GroupType]
	WHERE [Name] = 'Check in By Special Needs'	
);

select @SpecialNeedsAttributeId = Id from [Attribute] where [Key] = 'IsSpecialNeeds'
if @SpecialNeedsAttributeId is null or @SpecialNeedsAttributeId = ''
begin

	INSERT [Attribute] ( [IsSystem],[FieldTypeId],[EntityTypeId],[EntityTypeQualifierColumn],[EntityTypeQualifierValue],[Key],[Name],[Description],[Order],[IsGridColumn],[DefaultValue],[IsMultiValue],[IsRequired],[Guid]) 
	VALUES ( @IsSystem, @BooleanFieldTypeId, @GroupEntityTypeId, 'GroupTypeId', @SpecialNeedsGroupTypeId, 'IsSpecialNeeds', 'Is Special Needs',
		'Indicates if this group caters to those who have special needs.', @False, @False, 'True', @False, @False, NEWID()
	);

	SET @SpecialNeedsAttributeId = SCOPE_IDENTITY()
end


/* ====================================================== */
-- base check-in areas
/* ====================================================== */
if object_id('tempdb..#topAreas') is not null
begin
	drop table #topAreas
end
create table #topAreas (
	ID int IDENTITY(1,1),
	parentArea nvarchar(255),
	childArea nvarchar(255),
	inheritedType int
)

-- Check-in Area, GroupType, Inherited Type
insert #topAreas
values
('Creativity & Technology', 'Creativity & Tech Attendee', 15),
('Creativity & Technology', 'Creativity & Tech Volunteer', 15),
('Events', 'Event Attendee', 15),
('Events', 'Event Volunteer', 15),
('Fuse', 'Fuse Attendee', 17),
('Fuse', 'Fuse Volunteer', 15),
('Guest Services', 'Guest Services Attendee', 15),
('Guest Services', 'Guest Services Volunteer', 15),
('KidSpring', 'Nursery Attendee', 15),
('KidSpring', 'Preschool Attendee', 15),
('KidSpring', 'Elementary Attendee', 17),
('KidSpring', 'Special Needs Attendee', @SpecialNeedsGroupTypeId),
('KidSpring', 'Nursery Volunteer', 15),
('KidSpring', 'Preschool Volunteer', 15),
('KidSpring', 'Elementary Volunteer', 15),
('KidSpring', 'Special Needs Volunteer', 15),
('KidSpring', 'Support Volunteer', 15),
('KidSpring', 'Production Volunteer', 15),
('Next Steps', 'Next Steps Attendee', 15),
('Next Steps', 'Next Steps Volunteer', 15)

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
-- child attendee structure
('Elementary Attendee', 'Base Camp', 'Base Camp'), 
('Elementary Attendee', 'ImagiNation - 1st', 'ImagiNation - 1st'), 
('Elementary Attendee', 'ImagiNation - K', 'ImagiNation - K'), 
('Elementary Attendee', 'Jump Street - 2nd', 'Jump Street - 2nd'), 
('Elementary Attendee', 'Jump Street - 3rd', 'Jump Street - 3rd'), 
('Elementary Attendee', 'Shockwave - 4th', 'Shockwave - 4th'), 
('Elementary Attendee', 'Shockwave - 5th', 'Shockwave - 5th'), 
('Nursery Attendee', 'Wonder Way - 1', 'Wonder Way - 1'), 
('Nursery Attendee', 'Wonder Way - 2', 'Wonder Way - 2'), 
('Nursery Attendee', 'Wonder Way - 3', 'Wonder Way - 3'), 
('Nursery Attendee', 'Wonder Way - 4', 'Wonder Way - 4'), 
('Nursery Attendee', 'Wonder Way - 5', 'Wonder Way - 5'), 
('Nursery Attendee', 'Wonder Way - 6', 'Wonder Way - 6'), 
('Nursery Attendee', 'Wonder Way - 7', 'Wonder Way - 7'), 
('Nursery Attendee', 'Wonder Way - 8', 'Wonder Way - 8'), 
('Preschool Attendee', 'Base Camp Jr.', 'Base Camp Jr.'), 
('Preschool Attendee', 'Fire Station', 'Fire Station'), 
('Preschool Attendee', 'Lil'' Spring', 'Lil'' Spring'), 
('Preschool Attendee', 'SpringTown Police', 'SpringTown Police'), 
('Preschool Attendee', 'Pop''s Garage', 'Pop''s Garage'), 
('Preschool Attendee', 'Spring Fresh', 'Spring Fresh'), 
('Preschool Attendee', 'SpringTown Toys', 'SpringTown Toys'), 
('Preschool Attendee', 'Treehouse', 'Treehouse'), 
('Special Needs Attendee', 'Spring Zone Jr.', 'Spring Zone Jr.'), 
('Special Needs Attendee', 'Spring Zone', 'Spring Zone'), 

-- adult attendee/volunteer structure from COL
('Creativity & Tech Attendee', 'Choir', 'Choir'), 
('Creativity & Tech Attendee', 'Special Event Attendee', 'Special Event Attendee'), 
('Creativity & Tech Volunteer', 'Band', 'Band'), 
('Creativity & Tech Volunteer', 'Band Green Room', 'Band Green Room'), 
('Creativity & Tech Volunteer', 'IT Team', 'IT Team'), 
('Creativity & Tech Volunteer', 'Load In/Load Out', 'Load In/Load Out'), 
('Creativity & Tech Volunteer', 'New Serve Team', 'New Serve Team'), 
('Creativity & Tech Volunteer', 'Office Team', 'Office Team'), 
('Creativity & Tech Volunteer', 'Production Team', 'Production Team'), 
('Creativity & Tech Volunteer', 'Social Media/PR Team', 'Social Media/PR Team'), 
('Creativity & Tech Volunteer', 'Special Event Volunteer', 'Special Event Volunteer'), 
('Elementary Volunteer', 'Base Camp Volunteer', 'Base Camp Volunteer'), 
('Elementary Volunteer', 'Base Camp Team Leader', 'Base Camp Team Leader'), 
('Elementary Volunteer', 'Early Bird Volunteer', 'Early Bird Volunteer'), 
('Elementary Volunteer', 'Elementary Service Leader', 'Elementary Service Leader'), 
('Elementary Volunteer', 'Elementary Area Leader', 'Elementary Area Leader'), 
('Elementary Volunteer', 'ImagiNation Volunteer', 'ImagiNation Volunteer'), 
('Elementary Volunteer', 'ImagiNation Team Leader', 'ImagiNation Team Leader'), 
('Elementary Volunteer', 'Jump Street Volunteer', 'Jump Street Volunteer'), 
('Elementary Volunteer', 'Jump Street Team Leader', 'Jump Street Team Leader'), 
('Elementary Volunteer', 'Shockwave Volunteer', 'Shockwave Volunteer'), 
('Elementary Volunteer', 'Shockwave Team Leader', 'Shockwave Team Leader'), 
('Fuse Attendee', '10th Grade Student', '10th Grade Student'), 
('Fuse Attendee', '11th Grade Student', '11th Grade Student'), 
('Fuse Attendee', '12th Grade Student', '12th Grade Student'), 
('Fuse Attendee', '6th Grade Student', '6th Grade Student'), 
('Fuse Attendee', '7th Grade Student', '7th Grade Student'), 
('Fuse Attendee', '8th Grade Student', '8th Grade Student'), 
('Fuse Attendee', '9th Grade Student', '9th Grade Student'), 
('Fuse Attendee', 'Special Event Attendee', 'Special Event Attendee'), 
('Fuse Volunteer', 'Atrium', 'Atrium'), 
('Fuse Volunteer', 'Campus Safety', 'Campus Safety'), 
('Fuse Volunteer', 'Care', 'Care'), 
('Fuse Volunteer', 'Check-In', 'Check-In'), 
('Fuse Volunteer', 'Fuse Group Leader', 'Fuse Group Leader'), 
('Fuse Volunteer', 'Fuse Guest', 'Fuse Guest'), 
('Fuse Volunteer', 'Game Room', 'Game Room'), 
('Fuse Volunteer', 'Greeter', 'Greeter'), 
('Fuse Volunteer', 'Jump Off', 'Jump Off'), 
('Fuse Volunteer', 'Leadership Team', 'Leadership Team'), 
('Fuse Volunteer', 'Load In/Load Out', 'Load In/Load Out'), 
('Fuse Volunteer', 'Lounge', 'Lounge'), 
('Fuse Volunteer', 'New Serve', 'New Serve'), 
('Fuse Volunteer', 'Next Steps', 'Next Steps'), 
('Fuse Volunteer', 'Office Team', 'Office Team'), 
('Fuse Volunteer', 'Parking', 'Parking'), 
('Fuse Volunteer', 'Pick-Up', 'Pick-Up'), 
('Fuse Volunteer', 'Production', 'Production'), 
('Fuse Volunteer', 'Snack Bar', 'Snack Bar'), 
('Fuse Volunteer', 'Special Event Volunteer', 'Special Event Volunteer'), 
('Fuse Volunteer', 'Sports', 'Sports'), 
('Fuse Volunteer', 'Spring Zone', 'Spring Zone'), 
('Fuse Volunteer', 'Student Leader', 'Student Leader'), 
('Fuse Volunteer', 'Sunday Fuse Team', 'Sunday Fuse Team'), 
('Fuse Volunteer', 'Usher', 'Usher'), 
('Fuse Volunteer', 'VHQ', 'VHQ'), 
('Fuse Volunteer', 'Worship', 'Worship'), 
('Guest Services Attendee', 'VIP Room Attendee', 'VIP Room Attendee'), 
('Guest Services Attendee', 'Special Event Attendee', 'Special Event Attendee'), 
('Guest Services Attendee', 'Auditorium Reset Team', 'Auditorium Reset Team'), 
('Guest Services Attendee', 'Awake Team', 'Awake Team'), 
('Guest Services Attendee', 'Facility Cleaning Crew', 'Facility Cleaning Crew'), 
('Guest Services Attendee', 'Greeting Team', 'Greeting Team'), 
('Guest Services Attendee', 'Load In/Load Out', 'Load In/Load Out'), 
('Guest Services Attendee', 'Office Team', 'Office Team'), 
('Guest Services Attendee', 'Parking Team', 'Parking Team'), 
('Guest Services Attendee', 'VHQ Team', 'VHQ Team'), 
('Guest Services Volunteer', 'Campus Safety', 'Campus Safety'), 
('Guest Services Volunteer', 'Facilities Volunteer', 'Facilities Volunteer'), 
('Guest Services Volunteer', 'Finance Team', 'Finance Team'), 
('Guest Services Volunteer', 'VIP Room Volunteer', 'VIP Room Volunteer'), 
('Guest Services Volunteer', 'Guest Services Team', 'Guest Services Team'), 
('Guest Services Volunteer', 'New Serve Team', 'New Serve Team'), 
('Guest Services Volunteer', 'Service Coordinator', 'Service Coordinator'), 
('Guest Services Volunteer', 'Sign Language Team', 'Sign Language Team'), 
('Guest Services Volunteer', 'Special Event Volunteer', 'Special Event Volunteer'), 
('Guest Services Volunteer', 'Usher Team', 'Usher Team'), 
('Production Volunteer', 'Elementary Actor', 'Elementary Actor'), 
('Production Volunteer', 'Elementary Media', 'Elementary Media'), 
('Production Volunteer', 'Elementary Production Area Leader', 'Elementary Production Area Leader'), 
('Production Volunteer', 'Elementary Production Service Leader', 'Elementary Production Service Leader'), 
('Production Volunteer', 'Elementary Worship Leader', 'Elementary Worship Leader'), 
('Production Volunteer', 'Preschool Actor', 'Preschool Actor'), 
('Production Volunteer', 'Preschool Media', 'Preschool Media'), 
('Production Volunteer', 'Preschool Production Area Leader', 'Preschool Production Area Leader'), 
('Production Volunteer', 'Preschool Production Service Leader', 'Preschool Production Service Leader'), 
('Production Volunteer', 'Production Area Leader', 'Production Area Leader'), 
('Production Volunteer', 'Production Service Leader', 'Production Service Leader'), 
('Support Volunteer', 'Advocate', 'Advocate'), 
('Support Volunteer', 'Advocate Team Leader', 'Advocate Team Leader'), 
('Support Volunteer', 'KidSpring Area Leader', 'KidSpring Area Leader'), 
('Support Volunteer', 'Check-In Volunteer', 'Check-In Volunteer'), 
('Support Volunteer', 'Check-In Team Leader', 'Check-In Team Leader'), 
('Support Volunteer', 'First Time Team Volunteer', 'First Time Team Volunteer'), 
('Support Volunteer', 'First Time Team Leader', 'First Time Team Leader'), 
('Support Volunteer', 'Greeter', 'Greeter'), 
('Support Volunteer', 'Greeter Team Leader', 'Greeter Team Leader'), 
('Support Volunteer', 'Guest Services Service Leader', 'Guest Services Service Leader'), 
('Support Volunteer', 'Guest Services Area Leader', 'Guest Services Area Leader'), 
('Support Volunteer', 'Office Team', 'Office Team'), 
('Support Volunteer', 'Load In/Load Out', 'Load In/Load Out'), 
('Support Volunteer', 'New Serve Volunteer', 'New Serve Volunteer'), 
('Support Volunteer', 'New Serve Area Leader', 'New Serve Area Leader'), 
('Support Volunteer', 'New Serve Service Leader', 'New Serve Service Leader'), 
('Support Volunteer', 'Trainer', 'Trainer'), 
('Next Steps Attendee', '90 DTC Participant', '90 DTC Participant'), 
('Next Steps Attendee', 'Baptism Attendee', 'Baptism Attendee'), 
('Next Steps Attendee', 'Budget Class Attendee', 'Budget Class Attendee'), 
('Next Steps Attendee', 'Connect Care Participant', 'Connect Care Participant'), 
('Next Steps Attendee', 'Connect Event Attendee', 'Connect Event Attendee'), 
('Next Steps Attendee', 'Connect Group Participant', 'Connect Group Participant'), 
('Next Steps Attendee', 'Creativity & Tech Basics', 'Creativity & Tech Basics'), 
('Next Steps Attendee', 'Creativity & Tech First Look', 'Creativity & Tech First Look'), 
('Next Steps Attendee', 'Creativity & Tech First Serve', 'Creativity & Tech First Serve'), 
('Next Steps Attendee', 'Financial Coaching Attendee', 'Financial Coaching Attendee'), 
('Next Steps Attendee', 'Fuse Basics', 'Fuse Basics'), 
('Next Steps Attendee', 'Fuse First Look', 'Fuse First Look'), 
('Next Steps Attendee', 'Fuse First Serve', 'Fuse First Serve'), 
('Next Steps Attendee', 'Guest Services Basics', 'Guest Services Basics'), 
('Next Steps Attendee', 'Guest Services First Look', 'Guest Services First Look'), 
('Next Steps Attendee', 'Guest Services First Serve', 'Guest Services First Serve'), 
('Next Steps Attendee', 'KidSpring Basics', 'KidSpring Basics'), 
('Next Steps Attendee', 'KidSpring First Look', 'KidSpring First Look'), 
('Next Steps Attendee', 'KidSpring First Serve', 'KidSpring First Serve'), 
('Next Steps Attendee', 'New Serve Participant', 'New Serve Participant'), 
('Next Steps Attendee', 'Next Steps Basics', 'Next Steps Basics'), 
('Next Steps Attendee', 'Next Steps First Look', 'Next Steps First Look'), 
('Next Steps Attendee', 'Next Steps First Serve', 'Next Steps First Serve'), 
('Next Steps Attendee', 'Opportunities Tour', 'Opportunities Tour'), 
('Next Steps Attendee', 'Ownership Class Attendee', 'Ownership Class Attendee'), 
('Next Steps Attendee', 'Ownership Class Current Owner', 'Ownership Class Current Owner'), 
('Next Steps Attendee', 'Special Event Attendee', 'Special Event Attendee'), 
('Next Steps Attendee', 'Welcome & Wanted Participant', 'Welcome & Wanted Participant'), 
('Next Steps Volunteer', 'Baptism Volunteer', 'Baptism Volunteer'), 
('Next Steps Volunteer', 'Budget Class Volunteer', 'Budget Class Volunteer'), 
('Next Steps Volunteer', 'Care Office Team', 'Care Office Team'), 
('Next Steps Volunteer', 'Care Visitation Team', 'Care Visitation Team'), 
('Next Steps Volunteer', 'District Leader', 'District Leader'), 
('Next Steps Volunteer', 'Events Office Team', 'Events Office Team'), 
('Next Steps Volunteer', 'Financial Coaching Volunteer', 'Financial Coaching Volunteer'), 
('Next Steps Volunteer', 'Financial Planning Office Team', 'Financial Planning Office Team'), 
('Next Steps Volunteer', 'Group Leader', 'Group Leader'), 
('Next Steps Volunteer', 'Group Training', 'Group Training'), 
('Next Steps Volunteer', 'Groups Connector', 'Groups Connector'), 
('Next Steps Volunteer', 'Groups Office Team', 'Groups Office Team'), 
('Next Steps Volunteer', 'Load In/Load Out', 'Load In/Load Out'), 
('Next Steps Volunteer', 'New Serve Team', 'New Serve Team'), 
('Next Steps Volunteer', 'Next Steps Area', 'Next Steps Area'), 
('Next Steps Volunteer', 'Ownership Class Volunteer', 'Ownership Class Volunteer'), 
('Next Steps Volunteer', 'Prayer Team', 'Prayer Team'), 
('Next Steps Volunteer', 'Resource Center', 'Resource Center'), 
('Next Steps Volunteer', 'Special Event Volunteer', 'Special Event Volunteer'), 
('Next Steps Volunteer', 'Sunday Care Team', 'Sunday Care Team'), 
('Next Steps Volunteer', 'Writing Team', 'Writing Team'), 
('Nursery Volunteer', 'Early Bird Volunteer', 'Early Bird Volunteer'), 
('Nursery Volunteer', 'Wonder Way Service Leader', 'Wonder Way Service Leader'), 
('Nursery Volunteer', 'Wonder Way Area Leader', 'Wonder Way Area Leader'), 
('Nursery Volunteer', 'Wonder Way 1 Volunteer', 'Wonder Way 1 Volunteer'), 
('Nursery Volunteer', 'Wonder Way 2 Volunteer', 'Wonder Way 2 Volunteer'), 
('Nursery Volunteer', 'Wonder Way 3 Volunteer', 'Wonder Way 3 Volunteer'), 
('Nursery Volunteer', 'Wonder Way 4 Volunteer', 'Wonder Way 4 Volunteer'), 
('Nursery Volunteer', 'Wonder Way 5 Volunteer', 'Wonder Way 5 Volunteer'), 
('Nursery Volunteer', 'Wonder Way 6 Volunteer', 'Wonder Way 6 Volunteer'), 
('Nursery Volunteer', 'Wonder Way 7 Volunteer', 'Wonder Way 7 Volunteer'), 
('Nursery Volunteer', 'Wonder Way 8 Volunteer', 'Wonder Way 8 Volunteer'), 
('Nursery Volunteer', 'Wonder Way 1 Team Leader', 'Wonder Way 1 Team Leader'), 
('Nursery Volunteer', 'Wonder Way 2 Team Leader', 'Wonder Way 2 Team Leader'), 
('Nursery Volunteer', 'Wonder Way 3 Team Leader', 'Wonder Way 3 Team Leader'), 
('Nursery Volunteer', 'Wonder Way 4 Team Leader', 'Wonder Way 4 Team Leader'), 
('Nursery Volunteer', 'Wonder Way 5 Team Leader', 'Wonder Way 5 Team Leader'), 
('Nursery Volunteer', 'Wonder Way 6 Team Leader', 'Wonder Way 6 Team Leader'), 
('Nursery Volunteer', 'Wonder Way 7 Team Leader', 'Wonder Way 7 Team Leader'), 
('Nursery Volunteer', 'Wonder Way 8 Team Leader', 'Wonder Way 8 Team Leader'), 
('Preschool Volunteer', 'Base Camp Jr. Volunteer', 'Base Camp Jr. Volunteer'), 
('Preschool Volunteer', 'Fire Station Volunteer', 'Fire Station Volunteer'), 
('Preschool Volunteer', 'Fire Station Team Leader', 'Fire Station Team Leader'), 
('Preschool Volunteer', 'Lil'' Spring Volunteer', 'Lil'' Spring Volunteer'), 
('Preschool Volunteer', 'Lil'' Spring Team Leader', 'Lil'' Spring Team Leader'), 
('Preschool Volunteer', 'Police Volunteer', 'Police Volunteer'), 
('Preschool Volunteer', 'Police Team Leader', 'Police Team Leader'), 
('Preschool Volunteer', 'Pop''s Garage Volunteer', 'Pop''s Garage Volunteer'), 
('Preschool Volunteer', 'Pop''s Garage Team Leader', 'Pop''s Garage Team Leader'), 
('Preschool Volunteer', 'Early Bird Volunteer', 'Early Bird Volunteer'), 
('Preschool Volunteer', 'Preschool Service Leader', 'Preschool Service Leader'), 
('Preschool Volunteer', 'Preschool Area Leader', 'Preschool Area Leader'), 
('Preschool Volunteer', 'Spring Fresh Volunteer', 'Spring Fresh Volunteer'), 
('Preschool Volunteer', 'Spring Fresh Team Leader', 'Spring Fresh Team Leader'), 
('Preschool Volunteer', 'Toys Volunteer', 'Toys Volunteer'), 
('Preschool Volunteer', 'Toys Team Leader', 'Toys Team Leader'), 
('Preschool Volunteer', 'Treehouse Volunteer', 'Treehouse Volunteer'), 
('Preschool Volunteer', 'Treehouse Team Leader', 'Treehouse Team Leader'), 
('Special Needs Volunteer', 'Spring Zone Jr. Volunteer', 'Spring Zone Jr. Volunteer'), 
('Special Needs Volunteer', 'Spring Zone Service Leader', 'Spring Zone Service Leader'), 
('Special Needs Volunteer', 'Spring Zone Area Leader', 'Spring Zone Area Leader'), 
('Special Needs Volunteer', 'Spring Zone Volunteer', 'Spring Zone Volunteer')

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
('Creativity & Tech Volunteer', 'Design Team', 'Design Team'), 
('Creativity & Tech Volunteer', 'IT Team', 'IT Team'), 
('Creativity & Tech Volunteer', 'NewSpring Store Team', 'NewSpring Store Team'), 
('Creativity & Tech Volunteer', 'Social Media/PR Team', 'Social Media/PR Team'), 
('Creativity & Tech Volunteer', 'Video Production Team', 'Video Production Team'), 
('Creativity & Tech Volunteer', 'Web Dev Team', 'Web Dev Team'), 
('Fuse Volunteer', 'Fuse Office Team', 'Fuse Office Team'), 
('Fuse Volunteer', 'Special Event Attendee', 'Special Event Attendee'), 
('Fuse Volunteer', 'Special Event Volunteer', 'Special Event Volunteer'), 
('Guest Services Volunteer', 'Events Team', 'Events Team'), 
('Guest Services Volunteer', 'Finance Office Team', 'Finance Office Team'), 
('Guest Services Volunteer', 'GS Office Team', 'GS Office Team'), 
('Guest Services Volunteer', 'HR Team', 'HR Team'), 
('Guest Services Volunteer', 'Receptionist', 'Receptionist'), 
('Guest Services Volunteer', 'Special Event Attendee', 'Special Event Attendee'), 
('Guest Services Volunteer', 'Special Event Volunteer', 'Special Event Volunteer'), 
('KidSpring Volunteer', 'KS Office Team', 'KS Office Team'), 
('Next Steps Volunteer', 'Groups Office Team', 'Groups Office Team'), 
('Next Steps Volunteer', 'NS Office Team', 'NS Office Team'), 
('Next Steps Volunteer', 'Writing Team', 'Writing Team')

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
('NewSpring College', 'Acts', 'Acts'), 
('NewSpring College', 'All-Staff', 'All-Staff'), 
('NewSpring College', 'Bible', 'Bible'), 
('NewSpring College', 'Builders & Shepherds', 'Builders & Shepherds'), 
('NewSpring College', 'Character Forum', 'Character Forum'), 
('NewSpring College', 'Christian Beliefs I', 'Christian Beliefs I'), 
('NewSpring College', 'Christian Beliefs II', 'Christian Beliefs II'), 
('NewSpring College', 'Communication', 'Communication'), 
('NewSpring College', 'Ephesians', 'Ephesians'), 
('NewSpring College', 'Leadership Forum', 'Leadership Forum'), 
('NewSpring College', 'Leadership I', 'Leadership I'), 
('NewSpring College', 'Small Group', 'Small Group'), 
('NewSpring College', 'Working Group', 'Working Group')

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
-- set up check-in config areas
/* ====================================================== */

declare @AreaName nvarchar(255), @AreaId int, @AreaLocation nvarchar(255), @AreaLocationId int, 
	@ParentAreaName nvarchar(255), @ParentAreaId int, @DefaultRoleId int, @AttendanceRule int, 
	@AreaGroupId int, @InheritedTypeId int

declare @scopeIndex int, @numItems int		
select @scopeIndex = min(Id) from #topAreas
select @numItems = count(1) + @scopeIndex from #topAreas

while @scopeIndex <= @numItems
begin
	select @AreaName = '', @AreaLocationId = NULL, @AreaLocation = '', @AreaId = 0, @DefaultRoleId = 0
	select @ParentAreaName = parentArea, @AreaName = childArea, @InheritedTypeId = inheritedType
	from #topAreas where id = @scopeIndex

	if @AreaName <> ''
	begin

		declare @msg nvarchar(500)
		select @msg = 'Creating ' + @ParentAreaName + ' / ' + @AreaName
		RAISERROR ( @msg, 0, 0 ) WITH NOWAIT

		/* ====================================================== */
		-- create the parent if it doesn't exist
		/* ====================================================== */
		select @ParentAreaId = [Id] from GroupType
		where name = @ParentAreaName and InheritedGroupTypeId = @InheritedTypeId
		
		if @ParentAreaId is null
		begin
			insert grouptype (IsSystem, Name, [Description], GroupTerm, GroupMemberTerm, AllowMultipleLocations, 
				ShowInGroupList, ShowInNavigation, TakesAttendance, AttendanceRule, AttendancePrintTo,
				[Order], InheritedGroupTypeId, LocationSelectionMode, GroupTypePurposeValueId, [Guid],
				AllowedScheduleTypes, SendAttendanceReminder)
			select @IsSystem, @ParentAreaName, @ParentAreaName + ' Area', 'Group', 'Member', @False, @False, @False, 
				@False, 0, 0, 0, NULL, 0, @CheckInAreaTypePurpose, NEWID(), 0, @False

			select @ParentAreaId = SCOPE_IDENTITY()

			-- allow children of this grouptype
			insert GroupTypeAssociation
			values (@ParentAreaId, @ParentAreaId)

			/* ====================================================== */
			-- set default grouptype role
			/* ====================================================== */
			insert GroupTypeRole (isSystem, GroupTypeId, Name, [Order], IsLeader,
				[Guid], CanView, CanEdit)
			values (@IsSystem, @ParentAreaId, 'Member', 0, 0, NEWID(), 0, 0)

			select @DefaultRoleId = SCOPE_IDENTITY()

			update grouptype set DefaultGroupRoleId = @DefaultRoleId where id = @ParentAreaId
		end

		/* ====================================================== */
		-- create the child grouptype
		/* ====================================================== */
		insert grouptype (IsSystem, Name, [Description], GroupTerm, GroupMemberTerm, AllowMultipleLocations, 
			ShowInGroupList, ShowInNavigation, TakesAttendance, AttendanceRule, AttendancePrintTo,
			[Order], InheritedGroupTypeId, LocationSelectionMode, GroupTypePurposeValueId, [Guid],
			AllowedScheduleTypes, SendAttendanceReminder)
		select @IsSystem, @AreaName, @AreaName + ' GroupType', 'Group', 'Member', @True, @True, @True, 
			@True, 0, 0, 0, @InheritedTypeId, 0, NULL, NEWID(), 0, @False

		select @AreaId = SCOPE_IDENTITY()

		-- allow children of this grouptype
		insert GroupTypeAssociation
		values (@AreaId, @AreaId)

		/* ====================================================== */
		-- set default grouptype role
		/* ====================================================== */
		insert GroupTypeRole (isSystem, GroupTypeId, Name, [Order], IsLeader,
			[Guid], CanView, CanEdit)
		values (@IsSystem, @AreaId, 'Member', 0, 0, NEWID(), 0, 0)

		select @DefaultRoleId = SCOPE_IDENTITY()

		update grouptype set DefaultGroupRoleId = @DefaultRoleId where id = @AreaId

		/* ====================================================== */
		-- create matching group
		/* ====================================================== */
		insert [Group] (IsSystem, ParentGroupId, GroupTypeId, Name,
			[Description], IsSecurityRole, IsActive, [Order], [Guid], [IsPublic])
		select @IsSystem, NULL, @AreaId, @AreaName, @AreaName + ' Group', @False, @True, 0, NEWID(), @True


		/* ====================================================== */
		-- create parent location under campus location
		/* ====================================================== */
		insert Location (ParentLocationId, Name, IsActive, [Guid])
		select c.LocationId, @ParentAreaName, 1, NEWID()
		from Campus c
		where c.IsActive = @True
		
	end
	-- end empty area name

	select @scopeIndex = @scopeIndex + 1
end
-- end check-in config areas

/* ====================================================== */
-- insert campus locations
/* ====================================================== */
declare @campusGroupId int, @numCampusGroups int
select @campusGroupId = min(Id) from #campusGroups
select @numCampusGroups = count(1) + @campusGroupId from #campusGroups

while @campusGroupId <= @numCampusGroups
begin
	
	declare @GroupTypeName nvarchar(255) = '', @GroupTypeId int = 0, @GroupTypeGroupId int = 0, 
		@GroupName nvarchar(255) = '', @GroupId int = 0, @LocationName nvarchar(255) = '',
		@ParentGroupTypeName nvarchar(255) = '', @ParentGroupTypeId int = 0, @ParentLocationId int = 0

	select @GroupTypeName = groupTypeName, @GroupName = groupName, @LocationName = locationName
	from #campusGroups

	/* ====================================================== */
	-- create grouptype if it doesn't exist
	/* ====================================================== */
	select @GroupTypeId = gt.[Id], @GroupTypeGroupId = g.[Id]
	from GroupType gt
	inner join [Group] g
	on gt.id = g.GroupTypeId
	and gt.name = @GroupTypeName
	and g.name = @GroupTypeName

	if @GroupTypeId is not null
	begin	

		/* ====================================================== */
		-- create child group if it doesn't exist
		/* ====================================================== */
		select @GroupId = [Id] from [Group]
		where GroupTypeId = @GroupTypeId
		and name = @GroupName
		
		if @GroupId is null
		begin
			
			insert [Group] (IsSystem, ParentGroupId, GroupTypeId, Name,
				[Description], IsSecurityRole, IsActive, [Order], [Guid], [IsPublic])
			select @IsSystem, NULL, @GroupTypeid, @GroupName, @GroupName + ' Group', @False, @True, 0, NEWID(), @True

			select @GroupId = SCOPE_IDENTITY()
		end

		/* ====================================================== */
		-- get parent grouptype name
		/* ====================================================== */
		select @ParentGroupTypeName = gt.[Name], @ParentGroupTypeId = gt.[Id]
		from GroupTypeAssociation gta
		inner join GroupType gt
		on gt.Id = gta.GroupTypeId
		and gta.ChildGroupTypeId = @GroupTypeId

		/* ====================================================== */
		-- insert campus level locations
		/* ====================================================== */
		insert Location (ParentLocationId, Name, IsActive, [Guid])
		select l.Id, @LocationName, 1, NEWID()
		from Location l
		inner join Location pl
		on l.ParentLocationId = pl.Id
		and pl.Name = @ParentGroupTypeName
		inner join Campus c
		on c.LocationId = pl.Id
		where c.IsActive = @True

		/* ====================================================== */
		-- insert group locations
		/* ====================================================== */
		insert GroupLocation (Groupid, LocationId, IsMailingLocation, IsMappedLocation, [Guid])
		select @GroupId, l.Id, @False, @False, NEWID()
		from Location l
		inner join Location cl
		on l.ParentLocationId = cl.Id
		inner join Location pl
		on cl.ParentLocationId = pl.Id
		and pl.Name = @ParentGroupTypeName

	end
	-- end grouptype not empty

	select @campusGroupId = @campusGroupid + 1
end
-- end campus groups


/* ====================================================== */
-- Add IsSpecialNeeds attribute value to spring zone groups
/* ====================================================== */

INSERT [AttributeValue] ( [IsSystem],[AttributeId],[EntityId],[Value] ,[Guid] ) 
SELECT 0, @SpecialNeedsAttributeId, g.[Id], 'True', NEWID()
FROM [Group] g
--JOIN [Group] parent ON g.ParentGroupId = parent.Id
--JOIN [GroupType] parentGt ON parent.GroupTypeId = parentGt.Id
WHERE g.Name = 'Spring Zone' or g.Name = 'Spring Zone Jr.'


/* ====================================================== */
-- Add Central separately from campuses since groups and
-- grouptypes are vastly different
/* ====================================================== */

-- inform progress
RAISERROR ( 'Starting Central grouptypes & groups', 0, 0 ) WITH NOWAIT

DECLARE @CampusCode nvarchar(255), @CampusId int = 0, @CampusLocationId int = 0

--SELECT @CampusCode = 'CEN', @CampusName = 'Central', @CampusId = 0,
--	@CampusLocationId = 0, @DefaultRoleId = 0, @CampusGroupId = 0

--insert Campus (IsSystem, Name, ShortCode, [Guid], IsActive)
--values (@IsSystem, @CampusName, @CampusCode, NEWID(), 1)

--select @CampusId = SCOPE_IDENTITY()

set @CampusLocationId = SCOPE_IDENTITY()

update campus set LocationId = @CampusLocationId 
where id = @CampusId


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
		select 0, @campusCode + @Delimiter + @areaName, @campusCode + @Delimiter + @areaName + ' GroupType', 'Group', 'Member', NULL,
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
		values (@IsSystem, @currentAreaId, 'Member', 0, 0, NEWID(), 0, 0)

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
		select @IsSystem, @campusGroupId, @currentAreaId, @campusId, @areaName,
			@campusCode + @Delimiter + @areaName + ' Group', 0, 1, 0, NEWID()

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
				select @IsSystem, @areaGroupid, @currentAreaId, @campusId,  @childGroup,
					@childGroup + ' Group', 0, 1, 10, NEWID()

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

-- NOTE: @campusCode and @campusId already set to Central

/* ====================================================== */
-- create initial college area
/* ====================================================== */
insert grouptype (IsSystem, Name, Description, GroupTerm, GroupMemberTerm,
	DefaultGroupRoleId, AllowMultipleLocations, ShowInGroupList,
	ShowInNavigation, TakesAttendance, AttendanceRule, AttendancePrintTo,
	[Order], InheritedGroupTypeId, LocationSelectionMode, GroupTypePurposeValueId, [Guid],
	AllowedScheduleTypes, SendAttendanceReminder)
select 0, @campusName, @campusName + ' Check-in Area', 'Group', 'Member',
	NULL, 0, 1, 1, 0, 0, 1, 0, NULL, 0, @AreaTypePurpose, NEWID(), 0, 0

select @campusAreaId = SCOPE_IDENTITY()

insert GroupTypeRole (isSystem, GroupTypeId, Name, [Order], IsLeader,
	[Guid], CanView, CanEdit)
values (@IsSystem, @campusAreaId, 'Member', 0, 0, NEWID(), 0, 0)

select @defaultRoleId = SCOPE_IDENTITY()

update grouptype
set DefaultGroupRoleId = @defaultRoleId
where id = @campusAreaId

/* ====================================================== */
-- create college group
/* ====================================================== */
insert [Group] (IsSystem, ParentGroupId, GroupTypeId, CampusId, Name,
	Description, IsSecurityRole, IsActive, [Order], [Guid])
select @IsSystem, NULL, @campusAreaId, @campusId, @campusName,
	@campusName + ' Group', 0, 1, 0, NEWID()

select @campusGroupId = SCOPE_IDENTITY()

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
values (@IsSystem, @currentAreaId, 'Member', 0, 0, NEWID(), 0, 0)

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
select @IsSystem, NULL, @currentAreaId, @campusId, @areaName,
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
		select @IsSystem, @areaGroupid, @currentAreaId, @campusId,  @groupName,
			@groupName + ' Group', 0, 1, 10, NEWID()

		select @groupId = SCOPE_IDENTITY()

		-- set group location
		insert grouplocation (groupid, locationid, IsMailingLocation, IsMappedLocation, [Guid])
		values (@groupId, @locationId, 0, 0, NEWID())
	end
	-- end childGroup not empty

	set @scopeIndex = @scopeIndex + 1
end
-- end child groups		

/* ====================================================== */
-- Insert a campus to track WEB
/* ====================================================== */

-- inform progress
RAISERROR ( 'Adding WEB as a campus', 0, 0 ) WITH NOWAIT

SELECT @campusCode = 'WEB', @campusName = 'Web', @campusId = 0,
	@campusLocationId = 0, @defaultRoleId = 0, @campusGroupId = 0

insert Campus (IsSystem, Name, ShortCode, [Guid], IsActive)
values (@IsSystem, @campusName, @campusCode, NEWID(), @True)
-- end WEB insert

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
