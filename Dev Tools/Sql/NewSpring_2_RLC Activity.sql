/* ====================================================== */
-- NewSpring Script #2: 
-- Inserts RLC, activity ministries and activity groups.

-- Make sure you're using the right Rock database:

USE [Rock]

/* ====================================================== */

-- Set the F1 database name
DECLARE @F1 varchar(255) = 'newspring'


/* ====================================================== */
-- testing 
select top 10000 * from newspring..Activity_Group
select top 10000 * from newspring..Activity_Schedule where Activity_ID = 164134
select top 10000 * from newspring..ActivityMinistry

select * from newspring..ActivityMinistry where Activity_ID = 164134
select activity_id, min(activity_start_time), max(activity_end_time), count(1)
from newspring..Activity_Schedule 
group by Activity_ID
having count(1) > 1
order by Activity_ID



/* ====================================================== */