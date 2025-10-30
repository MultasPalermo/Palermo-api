using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Entity.Migrations
{
    /// <inheritdoc />
    public partial class sqlserver : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "ModelSecurity");

            migrationBuilder.EnsureSchema(
                name: "Parameters");

            migrationBuilder.EnsureSchema(
                name: "Entities");

            migrationBuilder.CreateTable(
                name: "AuthSession",
                schema: "ModelSecurity",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SessionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PersonId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastActivityAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AbsoluteExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsRevoked = table.Column<bool>(type: "bit", nullable: false),
                    Ip = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    UserAgent = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    active = table.Column<bool>(type: "bit", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthSession", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "department",
                schema: "Parameters",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: false),
                    daneCode = table.Column<int>(type: "int", nullable: false),
                    active = table.Column<bool>(type: "bit", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_department", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "documentType",
                schema: "Parameters",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: false),
                    abbreviation = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    active = table.Column<bool>(type: "bit", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_documentType", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "form",
                schema: "ModelSecurity",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Route = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Icon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    active = table.Column<bool>(type: "bit", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_form", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "InspectoraReport",
                schema: "Entities",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    report_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    total_fines = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    active = table.Column<bool>(type: "bit", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InspectoraReport", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "module",
                schema: "ModelSecurity",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    active = table.Column<bool>(type: "bit", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_module", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "paymentFrequency",
                schema: "Parameters",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    intervalPage = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    dueDayOfMonth = table.Column<int>(type: "int", nullable: false),
                    active = table.Column<bool>(type: "bit", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_paymentFrequency", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "permission",
                schema: "ModelSecurity",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    active = table.Column<bool>(type: "bit", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_permission", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "refreshTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    TokenHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsRevoked = table.Column<bool>(type: "bit", nullable: false),
                    ReplacedByTokenHash = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_refreshTokens", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "rol",
                schema: "ModelSecurity",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    active = table.Column<bool>(type: "bit", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rol", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "TypeInfraction",
                schema: "Entities",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    active = table.Column<bool>(type: "bit", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TypeInfraction", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "typePayment",
                schema: "Entities",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    paymentAgreementId = table.Column<int>(type: "int", nullable: false),
                    active = table.Column<bool>(type: "bit", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_typePayment", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "userNotification",
                schema: "Entities",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    shippingDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    active = table.Column<bool>(type: "bit", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userNotification", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "valueSmldv",
                schema: "Entities",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    value_smldv = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Current_Year = table.Column<int>(type: "int", nullable: false),
                    minimunWage = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    active = table.Column<bool>(type: "bit", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_valueSmldv", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "municipality",
                schema: "Parameters",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: false),
                    daneCode = table.Column<int>(type: "int", nullable: false),
                    departmentId = table.Column<int>(type: "int", nullable: false),
                    active = table.Column<bool>(type: "bit", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_municipality", x => x.id);
                    table.ForeignKey(
                        name: "FK_municipality_department_departmentId",
                        column: x => x.departmentId,
                        principalSchema: "Parameters",
                        principalTable: "department",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "formmodule",
                schema: "ModelSecurity",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    formid = table.Column<int>(type: "int", nullable: false),
                    moduleid = table.Column<int>(type: "int", nullable: false),
                    active = table.Column<bool>(type: "bit", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_formmodule", x => x.id);
                    table.ForeignKey(
                        name: "FK_FormModule_Form",
                        column: x => x.formid,
                        principalSchema: "ModelSecurity",
                        principalTable: "form",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FormModule_Module",
                        column: x => x.moduleid,
                        principalSchema: "ModelSecurity",
                        principalTable: "module",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "rolformpermission",
                schema: "ModelSecurity",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    rolid = table.Column<int>(type: "int", nullable: false),
                    formid = table.Column<int>(type: "int", nullable: false),
                    permissionid = table.Column<int>(type: "int", nullable: false),
                    active = table.Column<bool>(type: "bit", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rolformpermission", x => x.id);
                    table.ForeignKey(
                        name: "FK_RolFormPermission_Form",
                        column: x => x.formid,
                        principalSchema: "ModelSecurity",
                        principalTable: "form",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RolFormPermission_Permission",
                        column: x => x.permissionid,
                        principalSchema: "ModelSecurity",
                        principalTable: "permission",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RolFormPermission_Rol",
                        column: x => x.rolid,
                        principalSchema: "ModelSecurity",
                        principalTable: "rol",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Infraction",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeInfractionId = table.Column<int>(type: "int", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    numer_smldv = table.Column<int>(type: "int", nullable: false),
                    active = table.Column<bool>(type: "bit", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Infraction", x => x.id);
                    table.ForeignKey(
                        name: "FK_TypeInfraction_Infraction",
                        column: x => x.TypeInfractionId,
                        principalSchema: "Entities",
                        principalTable: "TypeInfraction",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "person",
                schema: "ModelSecurity",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    firstName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    lastName = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false),
                    phoneNumber = table.Column<string>(type: "varchar(20)", nullable: true),
                    address = table.Column<string>(type: "varchar(100)", nullable: true),
                    tipoUsuario = table.Column<int>(type: "int", nullable: false),
                    municipalityId = table.Column<int>(type: "int", nullable: true),
                    active = table.Column<bool>(type: "bit", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_person", x => x.id);
                    table.ForeignKey(
                        name: "FK_person_municipality_municipalityId",
                        column: x => x.municipalityId,
                        principalSchema: "Parameters",
                        principalTable: "municipality",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FineCalculationDetail",
                schema: "Entities",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    formula = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    totalCalculation = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                    valueSmldvId = table.Column<int>(type: "int", nullable: false),
                    typeInfractionId = table.Column<int>(type: "int", nullable: false),
                    SmldvValueAtCreation = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    active = table.Column<bool>(type: "bit", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FineCalculationDetail", x => x.id);
                    table.ForeignKey(
                        name: "FK_TypeInfraction_FineCalculationDetail",
                        column: x => x.typeInfractionId,
                        principalTable: "Infraction",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_ValueSmldv_FineCalculationDetail",
                        column: x => x.valueSmldvId,
                        principalSchema: "Entities",
                        principalTable: "valueSmldv",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "user",
                schema: "ModelSecurity",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PasswordHash = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true),
                    email = table.Column<string>(type: "varchar(150)", unicode: false, maxLength: 150, nullable: true),
                    PersonId = table.Column<int>(type: "int", nullable: true),
                    documentTypeId = table.Column<int>(type: "int", nullable: true),
                    documentNumber = table.Column<string>(type: "varchar(30)", nullable: true),
                    EmailVerified = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    EmailVerificationCode = table.Column<string>(type: "varchar(6)", unicode: false, maxLength: 6, nullable: true),
                    EmailVerificationExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EmailVerifiedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    active = table.Column<bool>(type: "bit", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user", x => x.id);
                    table.ForeignKey(
                        name: "FK_User_Person",
                        column: x => x.PersonId,
                        principalSchema: "ModelSecurity",
                        principalTable: "person",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_user_documentType_documentTypeId",
                        column: x => x.documentTypeId,
                        principalSchema: "Parameters",
                        principalTable: "documentType",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "roluser",
                schema: "ModelSecurity",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userId = table.Column<int>(type: "int", nullable: false),
                    rolId = table.Column<int>(type: "int", nullable: false),
                    active = table.Column<bool>(type: "bit", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roluser", x => x.id);
                    table.ForeignKey(
                        name: "FK_RolUser_Rol",
                        column: x => x.rolId,
                        principalSchema: "ModelSecurity",
                        principalTable: "rol",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RolUser_User",
                        column: x => x.userId,
                        principalSchema: "ModelSecurity",
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "userInfraction",
                schema: "Entities",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    dateInfraction = table.Column<DateTime>(type: "datetime2", nullable: false),
                    stateInfraction = table.Column<int>(type: "int", nullable: false),
                    InformationFine = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    InfractionId = table.Column<int>(type: "int", nullable: false),
                    UserNotificationId = table.Column<int>(type: "int", nullable: false),
                    amountToPay = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    smldvValueAtCreation = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    active = table.Column<bool>(type: "bit", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_userInfraction", x => x.id);
                    table.ForeignKey(
                        name: "FK_userInfraction_Infraction_InfractionId",
                        column: x => x.InfractionId,
                        principalTable: "Infraction",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_userInfraction_userNotification_UserNotificationId",
                        column: x => x.UserNotificationId,
                        principalSchema: "Entities",
                        principalTable: "userNotification",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_userInfraction_user_UserId",
                        column: x => x.UserId,
                        principalSchema: "ModelSecurity",
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "paymentAgreement",
                schema: "Entities",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    neighborhood = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    AgreementDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    expeditionCedula = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    AgreementStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AgreementEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    userInfractionId = table.Column<int>(type: "int", nullable: false),
                    paymentFrequencyId = table.Column<int>(type: "int", nullable: false),
                    typePaymentId = table.Column<int>(type: "int", nullable: false),
                    BaseAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AccruedInterest = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m),
                    OutstandingAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Installments = table.Column<int>(type: "int", nullable: true),
                    MonthlyFee = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IsPaid = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsCoactive = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    CoactiveActivatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastInterestAppliedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    active = table.Column<bool>(type: "bit", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_paymentAgreement", x => x.id);
                    table.ForeignKey(
                        name: "FK_PaymentAgreement_TypePayment",
                        column: x => x.typePaymentId,
                        principalSchema: "Entities",
                        principalTable: "typePayment",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PaymentAgreement_UserInfraction",
                        column: x => x.userInfractionId,
                        principalSchema: "Entities",
                        principalTable: "userInfraction",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_paymentAgreement_paymentFrequency_paymentFrequencyId",
                        column: x => x.paymentFrequencyId,
                        principalSchema: "Parameters",
                        principalTable: "paymentFrequency",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DocumentInfraction",
                schema: "Entities",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    inspectoraReportId = table.Column<int>(type: "int", nullable: false),
                    PaymentAgreementId = table.Column<int>(type: "int", nullable: false),
                    active = table.Column<bool>(type: "bit", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentInfraction", x => x.id);
                    table.ForeignKey(
                        name: "FK_DocumentInfraction_InspectoraReport",
                        column: x => x.inspectoraReportId,
                        principalSchema: "Entities",
                        principalTable: "InspectoraReport",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DocumentInfraction_PaymentAgreement",
                        column: x => x.PaymentAgreementId,
                        principalSchema: "Entities",
                        principalTable: "paymentAgreement",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "installmentSchedule",
                schema: "Entities",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Number = table.Column<int>(type: "int", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RemainingBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsPaid = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    PaymentAgreementId = table.Column<int>(type: "int", nullable: false),
                    active = table.Column<bool>(type: "bit", nullable: false),
                    is_deleted = table.Column<bool>(type: "bit", nullable: false),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_installmentSchedule", x => x.id);
                    table.ForeignKey(
                        name: "FK_InstallmentSchedule_PaymentAgreement",
                        column: x => x.PaymentAgreementId,
                        principalSchema: "Entities",
                        principalTable: "paymentAgreement",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "Entities",
                table: "InspectoraReport",
                columns: new[] { "id", "active", "created_date", "is_deleted", "message", "report_date", "total_fines" },
                values: new object[,]
                {
                    { 1, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false, "se integra una nueva multa", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 2m },
                    { 2, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false, "se integra una nueva multa", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 3m }
                });

            migrationBuilder.InsertData(
                schema: "Entities",
                table: "TypeInfraction",
                columns: new[] { "id", "Name", "active", "created_date", "is_deleted" },
                values: new object[,]
                {
                    { 1, "Infraccion de tipo uno", true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 2, "Infraccion de tipo dos", true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 3, "Infraccion de tipo tres", true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 4, "Infraccion de tipo cuatro", true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false }
                });

            migrationBuilder.InsertData(
                schema: "Parameters",
                table: "department",
                columns: new[] { "id", "active", "created_date", "daneCode", "is_deleted", "name" },
                values: new object[,]
                {
                    { 1, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 5, false, "Antioquia" },
                    { 2, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 25, false, "Cundinamarca" },
                    { 3, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 76, false, "Valle del Cauca" },
                    { 4, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 11, false, "Bogotá, D.C." }
                });

            migrationBuilder.InsertData(
                schema: "Parameters",
                table: "documentType",
                columns: new[] { "id", "abbreviation", "active", "created_date", "is_deleted", "name" },
                values: new object[,]
                {
                    { 1, "CC", true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false, "Cédula de Ciudadanía" },
                    { 2, "CE", true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false, "Cédula de Extranjería" },
                    { 3, "TI", true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false, "Tarjeta de Identidad" },
                    { 4, "PAS", true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false, "Pasaporte" }
                });

            migrationBuilder.InsertData(
                schema: "ModelSecurity",
                table: "form",
                columns: new[] { "id", "Icon", "Route", "active", "created_date", "description", "is_deleted", "name" },
                values: new object[,]
                {
                    { 1, "pi pi-fw pi-home", "acuerdoPago", true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Formulario de creacion de acuerdo de pago", false, "Formulario de acuerdo de pago" },
                    { 2, "pi pi-fw pi-homeing", "anexar-multas/multas", true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Formulario para agregar nuevas multas", false, "Formulario de creacion de multas" },
                    { 3, "pi pi-fw pi-id-card", "tipos-multas", true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Formulario tipo de  multas", false, "Formulario tipo de  multas" },
                    { 4, "pi pi-fw pi-check-square", "notificaciones/notificacion-multas", true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Formulario Notificacion de multas", false, "Notificacion de multas" },
                    { 5, "pi pi-fw pi-file", "formularios", true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Formularios", false, "Formularios" },
                    { 6, "pi pi-fw pi-clone", "form-modules", true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Formularios y  modules", false, "Formularios y  modules" },
                    { 7, "pi pi-fw pi-th-large", "modulos", true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Modulos", false, "Modulos" },
                    { 8, "pi pi-fw pi-users", "personas", true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Personas", false, "personas" },
                    { 9, "pi pi-fw pi-lock-open", "permisos", true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "permisos", false, "permisos" },
                    { 10, "pi pi-fw pi-key", "rol-form-permission", true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Roles Formularios y Permission", false, "Roles Formularios y Permission" },
                    { 11, "pi pi-fw pi-users", "roles", true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Roles", false, "Roles" },
                    { 12, "pi pi-fw pi-user", "usuarios", true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Usuarios", false, "Usuarios" },
                    { 13, "pi pi-fw pi-user-plus", "rol-user", true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Roles y Usuario", false, "Roles y Usuario" },
                    { 14, "pi pi-fw pi-briefcase", "parameters/department", true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "departamento", false, "departamento" },
                    { 15, "pi pi-fw pi-briefcase", "parameters/document-type", true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Tipo de Documento", false, "Tipo de Documento" },
                    { 16, "pi pi-fw pi-briefcase", "parameters/municipality", true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Municipio", false, "Municipio" },
                    { 17, "pi pi-fw pi-briefcase", "parameters/payment-frequency", true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Frecuencia de pago", false, "Frecuencia de pago " },
                    { 18, "pi pi-fw pi-briefcase", "dashboard", true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Perfil", false, "Perfil" },
                    { 19, "pi pi-fw pi-briefcase", "notificaciones", true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Notificacion de acuerdo ", false, "Notificacion de acuerdo" },
                    { 20, "pi pi-fw pi-home", "consultar-ingresar", true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "inicio ", false, "inicio" },
                    { 21, "pi pi-fw pi-home", "parameters/smdlv", true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "valor de SMDLV ", false, "valor de SMDLV" }
                });

            migrationBuilder.InsertData(
                schema: "ModelSecurity",
                table: "module",
                columns: new[] { "id", "active", "created_date", "description", "is_deleted", "name" },
                values: new object[,]
                {
                    { 1, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Inicio", false, "Inicio" },
                    { 2, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Contenido", false, "Contenido" },
                    { 3, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Gestion Avanzada", false, "Gestion Avanzada" },
                    { 4, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "perfil", false, "perfil" },
                    { 5, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "modulo de parametro", false, "modulo de parametro" }
                });

            migrationBuilder.InsertData(
                schema: "Parameters",
                table: "paymentFrequency",
                columns: new[] { "id", "active", "created_date", "dueDayOfMonth", "intervalPage", "is_deleted" },
                values: new object[,]
                {
                    { 1, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 15, "MENSUAL", false },
                    { 2, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, "QUINCENAL", false },
                    { 3, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 10, "BIMESTRAL", false }
                });

            migrationBuilder.InsertData(
                schema: "ModelSecurity",
                table: "permission",
                columns: new[] { "id", "active", "created_date", "description", "is_deleted", "name" },
                values: new object[,]
                {
                    { 1, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "permiso para leer formularios", false, "Leer" },
                    { 2, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "permiso para crear formularios", false, "Crear" },
                    { 3, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "permiso para editar formularios", false, "Editar" },
                    { 4, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "permiso para eliminar lógicamente formularios", false, "Eliminar" },
                    { 5, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "permiso para ver formularios eliminados", false, "VerEliminados" },
                    { 6, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "permiso para recuperar formularios eliminados", false, "Recuperar" }
                });

            migrationBuilder.InsertData(
                schema: "ModelSecurity",
                table: "rol",
                columns: new[] { "id", "active", "created_date", "description", "is_deleted", "name" },
                values: new object[,]
                {
                    { 1, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Rol con todos los permisos", false, "Administrador" },
                    { 2, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Rol con permisos limitados", false, "Finanza" }
                });

            migrationBuilder.InsertData(
                schema: "Entities",
                table: "typePayment",
                columns: new[] { "id", "active", "created_date", "is_deleted", "name", "paymentAgreementId" },
                values: new object[,]
                {
                    { 1, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false, "efectivo", 0 },
                    { 2, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false, "nequi", 0 },
                    { 3, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false, "tarjeta crédito", 0 },
                    { 4, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false, "tarjeta débito", 0 },
                    { 5, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false, "daviplata", 0 }
                });

            migrationBuilder.InsertData(
                schema: "Entities",
                table: "userNotification",
                columns: new[] { "id", "active", "created_date", "is_deleted", "message", "shippingDate" },
                values: new object[,]
                {
                    { 1, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false, "tienes una infraccion por favor acercate antes del 12 de marzo para sucdazanar tu multa o podria iniciar un cobro coativo luego del plazo", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 2, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false, "tienes una infraccion por favor acercate antes del 12 de julio para sucdazanar tu multa o podria iniciar un cobro coativo luego del plazo", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.InsertData(
                schema: "Entities",
                table: "valueSmldv",
                columns: new[] { "id", "Current_Year", "active", "created_date", "is_deleted", "minimunWage", "value_smldv" },
                values: new object[] { 1, 2024, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false, 1425000m, 43500m });

            migrationBuilder.InsertData(
                table: "Infraction",
                columns: new[] { "id", "TypeInfractionId", "active", "created_date", "description", "is_deleted", "numer_smldv" },
                values: new object[,]
                {
                    { 1, 1, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Irrespetar las normas propias de los lugares públicos tales como salas de velación, cementerios, clínicas, hospitales, bibliotecas y museos, entre otros.", false, 4 },
                    { 2, 1, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Emplear o inducir a los niños, niñas y adolescentes a utilizar indebidamente las telecomunicaciones o sistemas de emergencia", false, 4 },
                    { 3, 1, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Utilizar a estas personas para obtener beneficio económico o satisfacer interés personal.", false, 4 },
                    { 4, 1, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Limitar u obstruir las manifestaciones de afecto público que no configuren actos sexuales, de exhibicionismo en razón a la raza, orientación sexual, género u otra condición similar.", false, 4 },
                    { 5, 1, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ingresar o introducir niños, niñas o adolescentes a los actos o eventos que puedan causar daño a su integridad o en los cuales exista previa restricción de edad por parte de las autoridades de policía, o esté prohibido su ingreso por las normas vigentes.", false, 4 },
                    { 6, 1, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "No destruir en la fuente los envases de bebidas embriagantes.", false, 4 },
                    { 7, 1, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ingresar con boletería falsa.", false, 4 },
                    { 8, 1, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Vender o canjear boletería, manillas, credencial o identificaciones facilitando el ingreso a un espectáculo público, actuando por fuera de las operaciones autorizadas para determinado evento.", false, 4 },
                    { 9, 1, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ingresar al evento sin boletería, manilla, credencial o identificación dispuesta y autorizada para el mismo o trasladarse fraudulentamente a una localidad diferente a la que acredite su boleta, manilla, credencial o identificación dispuesta y autorizada.", false, 4 },
                    { 10, 1, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "No informar los protocolos de seguridad y evacuación en caso de emergencias a las personas que se encuentren en el lugar.", false, 4 },
                    { 11, 1, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "No fijar la señalización de los protocolos de seguridad en un lugar visible.", false, 4 },
                    { 12, 1, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Permitir el consumo de tabaco y/o sus derivados en lugares no autorizados por la ley y la normatividad vigente.", false, 4 },
                    { 13, 1, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Comercializar, almacenar, poseer o tener especies de flora o fauna que ofrezcan peligro para la integridad y la salud.", false, 4 },
                    { 14, 2, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Reñir, incitar o incurrir en confrontaciones violentas que puedan derivar en agresiones físicas.", false, 8 },
                    { 15, 2, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Amenazar con causar un daño físico a personas por cualquier medio.", false, 8 },
                    { 16, 2, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Portar armas, elementos cortantes, punzantes o semejantes, o sustancias peligrosas, en áreas comunes o lugares abiertos al público. Se exceptúa a quien demuestre que tales elementos o sustancias constituyen una herramienta de su actividad deportiva, oficio, profesión o estudio", false, 8 },
                    { 17, 2, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Portar armas neumáticas, de aire, de fogueo, de letalidad reducida o sprays, rociadores, aspersores o aerosoles de pimienta o cualquier elemento que se asimile a armas de fuego, en lugares abiertos al público donde se desarrollen aglomeraciones de personas o en aquellos donde se consuman bebidas embriagantes, o se advierta su utilización irregular, o se incurra en un comportamiento contrario a la convivencia.", false, 8 },
                    { 18, 2, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Sonidos o ruidos en actividades, fiestas, reuniones o eventos similares que afecten la convivencia del vecindario, cuando generen molestia por su impacto auditivo, en cuyo caso podrán las autoridades de Policía desactivar temporalmente la fuente del ruido, en caso de que el residente se niegue a desactivarlo;", false, 8 },
                    { 19, 2, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Destruir, averiar o deteriorar bienes dentro del área circundante de la institución o centro educativo.", false, 8 },
                    { 20, 2, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Irrespetar a las autoridades de Policía.", false, 8 },
                    { 21, 2, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Permitir que los niños, niñas y adolescentes sean tenedores de animales potencialmente peligrosos.", false, 8 },
                    { 22, 2, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "No permitir el acceso al predio sobre el cual pesa el gravamen de servidumbre para realizar el mantenimiento o la reparación.", false, 8 },
                    { 23, 2, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Vender, procesar o almacenar productos alimenticios en los sitios no permitidos o contrariando las normas vigentes.", false, 8 },
                    { 24, 2, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Propiciar la ocupación indebida del espacio público.", false, 8 },
                    { 25, 2, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Comprar, alquilar o usar equipo terminal móvil con reporte de hurto y/o extravío en la base de datos negativa de que trata el artículo 106 de la Ley 1453 de 2011 o equipo terminal móvil cuyo número de identificación físico o electrónico haya sido reprogramado, remarcado, modificado o suprimido.", false, 8 },
                    { 26, 2, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "hacer mucho ruido en un sitio publico", false, 8 },
                    { 27, 3, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Agredir físicamente a personas por cualquier medi", false, 16 },
                    { 28, 3, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Poner en riesgo a personas o bienes durante la instalación, utilización, mantenimiento o modificación de las estructuras de los servicios públicos.", false, 16 },
                    { 29, 3, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Modificar o alterar redes o instalaciones de servicios públicos.", false, 16 },
                    { 30, 3, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "No reparar oportunamente los daños ocasionados a la infraestructura de servicios públicos domiciliarios, cuando estas reparaciones corresponden al usuario.", false, 16 },
                    { 31, 3, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Sonidos o ruidos en actividades, fiestas, reuniones o eventos similares que afecten la convivencia del vecindario, cuando generen molestia por su impacto auditivo, en cuyo caso podrán las autoridades de Policía desactivar temporalmente la fuente del ruido, en caso de que el residente se niegue a desactivarlo;", false, 16 },
                    { 32, 3, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Cualquier medio de producción de sonidos o dispositivos o accesorios o maquinaria que produzcan ruidos, desde bienes muebles o inmuebles, en cuyo caso podrán las autoridades identificar, registrar y desactivar temporalmente la fuente del ruido, salvo sean originados en construcciones o reparaciones en horas permitidas;", false, 16 },
                    { 33, 3, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Consumir bebidas alcohólicas, drogas o sustancias prohibidas, dentro de la institución o centro educativo.", false, 16 },
                    { 34, 3, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Dificultar, obstruir o limitar información e insumos relacionados con los derechos sexuales y reproductivos de la mujer, del hombre y de la comunidad LGBTI, incluido el acceso de estos a métodos anticonceptivos.", false, 16 },
                    { 35, 3, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ejercer la prostitución sin el cumplimiento de las medidas sanitarias y de protección requeridas.", false, 16 },
                    { 36, 3, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Realizar actos sexuales o exhibicionistas en la vía pública o en lugares expuestos a esta.", false, 16 },
                    { 37, 3, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Carecer o no proporcionar los implementos de seguridad exigidos por la actividad, o proporcionarlos en mal estado de funcionamiento.", false, 16 },
                    { 38, 3, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Invadir los espacios no abiertos al público.", false, 16 },
                    { 39, 3, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Pretender ingresan o estar en posesión o tenencia de cualquier tipo de arma u objetos prohibidos por las normas vigentes, por el alcalde o su delegado", false, 16 },
                    { 40, 4, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Arrojar en las redes de alcantarillado, acueducto y de aguas lluvias, cualquier objeto, sustancia, residuo, escombros, lodo, combustibles o lubricantes, que alteren u obstruyan el normal funcionamiento.", false, 32 },
                    { 41, 4, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Permitir, auspiciar, tolerar, inducir o constreñir el ingreso de los niños, niñas y adolescentes a los lugares donde:", false, 32 },
                    { 42, 4, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Salvo actos circenses, prender o manipular fuego en el espacio público, lugar abierto al público, sin contar con la autorización del alcalde o su delegado o del responsable del sitio, sin cumplir las medidas de seguridad.", false, 32 },
                    { 43, 4, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Prender o manipular fuego, sustancias combustibles o mercancías peligrosas en medio de transporte público.", false, 32 },
                    { 44, 4, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Fabricar, tener, portar, distribuir, transportar, comercializar, manipular o usar sustancias prohibidas, elementos o residuos químicos o inflamables sin el cumplimiento de los requisitos establecidos.", false, 32 },
                    { 45, 4, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Realizar quemas o incendios que afecten la convivencia en cualquier lugar público o privado o en sitios prohibidos.", false, 32 },
                    { 46, 4, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Utilizar calderas, motores, máquinas o aparatos similares que no se encuentren en condiciones aptas de funcionamiento.", false, 32 },
                    { 47, 4, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Incumplir, desacatar, desconocer e impedir la función o la orden de Policía.", false, 32 },
                    { 48, 4, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Impedir, dificultar, obstaculizar o resistirse a procedimiento de identificación o individualización, por parte de las autoridades de Policía.", false, 32 },
                    { 49, 4, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Negarse a dar información veraz sobre lugar de residencia, domicilio y actividad a las autoridades de Policía cuando estas lo requieran en procedimientos de Policía", false, 32 },
                    { 50, 4, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Ofrecer cualquier tipo de resistencia a la aplicación de una medida o la utilización de un medio de Policía.", false, 32 },
                    { 51, 4, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Agredir por cualquier medio o lanzar objetos que puedan causar daño o sustancias que representen peligro a las autoridades de Policía.", false, 32 },
                    { 52, 4, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Utilizar inadecuadamente el sistema de número único de seguridad y emergencia.", false, 32 }
                });

            migrationBuilder.InsertData(
                schema: "ModelSecurity",
                table: "formmodule",
                columns: new[] { "id", "active", "created_date", "formid", "is_deleted", "moduleid" },
                values: new object[,]
                {
                    { 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, false, 2 },
                    { 2, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 4, false, 2 },
                    { 3, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 5, false, 3 },
                    { 4, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 6, false, 3 },
                    { 5, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 7, false, 3 },
                    { 6, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 8, false, 3 },
                    { 7, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 9, false, 3 },
                    { 8, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 10, false, 3 },
                    { 9, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 11, false, 3 },
                    { 10, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 13, false, 3 },
                    { 11, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 14, false, 5 },
                    { 12, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 15, false, 5 },
                    { 13, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 16, false, 5 },
                    { 14, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 17, false, 5 },
                    { 15, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 18, false, 2 },
                    { 16, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 19, false, 2 },
                    { 17, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 20, false, 2 },
                    { 18, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 21, false, 5 }
                });

            migrationBuilder.InsertData(
                schema: "Parameters",
                table: "municipality",
                columns: new[] { "id", "active", "created_date", "daneCode", "departmentId", "is_deleted", "name" },
                values: new object[,]
                {
                    { 1, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 5001, 1, false, "Medellín" },
                    { 2, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 76001, 3, false, "Cali" },
                    { 3, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 11001, 4, false, "Bogotá" },
                    { 4, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 25754, 2, false, "Soacha" }
                });

            migrationBuilder.InsertData(
                schema: "ModelSecurity",
                table: "rolformpermission",
                columns: new[] { "id", "formid", "permissionid", "rolid", "active", "created_date", "is_deleted" },
                values: new object[,]
                {
                    { 1, 1, 1, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 2, 1, 2, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 3, 1, 3, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 4, 1, 4, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 5, 1, 5, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 6, 1, 6, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 7, 2, 1, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 8, 2, 2, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 9, 2, 3, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 10, 2, 4, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 11, 2, 5, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 12, 2, 6, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 13, 3, 1, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 14, 3, 2, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 15, 3, 3, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 16, 3, 4, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 17, 3, 5, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 18, 3, 6, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 19, 4, 1, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 20, 4, 2, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 21, 4, 3, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 22, 4, 4, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 23, 4, 5, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 24, 4, 6, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 25, 5, 1, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 26, 5, 2, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 27, 5, 3, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 28, 5, 4, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 29, 5, 5, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 30, 5, 6, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 31, 6, 1, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 32, 6, 2, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 33, 6, 3, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 34, 6, 4, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 35, 6, 5, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 36, 6, 6, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 37, 7, 1, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 38, 7, 2, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 39, 7, 3, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 40, 7, 4, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 41, 7, 5, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 42, 7, 6, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 43, 8, 1, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 44, 8, 2, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 45, 8, 3, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 46, 8, 4, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 47, 8, 5, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 48, 8, 6, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 49, 9, 1, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 50, 9, 2, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 51, 9, 3, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 52, 9, 4, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 53, 9, 5, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 54, 9, 6, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 55, 10, 1, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 56, 10, 2, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 57, 10, 3, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 58, 10, 4, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 59, 10, 5, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 60, 10, 6, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 61, 11, 1, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 62, 11, 2, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 63, 11, 3, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 64, 11, 4, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 65, 11, 5, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 66, 11, 6, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 67, 12, 1, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 68, 12, 2, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 69, 12, 3, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 70, 12, 4, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 71, 12, 5, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 72, 12, 6, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 73, 13, 1, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 74, 13, 2, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 75, 13, 3, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 76, 13, 4, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 77, 13, 5, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 78, 13, 6, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 79, 14, 1, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 80, 14, 2, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 81, 14, 3, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 82, 14, 4, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 83, 14, 5, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 84, 14, 6, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 85, 15, 1, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 86, 15, 2, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 87, 15, 3, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 88, 15, 4, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 89, 15, 5, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 90, 15, 6, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 91, 16, 1, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 92, 16, 2, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 93, 16, 3, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 94, 16, 4, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 95, 16, 5, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 96, 16, 6, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 97, 17, 1, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 98, 17, 2, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 99, 17, 3, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 100, 17, 4, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 101, 17, 5, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 102, 17, 6, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 103, 18, 1, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 104, 18, 2, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 105, 18, 3, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 106, 18, 4, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 107, 18, 5, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 108, 18, 6, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 109, 19, 1, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 110, 19, 2, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 111, 19, 3, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 112, 19, 4, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 113, 19, 5, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 114, 19, 6, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 115, 20, 1, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 116, 20, 2, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 117, 20, 3, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 118, 20, 4, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 119, 20, 5, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 120, 20, 6, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 121, 21, 1, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 122, 21, 2, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 123, 21, 3, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 124, 21, 4, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 125, 21, 5, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 126, 21, 6, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 127, 4, 1, 2, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 128, 18, 1, 2, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 129, 19, 1, 2, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 130, 20, 1, 2, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false }
                });

            migrationBuilder.InsertData(
                schema: "ModelSecurity",
                table: "person",
                columns: new[] { "id", "active", "address", "created_date", "firstName", "is_deleted", "lastName", "municipalityId", "phoneNumber", "tipoUsuario" },
                values: new object[,]
                {
                    { 1, true, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Juan", false, "Pérez", 1, null, 3 },
                    { 2, true, null, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Sara", false, "Sofía", 4, null, 3 }
                });

            migrationBuilder.InsertData(
                schema: "ModelSecurity",
                table: "user",
                columns: new[] { "id", "EmailVerificationCode", "EmailVerificationExpiresAt", "EmailVerified", "EmailVerifiedAt", "PasswordHash", "PersonId", "active", "created_date", "documentNumber", "documentTypeId", "email", "is_deleted" },
                values: new object[,]
                {
                    { 1, null, null, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "admin123", 1, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "1234567890", 1, "camiloandreslosada901@gmail.com", false },
                    { 2, null, null, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "sara12312", 2, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "0123432121", 2, "sarita@gmail.com", false }
                });

            migrationBuilder.InsertData(
                schema: "ModelSecurity",
                table: "roluser",
                columns: new[] { "id", "rolId", "userId", "active", "created_date", "is_deleted" },
                values: new object[,]
                {
                    { 1, 1, 1, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 2, 2, 2, false, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false }
                });

            migrationBuilder.InsertData(
                schema: "Entities",
                table: "userInfraction",
                columns: new[] { "id", "InformationFine", "InfractionId", "UserId", "UserNotificationId", "active", "amountToPay", "created_date", "dateInfraction", "is_deleted", "smldvValueAtCreation", "stateInfraction" },
                values: new object[,]
                {
                    { 1, null, 1, 1, 1, true, 0m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false, 43500m, 0 },
                    { 2, null, 14, 1, 2, true, 0m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false, 43500m, 0 },
                    { 3, null, 27, 2, 1, true, 0m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false, 43500m, 0 },
                    { 4, null, 40, 2, 2, true, 0m, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false, 43500m, 0 }
                });

            migrationBuilder.InsertData(
                schema: "Entities",
                table: "paymentAgreement",
                columns: new[] { "id", "AgreementDescription", "AgreementEnd", "AgreementStart", "BaseAmount", "CoactiveActivatedOn", "Email", "Installments", "LastInterestAppliedOn", "MonthlyFee", "OutstandingAmount", "PhoneNumber", "active", "address", "created_date", "expeditionCedula", "is_deleted", "neighborhood", "paymentFrequencyId", "typePaymentId", "userInfractionId" },
                values: new object[,]
                {
                    { 1, "Acuerdo a 4 cuotas iguales.", new DateTime(2025, 5, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 130500m, null, "user1@example.com", 4, null, 32625m, 130500m, "3101234567", true, "Carrera 10 #45-20", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2020, 5, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Eduardo Santos", 1, 1, 1 },
                    { 2, "Acuerdo a 2 cuotas iguales.", new DateTime(2025, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 174000m, null, "user2@example.com", 2, null, 87000m, 174000m, "3009876543", true, "Carrera 1 #23-18", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2017, 1, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Panamá", 2, 2, 2 },
                    { 3, "Acuerdo a 8 cuotas iguales.", new DateTime(2025, 9, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 348000m, null, "user3@example.com", 8, null, 43500m, 348000m, "3015558888", true, "Calle 20 #15-40", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2018, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "La Merced", 2, 3, 3 },
                    { 4, "Acuerdo a 12 cuotas iguales.", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 835200m, null, "user4@example.com", 12, null, 69600m, 835200m, "3024449999", true, "Avenida 5 #45-12", new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), new DateTime(2019, 5, 22, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "San Martín", 3, 1, 4 }
                });

            migrationBuilder.InsertData(
                schema: "Entities",
                table: "DocumentInfraction",
                columns: new[] { "id", "PaymentAgreementId", "active", "created_date", "inspectoraReportId", "is_deleted" },
                values: new object[,]
                {
                    { 1, 1, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 1, false },
                    { 2, 2, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), 2, false }
                });

            migrationBuilder.InsertData(
                schema: "Entities",
                table: "installmentSchedule",
                columns: new[] { "id", "Amount", "Number", "PaymentAgreementId", "PaymentDate", "RemainingBalance", "active", "created_date", "is_deleted" },
                values: new object[,]
                {
                    { 1, 32625m, 1, 1, new DateTime(2025, 3, 1, 0, 0, 0, 0, DateTimeKind.Utc), 97900m, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false },
                    { 2, 32625m, 2, 1, new DateTime(2025, 4, 1, 0, 0, 0, 0, DateTimeKind.Utc), 65275m, true, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), false }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuthSession_SessionId",
                schema: "ModelSecurity",
                table: "AuthSession",
                column: "SessionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_department_daneCode",
                schema: "Parameters",
                table: "department",
                column: "daneCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_department_name",
                schema: "Parameters",
                table: "department",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentInfraction_inspectoraReportId",
                schema: "Entities",
                table: "DocumentInfraction",
                column: "inspectoraReportId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentInfraction_PaymentAgreementId",
                schema: "Entities",
                table: "DocumentInfraction",
                column: "PaymentAgreementId");

            migrationBuilder.CreateIndex(
                name: "IX_documentType_abbreviation",
                schema: "Parameters",
                table: "documentType",
                column: "abbreviation",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_documentType_name",
                schema: "Parameters",
                table: "documentType",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "IX_FineCalculationDetail_typeInfractionId",
                schema: "Entities",
                table: "FineCalculationDetail",
                column: "typeInfractionId");

            migrationBuilder.CreateIndex(
                name: "IX_FineCalculationDetail_valueSmldvId",
                schema: "Entities",
                table: "FineCalculationDetail",
                column: "valueSmldvId");

            migrationBuilder.CreateIndex(
                name: "IX_form_name",
                schema: "ModelSecurity",
                table: "form",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_formmodule_formid",
                schema: "ModelSecurity",
                table: "formmodule",
                column: "formid");

            migrationBuilder.CreateIndex(
                name: "IX_formmodule_moduleid",
                schema: "ModelSecurity",
                table: "formmodule",
                column: "moduleid");

            migrationBuilder.CreateIndex(
                name: "IX_Infraction_TypeInfractionId",
                table: "Infraction",
                column: "TypeInfractionId");

            migrationBuilder.CreateIndex(
                name: "IX_installmentSchedule_PaymentAgreementId",
                schema: "Entities",
                table: "installmentSchedule",
                column: "PaymentAgreementId");

            migrationBuilder.CreateIndex(
                name: "IX_module_name",
                schema: "ModelSecurity",
                table: "module",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_municipality_daneCode",
                schema: "Parameters",
                table: "municipality",
                column: "daneCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_municipality_departmentId_name",
                schema: "Parameters",
                table: "municipality",
                columns: new[] { "departmentId", "name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_municipality_name",
                schema: "Parameters",
                table: "municipality",
                column: "name");

            migrationBuilder.CreateIndex(
                name: "IX_paymentAgreement_paymentFrequencyId",
                schema: "Entities",
                table: "paymentAgreement",
                column: "paymentFrequencyId");

            migrationBuilder.CreateIndex(
                name: "IX_paymentAgreement_typePaymentId",
                schema: "Entities",
                table: "paymentAgreement",
                column: "typePaymentId");

            migrationBuilder.CreateIndex(
                name: "IX_paymentAgreement_userInfractionId",
                schema: "Entities",
                table: "paymentAgreement",
                column: "userInfractionId");

            migrationBuilder.CreateIndex(
                name: "IX_paymentFrequency_intervalPage",
                schema: "Parameters",
                table: "paymentFrequency",
                column: "intervalPage");

            migrationBuilder.CreateIndex(
                name: "IX_permission_name",
                schema: "ModelSecurity",
                table: "permission",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_person_municipalityId",
                schema: "ModelSecurity",
                table: "person",
                column: "municipalityId");

            migrationBuilder.CreateIndex(
                name: "IX_rol_name",
                schema: "ModelSecurity",
                table: "rol",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_rolformpermission_formid",
                schema: "ModelSecurity",
                table: "rolformpermission",
                column: "formid");

            migrationBuilder.CreateIndex(
                name: "IX_rolformpermission_permissionid",
                schema: "ModelSecurity",
                table: "rolformpermission",
                column: "permissionid");

            migrationBuilder.CreateIndex(
                name: "IX_RolFormPermission_Unique",
                schema: "ModelSecurity",
                table: "rolformpermission",
                columns: new[] { "rolid", "formid", "permissionid" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_roluser_rolId",
                schema: "ModelSecurity",
                table: "roluser",
                column: "rolId");

            migrationBuilder.CreateIndex(
                name: "IX_roluser_userId",
                schema: "ModelSecurity",
                table: "roluser",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_TypeInfraction_Name",
                schema: "Entities",
                table: "TypeInfraction",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_typePayment_name",
                schema: "Entities",
                table: "typePayment",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_documentTypeId",
                schema: "ModelSecurity",
                table: "user",
                column: "documentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_user_PersonId",
                schema: "ModelSecurity",
                table: "user",
                column: "PersonId",
                unique: true,
                filter: "[PersonId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_userInfraction_InfractionId",
                schema: "Entities",
                table: "userInfraction",
                column: "InfractionId");

            migrationBuilder.CreateIndex(
                name: "IX_userInfraction_UserId",
                schema: "Entities",
                table: "userInfraction",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_userInfraction_UserNotificationId",
                schema: "Entities",
                table: "userInfraction",
                column: "UserNotificationId");

            migrationBuilder.CreateIndex(
                name: "IX_valueSmldv_Current_Year",
                schema: "Entities",
                table: "valueSmldv",
                column: "Current_Year",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthSession",
                schema: "ModelSecurity");

            migrationBuilder.DropTable(
                name: "DocumentInfraction",
                schema: "Entities");

            migrationBuilder.DropTable(
                name: "FineCalculationDetail",
                schema: "Entities");

            migrationBuilder.DropTable(
                name: "formmodule",
                schema: "ModelSecurity");

            migrationBuilder.DropTable(
                name: "installmentSchedule",
                schema: "Entities");

            migrationBuilder.DropTable(
                name: "refreshTokens");

            migrationBuilder.DropTable(
                name: "rolformpermission",
                schema: "ModelSecurity");

            migrationBuilder.DropTable(
                name: "roluser",
                schema: "ModelSecurity");

            migrationBuilder.DropTable(
                name: "InspectoraReport",
                schema: "Entities");

            migrationBuilder.DropTable(
                name: "valueSmldv",
                schema: "Entities");

            migrationBuilder.DropTable(
                name: "module",
                schema: "ModelSecurity");

            migrationBuilder.DropTable(
                name: "paymentAgreement",
                schema: "Entities");

            migrationBuilder.DropTable(
                name: "form",
                schema: "ModelSecurity");

            migrationBuilder.DropTable(
                name: "permission",
                schema: "ModelSecurity");

            migrationBuilder.DropTable(
                name: "rol",
                schema: "ModelSecurity");

            migrationBuilder.DropTable(
                name: "typePayment",
                schema: "Entities");

            migrationBuilder.DropTable(
                name: "userInfraction",
                schema: "Entities");

            migrationBuilder.DropTable(
                name: "paymentFrequency",
                schema: "Parameters");

            migrationBuilder.DropTable(
                name: "Infraction");

            migrationBuilder.DropTable(
                name: "userNotification",
                schema: "Entities");

            migrationBuilder.DropTable(
                name: "user",
                schema: "ModelSecurity");

            migrationBuilder.DropTable(
                name: "TypeInfraction",
                schema: "Entities");

            migrationBuilder.DropTable(
                name: "person",
                schema: "ModelSecurity");

            migrationBuilder.DropTable(
                name: "documentType",
                schema: "Parameters");

            migrationBuilder.DropTable(
                name: "municipality",
                schema: "Parameters");

            migrationBuilder.DropTable(
                name: "department",
                schema: "Parameters");
        }
    }
}
