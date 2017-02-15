using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using CurrencyConverter.Data;

namespace CurrencyConverter.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20170214123603_reference_currency_on_rates")]
    partial class reference_currency_on_rates
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752");

            modelBuilder.Entity("CurrencyConverter.Models.Currency", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Code");

                    b.Property<string>("Name");

                    b.HasKey("ID");

                    b.ToTable("Currencies");
                });

            modelBuilder.Entity("CurrencyConverter.Models.CurrencyRate", b =>
                {
                    b.Property<int>("CurrencyId");

                    b.Property<int>("ReferenceCurrencyId");

                    b.Property<DateTime>("Date");

                    b.Property<decimal>("Rate");

                    b.HasKey("CurrencyId", "ReferenceCurrencyId", "Date");

                    b.HasIndex("ReferenceCurrencyId");

                    b.ToTable("CurrenciesRates");
                });

            modelBuilder.Entity("CurrencyConverter.Models.CurrencyRate", b =>
                {
                    b.HasOne("CurrencyConverter.Models.Currency", "Currency")
                        .WithMany("CurrencyRates")
                        .HasForeignKey("CurrencyId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CurrencyConverter.Models.Currency", "ReferenceCurrency")
                        .WithMany("CurrencyReferences")
                        .HasForeignKey("ReferenceCurrencyId");
                });
        }
    }
}
