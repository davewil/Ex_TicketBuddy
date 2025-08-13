using Migrations.Host;
using Tickets.Migrations;

return Migration.Upgrade(Settings.Database.Connection) ? 0 : -1;