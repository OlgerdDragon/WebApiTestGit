// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using DbApiContextForService;

namespace WebApiGeneral.Migrations
{
    [DbContext(typeof(DbApiContext))]
    partial class TownContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.10")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("WebApiGeneral.Models.Admin", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PersonId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PersonId")
                        .IsUnique();

                    b.ToTable("Admins");
                });

            modelBuilder.Entity("WebApiGeneral.Models.Husband", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PersonId")
                        .HasColumnType("int");

                    b.Property<int>("WifeId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PersonId")
                        .IsUnique();

                    b.HasIndex("WifeId")
                        .IsUnique();

                    b.ToTable("Husbands");
                });

            modelBuilder.Entity("WebApiGeneral.Models.Person", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Login")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Persons");
                });

            modelBuilder.Entity("WebApiGeneral.Models.Product", b =>
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

            modelBuilder.Entity("WebApiGeneral.Models.Shop", b =>
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

            modelBuilder.Entity("WebApiGeneral.Models.WantedProduct", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("BoughtStatus")
                        .HasColumnType("bit");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<int>("WifeId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.HasIndex("WifeId");

                    b.ToTable("WantedProducts");
                });

            modelBuilder.Entity("WebApiGeneral.Models.Wife", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PersonId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PersonId")
                        .IsUnique();

                    b.ToTable("Wifes");
                });

            modelBuilder.Entity("WebApiGeneral.Models.Admin", b =>
                {
                    b.HasOne("WebApiGeneral.Models.Person", "Persons")
                        .WithOne("Admins")
                        .HasForeignKey("WebApiGeneral.Models.Admin", "PersonId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Persons");
                });

            modelBuilder.Entity("WebApiGeneral.Models.Husband", b =>
                {
                    b.HasOne("WebApiGeneral.Models.Person", "Persons")
                        .WithOne("Husbands")
                        .HasForeignKey("WebApiGeneral.Models.Husband", "PersonId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("WebApiGeneral.Models.Wife", "Wifes")
                        .WithOne("Husbands")
                        .HasForeignKey("WebApiGeneral.Models.Husband", "WifeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Persons");

                    b.Navigation("Wifes");
                });

            modelBuilder.Entity("WebApiGeneral.Models.Product", b =>
                {
                    b.HasOne("WebApiGeneral.Models.Shop", "Shops")
                        .WithMany("Products")
                        .HasForeignKey("ShopId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Shops");
                });

            modelBuilder.Entity("WebApiGeneral.Models.WantedProduct", b =>
                {
                    b.HasOne("WebApiGeneral.Models.Product", "Products")
                        .WithMany("WantedProducts")
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("WebApiGeneral.Models.Wife", "Wifes")
                        .WithMany("WantedProducts")
                        .HasForeignKey("WifeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Products");

                    b.Navigation("Wifes");
                });

            modelBuilder.Entity("WebApiGeneral.Models.Wife", b =>
                {
                    b.HasOne("WebApiGeneral.Models.Person", "Persons")
                        .WithOne("Wifes")
                        .HasForeignKey("WebApiGeneral.Models.Wife", "PersonId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Persons");
                });

            modelBuilder.Entity("WebApiGeneral.Models.Person", b =>
                {
                    b.Navigation("Admins");

                    b.Navigation("Husbands");

                    b.Navigation("Wifes");
                });

            modelBuilder.Entity("WebApiGeneral.Models.Product", b =>
                {
                    b.Navigation("WantedProducts");
                });

            modelBuilder.Entity("WebApiGeneral.Models.Shop", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("WebApiGeneral.Models.Wife", b =>
                {
                    b.Navigation("Husbands");

                    b.Navigation("WantedProducts");
                });
#pragma warning restore 612, 618
        }
    }
}
