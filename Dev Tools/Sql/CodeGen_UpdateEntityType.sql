-- select EntityTypes that are in the current database, but not in a 'clean' database (installed but never started)
SELECT CONCAT('RockMigrationHelper.UpdateEntityType("', [Name], '","',  [FriendlyName], '","', [AssemblyName], '",', case [IsEntity] when 1 then 'true' else 'false' end, ',', case [IsSecured] when 1 then 'true' else 'false' end, ',"', [Guid], '");') [Up]
  FROM [EntityType]
  where [Guid] not in (select [Guid] from RockRMS_clean.dbo.EntityType)
  --and Name like 'Rock.Reporting.%'
  order by Name