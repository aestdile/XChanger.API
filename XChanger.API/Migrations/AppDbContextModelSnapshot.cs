using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using XChanger.API.DataAccess;


namespace XChanger.API.Migrations;

[DbContext(typeof(AppDbContext))]
partial class AppDbContextModelSnapshot : ModelSnapshot
{
    protected override void BuildModel(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasAnnotation("ProductVersion", "9.0.7")
            .HasAnnotation("Relational:MaxIdentifierLength", 128);

        SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

        modelBuilder.Entity("XChanger.API.Models.Data.Person", b =>
            {
                b.Property<Guid>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("uniqueidentifier");

                b.Property<int>("Age")
                    .HasColumnType("int");

                b.Property<string>("Name")
                    .IsRequired()
                    .HasColumnType("TEXT");

                b.HasKey("Id");

                b.ToTable("Persons");
            });

        modelBuilder.Entity("XChanger.API.Models.Data.Pet", b =>
            {
                b.Property<Guid>("Id")
                    .ValueGeneratedOnAdd()
                    .HasColumnType("uniqueidentifier");

                b.Property<string>("Name")
                    .IsRequired()
                    .HasColumnType("TEXT");

                b.Property<Guid>("PersonId")
                    .HasColumnType("uniqueidentifier");

                b.Property<int>("Type")
                    .HasColumnType("int");

                b.HasKey("Id");

                b.HasIndex("PersonId");

                b.ToTable("Pets");
            });

        modelBuilder.Entity("XChanger.API.Models.Data.Pet", b =>
            {
                b.HasOne("XChanger.API.Models.Data.Person", null)
                    .WithMany("Pets")
                    .HasForeignKey("PersonId")
                    .OnDelete(DeleteBehavior.Cascade)
                    .IsRequired();
            });

        modelBuilder.Entity("XChanger.API.Models.Data.Person", b =>
            {
                b.Navigation("Pets");
            });
    }
}
