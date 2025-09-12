using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Oikono.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class recipe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WeatherForecast",
                schema: "Oikono");

            migrationBuilder.CreateTable(
                name: "Assets",
                schema: "Oikono",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    FileName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    ContentType = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    Data = table.Column<byte[]>(type: "bytea", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Assets_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalSchema: "Oikono",
                        principalTable: "User",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Assets_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalSchema: "Oikono",
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Devices",
                schema: "Oikono",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Devices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Devices_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalSchema: "Oikono",
                        principalTable: "User",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Devices_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalSchema: "Oikono",
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Ingredients",
                schema: "Oikono",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ingredients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ingredients_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalSchema: "Oikono",
                        principalTable: "User",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Ingredients_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalSchema: "Oikono",
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Recipes",
                schema: "Oikono",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Portions = table.Column<int>(type: "integer", nullable: false),
                    Calories = table.Column<int>(type: "integer", nullable: true),
                    Rating = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recipes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Recipes_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalSchema: "Oikono",
                        principalTable: "User",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Recipes_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalSchema: "Oikono",
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SideDishes",
                schema: "Oikono",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SideDishes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SideDishes_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalSchema: "Oikono",
                        principalTable: "User",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SideDishes_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalSchema: "Oikono",
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                schema: "Oikono",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tags_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalSchema: "Oikono",
                        principalTable: "User",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Tags_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalSchema: "Oikono",
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Parts",
                schema: "Oikono",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    RecipeId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Parts_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalSchema: "Oikono",
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Parts_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalSchema: "Oikono",
                        principalTable: "User",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Parts_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalSchema: "Oikono",
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RecipeAssets",
                schema: "Oikono",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    RecipeId = table.Column<Guid>(type: "uuid", nullable: false),
                    AssetId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeAssets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecipeAssets_Assets_AssetId",
                        column: x => x.AssetId,
                        principalSchema: "Oikono",
                        principalTable: "Assets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecipeAssets_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalSchema: "Oikono",
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecipeAssets_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalSchema: "Oikono",
                        principalTable: "User",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RecipeAssets_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalSchema: "Oikono",
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Steps",
                schema: "Oikono",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    RecipeId = table.Column<Guid>(type: "uuid", nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Duration = table.Column<TimeSpan>(type: "interval", nullable: true),
                    Temperature = table.Column<int>(type: "integer", nullable: true),
                    DeviceId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Steps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Steps_Devices_DeviceId",
                        column: x => x.DeviceId,
                        principalSchema: "Oikono",
                        principalTable: "Devices",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Steps_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalSchema: "Oikono",
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Steps_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalSchema: "Oikono",
                        principalTable: "User",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Steps_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalSchema: "Oikono",
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RecipeSideDishes",
                schema: "Oikono",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    RecipeId = table.Column<Guid>(type: "uuid", nullable: false),
                    SideDishId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeSideDishes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecipeSideDishes_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalSchema: "Oikono",
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecipeSideDishes_SideDishes_SideDishId",
                        column: x => x.SideDishId,
                        principalSchema: "Oikono",
                        principalTable: "SideDishes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecipeSideDishes_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalSchema: "Oikono",
                        principalTable: "User",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RecipeSideDishes_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalSchema: "Oikono",
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RecipeTags",
                schema: "Oikono",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    RecipeId = table.Column<Guid>(type: "uuid", nullable: false),
                    TagId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipeTags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecipeTags_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalSchema: "Oikono",
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecipeTags_Tags_TagId",
                        column: x => x.TagId,
                        principalSchema: "Oikono",
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecipeTags_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalSchema: "Oikono",
                        principalTable: "User",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_RecipeTags_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalSchema: "Oikono",
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PartIngredients",
                schema: "Oikono",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    PartId = table.Column<Guid>(type: "uuid", nullable: false),
                    IngredientId = table.Column<Guid>(type: "uuid", nullable: false),
                    Amount = table.Column<double>(type: "double precision", nullable: false),
                    Unit = table.Column<int>(type: "integer", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartIngredients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PartIngredients_Ingredients_IngredientId",
                        column: x => x.IngredientId,
                        principalSchema: "Oikono",
                        principalTable: "Ingredients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PartIngredients_Parts_PartId",
                        column: x => x.PartId,
                        principalSchema: "Oikono",
                        principalTable: "Parts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PartIngredients_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalSchema: "Oikono",
                        principalTable: "User",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PartIngredients_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalSchema: "Oikono",
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Assets_CreatedBy",
                schema: "Oikono",
                table: "Assets",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Assets_UpdatedBy",
                schema: "Oikono",
                table: "Assets",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_CreatedBy",
                schema: "Oikono",
                table: "Devices",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Devices_Name",
                schema: "Oikono",
                table: "Devices",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Devices_UpdatedBy",
                schema: "Oikono",
                table: "Devices",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Ingredients_CreatedBy",
                schema: "Oikono",
                table: "Ingredients",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Ingredients_Name",
                schema: "Oikono",
                table: "Ingredients",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ingredients_UpdatedBy",
                schema: "Oikono",
                table: "Ingredients",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PartIngredients_CreatedBy",
                schema: "Oikono",
                table: "PartIngredients",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_PartIngredients_IngredientId",
                schema: "Oikono",
                table: "PartIngredients",
                column: "IngredientId");

            migrationBuilder.CreateIndex(
                name: "IX_PartIngredients_PartId",
                schema: "Oikono",
                table: "PartIngredients",
                column: "PartId");

            migrationBuilder.CreateIndex(
                name: "IX_PartIngredients_UpdatedBy",
                schema: "Oikono",
                table: "PartIngredients",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Parts_CreatedBy",
                schema: "Oikono",
                table: "Parts",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Parts_RecipeId",
                schema: "Oikono",
                table: "Parts",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_Parts_UpdatedBy",
                schema: "Oikono",
                table: "Parts",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeAssets_AssetId",
                schema: "Oikono",
                table: "RecipeAssets",
                column: "AssetId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeAssets_CreatedBy",
                schema: "Oikono",
                table: "RecipeAssets",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeAssets_RecipeId",
                schema: "Oikono",
                table: "RecipeAssets",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeAssets_UpdatedBy",
                schema: "Oikono",
                table: "RecipeAssets",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_CreatedBy",
                schema: "Oikono",
                table: "Recipes",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_UpdatedBy",
                schema: "Oikono",
                table: "Recipes",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeSideDishes_CreatedBy",
                schema: "Oikono",
                table: "RecipeSideDishes",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeSideDishes_RecipeId_SideDishId",
                schema: "Oikono",
                table: "RecipeSideDishes",
                columns: new[] { "RecipeId", "SideDishId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RecipeSideDishes_SideDishId",
                schema: "Oikono",
                table: "RecipeSideDishes",
                column: "SideDishId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeSideDishes_UpdatedBy",
                schema: "Oikono",
                table: "RecipeSideDishes",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeTags_CreatedBy",
                schema: "Oikono",
                table: "RecipeTags",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeTags_RecipeId_TagId",
                schema: "Oikono",
                table: "RecipeTags",
                columns: new[] { "RecipeId", "TagId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RecipeTags_TagId",
                schema: "Oikono",
                table: "RecipeTags",
                column: "TagId");

            migrationBuilder.CreateIndex(
                name: "IX_RecipeTags_UpdatedBy",
                schema: "Oikono",
                table: "RecipeTags",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SideDishes_CreatedBy",
                schema: "Oikono",
                table: "SideDishes",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SideDishes_Name",
                schema: "Oikono",
                table: "SideDishes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SideDishes_UpdatedBy",
                schema: "Oikono",
                table: "SideDishes",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Steps_CreatedBy",
                schema: "Oikono",
                table: "Steps",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Steps_DeviceId",
                schema: "Oikono",
                table: "Steps",
                column: "DeviceId");

            migrationBuilder.CreateIndex(
                name: "IX_Steps_RecipeId",
                schema: "Oikono",
                table: "Steps",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_Steps_UpdatedBy",
                schema: "Oikono",
                table: "Steps",
                column: "UpdatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_CreatedBy",
                schema: "Oikono",
                table: "Tags",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_Tags_Name",
                schema: "Oikono",
                table: "Tags",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tags_UpdatedBy",
                schema: "Oikono",
                table: "Tags",
                column: "UpdatedBy");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PartIngredients",
                schema: "Oikono");

            migrationBuilder.DropTable(
                name: "RecipeAssets",
                schema: "Oikono");

            migrationBuilder.DropTable(
                name: "RecipeSideDishes",
                schema: "Oikono");

            migrationBuilder.DropTable(
                name: "RecipeTags",
                schema: "Oikono");

            migrationBuilder.DropTable(
                name: "Steps",
                schema: "Oikono");

            migrationBuilder.DropTable(
                name: "Ingredients",
                schema: "Oikono");

            migrationBuilder.DropTable(
                name: "Parts",
                schema: "Oikono");

            migrationBuilder.DropTable(
                name: "Assets",
                schema: "Oikono");

            migrationBuilder.DropTable(
                name: "SideDishes",
                schema: "Oikono");

            migrationBuilder.DropTable(
                name: "Tags",
                schema: "Oikono");

            migrationBuilder.DropTable(
                name: "Devices",
                schema: "Oikono");

            migrationBuilder.DropTable(
                name: "Recipes",
                schema: "Oikono");

            migrationBuilder.CreateTable(
                name: "WeatherForecast",
                schema: "Oikono",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Summary = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    TemperatureC = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeatherForecast", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WeatherForecast_User_CreatedBy",
                        column: x => x.CreatedBy,
                        principalSchema: "Oikono",
                        principalTable: "User",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WeatherForecast_User_UpdatedBy",
                        column: x => x.UpdatedBy,
                        principalSchema: "Oikono",
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_WeatherForecast_CreatedBy",
                schema: "Oikono",
                table: "WeatherForecast",
                column: "CreatedBy");

            migrationBuilder.CreateIndex(
                name: "IX_WeatherForecast_UpdatedBy",
                schema: "Oikono",
                table: "WeatherForecast",
                column: "UpdatedBy");
        }
    }
}
