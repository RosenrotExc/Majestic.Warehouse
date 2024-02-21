using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Majestic.WarehouseService.Repository.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CarEntityCode",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarEntityCode", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Initiator",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Initiator", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "State",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_State", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CarEntity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CarName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    ModelName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    OwnerName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    OwnersPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DealersPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DealerNotes = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    CodeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CarEntity_CarEntityCode_CodeId",
                        column: x => x.CodeId,
                        principalTable: "CarEntityCode",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CarEntityState",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EntityId = table.Column<int>(type: "int", nullable: false),
                    RefId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ETag = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpireDateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    StateId = table.Column<int>(type: "int", nullable: false),
                    InitiatorId = table.Column<int>(type: "int", nullable: false),
                    Task = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarEntityState", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CarEntityState_CarEntity_EntityId",
                        column: x => x.EntityId,
                        principalTable: "CarEntity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CarEntityState_Initiator_InitiatorId",
                        column: x => x.InitiatorId,
                        principalTable: "Initiator",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CarEntityState_State_StateId",
                        column: x => x.StateId,
                        principalTable: "State",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CarEntity_CodeId",
                table: "CarEntity",
                column: "CodeId");

            migrationBuilder.CreateIndex(
                name: "IX_CarEntityCode_Id",
                table: "CarEntityCode",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CarEntityCode_Value",
                table: "CarEntityCode",
                column: "Value",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CarEntityState_CreateDateTime_ExpireDateTime_EntityId",
                table: "CarEntityState",
                columns: new[] { "CreateDateTime", "ExpireDateTime", "EntityId" });

            migrationBuilder.CreateIndex(
                name: "IX_CarEntityState_EntityId",
                table: "CarEntityState",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_CarEntityState_InitiatorId_ETag_StateId",
                table: "CarEntityState",
                columns: new[] { "InitiatorId", "ETag", "StateId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CarEntityState_RefId",
                table: "CarEntityState",
                column: "RefId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CarEntityState_StateId",
                table: "CarEntityState",
                column: "StateId");

            migrationBuilder.CreateIndex(
                name: "IX_Initiator_Value",
                table: "Initiator",
                column: "Value",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_State_Value",
                table: "State",
                column: "Value",
                unique: true);

            #region Populate states
            {
                var sqlScript = "";
                foreach (Models.Internal.State.Values value in Enum.GetValues(typeof(Models.Internal.State.Values)))
                {
                    sqlScript += $"INSERT INTO dbo.State (Value) VALUES ('{value}');\n";
                }

                migrationBuilder.Sql(sqlScript);
            }
            #endregion
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CarEntityState");

            migrationBuilder.DropTable(
                name: "CarEntity");

            migrationBuilder.DropTable(
                name: "Initiator");

            migrationBuilder.DropTable(
                name: "State");

            migrationBuilder.DropTable(
                name: "CarEntityCode");
        }
    }
}
