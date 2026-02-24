using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataContext.Migrations
{
    /// <inheritdoc />
    public partial class init2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Freelancers_FreelancerId",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Categories_FreelancerId",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "FreelancerId",
                table: "Categories");

            migrationBuilder.CreateTable(
                name: "FreelancerSkills",
                columns: table => new
                {
                    FreelancersFreelancerId = table.Column<int>(type: "int", nullable: false),
                    SkillsCategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FreelancerSkills", x => new { x.FreelancersFreelancerId, x.SkillsCategoryId });
                    table.ForeignKey(
                        name: "FK_FreelancerSkills_Categories_SkillsCategoryId",
                        column: x => x.SkillsCategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FreelancerSkills_Freelancers_FreelancersFreelancerId",
                        column: x => x.FreelancersFreelancerId,
                        principalTable: "Freelancers",
                        principalColumn: "FreelancerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FreelancerSkills_SkillsCategoryId",
                table: "FreelancerSkills",
                column: "SkillsCategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FreelancerSkills");

            migrationBuilder.AddColumn<int>(
                name: "FreelancerId",
                table: "Categories",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_FreelancerId",
                table: "Categories",
                column: "FreelancerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Freelancers_FreelancerId",
                table: "Categories",
                column: "FreelancerId",
                principalTable: "Freelancers",
                principalColumn: "FreelancerId");
        }
    }
}
