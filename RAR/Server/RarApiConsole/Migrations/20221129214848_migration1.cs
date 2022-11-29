using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace RarApiConsole.Migrations
{
    public partial class migration1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.CreateTable(
                name: "profiles",
                schema: "public",
                columns: table => new
                {
                    object_key = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    initials = table.Column<string>(type: "varchar(10)", nullable: true),
                    name = table.Column<string>(type: "varchar(40)", nullable: true),
                    surname = table.Column<string>(type: "varchar(70)", nullable: true),
                    email = table.Column<string>(type: "varchar(40)", nullable: true),
                    phone_number = table.Column<string>(type: "varchar(30)", nullable: true),
                    address = table.Column<string>(type: "varchar(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_profiles", x => x.object_key);
                });

            migrationBuilder.CreateTable(
                name: "tasks",
                schema: "public",
                columns: table => new
                {
                    object_key = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "varchar(100)", nullable: false),
                    points = table.Column<int>(type: "int", nullable: false),
                    description = table.Column<string>(type: "varchar(255)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tasks", x => x.object_key);
                });

            migrationBuilder.CreateTable(
                name: "candidates",
                schema: "public",
                columns: table => new
                {
                    object_key = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    fk_profile = table.Column<int>(type: "int", nullable: false),
                    profileobject_key = table.Column<int>(type: "integer", nullable: false),
                    referred_at = table.Column<DateTime>(type: "timestamp", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_candidates", x => x.object_key);
                    table.ForeignKey(
                        name: "FK_candidates_profiles_fk_profile",
                        column: x => x.fk_profile,
                        principalSchema: "public",
                        principalTable: "profiles",
                        principalColumn: "object_key",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_candidates_profiles_profileobject_key",
                        column: x => x.profileobject_key,
                        principalSchema: "public",
                        principalTable: "profiles",
                        principalColumn: "object_key",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "users",
                schema: "public",
                columns: table => new
                {
                    object_key = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    fk_profile = table.Column<int>(type: "int", nullable: true),
                    profileobject_key = table.Column<int>(type: "integer", nullable: true),
                    username = table.Column<string>(type: "varchar(100)", nullable: false),
                    password = table.Column<string>(type: "varchar(100)", nullable: false),
                    recruiter = table.Column<int>(type: "int", nullable: false),
                    creation_dt = table.Column<DateTime>(type: "timestamp", nullable: false),
                    modification_dt = table.Column<DateTime>(type: "timestamp", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.object_key);
                    table.ForeignKey(
                        name: "FK_users_profiles_fk_profile",
                        column: x => x.fk_profile,
                        principalSchema: "public",
                        principalTable: "profiles",
                        principalColumn: "object_key");
                    table.ForeignKey(
                        name: "FK_users_profiles_profileobject_key",
                        column: x => x.profileobject_key,
                        principalSchema: "public",
                        principalTable: "profiles",
                        principalColumn: "object_key");
                });

            migrationBuilder.CreateTable(
                name: "rewards",
                schema: "public",
                columns: table => new
                {
                    object_key = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    fk_user = table.Column<int>(type: "int", nullable: false),
                    userobject_key = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "varchar(50)", nullable: false),
                    award_dt = table.Column<DateTime>(type: "timestamp", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rewards", x => x.object_key);
                    table.ForeignKey(
                        name: "FK_rewards_users_fk_user",
                        column: x => x.fk_user,
                        principalSchema: "public",
                        principalTable: "users",
                        principalColumn: "object_key",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_rewards_users_userobject_key",
                        column: x => x.userobject_key,
                        principalSchema: "public",
                        principalTable: "users",
                        principalColumn: "object_key",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "scoreboards",
                schema: "public",
                columns: table => new
                {
                    object_key = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    fk_user = table.Column<int>(type: "int", nullable: false),
                    userobject_key = table.Column<int>(type: "integer", nullable: false),
                    name = table.Column<string>(type: "varchar(40)", nullable: false),
                    start_dt = table.Column<DateTime>(type: "timestamp", nullable: false),
                    end_dt = table.Column<DateTime>(type: "timestamp", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_scoreboards", x => x.object_key);
                    table.ForeignKey(
                        name: "FK_scoreboards_users_fk_user",
                        column: x => x.fk_user,
                        principalSchema: "public",
                        principalTable: "users",
                        principalColumn: "object_key",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_scoreboards_users_userobject_key",
                        column: x => x.userobject_key,
                        principalSchema: "public",
                        principalTable: "users",
                        principalColumn: "object_key",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "referrals",
                schema: "public",
                columns: table => new
                {
                    object_key = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    fk_user = table.Column<int>(type: "int", nullable: false),
                    userobject_key = table.Column<int>(type: "integer", nullable: false),
                    fk_task = table.Column<int>(type: "int", nullable: false),
                    taskobject_key = table.Column<int>(type: "integer", nullable: false),
                    fk_candidate = table.Column<int>(type: "int", nullable: false),
                    candidateobject_key = table.Column<int>(type: "integer", nullable: false),
                    fk_scoreboard = table.Column<int>(type: "int", nullable: false),
                    scoreboardobject_key = table.Column<int>(type: "integer", nullable: false),
                    creation_dt = table.Column<DateTime>(type: "timestamp", nullable: false),
                    modification_dt = table.Column<DateTime>(type: "timestamp", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_referrals", x => x.object_key);
                    table.ForeignKey(
                        name: "FK_referrals_candidates_candidateobject_key",
                        column: x => x.candidateobject_key,
                        principalSchema: "public",
                        principalTable: "candidates",
                        principalColumn: "object_key",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_referrals_candidates_fk_candidate",
                        column: x => x.fk_candidate,
                        principalSchema: "public",
                        principalTable: "candidates",
                        principalColumn: "object_key",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_referrals_scoreboards_fk_scoreboard",
                        column: x => x.fk_scoreboard,
                        principalSchema: "public",
                        principalTable: "scoreboards",
                        principalColumn: "object_key",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_referrals_scoreboards_scoreboardobject_key",
                        column: x => x.scoreboardobject_key,
                        principalSchema: "public",
                        principalTable: "scoreboards",
                        principalColumn: "object_key",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_referrals_tasks_fk_task",
                        column: x => x.fk_task,
                        principalSchema: "public",
                        principalTable: "tasks",
                        principalColumn: "object_key",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_referrals_tasks_taskobject_key",
                        column: x => x.taskobject_key,
                        principalSchema: "public",
                        principalTable: "tasks",
                        principalColumn: "object_key",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_referrals_users_fk_user",
                        column: x => x.fk_user,
                        principalSchema: "public",
                        principalTable: "users",
                        principalColumn: "object_key",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_referrals_users_userobject_key",
                        column: x => x.userobject_key,
                        principalSchema: "public",
                        principalTable: "users",
                        principalColumn: "object_key",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_candidates_fk_profile",
                schema: "public",
                table: "candidates",
                column: "fk_profile");

            migrationBuilder.CreateIndex(
                name: "IX_candidates_profileobject_key",
                schema: "public",
                table: "candidates",
                column: "profileobject_key");

            migrationBuilder.CreateIndex(
                name: "IX_referrals_candidateobject_key",
                schema: "public",
                table: "referrals",
                column: "candidateobject_key");

            migrationBuilder.CreateIndex(
                name: "IX_referrals_fk_candidate",
                schema: "public",
                table: "referrals",
                column: "fk_candidate");

            migrationBuilder.CreateIndex(
                name: "IX_referrals_fk_scoreboard",
                schema: "public",
                table: "referrals",
                column: "fk_scoreboard");

            migrationBuilder.CreateIndex(
                name: "IX_referrals_fk_task",
                schema: "public",
                table: "referrals",
                column: "fk_task");

            migrationBuilder.CreateIndex(
                name: "IX_referrals_fk_user",
                schema: "public",
                table: "referrals",
                column: "fk_user");

            migrationBuilder.CreateIndex(
                name: "IX_referrals_scoreboardobject_key",
                schema: "public",
                table: "referrals",
                column: "scoreboardobject_key");

            migrationBuilder.CreateIndex(
                name: "IX_referrals_taskobject_key",
                schema: "public",
                table: "referrals",
                column: "taskobject_key");

            migrationBuilder.CreateIndex(
                name: "IX_referrals_userobject_key",
                schema: "public",
                table: "referrals",
                column: "userobject_key");

            migrationBuilder.CreateIndex(
                name: "IX_rewards_fk_user",
                schema: "public",
                table: "rewards",
                column: "fk_user");

            migrationBuilder.CreateIndex(
                name: "IX_rewards_userobject_key",
                schema: "public",
                table: "rewards",
                column: "userobject_key");

            migrationBuilder.CreateIndex(
                name: "IX_scoreboards_fk_user",
                schema: "public",
                table: "scoreboards",
                column: "fk_user");

            migrationBuilder.CreateIndex(
                name: "IX_scoreboards_userobject_key",
                schema: "public",
                table: "scoreboards",
                column: "userobject_key");

            migrationBuilder.CreateIndex(
                name: "IX_users_fk_profile",
                schema: "public",
                table: "users",
                column: "fk_profile");

            migrationBuilder.CreateIndex(
                name: "IX_users_profileobject_key",
                schema: "public",
                table: "users",
                column: "profileobject_key");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "referrals",
                schema: "public");

            migrationBuilder.DropTable(
                name: "rewards",
                schema: "public");

            migrationBuilder.DropTable(
                name: "candidates",
                schema: "public");

            migrationBuilder.DropTable(
                name: "scoreboards",
                schema: "public");

            migrationBuilder.DropTable(
                name: "tasks",
                schema: "public");

            migrationBuilder.DropTable(
                name: "users",
                schema: "public");

            migrationBuilder.DropTable(
                name: "profiles",
                schema: "public");
        }
    }
}
