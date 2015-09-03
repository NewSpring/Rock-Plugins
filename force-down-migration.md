# To Force a Down Migration to Run

1. Find the `MigratePlugins` method within the `Global.asax.cs` file
2. Find this section:
```c#
// Iterate each assembly that contains plugin migrations
foreach ( var assemblyMigrations in assemblies )
{
    try
    {
        // Get the versions that have already been installed
        var installedVersions = pluginMigrationService.Queryable()
            .Where( m => m.PluginAssemblyName == assemblyMigrations.Key )
            .ToList();

        // Iterate each migration in the assembly in MigrationNumber order
        foreach ( var migrationType in assemblyMigrations.Value.OrderBy( t => t.Key ) )
        {
            // Check to make sure migration has not already been run
            if ( !installedVersions.Any( v => v.MigrationNumber == migrationType.Key ) )
            {
                using ( var sqlTxn = con.BeginTransaction() )
                {
                    bool transactionActive = true;
                    try
                    {
                        // Create an instance of the migration and run the up migration
                        var migration = Activator.CreateInstance( migrationType.Value ) as Rock.Plugin.Migration;
                        migration.SqlConnection = con;
                        migration.SqlTransaction = sqlTxn;
                        migration.Up();
                        sqlTxn.Commit();
                        transactionActive = false;

                        // Save the plugin migration version so that it is not run again
                        var pluginMigration = new PluginMigration();
                        pluginMigration.PluginAssemblyName = assemblyMigrations.Key;
                        pluginMigration.MigrationNumber = migrationType.Key;
                        pluginMigration.MigrationName = migrationType.Value.Name;
                        pluginMigrationService.Add( pluginMigration );
                        rockContext.SaveChanges();

                        result = true;
                    }
                    catch ( Exception ex )
                    {
                        if ( transactionActive )
                        {
                            sqlTxn.Rollback();
                        }
                        throw new Exception( string.Format( "Plugin Migration error occurred in {0}, {1}",
                            assemblyMigrations.Key, migrationType.Value.Name ), ex );
                    }
                }
            }
        }
    }
    catch ( Exception ex )
    {
        // If an exception occurs in an an assembly, log the error, and continue with next assembly
        LogError( ex, null );
    }
}
```

3. Change this line `foreach ( var migrationType in assemblyMigrations.Value.OrderBy( t => t.Key ) )` to be `foreach ( var migrationType in assemblyMigrations.Value.Where( t => t.Name == "MIGRATION_NAME_HERE" ) )`
4. Change this `migration.Up();` to `migration.Down();`
5. Delete this section:
```c#
// Save the plugin migration version so that it is not run again
var pluginMigration = new PluginMigration();
pluginMigration.PluginAssemblyName = assemblyMigrations.Key;
pluginMigration.MigrationNumber = migrationType.Key;
pluginMigration.MigrationName = migrationType.Value.Name;
pluginMigrationService.Add( pluginMigration );
rockContext.SaveChanges();
```
6. After testing locally, change your database connection strings to point to the database server you wish to modify, such as staging.
7. Run it.
8. Remove record from `PluginMigrations` table so that the migration's `up` will run again if desired.
9. **Do not** commit your changes to the `Global.asax.cs` file or gremlins will destroy your car.
