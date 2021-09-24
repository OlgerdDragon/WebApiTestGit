﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebApiTest.Data;

namespace WebApiTest.Migrations
{
    [DbContext(typeof(TowerContext))]
    [Migration("20210924093152_inPast")]
    partial class inPast
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.10")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("WebApiTest.Models.Admin", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Admins");
                });

            modelBuilder.Entity("WebApiTest.Models.Husband", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("WifeId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("WifeId");

                    b.ToTable("Husbands");
                });

            modelBuilder.Entity("WebApiTest.Models.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Price")
                        .HasColumnType("int");

                    b.Property<int>("ShopId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ShopId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("WebApiTest.Models.Shop", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Shops");
                });

            modelBuilder.Entity("WebApiTest.Models.WantedList", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("BoughtStatus")
                        .HasColumnType("bit");

                    b.Property<string>("NameProduct")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("WantedLists");
                });

            modelBuilder.Entity("WebApiTest.Models.Wife", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("WantedListId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("WantedListId");

                    b.ToTable("Wifes");
                });

            modelBuilder.Entity("WebApiTest.Models.Husband", b =>
                {
                    b.HasOne("WebApiTest.Models.Wife", "Wife")
                        .WithMany("Husbands")
                        .HasForeignKey("WifeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Wife");
                });

            modelBuilder.Entity("WebApiTest.Models.Product", b =>
                {
                    b.HasOne("WebApiTest.Models.Shop", "Shop")
                        .WithMany("Products")
                        .HasForeignKey("ShopId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Shop");
                });

            modelBuilder.Entity("WebApiTest.Models.Wife", b =>
                {
                    b.HasOne("WebApiTest.Models.WantedList", "WantedList")
                        .WithMany("Wifes")
                        .HasForeignKey("WantedListId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("WantedList");
                });

            modelBuilder.Entity("WebApiTest.Models.Shop", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("WebApiTest.Models.WantedList", b =>
                {
                    b.Navigation("Wifes");
                });

            modelBuilder.Entity("WebApiTest.Models.Wife", b =>
                {
                    b.Navigation("Husbands");
                });
#pragma warning restore 612, 618
        }
    }
}
