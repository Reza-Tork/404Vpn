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
                    Title = table.Column<string>(type: "text", nullable: true),
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
                    { 1, 1, "🌐 سلام <NAME> عزیز!\r\nبه ربات رسمی وی‌پی‌ان 404 خوش اومدی! 🚀\r\n\r\nاینجا قراره با چند کلیک ساده، به یه اینترنت آزاد، پرسرعت و امن دسترسی داشته باشی! 🔓⚡️\r\nما بهت قول می‌دیم دیگه با محدودیت خداحافظی کنی! 💥\r\n\r\nبرای شروع، یکی از گزینه‌های زیر رو انتخاب کن و از تجربه بی‌دغدغه اینترنت لذت ببر 😎👇\r\n\r\n✨ امنیت بالا، سرعت خفن، قیمت منصفانه\r\n💬 هر سوالی داشتی، <a href='https://t.me/the404vpnSupport'>پشتیبانی</a> همیشه آنلاینه!\r\n\r\n🆔 @the404vpnRobot" },
                    { 2, 2, "🛍 انتخاب سرویس، قدم اول به سوی آزادی بی‌مرز در اینترنت!\r\n\r\n📦 ما برات مجموعه‌ای از سرویس‌های حرفه‌ای آماده کردیم که هم از نظر سرعت و امنیت فوق‌العاده‌ان، هم از نظر قیمت، کاملاً منصفانه! \r\n\r\n✨ فقط کافیه پلنی که مناسب توئه رو انتخاب کنی و در کمتر از چند ثانیه، وصل شی به یه دنیای بدون محدودیت!\r\n\r\n💡 نکته مهم: هر سرویس با توجه به نیازت طراحی شده \r\nچه کاربر روزمره باشی، تریدر, استریمر یا گیمر حرفه‌ای 🎮\r\n\r\n👇 از لیست زیر پلن دلخواهتو انتخاب کن و برو برای اتصال بی‌دغدغه!" },
                    { 3, 3, "♻️ یکی از سرویس هایه مورد نظر برای تمدیدت رو انتخاب کن" },
                    { 4, 4, "📦 سرویس‌های فعال شما\r\n\r\nاینجا می‌تونی مشخصات همه‌ی سرویس‌هات رو ببینی، وضعیتشون رو چک کنی و اگه لازم بود تمدید یا ارتقاشون بدی \r\n👇 لیست سرویس‌های شما:" },
                    { 5, 5, "🔋 افزایش حجم سرویس\r\n\r\nسرویس انتخاب‌شده: <TITLE> ✅\r\nحالا فقط کافیه مقدار حجمی که نیاز داری رو انتخاب کنی 👇\r\n\r\n📦 هر چقدر بیشتر، اتصال طولانی‌تر و بی‌دردسرتر!" },
                    { 6, 6, "TextMessage-PlansMessage" },
                    { 7, 7, "💰 کیف پول شما\r\nاز موجودیت می‌تونی برای خرید، تمدید یا افزایش حجم استفاده کنی ✨\r\n\r\nهمچنین برای شارژ حساب روی دکمه زیر کلیک کنید" },
                    { 8, 8, "🛟 نیاز به راهنمایی یا مشکلی پیش اومده؟\r\n\r\nنگران نباش، تیم پشتیبانی ما اینجاست تا هر زمان که نیاز داشتی، کنارت باشه 🙌\r\nچه مشکلت مربوط به خرید، اتصال، یا هر مورد دیگه‌ای باشه، فقط کافیه به پشتیبانی پیام بدی تا سریع راهنماییت کنیم 🛠💬\r\n\r\n🆔 آیدی پشتیبانی:\r\n@the404vpnSupport\r\n\r\n🕒 ساعات پاسخ‌گویی: شبانه روز, حتی روزهای تعطیل!\r\n\r\n✨ ما بهت قول می‌دیم تجربه‌ات از 404 نت همیشه راحت، شفاف و بدون دغدغه باشه.\r\nدر هر مرحله‌ای که بودی، پشتیبانی یه پیام باهاته 🤝" },
                    { 9, 9, "پیام راهنما" },
                    { 10, 10, "🌐 سلام <NAME> عزیز!\r\nبه ربات رسمی وی‌پی‌ان 404 خوش اومدی! 🚀\r\n\r\nاینجا قراره با چند کلیک ساده، به یه اینترنت آزاد، پرسرعت و امن دسترسی داشته باشی! 🔓⚡️\r\nما بهت قول می‌دیم دیگه با محدودیت خداحافظی کنی! 💥\r\n\r\nبرای شروع، یکی از گزینه‌های زیر رو انتخاب کن و از تجربه بی‌دغدغه اینترنت لذت ببر 😎👇\r\n\r\n✨ امنیت بالا، سرعت خفن، قیمت منصفانه\r\n💬 هر سوالی داشتی، <a href='https://t.me/the404vpnSupport'>پشتیبانی</a> همیشه آنلاینه!\r\n\r\n🆔 @the404vpnRobot" },
                    { 11, 11, "سرویس انتخاب شده: <NAME>\r\nانتخاب مدت زمان سرویس:" },
                    { 12, 12, "سرویس انتخاب شده: <NAME>\r\nمدت زمان سرویس: <MONTH>\r\nانتخاب ترافیک:" },
                    { 13, 13, "🧾 فاکتور سرویس شما آماده‌ست!\r\n\r\nهمه‌چیز برای شروع یه تجربه سریع، امن و بدون محدودیت آماده‌ست! 🚀\r\n\r\n🔹 سرویس انتخاب‌شده: <NAME>\r\n📅 مدت زمان: <MONTH> ماه\r\n📦 حجم: <TRAFFIC> گیگ\r\n\r\n💰 مبلغ نهایی: <PRICE> تومان\r\n\r\n✨ با پرداخت این فاکتور، فقط چند ثانیه تا اتصال به یه اینترنت پایدار و پرسرعت فاصله داری!\r\n👇 حالا فقط کافیه روش پرداختتو انتخاب کنی و بریم برای فعال‌سازی" },
                    { 14, 14, "🧾 جزئیات سرویس شما\r\n\r\n📌 نام نمایشی: <TITLE>\r\n🌐 سرویس: <SERVICE>\r\n📶 وضعیت: <STATUS>\r\n📝 یادداشت: <NOTE>\r\n\r\n✨ برای مدیریت سرویس‌ات میتونی از دکمه های زیر استفاده کنی" },
                    { 15, 15, "سرویس انتخاب شده - وارد کردن حجم" },
                    { 16, 16, "سرویس انتخاب شده جهت تمدید - تعداد ماه" },
                    { 17, 17, "💳 شارژ کیف پول\r\n📌 با شارژ کیف پول، خرید و تمدید سرویس‌ها سریع‌تر و راحت‌تر انجام می‌شه!\r\n\r\nحالا مبلغ موردنظرت رو برای شارژ حساب وارد کن 👇" },
                    { 18, 18, "💰 مبلغ وارد شده: <AMOUNT>\r\n\r\nحالا فقط کافیه روش پرداختت رو انتخاب کنی 👇" },
                    { 19, 19, "روش پرداخت: کارت به کارت🏦 روش پرداخت: کارت به کارت\r\n\r\n💳 مبلغ: <code><AMOUNT></code>\r\n\r\n🔢 شماره کارت: <code><CARD></code>\r\n\r\nلطفاً مبلغ رو به شماره کارت بالا واریز کن و رسید پرداخت رو همینجا برای ما ارسال کن ✅\r\n📌 بعد از تایید، کیف پولت به‌صورت خودکار شارژ می‌شه و شمارو در اطلاع میزاریم\r\n\r\n🛟 مشکلی داشتی؟ پشتیبانی همیشه در دسترسته: @the404vpnSupport" },
                    { 22, 22, "✨ جزئیات سرویس اختصاصی شما\r\n📌 نام نمایشی: <code><TITLE></code>\r\n🌐 نوع سرویس: <code><SERVICE></code>\r\n⚙️ وضعیت فعلی: <code><STATUS></code>\r\n📝 یادداشت اختصاصی: <code><NOTE></code>\r\n\r\n🕰 تاریخ شروع: <b><CREATE></b>\r\n⏳ تاریخ پایان: <b><EXPIRE></b>\r\n📊 مصرف شما تا این لحظه:\r\n<USED> گیگ از <BANDWIDTH> گیگ\r\n" }
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
                values: new object[] { 1, null, "Main", new DateTime(2025, 4, 19, 10, 20, 14, 158, DateTimeKind.Utc).AddTicks(6503), "Admin", 0, "", 7880935437L, "MrMorphling" });

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
