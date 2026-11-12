using System;
using System.Collections.Generic;
using HotelProject.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelProject.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Booking> Bookings { get; set; }

    public virtual DbSet<Check> Checks { get; set; }

    public virtual DbSet<Guest> Guests { get; set; }

    public virtual DbSet<Journal> Journals { get; set; }

    public virtual DbSet<Organization> Organizations { get; set; }

    public virtual DbSet<Place> Places { get; set; }

    public virtual DbSet<Room> Rooms { get; set; }

    public virtual DbSet<RoomCategory> RoomCategories { get; set; }

    public virtual DbSet<Service> Services { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_unicode_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.IdBooking).HasName("PRIMARY");

            entity.ToTable("bookings");

            entity.HasIndex(e => e.BookingNumber, "bookingNumber").IsUnique();

            entity.HasIndex(e => e.OrganizationId, "organization_id");

            entity.Property(e => e.IdBooking).HasColumnName("idBooking");
            entity.Property(e => e.BookingNumber)
                .HasComment("Бронь ")
                .HasColumnName("bookingNumber");
            entity.Property(e => e.Contact)
                .HasMaxLength(150)
                .HasComment("Контактное лицо")
                .HasColumnName("contact");
            entity.Property(e => e.DateRequest)
                .HasComment("Дата заявки")
                .HasColumnName("dateRequest");
            entity.Property(e => e.OrganizationId)
                .HasMaxLength(20)
                .HasComment("организация")
                .HasColumnName("organization_id");

            entity.HasOne(d => d.Organization).WithMany(p => p.Bookings)
                .HasForeignKey(d => d.OrganizationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("booking_ibfk_1");
        });

        modelBuilder.Entity<Check>(entity =>
        {
            entity.HasKey(e => e.IdCheck).HasName("PRIMARY");

            entity.ToTable("checks");

            entity.HasIndex(e => e.BookingId, "booking_id");

            entity.HasIndex(e => e.CheckNumber, "invoiceNumber").IsUnique();

            entity.Property(e => e.IdCheck).HasColumnName("idCheck");
            entity.Property(e => e.Amount)
                .HasPrecision(10, 2)
                .HasComment("счет к оплате")
                .HasColumnName("amount");
            entity.Property(e => e.BookingId)
                .HasComment("Бронь")
                .HasColumnName("booking_id");
            entity.Property(e => e.CheckNumber)
                .HasMaxLength(20)
                .HasComment("Счёт")
                .HasColumnName("checkNumber");
            entity.Property(e => e.Date).HasComment("Дата выставления");

            entity.HasOne(d => d.Booking).WithMany(p => p.Checks)
                .HasForeignKey(d => d.BookingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("invoice_ibfk_1");
        });

        modelBuilder.Entity<Guest>(entity =>
        {
            entity.HasKey(e => e.IdGuest).HasName("PRIMARY");

            entity.ToTable("guests");

            entity.HasIndex(e => e.GuestCard, "guestCard").IsUnique();

            entity.Property(e => e.IdGuest).HasColumnName("idGuest");
            entity.Property(e => e.BirthDate)
                .HasComment("Дата рождения")
                .HasColumnName("birthDate");
            entity.Property(e => e.FullName)
                .HasMaxLength(100)
                .HasComment("полное имя фамилия отчество")
                .HasColumnName("fullName");
            entity.Property(e => e.GuestCard)
                .HasMaxLength(5)
                .HasComment("Карта гостя")
                .HasColumnName("guestCard");
        });

        modelBuilder.Entity<Journal>(entity =>
        {
            entity.HasKey(e => e.IdJournal).HasName("PRIMARY");

            entity.ToTable("journals");

            entity.HasIndex(e => e.BookingId, "booking_id");

            entity.HasIndex(e => e.DocumentNumber, "documentNumber").IsUnique();

            entity.HasIndex(e => e.ServiceId, "service_id");

            entity.Property(e => e.IdJournal).HasColumnName("idJournal");
            entity.Property(e => e.BookingId)
                .HasComment("Бронь")
                .HasColumnName("booking_id");
            entity.Property(e => e.DocumentNumber)
                .HasMaxLength(5)
                .HasComment("Документ")
                .HasColumnName("documentNumber");
            entity.Property(e => e.Quantity)
                .HasComment("Количество")
                .HasColumnName("quantity");
            entity.Property(e => e.ServiceDate)
                .HasComment("Дата")
                .HasColumnName("serviceDate");
            entity.Property(e => e.ServiceId)
                .HasComment("Услуга")
                .HasColumnName("service_id");

            entity.HasOne(d => d.Booking).WithMany(p => p.Journals)
                .HasForeignKey(d => d.BookingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("service_log_ibfk_1");

            entity.HasOne(d => d.Service).WithMany(p => p.Journals)
                .HasForeignKey(d => d.ServiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("service_log_ibfk_2");
        });

        modelBuilder.Entity<Organization>(entity =>
        {
            entity.HasKey(e => e.IdOrganization).HasName("PRIMARY");

            entity.ToTable("organizations");

            entity.Property(e => e.IdOrganization)
                .HasMaxLength(20)
                .HasComment("ID организации")
                .HasColumnName("idOrganization");
            entity.Property(e => e.Address)
                .HasMaxLength(100)
                .HasComment("Адрес")
                .HasColumnName("address");
            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .HasComment("Название")
                .HasColumnName("name");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .HasComment("Телефон")
                .HasColumnName("phone");
        });

        modelBuilder.Entity<Place>(entity =>
        {
            entity.HasKey(e => e.IdPlace).HasName("PRIMARY");

            entity.ToTable("places");

            entity.HasIndex(e => e.BookingId, "booking_id");

            entity.HasIndex(e => e.RecordNumber, "recordNumber").IsUnique();

            entity.HasIndex(e => e.RoomId, "room_id");

            entity.Property(e => e.IdPlace).HasColumnName("idPlace");
            entity.Property(e => e.Arrive)
                .HasComment("Заезд")
                .HasColumnName("arrive");
            entity.Property(e => e.BookingId)
                .HasComment("Бронь")
                .HasColumnName("booking_id");
            entity.Property(e => e.Exit)
                .HasComment("Выезд")
                .HasColumnName("exit");
            entity.Property(e => e.RecordNumber)
                .HasMaxLength(10)
                .HasComment("Запись")
                .HasColumnName("recordNumber");
            entity.Property(e => e.RoomId)
                .HasComment("Номер")
                .HasColumnName("room_id");

            entity.HasOne(d => d.Booking).WithMany(p => p.Places)
                .HasForeignKey(d => d.BookingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("accommodation_ibfk_1");

            entity.HasOne(d => d.Room).WithMany(p => p.Places)
                .HasForeignKey(d => d.RoomId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("accommodation_ibfk_2");

            entity.HasMany(d => d.Guests).WithMany(p => p.Places)
                .UsingEntity<Dictionary<string, object>>(
                    "PlaceGuest",
                    r => r.HasOne<Guest>().WithMany()
                        .HasForeignKey("GuestId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("accommodation_guest_ibfk_2"),
                    l => l.HasOne<Place>().WithMany()
                        .HasForeignKey("PlaceId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("accommodation_guest_ibfk_1"),
                    j =>
                    {
                        j.HasKey("PlaceId", "GuestId")
                            .HasName("PRIMARY")
                            .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });
                        j.ToTable("place_guests");
                        j.HasIndex(new[] { "GuestId" }, "guest_id");
                        j.IndexerProperty<int>("PlaceId").HasColumnName("place_id");
                        j.IndexerProperty<int>("GuestId").HasColumnName("guest_id");
                    });
        });

        modelBuilder.Entity<Room>(entity =>
        {
            entity.HasKey(e => e.IdRoom).HasName("PRIMARY");

            entity.ToTable("rooms");

            entity.HasIndex(e => e.CategoryId, "category_id");

            entity.HasIndex(e => e.CodeRoom, "codeRoom").IsUnique();

            entity.Property(e => e.IdRoom).HasColumnName("idRoom");
            entity.Property(e => e.Address)
                .HasMaxLength(50)
                .HasComment("Адрес")
                .HasColumnName("address");
            entity.Property(e => e.Block)
                .HasComment("Корпус")
                .HasColumnType("enum('Гостевой корпус','Главный корпус')")
                .HasColumnName("block");
            entity.Property(e => e.Capacity)
                .HasComment("Мест")
                .HasColumnName("capacity");
            entity.Property(e => e.CategoryId)
                .HasComment("Категория")
                .HasColumnName("category_id");
            entity.Property(e => e.CodeRoom)
                .HasMaxLength(5)
                .HasComment("Обозначение")
                .HasColumnName("codeRoom");
            entity.Property(e => e.RoomNumber)
                .HasComment("Номер")
                .HasColumnName("roomNumber");

            entity.HasOne(d => d.Category).WithMany(p => p.Rooms)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("room_ibfk_1");
        });

        modelBuilder.Entity<RoomCategory>(entity =>
        {
            entity.HasKey(e => e.IdCategory).HasName("PRIMARY");

            entity.ToTable("room_categories");

            entity.HasIndex(e => e.CategoryName, "categoryName").IsUnique();

            entity.Property(e => e.IdCategory).HasColumnName("idCategory");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(30)
                .HasComment("Название категории")
                .HasColumnName("categoryName");
            entity.Property(e => e.Price)
                .HasPrecision(10, 2)
                .HasComment("цена за ночь\n")
                .HasColumnName("price");
        });

        modelBuilder.Entity<Service>(entity =>
        {
            entity.HasKey(e => e.IdService).HasName("PRIMARY");

            entity.ToTable("services");

            entity.HasIndex(e => e.ServiceCode, "serviceCode").IsUnique();

            entity.Property(e => e.IdService).HasColumnName("idService");
            entity.Property(e => e.Price)
                .HasPrecision(10, 2)
                .HasComment("цена")
                .HasColumnName("price");
            entity.Property(e => e.ServiceCode)
                .HasMaxLength(10)
                .HasComment("Код услуги")
                .HasColumnName("serviceCode");
            entity.Property(e => e.ServiceName)
                .HasMaxLength(100)
                .HasComment("Услуга")
                .HasColumnName("serviceName");
            entity.Property(e => e.Unit)
                .HasMaxLength(50)
                .HasComment("единица")
                .HasColumnName("unit");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.IdUser).HasName("PRIMARY");

            entity.ToTable("users");

            entity.HasIndex(e => e.Login, "login_UNIQUE").IsUnique();

            entity.Property(e => e.IdUser)
                .HasComment("id пользователя")
                .HasColumnName("idUser");
            entity.Property(e => e.Failed)
                .HasComment("количество неудачных входов\n")
                .HasColumnName("failed");
            entity.Property(e => e.IsBlocked).HasColumnName("isBlocked");
            entity.Property(e => e.Login)
                .HasMaxLength(50)
                .HasComment("логин пользователя")
                .HasColumnName("login");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(64)
                .HasColumnName("passwordHash");
            entity.Property(e => e.Role)
                .HasDefaultValueSql("'User'")
                .HasComment("роль пользователя в системе по умолчанию user\n")
                .HasColumnType("enum('Admin','User')")
                .HasColumnName("role");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
