using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bison.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class MakeObservationTaxonOptional : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Taxons_TaxonId",
                table: "Posts");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Taxons_TaxonId",
                table: "Posts",
                column: "TaxonId",
                principalTable: "Taxons",
                principalColumn: "TaxonId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Taxons_TaxonId",
                table: "Posts");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Taxons_TaxonId",
                table: "Posts",
                column: "TaxonId",
                principalTable: "Taxons",
                principalColumn: "TaxonId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
