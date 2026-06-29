using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace task_manager.Infraestructure.Migrations
{
    /// <inheritdoc />
    public partial class AdminRol : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                IF NOT EXISTS(SELECT Id FROM AspNetRoles WHERE Id='7b8dabc5-1b35-48b8-b79c-aa4dbbe73be5')
                BEGIN
	                INSERT AspNetRoles(
		                Id,
		                Name,
		                NormalizedName
	                ) VALUES(
		                '7b8dabc5-1b35-48b8-b79c-aa4dbbe73be5',
		                'Admin',
		                'ADMIN'
                    );
                END;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE AspNetRoles WHERE Id='7b8dabc5-1b35-48b8-b79c-aa4dbbe73be5'");
        }
    }
}
