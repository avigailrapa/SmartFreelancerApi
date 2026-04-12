using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataContext.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FreelancerSkills_Freelancers_FreelancersFreelancerId",
                table: "FreelancerSkills");

            migrationBuilder.RenameColumn(
                name: "FreelancersFreelancerId",
                table: "FreelancerSkills",
                newName: "SkillFreelancersFreelancerId");

            migrationBuilder.CreateTable(
                name: "FreelancerSpecializations",
                columns: table => new
                {
                    SpecializationFreelancersFreelancerId = table.Column<int>(type: "int", nullable: false),
                    SpecializationsCategoryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FreelancerSpecializations", x => new { x.SpecializationFreelancersFreelancerId, x.SpecializationsCategoryId });
                    table.ForeignKey(
                        name: "FK_FreelancerSpecializations_Categories_SpecializationsCategoryId",
                        column: x => x.SpecializationsCategoryId,
                        principalTable: "Categories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FreelancerSpecializations_Freelancers_SpecializationFreelancersFreelancerId",
                        column: x => x.SpecializationFreelancersFreelancerId,
                        principalTable: "Freelancers",
                        principalColumn: "FreelancerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FreelancerSpecializations_SpecializationsCategoryId",
                table: "FreelancerSpecializations",
                column: "SpecializationsCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_FreelancerSkills_Freelancers_SkillFreelancersFreelancerId",
                table: "FreelancerSkills",
                column: "SkillFreelancersFreelancerId",
                principalTable: "Freelancers",
                principalColumn: "FreelancerId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FreelancerSkills_Freelancers_SkillFreelancersFreelancerId",
                table: "FreelancerSkills");

            migrationBuilder.DropTable(
                name: "FreelancerSpecializations");

            migrationBuilder.RenameColumn(
                name: "SkillFreelancersFreelancerId",
                table: "FreelancerSkills",
                newName: "FreelancersFreelancerId");

            migrationBuilder.AddForeignKey(
                name: "FK_FreelancerSkills_Freelancers_FreelancersFreelancerId",
                table: "FreelancerSkills",
                column: "FreelancersFreelancerId",
                principalTable: "Freelancers",
                principalColumn: "FreelancerId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
