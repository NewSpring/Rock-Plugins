/*

This script is intended to be run within the SQL runner in the Rock Power Tools.  

It will set the Apollos sync settings since these are sensitive and should not be
stored in a public repo as a migration.

*/


-- These are the values that will be written to all of the sync workflow attributes
DECLARE @syncUrl AS NVARCHAR(MAX) = 'PUT ROOT APOLLOS URL HERE';
DECLARE @tokenValue AS NVARCHAR(MAX) = 'PUT TOKEN VALUE HERE';


-- ========================================================================= --


DECLARE @syncEntityName AS NVARCHAR(MAX) = 'cc.newspring.Apollos.Workflow.Action.APISync';
DECLARE @syncEntityId AS INT;

SELECT @syncEntityId = Id
FROM EntityType
WHERE Name = @syncEntityName;

DECLARE @categoryName AS NVARCHAR(MAX) = 'Apollos API Sync Category';
DECLARE @categoryId AS INT;

SELECT @categoryId = Id
FROM Category
WHERE Name = @categoryName;

-- Update sync URL
UPDATE AttributeValue
SET Value = @syncUrl
WHERE 
	EntityId IN (
		SELECT Id FROM WorkflowActionType WHERE ActivityTypeId IN (
			SELECT Id FROM WorkflowActivityType WHERE WorkflowTypeId IN (
				SELECT Id FROM WorkflowType WHERE CategoryId = @categoryId
			)
		)
	AND AttributeId IN (
		SELECT Id
		FROM Attribute
		WHERE
			EntityTypeQualifierColumn = 'EntityTypeId'
			AND EntityTypeQualifierValue = @syncEntityId
			AND Name = 'Sync URL'
	)
);

-- Update token value
UPDATE AttributeValue
SET Value = @tokenValue
WHERE 
	EntityId IN (
		SELECT Id FROM WorkflowActionType WHERE ActivityTypeId IN (
			SELECT Id FROM WorkflowActivityType WHERE WorkflowTypeId IN (
				SELECT Id FROM WorkflowType WHERE CategoryId = @categoryId
			)
		)
	AND AttributeId IN (
		SELECT Id
		FROM Attribute
		WHERE
			EntityTypeQualifierColumn = 'EntityTypeId'
			AND EntityTypeQualifierValue = @syncEntityId
			AND Name = 'Token Value'
	)
);