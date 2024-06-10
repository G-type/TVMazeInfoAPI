using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TVMazeInfoAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddNewEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Genres",
                table: "Shows",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ImageId",
                table: "Shows",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Language",
                table: "Shows",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Shows",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NetworkId",
                table: "Shows",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OfficialSite",
                table: "Shows",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Premiered",
                table: "Shows",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RatingId",
                table: "Shows",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Runtime",
                table: "Shows",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Shows",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Summary",
                table: "Shows",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Shows",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    Code = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Timezone = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Code);
                });

            migrationBuilder.CreateTable(
                name: "Image",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Medium = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Original = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Image", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rating",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Average = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rating", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Networks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CountryCode = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Networks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Networks_Countries_CountryCode",
                        column: x => x.CountryCode,
                        principalTable: "Countries",
                        principalColumn: "Code");
                });

            migrationBuilder.CreateTable(
                name: "Characters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Characters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Characters_Image_ImageId",
                        column: x => x.ImageId,
                        principalTable: "Image",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Episodes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Season = table.Column<int>(type: "int", nullable: true),
                    Number = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Airdate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Runtime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Summary = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImageId = table.Column<int>(type: "int", nullable: true),
                    ShowId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Episodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Episodes_Image_ImageId",
                        column: x => x.ImageId,
                        principalTable: "Image",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Episodes_Shows_ShowId",
                        column: x => x.ShowId,
                        principalTable: "Shows",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "People",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Birthday = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CountryCode = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ImageId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_People", x => x.Id);
                    table.ForeignKey(
                        name: "FK_People_Countries_CountryCode",
                        column: x => x.CountryCode,
                        principalTable: "Countries",
                        principalColumn: "Code");
                    table.ForeignKey(
                        name: "FK_People_Image_ImageId",
                        column: x => x.ImageId,
                        principalTable: "Image",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Casts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonId = table.Column<int>(type: "int", nullable: true),
                    CharacterId = table.Column<int>(type: "int", nullable: true),
                    ShowId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Casts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Casts_Characters_CharacterId",
                        column: x => x.CharacterId,
                        principalTable: "Characters",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Casts_People_PersonId",
                        column: x => x.PersonId,
                        principalTable: "People",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Casts_Shows_ShowId",
                        column: x => x.ShowId,
                        principalTable: "Shows",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Crews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PersonId = table.Column<int>(type: "int", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShowId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Crews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Crews_People_PersonId",
                        column: x => x.PersonId,
                        principalTable: "People",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Crews_Shows_ShowId",
                        column: x => x.ShowId,
                        principalTable: "Shows",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Shows_ImageId",
                table: "Shows",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_Shows_NetworkId",
                table: "Shows",
                column: "NetworkId",
                unique: true,
                filter: "[NetworkId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Shows_RatingId",
                table: "Shows",
                column: "RatingId");

            migrationBuilder.CreateIndex(
                name: "IX_Casts_CharacterId",
                table: "Casts",
                column: "CharacterId");

            migrationBuilder.CreateIndex(
                name: "IX_Casts_PersonId",
                table: "Casts",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Casts_ShowId",
                table: "Casts",
                column: "ShowId");

            migrationBuilder.CreateIndex(
                name: "IX_Characters_ImageId",
                table: "Characters",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_Crews_PersonId",
                table: "Crews",
                column: "PersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Crews_ShowId",
                table: "Crews",
                column: "ShowId");

            migrationBuilder.CreateIndex(
                name: "IX_Episodes_ImageId",
                table: "Episodes",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_Episodes_ShowId",
                table: "Episodes",
                column: "ShowId");

            migrationBuilder.CreateIndex(
                name: "IX_Networks_CountryCode",
                table: "Networks",
                column: "CountryCode",
                unique: true,
                filter: "[CountryCode] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_People_CountryCode",
                table: "People",
                column: "CountryCode");

            migrationBuilder.CreateIndex(
                name: "IX_People_ImageId",
                table: "People",
                column: "ImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Shows_Image_ImageId",
                table: "Shows",
                column: "ImageId",
                principalTable: "Image",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Shows_Networks_NetworkId",
                table: "Shows",
                column: "NetworkId",
                principalTable: "Networks",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Shows_Rating_RatingId",
                table: "Shows",
                column: "RatingId",
                principalTable: "Rating",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shows_Image_ImageId",
                table: "Shows");

            migrationBuilder.DropForeignKey(
                name: "FK_Shows_Networks_NetworkId",
                table: "Shows");

            migrationBuilder.DropForeignKey(
                name: "FK_Shows_Rating_RatingId",
                table: "Shows");

            migrationBuilder.DropTable(
                name: "Casts");

            migrationBuilder.DropTable(
                name: "Crews");

            migrationBuilder.DropTable(
                name: "Episodes");

            migrationBuilder.DropTable(
                name: "Networks");

            migrationBuilder.DropTable(
                name: "Rating");

            migrationBuilder.DropTable(
                name: "Characters");

            migrationBuilder.DropTable(
                name: "People");

            migrationBuilder.DropTable(
                name: "Countries");

            migrationBuilder.DropTable(
                name: "Image");

            migrationBuilder.DropIndex(
                name: "IX_Shows_ImageId",
                table: "Shows");

            migrationBuilder.DropIndex(
                name: "IX_Shows_NetworkId",
                table: "Shows");

            migrationBuilder.DropIndex(
                name: "IX_Shows_RatingId",
                table: "Shows");

            migrationBuilder.DropColumn(
                name: "Genres",
                table: "Shows");

            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "Shows");

            migrationBuilder.DropColumn(
                name: "Language",
                table: "Shows");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Shows");

            migrationBuilder.DropColumn(
                name: "NetworkId",
                table: "Shows");

            migrationBuilder.DropColumn(
                name: "OfficialSite",
                table: "Shows");

            migrationBuilder.DropColumn(
                name: "Premiered",
                table: "Shows");

            migrationBuilder.DropColumn(
                name: "RatingId",
                table: "Shows");

            migrationBuilder.DropColumn(
                name: "Runtime",
                table: "Shows");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Shows");

            migrationBuilder.DropColumn(
                name: "Summary",
                table: "Shows");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Shows");
        }
    }
}
