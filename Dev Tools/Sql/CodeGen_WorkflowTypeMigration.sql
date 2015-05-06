set nocount on
declare
@crlf varchar(2) = char(13) + char(10)

begin

IF OBJECT_ID('tempdb..#codeTable') IS NOT NULL
    DROP TABLE #codeTable

IF OBJECT_ID('tempdb..#knownGuidsToIgnore') IS NOT NULL
    DROP TABLE #knownGuidsToIgnore

create table #knownGuidsToIgnore(
    [Guid] UniqueIdentifier, 
    CONSTRAINT [pk_knownGuidsToIgnore] PRIMARY KEY CLUSTERED  ( [Guid]) 
);

-- Categories
insert into #knownGuidsToIgnore values 
('8F8B272D-D351-485E-86D6-3EE5B7C84D99')  --Checkin

-- Workflow Types
--insert into #knownGuidsToIgnore values 
--('011E9F5A-60D4-4FF5-912A-290881E37EAF'),  --Checkin
--('C93EEC26-4BE3-4EB5-92D4-5C30EEF069D9'),  --Parse Labels
--('221BF486-A82C-40A7-85B7-BB44DA45582F'),  --Person Data Error
--('236AB611-EDE8-42B5-B559-6B6A88ADDDCB'),  --External Inquiry
--('417D8016-92DC-4F25-ACFF-A071B591FA4F'),  --Facilities Request
--('51FE9641-FB8F-41BF-B09E-235900C3E53E'),  --IT Support
--('036F2F0B-C2DC-49D0-A17B-CCDAC7FC71E2'),  --Photo Request
--('655BE2A4-2735-4CF9-AEC8-7EF5BE92724C'),  --Position Approval
--('16D12EF7-C546-4039-9036-B73D118EDC90'),  --Background Check
--('885CBA61-44EA-4B4A-B6E1-289041B6A195'),  --DISC Request
--('C9D8F6A2-CE98-4DD5-8963-9403D02F9CC8'),  --Apollos UserLogin Delete Workflow Type
--('7CB0BE68-98B9-44FC-90E8-D78DD59DE3DC'),  --Apollos UserLogin Save Workflow Type
--('8F066EB1-95E9-43C7-81F7-D6A7CB27B90B'),  --Apollos Person Delete Workflow Type
--('C1EC88CB-7F52-4938-ADEF-C28959F38F96'),  --Apollos Person Save Workflow Type
--('6E8CD562-A1DA-4E13-A45C-853DB56E0014'),  --Attended Check-in
--('F3218F81-423F-46EA-B748-ED7AB365CD07'),  --Send A Text Message
--('C53337C8-7352-4159-8B8C-D0E4076FF96C')   --Request For Criminal Records

insert into #knownGuidsToIgnore values 
('011E9F5A-60D4-4FF5-912A-290881E37EAF'),
('C93EEC26-4BE3-4EB5-92D4-5C30EEF069D9'),
('221BF486-A82C-40A7-85B7-BB44DA45582F'),
('236AB611-EDE8-42B5-B559-6B6A88ADDDCB'),
('417D8016-92DC-4F25-ACFF-A071B591FA4F'),
('51FE9641-FB8F-41BF-B09E-235900C3E53E'),
('036F2F0B-C2DC-49D0-A17B-CCDAC7FC71E2'),
('655BE2A4-2735-4CF9-AEC8-7EF5BE92724C'),
('16D12EF7-C546-4039-9036-B73D118EDC90'),
('885CBA61-44EA-4B4A-B6E1-289041B6A195'),
('C9D8F6A2-CE98-4DD5-8963-9403D02F9CC8'),
('7CB0BE68-98B9-44FC-90E8-D78DD59DE3DC'),
('8F066EB1-95E9-43C7-81F7-D6A7CB27B90B'),
('C1EC88CB-7F52-4938-ADEF-C28959F38F96'),
('6E8CD562-A1DA-4E13-A45C-853DB56E0014'),
('3BCD6A54-2204-4404-B320-BB456F9ECA98'),
('08765EA3-2973-4AA8-BD1F-F8C0E86A4E44'),
('259D88F2-D1CD-48FC-802F-C519CDFEBACA'),
('638970FA-0261-4A98-BD86-7C110BB1B158'),
('E178756A-3968-4C22-AF62-41E9F42C693C'),
('D96FF580-362E-461F-9952-7EC885563C09'),
('8A28F344-A897-44E2-BFCA-D323FE04C2EA'),
('FBA8DB8E-06B1-4400-A486-C8EB0DB0467D'),
('B84E951A-8C10-4387-A96E-16A4CCAFB1E9'),
('AFACD892-A663-4DEE-B9CF-01DE6001308F'),
('9F1AAA8F-991D-4C6E-B2F1-1232672658A9'),
('92CA5A5A-3AF4-47FB-8BB3-5741B1F517BA'),
('509C62AF-05FC-4E3A-822E-0FFDFC82E965'),
('E94306BF-E623-48A4-9729-AAB5C2291C03'),
('D960BF6E-93CA-48B4-B31B-B4C354838641'),
('3DED7E24-0B8C-4CBC-A79F-1CDC8A8F4D7D'),
('F868FE91-9D54-43EA-873A-A1A3A5316EAE'),
('9CA0354B-02D1-41CA-805D-1ADDF6B83A6B'),
('D58B5E92-EA82-421B-931C-9CADAB31327E'),
('5F65B858-9433-4CED-BD61-B20CAFBB581E'),
('6F5BA05C-24EE-494A-83AB-6F9FE1312461'),
('410BB92C-5A06-4E26-B12E-C446C19B6299')



create table #codeTable (
    Id int identity(1,1) not null,
    CodeText nvarchar(max),
    CONSTRAINT [pk_codeTable] PRIMARY KEY CLUSTERED  ( [Id]) );
    
	-- field Types
	insert into #codeTable
	SELECT
        '            RockMigrationHelper.UpdateFieldType("'+    
        ft.Name+ '","'+ 
        ISNULL(ft.Description,'')+ '","'+ 
        ft.Assembly+ '","'+ 
        ft.Class+ '","'+ 
        CONVERT(nvarchar(50), ft.Guid)+ '");'+
        @crlf
    from [FieldType] [ft]
    where (ft.IsSystem = 0)
	
	-- entitiy types
    insert into #codeTable
    values 
		('            RockMigrationHelper.UpdateEntityType("Rock.Model.Workflow", "3540E9A7-FE30-43A9-8B0A-A372B63DFC93", true, true);' + @crlf ),
		('            RockMigrationHelper.UpdateEntityType("Rock.Model.WorkflowActivity", "2CB52ED0-CB06-4D62-9E2C-73B60AFA4C9F", true, true);' + @crlf ),
		('            RockMigrationHelper.UpdateEntityType("Rock.Model.WorkflowActionType", "23E3273A-B137-48A3-9AFF-C8DC832DDCA6", true, true);' + @crlf )

	-- Action entity types
    insert into #codeTable
    SELECT DISTINCT
        '            RockMigrationHelper.UpdateEntityType("'+
		[et].[name]+ '","'+   
        CONVERT(nvarchar(50), [et].[Guid])+ '",'+     
		(CASE [et].[IsEntity] WHEN 1 THEN 'true' ELSE 'false' END) + ','+
		(CASE [et].[IsSecured] WHEN 1 THEN 'true' ELSE 'false' END) + ');' +
        @crlf
    from [WorkflowActionType] [a]
	inner join [WorkflowActivityType] [at] on [a].[ActivityTypeId] = [at].[id]
	inner join [WorkflowType] [wt] on [at].[WorkflowTypeId] = [wt].[id]
	inner join [EntityType] [et] on [et].[Id] = [a].[EntityTypeId]
    where [wt].[Guid] not in (select [Guid] from #knownGuidsToIgnore)

    insert into #codeTable
    SELECT @crlf

	-- Action entity type attributes
    insert into #codeTable
    SELECT DISTINCT
        '            RockMigrationHelper.UpdateWorkflowActionEntityAttribute("'+ 
        CONVERT(nvarchar(50), [aet].[Guid])+ '","'+   
        CONVERT(nvarchar(50), [ft].[Guid])+ '","'+     
        [a].[Name]+ '","'+  
        [a].[Key]+ '","'+ 
        ISNULL(REPLACE([a].[Description],'"','\"'),'')+ '",'+ 
        CONVERT(varchar, [a].[Order])+ ',@"'+ 
        ISNULL([a].[DefaultValue],'')+ '","'+
        CONVERT(nvarchar(50), [a].[Guid])+ '");' +
        ' // ' + aet.Name + ':'+ a.Name+
        @crlf
	from [Attribute] [a] 
	inner join [EntityType] [et] on [et].[Id] = [a].[EntityTypeId] and [et].Name = 'Rock.Model.WorkflowActionType'
    inner join [FieldType] [ft] on [ft].[Id] = [a].[FieldTypeId]
	inner join [EntityType] [aet] on CONVERT(varchar, [aet].[id]) = [a].[EntityTypeQualifierValue]
    where [a].[EntityTypeQualifierColumn] = 'EntityTypeId'
	and [aet].[id] in (
		select distinct [at].[EntityTypeId]
		from [WorkflowType] [wt]
		inner join [WorkflowActivityType] [act] on [act].[WorkflowTypeId] = [wt].[id]
		inner join [WorkflowActionType] [at] on [at].[ActivityTypeId] = [act].[id]
		and [wt].[Guid] not in (select [Guid] from #knownGuidsToIgnore)
	)

    insert into #codeTable
    SELECT @crlf

	-- categories
    insert into #codeTable
    SELECT 
		'            RockMigrationHelper.UpdateCategory("' +
        CONVERT( nvarchar(50), [e].[Guid]) + '","'+ 
        [c].[Name] +  '","'+
        [c].[IconCssClass] +  '","'+
        ISNULL(REPLACE([c].[Description],'"','\"'),'')+ '","'+ 
        CONVERT( nvarchar(50), [c].[Guid])+ '",'+
		CONVERT( nvarchar, [c].[Order] )+ ');' +
		' // ' + c.Name +
        @crlf
    FROM [Category] [c]
    join [EntityType] [e] on [e].[Id] = [c].[EntityTypeId]
    where [c].[IsSystem] = 0
    and [c].[Guid] not in (select [Guid] from #knownGuidsToIgnore)
	and [e].[Name] = 'Rock.Model.WorkflowType'
    order by [c].[Order]

    insert into #codeTable
    SELECT @crlf

	-- Workflow Type
    insert into #codeTable
    SELECT 
        '            RockMigrationHelper.UpdateWorkflowType('+ 
		(CASE [wt].[IsSystem] WHEN 1 THEN 'true' ELSE 'false' END) + ','+
		(CASE [wt].[IsActive] WHEN 1 THEN 'true' ELSE 'false' END) + ',"'+
        [wt].[Name]+ '","'+  
        ISNULL(REPLACE([wt].[Description],'"','\"'),'')+ '","'+ 
        CONVERT(nvarchar(50), [c].[Guid])+ '","'+     
        [wt].[WorkTerm]+ '","'+
        ISNULL([wt].[IconCssClass],'')+ '",'+ 
        CONVERT(varchar, ISNULL([wt].[ProcessingIntervalSeconds],0))+ ','+
		(CASE [wt].[IsPersisted] WHEN 1 THEN 'true' ELSE 'false' END) + ','+
        CONVERT(varchar, [wt].[LoggingLevel])+ ',"'+
		CONVERT(nvarchar(50), [wt].[Guid])+ '");'+
        ' // ' + wt.Name + 
        @crlf
    from [WorkflowType] [wt]
	inner join [Category] [c] on [c].[Id] = [wt].[CategoryId] 
    and [wt].[Guid] not in (select [Guid] from #knownGuidsToIgnore)

    insert into #codeTable
    SELECT @crlf

	-- Workflow Type Attributes
    insert into #codeTable
    SELECT 
        '            RockMigrationHelper.UpdateWorkflowTypeAttribute("'+ 
        CONVERT(nvarchar(50), wt.Guid)+ '","'+   
        CONVERT(nvarchar(50), ft.Guid)+ '","'+     
        a.Name+ '","'+  
        a.[Key]+ '","'+ 
        ISNULL(a.Description,'')+ '",'+ 
        CONVERT(varchar, a.[Order])+ ',@"'+ 
        ISNULL(a.DefaultValue,'')+ '","'+
        CONVERT(nvarchar(50), a.Guid)+ '");' +
        ' // ' + wt.Name + ':'+ a.Name+
        @crlf
    from [WorkflowType] [wt]
	inner join [Attribute] [a] on cast([a].[EntityTypeQualifierValue] as int) = [wt].[Id] 
	inner join [EntityType] [et] on [et].[Id] = [a].[EntityTypeId] and [et].Name = 'Rock.Model.Workflow'
    inner join [FieldType] [ft] on [ft].[Id] = [a].[FieldTypeId]
    where EntityTypeQualifierColumn = 'WorkflowTypeId'
    and [wt].[Guid] not in (select [Guid] from #knownGuidsToIgnore)

    insert into #codeTable
    SELECT @crlf

	-- Workflow Type Attribute Qualifiers
    insert into #codeTable
    SELECT 
        '            RockMigrationHelper.AddAttributeQualifier("'+ 
        CONVERT(nvarchar(50), a.Guid)+ '","'+   
        [aq].[Key]+ '",@"'+ 
        ISNULL([aq].[Value],'')+ '","'+
        CONVERT(nvarchar(50), [aq].[Guid])+ '");' +
        ' // ' + [wt].[Name] + ':'+ [a].[Name]+ ':'+ [aq].[Key]+
        @crlf
    from [WorkflowType] [wt]
	inner join [Attribute] [a] on cast([a].[EntityTypeQualifierValue] as int) = [wt].[Id] 
	inner join [AttributeQualifier] [aq] on [aq].[AttributeId] = [a].[Id]
	inner join [EntityType] [et] on [et].[Id] = [a].[EntityTypeId] and [et].Name = 'Rock.Model.Workflow'
    where [a].[EntityTypeQualifierColumn] = 'WorkflowTypeId'
    and [wt].[Guid] not in (select [Guid] from #knownGuidsToIgnore)

    insert into #codeTable
    SELECT @crlf

	-- Workflow Activity Type
    insert into #codeTable
    SELECT 
        '            RockMigrationHelper.UpdateWorkflowActivityType("'+ 
        CONVERT(nvarchar(50), [wt].[Guid])+ '",'+     
		(CASE [at].[IsActive] WHEN 1 THEN 'true' ELSE 'false' END) + ',"'+
        [at].[Name]+ '","'+  
        ISNULL(REPLACE([at].[Description],'"','\"'),'')+ '",'+ 
		(CASE [at].IsActivatedWithWorkflow WHEN 1 THEN 'true' ELSE 'false' END) + ','+
        CONVERT(varchar, [at].[Order])+ ',"'+
        CONVERT(nvarchar(50), [at].[Guid])+ '");' +
        ' // ' + wt.Name + ':'+ at.Name+
        @crlf
    from [WorkflowActivityType] [at]
	inner join [WorkflowType] [wt] on [wt].[id] = [at].[WorkflowTypeId]
    where [wt].[Guid] not in (select [Guid] from #knownGuidsToIgnore)

    insert into #codeTable
    SELECT @crlf

	-- Workflow Activity Type Attributes
    insert into #codeTable
    SELECT 
        '            RockMigrationHelper.UpdateWorkflowActivityTypeAttribute("'+ 
        CONVERT(nvarchar(50), at.Guid)+ '","'+   
        CONVERT(nvarchar(50), ft.Guid)+ '","'+     
        a.Name+ '","'+  
        a.[Key]+ '","'+ 
        ISNULL(a.Description,'')+ '",'+ 
        CONVERT(varchar, a.[Order])+ ',@"'+ 
        ISNULL(a.DefaultValue,'')+ '","'+
        CONVERT(nvarchar(50), a.Guid)+ '");' +
        ' // ' + wt.Name + ':'+ at.Name + ':'+ a.Name+
        @crlf
    from [WorkflowType] [wt]
	inner join [WorkflowActivityType] [at] on [at].[WorkflowTypeId] = [wt].[id]
	inner join [Attribute] [a] on cast([a].[EntityTypeQualifierValue] as int) = [at].[Id] 
	inner join [EntityType] [et] on [et].[Id] = [a].[EntityTypeId] and [et].Name = 'Rock.Model.WorkflowActivity'
    inner join [FieldType] [ft] on [ft].[Id] = [a].[FieldTypeId]
    where [a].[EntityTypeQualifierColumn] = 'ActivityTypeId'
    and [wt].[Guid] not in (select [Guid] from #knownGuidsToIgnore)

    insert into #codeTable
    SELECT @crlf

	-- Workflow Activity Type Attribute Qualifiers
    insert into #codeTable
    SELECT 
        '            RockMigrationHelper.AddAttributeQualifier("'+ 
        CONVERT(nvarchar(50), a.Guid)+ '","'+   
        [aq].[Key]+ '",@"'+ 
        ISNULL([aq].[Value],'')+ '","'+
        CONVERT(nvarchar(50), [aq].[Guid])+ '");' +
        ' // ' + [wt].[Name] + ':'+ [a].[Name]+ ':'+ [aq].[Key]+
        @crlf
    from [WorkflowType] [wt]
	inner join [WorkflowActivityType] [at] on [at].[WorkflowTypeId] = [wt].[id]
	inner join [Attribute] [a] on cast([a].[EntityTypeQualifierValue] as int) = [at].[Id] 
	inner join [AttributeQualifier] [aq] on [aq].[AttributeId] = [a].[Id]
	inner join [EntityType] [et] on [et].[Id] = [a].[EntityTypeId] and [et].Name = 'Rock.Model.WorkflowActivity'
    where [a].[EntityTypeQualifierColumn] = 'ActivityTypeId'
    and [wt].[Guid] not in (select [Guid] from #knownGuidsToIgnore)

    insert into #codeTable
    SELECT @crlf

	-- Action Forms
    insert into #codeTable
    SELECT 
        '            RockMigrationHelper.UpdateWorkflowActionForm(@"'+ 
        ISNULL([f].[Header],'')+ '",@"'+ 
        ISNULL([f].[Footer],'')+ '","'+ 
        ISNULL([f].[Actions],'')+ '","'+ 
		(CASE WHEN [se].[Guid] IS NULL THEN '' ELSE CONVERT(nvarchar(50), [se].[Guid]) END) + '",'+
		(CASE [f].[IncludeActionsInNotification] WHEN 1 THEN 'true' ELSE 'false' END) + ',"'+
        ISNULL(CONVERT(nvarchar(50), [f].[ActionAttributeGuid]),'')+ '","'+ 
		CONVERT(nvarchar(50), [f].[Guid])+ '");' +
        ' // ' + wt.Name + ':'+ at.Name + ':'+ a.Name+
        @crlf
    from [WorkflowActionForm] [f]
	inner join [WorkflowActionType] [a] on [a].[WorkflowFormId] = [f].[id]
	inner join [WorkflowActivityType] [at] on [at].[id] = [a].[ActivityTypeId]
	inner join [WorkflowType] [wt] on [wt].[id] = [at].[WorkflowTypeId]
	left outer join [SystemEmail] [se] on [se].[id] = [f].[NotificationSystemEmailId]
    where [wt].[Guid] not in (select [Guid] from #knownGuidsToIgnore)

    insert into #codeTable
    SELECT @crlf

	-- Action Form Attributes
    insert into #codeTable
    SELECT 
        '            RockMigrationHelper.UpdateWorkflowActionFormAttribute("'+ 
		CONVERT(nvarchar(50), [f].[Guid])+ '","' +
		CONVERT(nvarchar(50), [a].[Guid])+ '",' +
		CONVERT(varchar, [fa].[Order])+ ',' +
		(CASE [fa].[IsVisible] WHEN 1 THEN 'true' ELSE 'false' END) + ','+
		(CASE [fa].[IsReadOnly] WHEN 1 THEN 'true' ELSE 'false' END) + ','+
		(CASE [fa].[IsRequired] WHEN 1 THEN 'true' ELSE 'false' END) + ',"'+
		CONVERT(nvarchar(50), [fa].[Guid])+ '");' +
        ' // '+ wt.Name+ ':'+ act.Name+ ':'+ at.Name+ ':'+ a.Name+
        @crlf
    from [WorkflowActionFormAttribute] [fa]
	inner join [WorkflowActionForm] [f] on [f].[id] = [fa].[WorkflowActionFormId]
	inner join [Attribute] [a] on [a].[id] = [fa].[AttributeId]
	inner join [WorkflowActionType] [at] on [at].[WorkflowFormId] = [f].[id]
	inner join [WorkflowActivityType] [act] on [act].[id] = [at].[ActivityTypeId]
	inner join [WorkflowType] [wt] on [wt].[id] = [act].[WorkflowTypeId]
    where [wt].[Guid] not in (select [Guid] from #knownGuidsToIgnore)

    insert into #codeTable
    SELECT @crlf

	-- Workflow Action Type
    insert into #codeTable
    SELECT 
        '            RockMigrationHelper.UpdateWorkflowActionType("'+ 
        CONVERT(nvarchar(50), [at].[Guid])+ '","'+     
        [a].[Name]+ '",'+  
        CONVERT(varchar, [a].[Order])+ ',"'+
        CONVERT(nvarchar(50), [et].[Guid])+ '",'+     
		(CASE [a].[IsActionCompletedOnSuccess] WHEN 1 THEN 'true' ELSE 'false' END) + ','+
		(CASE [a].[IsActivityCompletedOnSuccess] WHEN 1 THEN 'true' ELSE 'false' END) + ',"'+
		(CASE WHEN [f].[Guid] IS NULL THEN '' ELSE CONVERT(nvarchar(50), [f].[Guid]) END) + '","'+
        ISNULL(CONVERT(nvarchar(50), [a].[CriteriaAttributeGuid]),'')+ '",'+ 
        CONVERT(varchar, [a].[CriteriaComparisonType])+ ',"'+ 
        ISNULL([a].[CriteriaValue],'')+ '","'+ 
        CONVERT(nvarchar(50), [a].[Guid])+ '");' +
        ' // '+ wt.Name+ ':'+ at.Name+ ':'+ a.Name+
        @crlf
    from [WorkflowActionType] [a]
	inner join [WorkflowActivityType] [at] on [at].[id] = [a].[ActivityTypeId]
	inner join [WorkflowType] [wt] on [wt].[id] = [at].[WorkflowTypeId]
	inner join [EntityType] [et] on [et].[id] = [a].[EntityTypeId]
	left outer join [WorkflowActionForm] [f] on [f].[id] = [a].[WorkflowFormId]
    where [wt].[Guid] not in (select [Guid] from #knownGuidsToIgnore)

    insert into #codeTable
    SELECT @crlf


    -- Workflow Action Type attributes values 
    insert into #codeTable
    SELECT 
		CASE WHEN [FT].[Guid] = 'E4EAB7B2-0B76-429B-AFE4-AD86D7428C70' THEN
        '            RockMigrationHelper.AddActionTypePersonAttributeValue("' ELSE
        '            RockMigrationHelper.AddActionTypeAttributeValue("' END+
        CONVERT(nvarchar(50), at.Guid)+ '","'+ 
        CONVERT(nvarchar(50), a.Guid)+ '",@"'+ 
		REPLACE(ISNULL(av.Value,''), '"', '""') + '");'+
        ' // '+ wt.Name+ ':'+ act.Name+ ':'+ at.Name+ ':'+ a.Name +
        @crlf
    from [AttributeValue] [av]
    inner join [WorkflowActionType] [at] on [at].[Id] = [av].[EntityId]
    inner join [Attribute] [a] on [a].[id] = [av].[AttributeId] AND [a].EntityTypeQualifierValue = CONVERT(nvarchar, [at].EntityTypeId)
	inner join [FieldType] [ft] on [ft].[id] = [a].[FieldTypeId] 
	inner join [EntityType] [et] on [et].[id] = [a].[EntityTypeId] and [et].[Name] = 'Rock.Model.WorkflowActionType'
    inner join [WorkflowActivityType] [act] on [act].[Id] = [at].[ActivityTypeId]
	inner join [WorkflowType] [wt] on [wt].[Id] = [act].[WorkflowTypeId]
    and [wt].[Guid] not in (select [Guid] from #knownGuidsToIgnore) 
	order by [wt].[Order], [act].[Order], [at].[Order], [a].[Order]

    select CodeText [MigrationUp] from #codeTable
    where REPLACE(CodeText, @crlf, '') != ''
    order by Id

IF OBJECT_ID('tempdb..#codeTable') IS NOT NULL
    DROP TABLE #codeTable

IF OBJECT_ID('tempdb..#knownGuidsToIgnore') IS NOT NULL
    DROP TABLE #knownGuidsToIgnore

end