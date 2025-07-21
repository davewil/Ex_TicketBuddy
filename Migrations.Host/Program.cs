using Migrations;
using Migrations.Host;

var result = Migration.Upgrade(Settings.Database.Connection);
return result ? 0 : -1;