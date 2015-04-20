-- if schedule name is Base Schedule or 8:30%, ignore the schedule but convert assignment
-- ignore assignment that doesn't have RLC
-- ignore assignment that's not active


select count(1) from newspring..Staffing_Assignment 
where Is_Active = 0
