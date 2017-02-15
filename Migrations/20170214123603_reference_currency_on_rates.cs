using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CurrencyConverter.Migrations
{
    public partial class reference_currency_on_rates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CurrenciesRates",
                table: "CurrenciesRates");

            migrationBuilder.AddColumn<int>(
                name: "ReferenceCurrencyId",
                table: "CurrenciesRates",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_CurrenciesRates",
                table: "CurrenciesRates",
                columns: new[] { "CurrencyId", "ReferenceCurrencyId", "Date" });

            migrationBuilder.CreateIndex(
                name: "IX_CurrenciesRates_ReferenceCurrencyId",
                table: "CurrenciesRates",
                column: "ReferenceCurrencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_CurrenciesRates_Currencies_ReferenceCurrencyId",
                table: "CurrenciesRates",
                column: "ReferenceCurrencyId",
                principalTable: "Currencies",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CurrenciesRates_Currencies_ReferenceCurrencyId",
                table: "CurrenciesRates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CurrenciesRates",
                table: "CurrenciesRates");

            migrationBuilder.DropIndex(
                name: "IX_CurrenciesRates_ReferenceCurrencyId",
                table: "CurrenciesRates");

            migrationBuilder.DropColumn(
                name: "ReferenceCurrencyId",
                table: "CurrenciesRates");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CurrenciesRates",
                table: "CurrenciesRates",
                columns: new[] { "CurrencyId", "Date" });
        }
    }
}
