﻿// <auto-generated />
using System;
using FrontEnd.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FrontEnd.Persistence.Migrations
{
    [DbContext(typeof(FrontEndDbContext))]
    [Migration("20230605132628_Correct_Location")]
    partial class Correct_Location
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.5");

            modelBuilder.Entity("Domain.Models.DutyModel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("DaysDue")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Deleted")
                        .HasColumnType("INTEGER");

                    b.Property<string>("DutyType")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("TEXT");

                    b.Property<long>("DutyTypeId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("EntityModifiedOn")
                        .HasColumnType("TEXT");

                    b.Property<string>("Gender")
                        .HasMaxLength(1)
                        .HasColumnType("TEXT");

                    b.Property<long?>("LivestockTypeId")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("ModifiedBy")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("ModifiedOn")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("TEXT");

                    b.Property<string>("Relationship")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("TEXT");

                    b.Property<bool>("SystemRequired")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Duties");
                });

            modelBuilder.Entity("Domain.Models.EventModel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Color")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("TEXT");

                    b.Property<bool>("Deleted")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("EntityModifiedOn")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ModifiedBy")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("ModifiedOn")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("Domain.Models.FarmLocationModel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Country")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Deleted")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("EntityModifiedOn")
                        .HasColumnType("TEXT");

                    b.Property<long?>("Latitude")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("Longitude")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("ModifiedBy")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("ModifiedOn")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("StreetAddress")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid>("TenantId")
                        .HasColumnType("TEXT");

                    b.Property<string>("Zip")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Farms");
                });

            modelBuilder.Entity("Domain.Models.LandPlotModel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Area")
                        .HasPrecision(18, 3)
                        .HasColumnType("TEXT");

                    b.Property<string>("AreaUnit")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("Deleted")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("EntityModifiedOn")
                        .HasColumnType("TEXT");

                    b.Property<long>("FarmLocationId")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("LandPlotModelId")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("ModifiedBy")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("ModifiedOn")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<long?>("ParentPlotId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Usage")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("LandPlotModelId");

                    b.ToTable("LandPlots");
                });

            modelBuilder.Entity("Domain.Models.LivestockBreedModel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Deleted")
                        .HasColumnType("INTEGER");

                    b.Property<string>("EmojiChar")
                        .IsRequired()
                        .HasMaxLength(2)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("EntityModifiedOn")
                        .HasColumnType("TEXT");

                    b.Property<int>("GestationPeriod")
                        .HasColumnType("INTEGER");

                    b.Property<int>("HeatPeriod")
                        .HasColumnType("INTEGER");

                    b.Property<long>("LivestockTypeId")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("LivestockTypeModelId")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("ModifiedBy")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("ModifiedOn")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("LivestockTypeModelId");

                    b.ToTable("LivestockBreeds");
                });

            modelBuilder.Entity("Domain.Models.LivestockFeedAnalysisModel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime?>("DatePrinted")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DateReceived")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DateReported")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DateSampled")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Deleted")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("EntityModifiedOn")
                        .HasColumnType("TEXT");

                    b.Property<long>("FeedId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("LabNumber")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ModifiedBy")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("ModifiedOn")
                        .HasColumnType("TEXT");

                    b.Property<string>("TestCode")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("LivestockFeedAnalyses");
                });

            modelBuilder.Entity("Domain.Models.LivestockFeedAnalysisParameterModel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Deleted")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("EntityModifiedOn")
                        .HasColumnType("TEXT");

                    b.Property<string>("Method")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ModifiedBy")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("ModifiedOn")
                        .HasColumnType("TEXT");

                    b.Property<string>("Parameter")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<int>("ReportOrder")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SubParameter")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<string>("Unit")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("LivestockFeedAnalysisParameters");
                });

            modelBuilder.Entity("Domain.Models.LivestockFeedAnalysisResultModel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<long>("AnalysisId")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("AsFed")
                        .HasPrecision(18, 2)
                        .HasColumnType("TEXT");

                    b.Property<bool>("Deleted")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Dry")
                        .HasPrecision(18, 2)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("EntityModifiedOn")
                        .HasColumnType("TEXT");

                    b.Property<long?>("LivestockFeedAnalysisModelId")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("ModifiedBy")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("ModifiedOn")
                        .HasColumnType("TEXT");

                    b.Property<long>("ParameterId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("LivestockFeedAnalysisModelId");

                    b.ToTable("LivestockFeedAnalysisResults");
                });

            modelBuilder.Entity("Domain.Models.LivestockFeedDistributionModel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("DatePerformed")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Deleted")
                        .HasColumnType("INTEGER");

                    b.Property<bool?>("Discarded")
                        .IsRequired()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("EntityModifiedOn")
                        .HasColumnType("TEXT");

                    b.Property<long>("FeedId")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("LivestockFeedModelId")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("ModifiedBy")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("ModifiedOn")
                        .HasColumnType("TEXT");

                    b.Property<string>("Note")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Quantity")
                        .HasPrecision(18, 3)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("LivestockFeedModelId");

                    b.ToTable("LivestockFeedDistributions");
                });

            modelBuilder.Entity("Domain.Models.LivestockFeedModel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Active")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("Cutting")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Deleted")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Distribution")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("EntityModifiedOn")
                        .HasColumnType("TEXT");

                    b.Property<string>("FeedType")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("TEXT");

                    b.Property<long>("LivestockTypeId")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("LivestockTypeModelId")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("ModifiedBy")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("ModifiedOn")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Quantity")
                        .HasPrecision(18, 3)
                        .HasColumnType("TEXT");

                    b.Property<string>("QuantityUnit")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("TEXT");

                    b.Property<decimal>("QuantityWarning")
                        .HasPrecision(18, 3)
                        .HasColumnType("TEXT");

                    b.Property<string>("Source")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("LivestockTypeModelId");

                    b.ToTable("LivestockFeeds");
                });

            modelBuilder.Entity("Domain.Models.LivestockFeedServingModel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Deleted")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("EntityModifiedOn")
                        .HasColumnType("TEXT");

                    b.Property<long>("FeedId")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("LivestockFeedModelId")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("ModifiedBy")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("ModifiedOn")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Serving")
                        .HasPrecision(18, 3)
                        .HasColumnType("TEXT");

                    b.Property<string>("ServingFrequency")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<long>("StatusId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("LivestockFeedModelId");

                    b.ToTable("LivestockFeedServings");
                });

            modelBuilder.Entity("Domain.Models.LivestockModel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("BeingManaged")
                        .HasColumnType("INTEGER");

                    b.Property<string>("BirthDefect")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("Birthdate")
                        .HasColumnType("TEXT");

                    b.Property<bool>("BornDefective")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("BottleFed")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Deleted")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("EntityModifiedOn")
                        .HasColumnType("TEXT");

                    b.Property<long?>("FatherId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("ForSale")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Gender")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("TEXT");

                    b.Property<bool>("InMilk")
                        .HasColumnType("INTEGER");

                    b.Property<long>("LivestockBreedId")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("LivestockBreedModelId")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("ModifiedBy")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("ModifiedOn")
                        .HasColumnType("TEXT");

                    b.Property<long?>("MotherId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("TEXT");

                    b.Property<bool>("Sterile")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Variety")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("LivestockBreedModelId");

                    b.ToTable("Livestocks");
                });

            modelBuilder.Entity("Domain.Models.LivestockStatusModel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("BeingManaged")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("TEXT");

                    b.Property<string>("BottleFed")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("TEXT");

                    b.Property<bool>("DefaultStatus")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Deleted")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("EntityModifiedOn")
                        .HasColumnType("TEXT");

                    b.Property<string>("ForSale")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("TEXT");

                    b.Property<string>("InMilk")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("TEXT");

                    b.Property<long>("LivestockTypeId")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("LivestockTypeModelId")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("ModifiedBy")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("ModifiedOn")
                        .HasColumnType("TEXT");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("TEXT");

                    b.Property<string>("Sterile")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("LivestockTypeModelId");

                    b.ToTable("LivestockStatuses");
                });

            modelBuilder.Entity("Domain.Models.LivestockTypeModel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Care")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("TEXT");

                    b.Property<bool>("Deleted")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("EntityModifiedOn")
                        .HasColumnType("TEXT");

                    b.Property<string>("GroupName")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ModifiedBy")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("ModifiedOn")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("TEXT");

                    b.Property<string>("ParentFemaleName")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("TEXT");

                    b.Property<string>("ParentMaleName")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("LivestockTypes");
                });

            modelBuilder.Entity("Domain.Models.MilestoneModel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Deleted")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("EntityModifiedOn")
                        .HasColumnType("TEXT");

                    b.Property<long?>("LivestockTypeId")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("ModifiedBy")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("ModifiedOn")
                        .HasColumnType("TEXT");

                    b.Property<string>("Subcategory")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("TEXT");

                    b.Property<bool>("SystemRequired")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Milestones");
                });

            modelBuilder.Entity("Domain.Models.ScheduledDutyModel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<Guid?>("CompletedBy")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("CompletedOn")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Deleted")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Dismissed")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("DueOn")
                        .HasColumnType("TEXT");

                    b.Property<long>("DutyId")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("DutyModelId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("EntityModifiedOn")
                        .HasColumnType("TEXT");

                    b.Property<long?>("EventModelId")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("ModifiedBy")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("ModifiedOn")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("ReminderDays")
                        .HasPrecision(18, 3)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CompletedOn");

                    b.HasIndex("Dismissed");

                    b.HasIndex("DueOn");

                    b.HasIndex("DutyModelId");

                    b.HasIndex("EventModelId");

                    b.ToTable("ScheduledDuties");
                });

            modelBuilder.Entity("Domain.Models.TenantModel", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Deleted")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("EntityModifiedOn")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("GuidId")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("ModifiedBy")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("ModifiedOn")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("TenantUserAdminId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Tenants");
                });

            modelBuilder.Entity("DutyModelEventModel", b =>
                {
                    b.Property<long>("DutiesId")
                        .HasColumnType("INTEGER");

                    b.Property<long>("EventsId")
                        .HasColumnType("INTEGER");

                    b.HasKey("DutiesId", "EventsId");

                    b.HasIndex("EventsId");

                    b.ToTable("DutyModelEventModel");
                });

            modelBuilder.Entity("DutyModelMilestoneModel", b =>
                {
                    b.Property<long>("DutiesId")
                        .HasColumnType("INTEGER");

                    b.Property<long>("MilestonesId")
                        .HasColumnType("INTEGER");

                    b.HasKey("DutiesId", "MilestonesId");

                    b.HasIndex("MilestonesId");

                    b.ToTable("DutyModelMilestoneModel");
                });

            modelBuilder.Entity("EventModelMilestoneModel", b =>
                {
                    b.Property<long>("EventsId")
                        .HasColumnType("INTEGER");

                    b.Property<long>("MilestonesId")
                        .HasColumnType("INTEGER");

                    b.HasKey("EventsId", "MilestonesId");

                    b.HasIndex("MilestonesId");

                    b.ToTable("EventModelMilestoneModel");
                });

            modelBuilder.Entity("LandPlotModelLivestockModel", b =>
                {
                    b.Property<long>("LivestocksId")
                        .HasColumnType("INTEGER");

                    b.Property<long>("LocationsId")
                        .HasColumnType("INTEGER");

                    b.HasKey("LivestocksId", "LocationsId");

                    b.HasIndex("LocationsId");

                    b.ToTable("LandPlotModelLivestockModel");
                });

            modelBuilder.Entity("LivestockModelLivestockStatusModel", b =>
                {
                    b.Property<long>("LivestocksId")
                        .HasColumnType("INTEGER");

                    b.Property<long>("StatusesId")
                        .HasColumnType("INTEGER");

                    b.HasKey("LivestocksId", "StatusesId");

                    b.HasIndex("StatusesId");

                    b.ToTable("LivestockModelLivestockStatusModel");
                });

            modelBuilder.Entity("Domain.Models.LandPlotModel", b =>
                {
                    b.HasOne("Domain.Models.LandPlotModel", null)
                        .WithMany("Subplots")
                        .HasForeignKey("LandPlotModelId");
                });

            modelBuilder.Entity("Domain.Models.LivestockBreedModel", b =>
                {
                    b.HasOne("Domain.Models.LivestockTypeModel", null)
                        .WithMany("Breeds")
                        .HasForeignKey("LivestockTypeModelId");
                });

            modelBuilder.Entity("Domain.Models.LivestockFeedAnalysisResultModel", b =>
                {
                    b.HasOne("Domain.Models.LivestockFeedAnalysisModel", null)
                        .WithMany("Results")
                        .HasForeignKey("LivestockFeedAnalysisModelId");
                });

            modelBuilder.Entity("Domain.Models.LivestockFeedDistributionModel", b =>
                {
                    b.HasOne("Domain.Models.LivestockFeedModel", null)
                        .WithMany("Distributions")
                        .HasForeignKey("LivestockFeedModelId");
                });

            modelBuilder.Entity("Domain.Models.LivestockFeedModel", b =>
                {
                    b.HasOne("Domain.Models.LivestockTypeModel", null)
                        .WithMany("Feeds")
                        .HasForeignKey("LivestockTypeModelId");
                });

            modelBuilder.Entity("Domain.Models.LivestockFeedServingModel", b =>
                {
                    b.HasOne("Domain.Models.LivestockFeedModel", null)
                        .WithMany("Servings")
                        .HasForeignKey("LivestockFeedModelId");
                });

            modelBuilder.Entity("Domain.Models.LivestockModel", b =>
                {
                    b.HasOne("Domain.Models.LivestockBreedModel", null)
                        .WithMany("Livestocks")
                        .HasForeignKey("LivestockBreedModelId");
                });

            modelBuilder.Entity("Domain.Models.LivestockStatusModel", b =>
                {
                    b.HasOne("Domain.Models.LivestockTypeModel", null)
                        .WithMany("Statuses")
                        .HasForeignKey("LivestockTypeModelId");
                });

            modelBuilder.Entity("Domain.Models.ScheduledDutyModel", b =>
                {
                    b.HasOne("Domain.Models.DutyModel", null)
                        .WithMany("ScheduledDuties")
                        .HasForeignKey("DutyModelId");

                    b.HasOne("Domain.Models.EventModel", null)
                        .WithMany("ScheduledDuties")
                        .HasForeignKey("EventModelId");
                });

            modelBuilder.Entity("DutyModelEventModel", b =>
                {
                    b.HasOne("Domain.Models.DutyModel", null)
                        .WithMany()
                        .HasForeignKey("DutiesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Models.EventModel", null)
                        .WithMany()
                        .HasForeignKey("EventsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DutyModelMilestoneModel", b =>
                {
                    b.HasOne("Domain.Models.DutyModel", null)
                        .WithMany()
                        .HasForeignKey("DutiesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Models.MilestoneModel", null)
                        .WithMany()
                        .HasForeignKey("MilestonesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("EventModelMilestoneModel", b =>
                {
                    b.HasOne("Domain.Models.EventModel", null)
                        .WithMany()
                        .HasForeignKey("EventsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Models.MilestoneModel", null)
                        .WithMany()
                        .HasForeignKey("MilestonesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("LandPlotModelLivestockModel", b =>
                {
                    b.HasOne("Domain.Models.LivestockModel", null)
                        .WithMany()
                        .HasForeignKey("LivestocksId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Models.LandPlotModel", null)
                        .WithMany()
                        .HasForeignKey("LocationsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("LivestockModelLivestockStatusModel", b =>
                {
                    b.HasOne("Domain.Models.LivestockModel", null)
                        .WithMany()
                        .HasForeignKey("LivestocksId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Domain.Models.LivestockStatusModel", null)
                        .WithMany()
                        .HasForeignKey("StatusesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Domain.Models.DutyModel", b =>
                {
                    b.Navigation("ScheduledDuties");
                });

            modelBuilder.Entity("Domain.Models.EventModel", b =>
                {
                    b.Navigation("ScheduledDuties");
                });

            modelBuilder.Entity("Domain.Models.LandPlotModel", b =>
                {
                    b.Navigation("Subplots");
                });

            modelBuilder.Entity("Domain.Models.LivestockBreedModel", b =>
                {
                    b.Navigation("Livestocks");
                });

            modelBuilder.Entity("Domain.Models.LivestockFeedAnalysisModel", b =>
                {
                    b.Navigation("Results");
                });

            modelBuilder.Entity("Domain.Models.LivestockFeedModel", b =>
                {
                    b.Navigation("Distributions");

                    b.Navigation("Servings");
                });

            modelBuilder.Entity("Domain.Models.LivestockTypeModel", b =>
                {
                    b.Navigation("Breeds");

                    b.Navigation("Feeds");

                    b.Navigation("Statuses");
                });
#pragma warning restore 612, 618
        }
    }
}