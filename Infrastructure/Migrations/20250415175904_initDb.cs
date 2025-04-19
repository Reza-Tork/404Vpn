using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class initDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApiInfos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Token = table.Column<string>(type: "text", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiInfos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BotMessages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Command = table.Column<int>(type: "integer", nullable: false),
                    Message = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BotMessages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BotSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Key = table.Column<string>(type: "text", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BotSettings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Discounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    DiscountPercent = table.Column<int>(type: "integer", nullable: false),
                    Capacity = table.Column<int>(type: "integer", nullable: false),
                    ExpireDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Discounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MonthPlans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Month = table.Column<int>(type: "integer", nullable: false),
                    PricePerMonth = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonthPlans", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Tags = table.Column<string>(type: "text", nullable: false),
                    Index = table.Column<int>(type: "integer", nullable: false),
                    Capacity = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Price = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: true),
                    FirstName = table.Column<string>(type: "text", nullable: true),
                    LastName = table.Column<string>(type: "text", nullable: true),
                    JoinDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Step = table.Column<int>(type: "integer", nullable: false),
                    StepData = table.Column<string>(type: "text", nullable: true),
                    DiscountId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_Discounts_DiscountId",
                        column: x => x.DiscountId,
                        principalTable: "Discounts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TrafficPlans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Bandwidth = table.Column<int>(type: "integer", nullable: false),
                    PricePerGb = table.Column<int>(type: "integer", nullable: false),
                    MonthPlanId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrafficPlans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrafficPlans_MonthPlans_MonthPlanId",
                        column: x => x.MonthPlanId,
                        principalTable: "MonthPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Admins",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    Step = table.Column<int>(type: "integer", nullable: false),
                    StepData = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Admins_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UsersSubscriptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Username = table.Column<string>(type: "text", nullable: false),
                    Bandwidth = table.Column<int>(type: "integer", nullable: false),
                    ExpireTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    ServiceId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UsersSubscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UsersSubscriptions_Services_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "Services",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UsersSubscriptions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Wallets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Balance = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wallets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Wallets_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Factors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Amount = table.Column<int>(type: "integer", nullable: false),
                    UniqueKey = table.Column<string>(type: "text", nullable: false),
                    State = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    UserSubscriptionId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Factors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Factors_UsersSubscriptions_UserSubscriptionId",
                        column: x => x.UserSubscriptionId,
                        principalTable: "UsersSubscriptions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Factors_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "BotMessages",
                columns: new[] { "Id", "Command", "Message" },
                values: new object[,]
                {
                    { 1, 1, "پیام کامند استارت" },
                    { 2, 2, "پیام خرید سرویس - مرحله انتخاب سرویس" },
                    { 3, 3, "پیام تمدید سرویس - مرحله انتخاب سرویس جهت تمدید" },
                    { 4, 4, "پیام سرویس های من - مرحله نمایش سرویس ها" },
                    { 5, 5, "حجم اضافه - انتخاب سرویس جهت افزودن حجم" },
                    { 6, 6, "TextMessage-PlansMessage" },
                    { 7, 7, "پیام کیف پول - نمایش بالانس" },
                    { 8, 8, "پیام پشتیبانی - نمایش ایدی اکانت پشتیبانی" },
                    { 9, 9, "پیام راهنما" },
                    { 10, 10, "به منوی اصلی بازگشتید" },
                    { 11, 11, "سرویس انتخاب شده: <NAME>\r\nانتخاب مدت زمان سرویس:" },
                    { 12, 12, "سرویس انتخاب شده: <NAME>\r\nمدت زمان سرویس: <MONTH>\r\nانتخاب ترافیک:" },
                    { 13, 13, "سرویس انتخاب شده: <NAME>\r\nمدت زمان سرویس: <MONTH>\r\nمقدار ترافیک: <TRAFFIC>\r\nمبلغ نهایی: <PRICE>\r\nفاکتور ساخته شد ، انتخاب روش پرداخت: " },
                    { 14, 14, "نام نمایشی: <TITLE>\r\nسرویس: <SERVICE>\r\nوضعیت: <STATUS>\r\nیادداشت: <NOTE>" },
                    { 15, 15, "سرویس انتخاب شده - وارد کردن حجم" },
                    { 16, 16, "سرویس انتخاب شده جهت تمدید - تعداد ماه" },
                    { 17, 17, "شارژ ولت انتخاب شده - وارد کردن مبلغ" },
                    { 18, 18, "مبلغ وارد شده: <AMOUNT>\r\nروش پرداخت را انتخاب کنید:" },
                    { 19, 19, "روش پرداخت: کارت به کارت\r\nمبلغ: <code><AMOUNT></code>\r\nشماره کارت: <code><CARD></code>\r\nبه شماره کارت بالا واریز کنید و رسید بفرستید" }
                });

            migrationBuilder.InsertData(
                table: "BotSettings",
                columns: new[] { "Id", "Key", "Value" },
                values: new object[,]
                {
                    { 1, "DOMAIN", "https://library98.ir/" },
                    { 2, "STATUS", "1" },
                    { 3, "MIN_AMOUNT", "50000" },
                    { 4, "MAX_AMOUNT", "500000" },
                    { 5, "RECEIPT_CHATID", "-1002583876730" },
                    { 6, "CARD", "0000000000000000" }
                });

            migrationBuilder.InsertData(
                table: "MonthPlans",
                columns: new[] { "Id", "Month", "PricePerMonth" },
                values: new object[,]
                {
                    { 1, 1, 5000 },
                    { 2, 3, 3000 },
                    { 3, 6, 2000 }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "DiscountId", "FirstName", "JoinDate", "LastName", "Step", "StepData", "UserId", "Username" },
                values: new object[] { 1, null, "Main", new DateTime(2025, 4, 15, 17, 59, 3, 515, DateTimeKind.Utc).AddTicks(3298), "Admin", 0, "", 7880935437L, "MrMorphling" });

            migrationBuilder.InsertData(
                table: "Admins",
                columns: new[] { "Id", "Step", "StepData", "UserId" },
                values: new object[] { 1, 0, null, 1 });

            migrationBuilder.InsertData(
                table: "TrafficPlans",
                columns: new[] { "Id", "Bandwidth", "MonthPlanId", "PricePerGb" },
                values: new object[,]
                {
                    { 1, 15, 1, 3000 },
                    { 2, 30, 1, 2850 },
                    { 3, 45, 1, 2750 },
                    { 4, 30, 2, 2750 },
                    { 5, 60, 2, 2650 },
                    { 6, 90, 2, 2500 },
                    { 7, 150, 3, 2000 },
                    { 8, 200, 3, 1800 },
                    { 9, 450, 3, 1500 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Admins_UserId",
                table: "Admins",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Factors_UserId",
                table: "Factors",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Factors_UserSubscriptionId",
                table: "Factors",
                column: "UserSubscriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_TrafficPlans_MonthPlanId",
                table: "TrafficPlans",
                column: "MonthPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_DiscountId",
                table: "Users",
                column: "DiscountId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersSubscriptions_ServiceId",
                table: "UsersSubscriptions",
                column: "ServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_UsersSubscriptions_UserId",
                table: "UsersSubscriptions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Wallets_UserId",
                table: "Wallets",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Admins");

            migrationBuilder.DropTable(
                name: "ApiInfos");

            migrationBuilder.DropTable(
                name: "BotMessages");

            migrationBuilder.DropTable(
                name: "BotSettings");

            migrationBuilder.DropTable(
                name: "Factors");

            migrationBuilder.DropTable(
                name: "TrafficPlans");

            migrationBuilder.DropTable(
                name: "Wallets");

            migrationBuilder.DropTable(
                name: "UsersSubscriptions");

            migrationBuilder.DropTable(
                name: "MonthPlans");

            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Discounts");
        }
    }
}
