/* ====================================================== */
-- NewSpring Script #0:
-- Blanks out non-core grouptypes and groups 

-- Make sure you're using the right Rock database:

USE Rock

/* ====================================================== */

-- Enable production mode for performance
--SET NOCOUNT ON

declare @True bit = 1
declare @False bit = 0

-- gather grouptypes to delete
if object_id('tempdb..#grouptypes') is not null
begin
	drop table #grouptypes
end
create table #grouptypes (
	ID int,
	areaName nvarchar(255)
)

insert #grouptypes
select ID, Name
from grouptype
where issystem = @False
and name not like 'Check in%'
and name not like 'Small Group%'

if object_id('tempdb..#groups') is not null
begin
	drop table #groups
end
create table #groups (
	ID int,
	name nvarchar(255)
)

insert #groups
select ID, Name
from [group]
where issystem = @False
and grouptypeid in (
	select id from #grouptypes
)

-- delete attendance
delete a
from attendance a
where a.groupid in (
	select id from #groups
)

-- delete group members
delete gm 
from groupmember gm
inner join [group] g
on gm.groupid = g.id
and g.id in (
	select id from #groups
)

-- delete locations (and group locations)
delete l
from location l
inner join grouplocation gl
on l.id = gl.locationid
and gl.groupid in (
	select id from #groups
)

-- delete groups
delete from [group]
where parentgroupid in  (
	select id from #groups
)

delete from [group]
where id in  (
	select id from #groups
)

-- delete from grouptypes
delete GroupTypeAssociation
where grouptypeid in (
	select id from #grouptypes
)

delete grouptype
where id in (
	select id from #grouptypes
)

use master