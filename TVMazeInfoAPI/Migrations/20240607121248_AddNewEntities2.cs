using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TVMazeInfoAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddNewEntities2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shows_Image_ImageId",
                table: "Shows");

            migrationBuilder.DropForeignKey(
                name: "FK_Shows_Rating_RatingId",
                table: "Shows");

            migrationBuilder.DropIndex(
                name: "IX_Shows_RatingId",
                table: "Shows");

            migrationBuilder.RenameColumn(
                name: "RatingId",
                table: "Shows",
                newName: "Weight");

            migrationBuilder.RenameColumn(
                name: "ImageId",
                table: "Shows",
                newName: "WebChannelId");

            migrationBuilder.RenameIndex(
                name: "IX_Shows_ImageId",
                table: "Shows",
                newName: "IX_Shows_WebChannelId");

            migrationBuilder.AddColumn<int>(
                name: "AverageRuntime",
                table: "Shows",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DvdCountry",
                table: "Shows",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Ended",
                table: "Shows",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Schedule",
                table: "Shows",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Updated",
                table: "Shows",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Url",
                table: "Shows",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ShowId",
                table: "Rating",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ShowId",
                table: "Image",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Externals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Tvrage = table.Column<int>(type: "int", nullable: true),
                    Thetvdb = table.Column<int>(type: "int", nullable: true),
                    Imdb = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShowId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Externals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Externals_Shows_ShowId",
                        column: x => x.ShowId,
                        principalTable: "Shows",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PreviousEpisode",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Href = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreviousEpisode", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Self",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Href = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Self", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WebChannel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CountryCode = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    OfficialSite = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WebChannel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WebChannel_Countries_CountryCode",
                        column: x => x.CountryCode,
                        principalTable: "Countries",
                        principalColumn: "Code");
                });

            migrationBuilder.CreateTable(
                name: "Links",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SelfId = table.Column<int>(type: "int", nullable: true),
                    PreviousEpisodeId = table.Column<int>(type: "int", nullable: true),
                    ShowId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Links", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Links_PreviousEpisode_PreviousEpisodeId",
                        column: x => x.PreviousEpisodeId,
                        principalTable: "PreviousEpisode",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Links_Self_SelfId",
                        column: x => x.SelfId,
                        principalTable: "Self",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Links_Shows_ShowId",
                        column: x => x.ShowId,
                        principalTable: "Shows",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rating_ShowId",
                table: "Rating",
                column: "ShowId",
                unique: true,
                filter: "[ShowId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Image_ShowId",
                table: "Image",
                column: "ShowId",
                unique: true,
                filter: "[ShowId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Externals_ShowId",
                table: "Externals",
                column: "ShowId",
                unique: true,
                filter: "[ShowId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Links_PreviousEpisodeId",
                table: "Links",
                column: "PreviousEpisodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Links_SelfId",
                table: "Links",
                column: "SelfId");

            migrationBuilder.CreateIndex(
                name: "IX_Links_ShowId",
                table: "Links",
                column: "ShowId",
                unique: true,
                filter: "[ShowId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_WebChannel_CountryCode",
                table: "WebChannel",
                column: "CountryCode");

            migrationBuilder.AddForeignKey(
                name: "FK_Image_Shows_ShowId",
                table: "Image",
                column: "ShowId",
                principalTable: "Shows",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Rating_Shows_ShowId",
                table: "Rating",
                column: "ShowId",
                principalTable: "Shows",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Shows_WebChannel_WebChannelId",
                table: "Shows",
                column: "WebChannelId",
                principalTable: "WebChannel",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Image_Shows_ShowId",
                table: "Image");

            migrationBuilder.DropForeignKey(
                name: "FK_Rating_Shows_ShowId",
                table: "Rating");

            migrationBuilder.DropForeignKey(
                name: "FK_Shows_WebChannel_WebChannelId",
                table: "Shows");

            migrationBuilder.DropTable(
                name: "Externals");

            migrationBuilder.DropTable(
                name: "Links");

            migrationBuilder.DropTable(
                name: "WebChannel");

            migrationBuilder.DropTable(
                name: "PreviousEpisode");

            migrationBuilder.DropTable(
                name: "Self");

            migrationBuilder.DropIndex(
                name: "IX_Rating_ShowId",
                table: "Rating");

            migrationBuilder.DropIndex(
                name: "IX_Image_ShowId",
                table: "Image");

            migrationBuilder.DropColumn(
                name: "AverageRuntime",
                table: "Shows");

            migrationBuilder.DropColumn(
                name: "DvdCountry",
                table: "Shows");

            migrationBuilder.DropColumn(
                name: "Ended",
                table: "Shows");

            migrationBuilder.DropColumn(
                name: "Schedule",
                table: "Shows");

            migrationBuilder.DropColumn(
                name: "Updated",
                table: "Shows");

            migrationBuilder.DropColumn(
                name: "Url",
                table: "Shows");

            migrationBuilder.DropColumn(
                name: "ShowId",
                table: "Rating");

            migrationBuilder.DropColumn(
                name: "ShowId",
                table: "Image");

            migrationBuilder.RenameColumn(
                name: "Weight",
                table: "Shows",
                newName: "RatingId");

            migrationBuilder.RenameColumn(
                name: "WebChannelId",
                table: "Shows",
                newName: "ImageId");

            migrationBuilder.RenameIndex(
                name: "IX_Shows_WebChannelId",
                table: "Shows",
                newName: "IX_Shows_ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_Shows_RatingId",
                table: "Shows",
                column: "RatingId");

            migrationBuilder.AddForeignKey(
                name: "FK_Shows_Image_ImageId",
                table: "Shows",
                column: "ImageId",
                principalTable: "Image",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Shows_Rating_RatingId",
                table: "Shows",
                column: "RatingId",
                principalTable: "Rating",
                principalColumn: "Id");
        }
    }
}
