using Microsoft.EntityFrameworkCore;
using Migrations;

var dbBuilder = new ApplicationContextFactory();
using var context = dbBuilder.CreateDbContext(args);
context.Database.Migrate();