using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Oikono.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAuth : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RefreshTokens_Users_CreatedBy",
                schema: "Oikono",
                table: "RefreshTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_RefreshTokens_Users_UpdatedBy",
                schema: "Oikono",
                table: "RefreshTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_RefreshTokens_Users_UserId",
                schema: "Oikono",
                table: "RefreshTokens");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                schema: "Oikono",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RefreshTokens",
                schema: "Oikono",
                table: "RefreshTokens");

            migrationBuilder.RenameTable(
                name: "Users",
                schema: "Oikono",
                newName: "User",
                newSchema: "Oikono");

            migrationBuilder.RenameTable(
                name: "RefreshTokens",
                schema: "Oikono",
                newName: "RefreshToken",
                newSchema: "Oikono");

            migrationBuilder.RenameIndex(
                name: "IX_RefreshTokens_UserId",
                schema: "Oikono",
                table: "RefreshToken",
                newName: "IX_RefreshToken_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_RefreshTokens_UpdatedBy",
                schema: "Oikono",
                table: "RefreshToken",
                newName: "IX_RefreshToken_UpdatedBy");

            migrationBuilder.RenameIndex(
                name: "IX_RefreshTokens_CreatedBy",
                schema: "Oikono",
                table: "RefreshToken",
                newName: "IX_RefreshToken_CreatedBy");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                schema: "Oikono",
                table: "User",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2024, 1, 24, 21, 3, 37, 757, DateTimeKind.Utc).AddTicks(6030),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2023, 12, 10, 10, 47, 5, 612, DateTimeKind.Utc).AddTicks(5861));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                schema: "Oikono",
                table: "User",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2024, 1, 24, 21, 3, 37, 757, DateTimeKind.Utc).AddTicks(5626),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2023, 12, 10, 10, 47, 5, 612, DateTimeKind.Utc).AddTicks(5559));

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                schema: "Oikono",
                table: "RefreshToken",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2024, 1, 24, 21, 3, 37, 751, DateTimeKind.Utc).AddTicks(1220),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2023, 12, 10, 10, 47, 5, 608, DateTimeKind.Utc).AddTicks(9010));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                schema: "Oikono",
                table: "RefreshToken",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2024, 1, 24, 21, 3, 37, 750, DateTimeKind.Utc).AddTicks(9337),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2023, 12, 10, 10, 47, 5, 608, DateTimeKind.Utc).AddTicks(7012));

            migrationBuilder.AddPrimaryKey(
                name: "PK_User",
                schema: "Oikono",
                table: "User",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RefreshToken",
                schema: "Oikono",
                table: "RefreshToken",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Permission",
                schema: "Oikono",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false,
                        defaultValue: new DateTime(2024, 1, 24, 21, 3, 37, 748, DateTimeKind.Utc).AddTicks(9952)),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false,
                        defaultValue: new DateTime(2024, 1, 24, 21, 3, 37, 749, DateTimeKind.Utc).AddTicks(385)),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_Permission", x => x.Id); });

            migrationBuilder.CreateTable(
                name: "Policy",
                schema: "Oikono",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false,
                        defaultValue: new DateTime(2024, 1, 24, 21, 3, 37, 750, DateTimeKind.Utc).AddTicks(1107)),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false,
                        defaultValue: new DateTime(2024, 1, 24, 21, 3, 37, 750, DateTimeKind.Utc).AddTicks(1524)),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_Policy", x => x.Id); });

            migrationBuilder.CreateTable(
                name: "Role",
                schema: "Oikono",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false,
                        defaultValue: new DateTime(2024, 1, 24, 21, 3, 37, 757, DateTimeKind.Utc).AddTicks(2149)),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false,
                        defaultValue: new DateTime(2024, 1, 24, 21, 3, 37, 757, DateTimeKind.Utc).AddTicks(2608)),
                    Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false)
                },
                constraints: table => { table.PrimaryKey("PK_Role", x => x.Id); });

            migrationBuilder.CreateTable(
                name: "UserPermission",
                schema: "Oikono",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false,
                        defaultValue: new DateTime(2024, 1, 24, 21, 3, 37, 758, DateTimeKind.Utc).AddTicks(5175)),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false,
                        defaultValue: new DateTime(2024, 1, 24, 21, 3, 37, 758, DateTimeKind.Utc).AddTicks(5601)),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    PermissionId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPermission", x => x.Id);
                    table.UniqueConstraint("AK_UserPermission_UserId_PermissionId",
                        x => new { x.UserId, x.PermissionId });
                    table.ForeignKey(
                        name: "FK_UserPermission_Permission_PermissionId",
                        column: x => x.PermissionId,
                        principalSchema: "Oikono",
                        principalTable: "Permission",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserPermission_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "Oikono",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserPolicy",
                schema: "Oikono",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false,
                        defaultValue: new DateTime(2024, 1, 24, 21, 3, 37, 760, DateTimeKind.Utc).AddTicks(1886)),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false,
                        defaultValue: new DateTime(2024, 1, 24, 21, 3, 37, 760, DateTimeKind.Utc).AddTicks(2486)),
                    PolicyId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPolicy", x => x.Id);
                    table.UniqueConstraint("AK_UserPolicy_UserId_PolicyId", x => new { x.UserId, x.PolicyId });
                    table.ForeignKey(
                        name: "FK_UserPolicy_Policy_PolicyId",
                        column: x => x.PolicyId,
                        principalSchema: "Oikono",
                        principalTable: "Policy",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserPolicy_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "Oikono",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                schema: "Oikono",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false,
                        defaultValue: new DateTime(2024, 1, 24, 21, 3, 37, 761, DateTimeKind.Utc).AddTicks(7968)),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false,
                        defaultValue: new DateTime(2024, 1, 24, 21, 3, 37, 761, DateTimeKind.Utc).AddTicks(8439)),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoleId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => x.Id);
                    table.UniqueConstraint("AK_UserRole_UserId_RoleId", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRole_Role_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "Oikono",
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRole_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "Oikono",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "Oikono",
                table: "Permission",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("85691f11-36f8-4a88-8994-99b422d6f3af"), "Set" },
                    { new Guid("b4d43f1e-16da-4c33-9a1d-115479c3c77b"), "Get" }
                });

            migrationBuilder.InsertData(
                schema: "Oikono",
                table: "Policy",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("b84a43f6-413e-41d9-851e-831ad0cd5f4a"), "SelfOrAdmin" });

            migrationBuilder.InsertData(
                schema: "Oikono",
                table: "Role",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("839b7836-cc23-4a4d-9711-28fca2bf1524"), "Admin" });

            migrationBuilder.CreateIndex(
                name: "IX_UserPermission_PermissionId",
                schema: "Oikono",
                table: "UserPermission",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPolicy_PolicyId",
                schema: "Oikono",
                table: "UserPolicy",
                column: "PolicyId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_RoleId",
                schema: "Oikono",
                table: "UserRole",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshToken_User_CreatedBy",
                schema: "Oikono",
                table: "RefreshToken",
                column: "CreatedBy",
                principalSchema: "Oikono",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshToken_User_UpdatedBy",
                schema: "Oikono",
                table: "RefreshToken",
                column: "UpdatedBy",
                principalSchema: "Oikono",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshToken_User_UserId",
                schema: "Oikono",
                table: "RefreshToken",
                column: "UserId",
                principalSchema: "Oikono",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RefreshToken_User_CreatedBy",
                schema: "Oikono",
                table: "RefreshToken");

            migrationBuilder.DropForeignKey(
                name: "FK_RefreshToken_User_UpdatedBy",
                schema: "Oikono",
                table: "RefreshToken");

            migrationBuilder.DropForeignKey(
                name: "FK_RefreshToken_User_UserId",
                schema: "Oikono",
                table: "RefreshToken");

            migrationBuilder.DropTable(
                name: "UserPermission",
                schema: "Oikono");

            migrationBuilder.DropTable(
                name: "UserPolicy",
                schema: "Oikono");

            migrationBuilder.DropTable(
                name: "UserRole",
                schema: "Oikono");

            migrationBuilder.DropTable(
                name: "Permission",
                schema: "Oikono");

            migrationBuilder.DropTable(
                name: "Policy",
                schema: "Oikono");

            migrationBuilder.DropTable(
                name: "Role",
                schema: "Oikono");

            migrationBuilder.DropPrimaryKey(
                name: "PK_User",
                schema: "Oikono",
                table: "User");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RefreshToken",
                schema: "Oikono",
                table: "RefreshToken");

            migrationBuilder.RenameTable(
                name: "User",
                schema: "Oikono",
                newName: "Users",
                newSchema: "Oikono");

            migrationBuilder.RenameTable(
                name: "RefreshToken",
                schema: "Oikono",
                newName: "RefreshTokens",
                newSchema: "Oikono");

            migrationBuilder.RenameIndex(
                name: "IX_RefreshToken_UserId",
                schema: "Oikono",
                table: "RefreshTokens",
                newName: "IX_RefreshTokens_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_RefreshToken_UpdatedBy",
                schema: "Oikono",
                table: "RefreshTokens",
                newName: "IX_RefreshTokens_UpdatedBy");

            migrationBuilder.RenameIndex(
                name: "IX_RefreshToken_CreatedBy",
                schema: "Oikono",
                table: "RefreshTokens",
                newName: "IX_RefreshTokens_CreatedBy");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                schema: "Oikono",
                table: "Users",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2023, 12, 10, 10, 47, 5, 612, DateTimeKind.Utc).AddTicks(5861),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2024, 1, 24, 21, 3, 37, 757, DateTimeKind.Utc).AddTicks(6030));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                schema: "Oikono",
                table: "Users",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2023, 12, 10, 10, 47, 5, 612, DateTimeKind.Utc).AddTicks(5559),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2024, 1, 24, 21, 3, 37, 757, DateTimeKind.Utc).AddTicks(5626));

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                schema: "Oikono",
                table: "RefreshTokens",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2023, 12, 10, 10, 47, 5, 608, DateTimeKind.Utc).AddTicks(9010),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2024, 1, 24, 21, 3, 37, 751, DateTimeKind.Utc).AddTicks(1220));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                schema: "Oikono",
                table: "RefreshTokens",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(2023, 12, 10, 10, 47, 5, 608, DateTimeKind.Utc).AddTicks(7012),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValue: new DateTime(2024, 1, 24, 21, 3, 37, 750, DateTimeKind.Utc).AddTicks(9337));

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                schema: "Oikono",
                table: "Users",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RefreshTokens",
                schema: "Oikono",
                table: "RefreshTokens",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshTokens_Users_CreatedBy",
                schema: "Oikono",
                table: "RefreshTokens",
                column: "CreatedBy",
                principalSchema: "Oikono",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshTokens_Users_UpdatedBy",
                schema: "Oikono",
                table: "RefreshTokens",
                column: "UpdatedBy",
                principalSchema: "Oikono",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshTokens_Users_UserId",
                schema: "Oikono",
                table: "RefreshTokens",
                column: "UserId",
                principalSchema: "Oikono",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}