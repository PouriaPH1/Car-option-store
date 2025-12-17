using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogManagement.Infrastructure.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class InitSqlite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ArticleCategories",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    Picture = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    PictureAlt = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    PictureTitle = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    Description = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: true),
                    ShowOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    Slug = table.Column<string>(type: "TEXT", maxLength: 600, nullable: true),
                    Keywords = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    MetaDescription = table.Column<string>(type: "TEXT", maxLength: 150, nullable: true),
                    CanonicalAddress = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    CreationDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Articles",
                columns: table => new
                {
                    Id = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    ShortDescription = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    Picture = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    PictureAlt = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    PictureTitle = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    PublishDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Slug = table.Column<string>(type: "TEXT", maxLength: 600, nullable: true),
                    Keywords = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    MetaDescription = table.Column<string>(type: "TEXT", maxLength: 150, nullable: true),
                    CanonicalAddress = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    CategoryId = table.Column<long>(type: "INTEGER", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Articles_ArticleCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "ArticleCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Articles_CategoryId",
                table: "Articles",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Articles");

            migrationBuilder.DropTable(
                name: "ArticleCategories");
        }
    }
}
